using System;
using System.Collections.Generic;

using System.Text;

namespace FlyffCluster
{
    public class Character
    {
        public int id = 0,
            slot = 0,
            mapID = 0,
            hairstyle = 0,
            facemodel = 0,
            gender = 0,
            jobid = 0,
            level = 0,
            strength = 0,
            stamina = 0,
            dexterity = 0,
            intelligence = 0;
        public List<int> items = new List<int>();
        
        public string charactername = "",
                      haircolor = "";
        public float x = 0, y = 0, z = 0;
    }
}
