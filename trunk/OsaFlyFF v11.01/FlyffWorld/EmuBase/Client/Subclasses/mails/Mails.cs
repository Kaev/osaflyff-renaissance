using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public class Mails
    {
        public int mailid = 0,
                   fromCharID = 0,
                   toCharID = 0,
                   isRead = 0,
                   attachedPenya = 0,
                   date =0;

        public Item attachedItem;

        public string topic = "",
                      message = "";
   

    }
    public class CharacterList
    {
        public string CharacterName = "";
        public int dwCharID = 0;

        public CharacterList(string name, int id)
        {
            this.CharacterName = name;
            this.dwCharID = id;
        }
        public CharacterList()
        { }
    }
}
