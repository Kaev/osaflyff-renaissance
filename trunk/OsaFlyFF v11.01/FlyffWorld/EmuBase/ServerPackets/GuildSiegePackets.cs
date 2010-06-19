using System;
using System.Text;

namespace FlyffWorld
{
    public partial class Client
    {

        public void GuildSiedgeTimer()
        {
            Packet pak = new Packet();
            pak.StartNewMergedPacket(dwMoverID,0xB8);            
            pak.Addbyte(16);
            pak.Addint32(dwMoverID);
            pak.Addint32(2);
            pak.StartNewMergedPacket(dwMoverID, 0xF4);
            pak.Send(this);
        }


    }
}
