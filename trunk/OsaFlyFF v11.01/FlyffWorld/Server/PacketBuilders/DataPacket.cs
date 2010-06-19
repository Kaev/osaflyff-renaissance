using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public class DataPacket
    {
        public byte[] buffer;
        public int size=0;
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
            int len = Readint();
            string ret = "";
            for (int i = 0; i < len; i++)
                ret += (char)buffer[_pointer++];
            return ret;
        }
        public float Readfloat()
        {
            return BitConverter.ToSingle(new byte[] { buffer[_pointer++], buffer[_pointer++], buffer[_pointer++], buffer[_pointer++] }, 0);
        }
        public int Readint()
        {
            return BitConverter.ToInt32(new byte[] { buffer[_pointer++], buffer[_pointer++], buffer[_pointer++], buffer[_pointer++] }, 0);
        }
        public short Readshort()
        {                
            return BitConverter.ToInt16(new byte[] { buffer[_pointer++], buffer[_pointer++] }, 0);
        }
        public void IncreasePosition(int amount)
        {
            this._pointer += amount;
        }
        public byte Readbyte()
        {
            return buffer[_pointer++];
        }
        public DataPacket CutBuffer(int fromByteIndex, int untilByteIndex)
        {
            DataPacket newdp = new DataPacket();
            byte[] buffer = new byte[1000];
            int diff = untilByteIndex - fromByteIndex;
            for (int i = 0; i < diff; i++)
                buffer[i] = this.buffer[fromByteIndex + i];
            newdp.buffer = buffer;
            newdp.size = diff;
            return newdp;
        }
        public DataPacket()
        {
            this.buffer = new byte[1000];
        }
    }
}
