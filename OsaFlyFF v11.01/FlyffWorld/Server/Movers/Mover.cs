using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public partial class Mover : PacketCommands
    {
        public bool bIsTraced = false;
        public long qwLastTraceTime = DLL.time();
        public Lock moverLock = new Lock();
        public List<Mover> c_spawns = new List<Mover>();
        private int MOVER_TYPE;
        public int dwMoverSize = 100;
        public long qwLastMovementCalc = DLL.clock();
        public Point c_position = new Point(0, 0, 0, 0);
        public Point c_destiny = new Point(0, 0, 0, 0);
        public MoverAttributes c_attributes = new MoverAttributes();
        public Client child;
        public int dwMoverID;
        public const int MOVER_NPC = 0,
                         MOVER_PLAYER = 1,
                         MOVER_MONSTER = 2,
                         MOVER_ITEM = 3; // Nicco->Drops
        public Mover c_target = null;
        private bool m_battle = false;
        public bool bIsTargeted = false;
        public bool bIsFollowing = false;
        private double m_dwMoverSpeed = 0.1;
        /// [Adidishen]
        /// Below is virtual so we can override it w/ some function
        /// Also, 0.1 is the default speed modifier, for players.
        /// This way we only need to set the mob's default speed
        /// But then it's virtual so we don't even need to set it -- we can override it :p
        public virtual double dwMoverSpeed
        {
            get
            {
                return m_dwMoverSpeed;
            }
            set
            {
                m_dwMoverSpeed = value;
            }
        }
        public bool bIsFighting
        {
            get
            {
                return c_target != null && m_battle;
            }
            set
            {
                if (value == false)
                {
                    c_target = null;
                    m_battle = false;
                }
                else if (c_target != null && value)
                    m_battle = true;
            }
        }
        public bool bIsKeyboardWalking = false;
        public void followMover(int moverID)
        {
            //c_target = MoversHandler.getMoverByMoverID(moverID);
            if (c_target == null)
                return;
            bIsFollowing = true;
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID, PAK_MOVING_FOLLOW);
            pak.Addint(moverID);
            pak.Addint(0);
            if (child != null)
                pak.Send(child);
            SendToVisible(pak);
        }
        public Mover(int moverType)
        {
            MOVER_TYPE = moverType;
            dwMoverID = MoversHandler.NewMoverID();
        }
        public Mover(Client child)
        {
            MOVER_TYPE = MOVER_PLAYER;
            dwMoverID = MoversHandler.NewMoverID();
            this.child = child;
        }
        public int MoverType
        {
            get
            {
                return MOVER_TYPE;
            }
        }
        public void SendToVisible(Packet pak)
        {
            for (int i = 0; i < c_spawns.Count; i++)
            {
                if (c_spawns[i] is Client)
                    pak.Send((Client)c_spawns[i]);
            }
            
        }
        public void UpdatePositionValues()
        {
            float distx = 0, distz = 0, distall = 0;
            distx = c_destiny.x - c_position.x;
            distz = c_destiny.z - c_position.z;
            distall = (float)Math.Sqrt(distx * distx + distz * distz);
            double etime = DLL.clock() - qwLastMovementCalc;
            qwLastMovementCalc = DLL.clock();
            // Contribution of Caali
            double ntime = distall / (c_attributes[DST_SPEED] * 0.0006 * dwMoverSpeed);
            if (ntime <= etime || distall == 0 || c_position.IsInSphere(c_destiny, 0.1f))
            {
                c_position = c_destiny;
            }
            else
            {
                float moveX = distx * ((float)etime / (float)ntime);
                float moveZ = distz * ((float)etime / (float)ntime);
                if (bIsKeyboardWalking)
                {
                    c_position.MoveByAngleDegree2D((float)Math.Sqrt(moveX * moveX + moveZ * moveZ), c_position.angle);
                }
                else
                {
                    c_position.x += moveX;
                    c_position.z += moveZ;
                }
            }
        }
    }
}
