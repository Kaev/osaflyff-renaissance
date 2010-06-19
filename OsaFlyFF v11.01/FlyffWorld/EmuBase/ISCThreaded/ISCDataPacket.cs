using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    class ISCDataPacket
    {
        public byte[] buffer;
        public int size = 0;
        public int _pointer = 0;
        public void ResetPointer() // reset to 0
        {
            this._pointer = 0;
        }
        public void ResetPointer(int pos) // set pos to x
        {
            this._pointer = pos;
        }

        public string Readstring()
        {
            try
            {
                byte[] byLen = { buffer[_pointer++], buffer[_pointer++], buffer[_pointer++], buffer[_pointer++] };
                int len = Packets.ParseInt(byLen);
                string ret = "";
                for (int i = 0; i < len; i++)
                    ret += (char)buffer[_pointer++];
                return ret;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public float Readfloat()
        {
            try
            {
                return BitConverter.ToSingle(new byte[] { buffer[_pointer++], buffer[_pointer++], buffer[_pointer++], buffer[_pointer++] }, 0);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public int Readint()
        {
            try
            {
                return Packets.ParseInt(new byte[] { buffer[_pointer++], buffer[_pointer++], buffer[_pointer++], buffer[_pointer++] });
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public short Readshort()
        {
            try
            {
                return BitConverter.ToInt16(new byte[] { buffer[_pointer++], buffer[_pointer] }, 0);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public void IncreasePosition(int amount)
        {
            this._pointer += amount;
        }
        public byte Readbyte()
        {
            try
            {
                return buffer[_pointer++];
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public ISCDataPacket()
        {
            this.buffer = new byte[500];
        }
    }
}
