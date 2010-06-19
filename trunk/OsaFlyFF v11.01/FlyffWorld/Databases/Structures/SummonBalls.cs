using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FlyffWorld
{
    public class SummonBalls
    {
        public int itemID;
        public struct balls
        {
            public int moverID;
            public int chance;
            public int amount;
            public balls(int moverid, int chance, int amount)
            {
                this.moverID = moverid;
                this.chance = chance;
                this.amount = amount;
            }
        }
        public List<balls> listballs = new List<balls>();

        public static bool LoadSummonBalls()
        {
            int SummonBallItems = 0;

            try
            {

                FileInfo SummonBallFile = new FileInfo(@"db\SummonBalls.txt");

                string[] ReadContent = File.ReadAllLines(SummonBallFile.FullName);

                for (int i = 0; i < ReadContent.Length - 1; i++)
                {
                    if (ReadContent[i].ToString().StartsWith("{"))
                    {
                        SummonBalls item = new SummonBalls();
                        item.itemID = Convert.ToInt32(ReadContent[i - 1]);

                        i++;
                        while (!ReadContent[i].StartsWith("}"))
                        {
                            balls bl = new balls();

                            String[] splitter = ReadContent[i].Split(",".ToCharArray());

                            string replacer = splitter[0].Replace("  ", "");
                            bl.moverID = Convert.ToInt32(splitter[0]);

                            bl.chance = Convert.ToInt32(splitter[1]);

                            item.listballs.Add(bl);

                            i++;
                        }

                        WorldServer.world_summonballs.Add(item);

                        SummonBallItems++;

                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(Log.MessageType.error, "Failed to load SummonBalls File at : {0}\r\n{1}", ex.Message, ex.StackTrace);
            }
            Log.Write(Log.MessageType.load, "Loaded {0} SummonBalls.", SummonBallItems);
            return true;
        }
    }
}

