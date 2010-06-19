using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public class LuaBinary
    {
        private long _bits = 0;
        public void AddValue(int value, int bitCount)
        {
            _bits <<= bitCount;
            _bits |= value;
        }
        public int dword
        {
            get
            {
                return (int)_bits;
            }
        }
        public long qword
        {
            get
            {
                return _bits;
            }
        }
        public ulong qword_u
        {
            get
            {
                return (ulong)_bits;
            }
        }
        public uint dword_u
        {
            get
            {
                return (uint)_bits;
            }
        }
    }
}
