/*using System;
using System.Collections.Generic;

using System.Text;

namespace FlyffWorld
{
    public enum NetworkStatus  
    {
        Unknown, Offline, Online, AFK, Addict, Eating, Resting, Moving
    }

}

*/
using System;
using System.Collections.Generic;

using System.Text;

namespace FlyffWorld
{
    public struct NetworkStatus
    {
        public const byte Online = 0,
                          Offline = 1,
                          AFK = 3,
                          Addict = 4,
                          Eating = 5,
                          Resting = 6,
                          Moving = 7,
                          MinValue = Online,
                          MaxValue = Moving;

    }
}
