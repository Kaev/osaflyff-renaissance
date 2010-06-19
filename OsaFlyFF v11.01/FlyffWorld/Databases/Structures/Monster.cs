using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public class Monster : Mover
    {
        public void ClearTarget()
        {
            bIsFighting = false;
            bIsFollowing = false;
            bIsBlocked = false;
            c_destiny = absolute_position;
            SendMoverNewDestination();
            c_target = null;
        }
        /// [Adidishen]
        /// TODO: Move to WorldHelper class (the 2 functions below)
        public static MonsterData getMobDataByID(int mob_model)
        {
            for (int i = 0; i < WorldServer.data_items.Count; i++)
            {
                MonsterData mob = WorldServer.data_mobs[i];
                if (mob.mobID == mob_model)
                    return mob;
            }
            return null;
        }
        public static Monster getMobByMoverID(int mID)
        {
            for (int i = 0; i < WorldServer.world_monsters.Count; i++)
            {
                Monster mob;
                if ((mob = WorldServer.world_monsters[i]) != null && mob.dwMoverID == mID)
                    return mob;
            }
            return null;
        }
        public int mob_model = 0, mob_mapID = 1;
        //add something to say if we want it to respawn or not
        public bool respawn = true;
        Point abspos;
        public Point absolute_position
        {
            get
            {
                return abspos;
            }
        }
        MonsterData _data = null;
        public MonsterData Data
        {
            get
            {
                if (_data != null)
                    return _data;
                _data = Monster.getMobDataByID(mob_model);
                if (_data == null)
                {
                    Log.Write(Log.MessageType.error, "Monster.Data & Monster.getMobByID({0}): failed", mob_model);
                    return new MonsterData();
                }
                return _data;
            }
        }
        public Timers timers = new Timers(); // create MonsterTimers
        public bool mob_aggressive = false;
        public bool mob_isdead = false;
        public int mob_respawndelay = 10;
        public long next_move_time;
        public long next_spawn_time;
       
        public long next_despawn_time;
        /// [Adidishen]
        /// Override dwMoverSpeed of Mover
        public override double dwMoverSpeed
        {
            get
            {
                return Data.mobMoveSpeed;
            }
            set
            {
                Data.mobMoveSpeed = value;
            }
        }
        public Monster(int model, Point pos, int mapID, bool aggressive)
            : base(MOVER_MONSTER)
        {
            next_move_time = DLL.time() + DiceRoller.RandomNumber(5, 15);
            c_position = abspos = pos;
            mob_model = model;
            mob_mapID = mapID;
            mob_aggressive = aggressive;
            c_attributes[DST_HP] = c_attributes[DST_HP_MAX] = Data.mobHP;
            dwMoverSize = Data.mobSize;
        }
        public void mob_OnDeath()
        {
            c_target = null;
            bIsFighting = false;
            bIsFollowing = false;
            bIsFollowing = false;
            mob_isdead = true;
            next_spawn_time = DLL.time() + DiceRoller.RandomNumber(mob_respawndelay - 3, mob_respawndelay + 8) + 5; // +5: start counting 2 seconds after despawn time
            next_despawn_time = DLL.time() + 3;
        }

        public void attackPlayer()
        {
            if (mob_isdead)
                return;
            if (c_target == null)
            {
                bIsFighting = false;
                bIsTargeted = false;
                timers.nextAttack = 0;
                return;
            }
            
            if (!c_position.IsVisible(c_target.c_position))
            {
                ClearTarget();
                return;
            }
            if (!c_position.IsInCircle(c_target.c_position, 1.5f))
            {
                next_move_time = DLL.time() + WorldServer.c_random.Next(1, 3);
                //c_destiny = c_target.c_position;
                //SendMoverNewDestination();
                
                followMover(c_target.dwMoverID,0);
                
                SendUnknowFollowRelatedPacket(c_target.dwMoverID);
                return;
            }
            Client client = (Client)c_target;
            //we neeed to check if client use darkillusion, if yes mob stop is attack
            if (client.c_data.buffs.Count > 0)
            {
                for (int i = 0; i < client.c_data.buffs.Count; i++)
                {
                    if (client.c_data.buffs[i]._skill.dwNameID == 193)
                    {
                        ClearTarget();
                        return;
                    }
                }
            }
            bIsFighting = true;
            
           
            int damage = client.c_data.f_CalculateDamagefromMonster(this);
            
            // Hitrate
            //if mob is affected by dark is hit rate must be reduce
            if (antibuffs.Count > 0) //to do when monster hit rate will be done
            {
                for (int i = 0; i < antibuffs.Count; i++)
                {
                    if (antibuffs[i]._skill.dwNameID == 202) //autoshot
                    {
                        //we must decrease monster hit rate function of skill lvl
                        //hitrate -= antibuffs[i]._skill.dwdestData1; 
                    }
                }
            }
            //if mob miss player and player have counter attack buff
            //we send an attack with counter attack damage
            if (client.c_data.buffs.Count > 0)
            {
                for (int i = 0; i < client.c_data.buffs.Count; i++)
                {
                    if (client.c_data.buffs[i]._skill.dwNameID == 204) //if player has skill counter attack
                    {
                        CPSUData PSUData = new CPSUData();
                        PSUData.Caster = client;
                        PSUData.Target.SetMover(this);
                        PSUData.SkillData = client.c_data.buffs[i]._skill;
                        PSUData.SkillName = SkillNames.GetSkillNameBySkillID(client.c_data.buffs[i]._skill.dwNameID);
                        PSUData.Skill = client.c_data.buffs[i]._skill;
                        UseSkill.Attack(PSUData);
                        
                    }                   
                }
            }
            // Blockrate
            // Critrate

            damage = damage > c_target.c_attributes[DST_HP] ? c_target.c_attributes[DST_HP] : damage;

            c_target.c_attributes[DST_HP] -= damage;
            c_target.SendMoverDamaged(dwMoverID, damage, AttackFlags.NORMAL,c_target.c_position,0); // Normal/Crit/block/miss
            client.SendPlayerAttribSet(FlyFF.DST_HP, c_target.c_attributes[DST_HP]);
            #region part needed for skills
            //for reflecthit we must take in memory latest damage value
            client.c_data.dwlastdamage = damage;
            #region Prevention skill

            if (client == null)
               return;
            if (c_target.c_attributes[DST_HP] <= client.c_data.f_MaxHP * 10/100)//if HP inferior à 10%
            {
                //see if target has prevention skill...
                for (int i = client.c_data.buffs.Count-1; i >=0 ; i--)
                    {
                        Buff cbuff = client.c_data.buffs[i];
                        if (cbuff._skill.dwNameID == 48) //prevention
                        {
                    
                            c_target.c_attributes[DST_HP] += client.c_data.f_MaxHP * cbuff._skill.dwChangeParamVal1 / 100;
                            if (c_target.c_attributes[DST_HP] > client.c_data.f_MaxHP)
                            c_target.c_attributes[DST_HP] = client.c_data.f_MaxHP;
                            //send new hp value

                            client.SendPlayerAttribSet(FlyFF.DST_HP, c_target.c_attributes[FlyFF.DST_HP]);
                            //remove prevention from buff
                            
                            cbuff.dwTime = 0;
                            client.SendPlayerBuff(cbuff);
                            client.c_data.buffs.Remove(cbuff);
                        }
                    }
            }
            #endregion
            #region Cruci spell/Pain reflection
            //now if the player is a psykeeper with crucio spell we must reflect half of the damage done
            if (client.c_attributes[DST_REFLECT_DAMAGE] > 0)
            {
                int reflectdamage = damage * client.c_attributes[DST_REFLECT_DAMAGE] / 100;
                c_attributes[DST_HP] -= reflectdamage;
                SendMoverDamaged(dwMoverID, reflectdamage, AttackFlags.MAGIC,c_position,0);
                if (c_attributes[DST_HP] <= 0)
                {
                    int xprate = 1;
                    if (client.c_data.dwLevel < 60) xprate = Server.exp_rate;
                    else if (client.c_data.dwLevel >= 60 && client.c_data.dwLevel < 90) xprate = Server.exp_rate60;
                    else if (client.c_data.dwLevel >= 90) xprate = Server.exp_rate90;
                    client.c_data.qwExperience += (int)((double)(this.Data.mobExpPoints) * (double)xprate * client.c_data.f_RateModifierEXP);
                    client.OnCheckLevelGroup();
                    client.SendPlayerCombatInfo();
                    SendMoverDeath();
                    c_target = null;
                    mob_OnDeath();
                    client.DropMobItems(this, client, client.c_data.dwMapID);
                }
            }
            #endregion
            #endregion
            if (c_target.c_attributes[DST_HP] <= 0)
            {
                c_target.SendMoverDeath();
                
               //we must delete all buff
                Buff.ClearAllBuffs(client);
                ClearTarget();
                DelayedActions.ClearMoverAntibuffAndDelayed(c_target);
            }

            /// [Adidishen]
            /// Removed. specific function added in threads.
            
            /*
            if (c_target.c_attributes[DST_HP] <= 0)
            {
                c_target.sendMoverDeath();
                Mover lasttarget = c_target;
                c_target = null;
                bIsFighting = false;
                bIsTargeted = false;
                bIsFollowing = false;
                c_destiny = aggro_position;
                sendSetMoverDestiny();
                for (int j = 0; j < WorldServer.world_monsters.Count; j++)
                {
                    Monster thismonster = WorldServer.world_monsters[j];
                    if (thismonster.bIsFighting)
                    {
                        if (thismonster.c_target.dwMoverID == lasttarget.dwMoverID)
                        {
                            thismonster.c_target = null;
                            thismonster.bIsFighting = false;
                            thismonster.bIsTargeted = false;
                            thismonster.bIsFollowing = false;
                            thismonster.c_destiny = thismonster.absolute_position;
                            thismonster.sendSetMoverDestiny();
                        }
                    }
                }
                 
                
                timers.nextAttack = 0;
            }*/
        }
    }
}