using System;
using System.Collections.Generic;

using System.Text;

namespace FlyffWorld
{
    public class Timers
    {
        public Timers() { }
        public void ResetAll()
        {
            nextRecovery    = DLL.time() + 10;
            nextSave        = DLL.time() + 120;
            nextCheer       = DLL.time() + 3600;
            nextBuffCheck   = DLL.time() + 10;
            nextAttack = 0;
        }
        public long nextRecovery     = 0,
                    nextSave         = 0,
                    nextCheer        = 0,
                    nextAttack = 0,
                    nextBuffCheck    = 0;
    }
}
