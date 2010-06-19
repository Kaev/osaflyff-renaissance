using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public class Remarks
    {
        public static void Append()
        {
            // Add your remarks on the current build HERE.
            // Use the following syntax:
            // Server.AppendRemark((string), (arguments));
            Server.AppendRemark("V11.01 note : osAflyff emulator updates");
            Server.AppendRemark("Collect system is done");
            Server.AppendRemark("Some change in the gm commands");
            Server.AppendRemark("Quest system have been remove for the moment");
        }
    }
}
