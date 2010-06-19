using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    class AuthType
    {
        public const int    // Authentication types for bool array usage.
                            PASSWORD    =   0,
                            VERSION     =   1,
                            SERVERTYPE  =   2,
                            SERVERINFO  =   3;
    }
    class PID
    {
        public const int    // Packet headers for ISC server >>> other servers
                            PASSWORD_REQUEST    = 0x90,
                            VERSION_REQUEST     = 0x91,
                            SERVERTYPE_REQUEST  = 0x92,
                            SERVERINFO_REQUEST  = 0x93;
        public const int    // Packet headers from login server
                            DISCONNECT_USER     = 0xA0;
        //public const int  // Packet headers from cluster servers // Got none ATM

        public const int    // Packet headers from world servers 
                            NOTICE_ALL          = 0xC0,
                            REFRESH_SERVER_CONF = 0xC1;

        public const int    // Packet headers from misc
                            MUTE_USER           = 0xD1,
                            TELEPORT_USER       = 0xD2;
        public const int    // Packet headers from ISC server <<< other servers (reply to 0x9X)
                            PASSWORD_REPLY      = 0x80,
                            VERSION_REPLY       = 0x81,
                            SERVERTYPE_REPLY    = 0x82,
                            SERVERINFO_REPLY    = 0x83;
    }
}
