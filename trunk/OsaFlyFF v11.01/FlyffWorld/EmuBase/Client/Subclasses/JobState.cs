using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using LuaInterface;

/// [Adidishen]
/// Now you don't need to modify using directives. :)

namespace FlyffWorld
{
    public enum JobState
    {
        Vagrant, FirstJob, SecondJob, Master, Hero
    }
}
