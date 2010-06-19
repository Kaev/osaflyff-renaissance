using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    class ISCHandler
    {
        ISCServer parent;
        public ISCHandler(ISCServer _)
        {
            parent = _;
        }
        public void Parse(ISCDataPacket dp)
        {
            dp.ResetPointer(5); // Skip 5E&size
            int header = dp.Readint();
            switch (header)
            {
                case PID.VERSION_REQUEST:
                    sendVersion();
                    break;
                case PID.SERVERTYPE_REQUEST:
                    sendServertype();
                    break;
                case PID.PASSWORD_REQUEST:
                    sendPassword();
                    break;
                case PID.SERVERINFO_REQUEST:
                    sendServerinfo();
                    break;
                case PID.REFRESH_SERVER_CONF:
                    Server.Refresh();
                    break;
                default:
                    break;
            }
        }
        public void sendDisconnect(int u)
        {
            Packet pak = new Packet();
            pak.Addint(PID.DISCONNECT_USER);
            pak.Addint(u);
            pak.Send(parent.cClone);
        }
        public void sendRefresh()
        {
            Packet pak = new Packet();
            pak.Addint(PID.REFRESH_SERVER_CONF);
            pak.Send(parent.cClone);
        }
        public void sendVersion()
        {
            Packet pak = new Packet();
            pak.Addint(PID.VERSION_REPLY);
            pak.Addbyte(ISCConfig.majorVersion);
            pak.Addbyte(ISCConfig.minorVersion);
            pak.Send(parent.cClone);
        }
        public void sendPassword()
        {
            Packet pak = new Packet();
            pak.Addint(PID.PASSWORD_REPLY);
            pak.Addstring(ISCConfig.inter_password);
            pak.Send(parent.cClone);
        }
        public void sendServertype()
        {
            Packet pak = new Packet();
            pak.Addint(PID.SERVERTYPE_REPLY);
            pak.Addshort(ServerType.WORLDSERVER);
            pak.Send(parent.cClone);
        }
        public void sendServerinfo()
        {
            Packet pak = new Packet();
            pak.Addint(PID.SERVERINFO_REPLY);
            pak.Addint(Server.clusterid);
            pak.Addint(Server.worldid);
            pak.Send(parent.cClone);
        }
    }
}