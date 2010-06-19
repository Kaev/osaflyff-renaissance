using System;
using System.IO;
using System.Collections.Generic;

namespace ISC
{
    public class DataPacket : PacketBase
    {
        private BinaryReader m_memReader;
        public override byte[] buffer
        {
            get
            {
                return base.buffer;
            }
            set
            {
                m_memBuffer.Seek(0, SeekOrigin.Begin);
                m_memBuffer.Write(value, 0, value.Length);
            }
        }

        public int dwRecvSize = 0;

        public DataPacket()
        {
            m_memBuffer = new MemoryStream(1452);
            m_memReader = new BinaryReader(m_memBuffer);
        }
        /// <summary>
        /// Splits the raw packet into an array of packets, in case the raw packet was nagle'd.
        /// </summary>
        /// <param name="buffer">The packet buffer.</param>
        /// <seealso cref="http://www.google.com/search?q=Nagle+algorithm"/>
        public static DataPacket[] SplitNaglePackets(byte[] buffer)
        {
            /// [Adidishen]
            /// New fragmenting algorithm.

            List<DataPacket> packets = new List<DataPacket>();
            int dwSize = buffer.Length;
            int dwOffset = 0;
            BinaryReader r = new BinaryReader(new MemoryStream(buffer));

            while (dwOffset < dwSize - 1)
            {
                r.BaseStream.Position = dwOffset;
                r.ReadByte(); // 0x5E
                int dwCurrentSize = r.ReadInt32();
                if (dwCurrentSize == 0)
                    break;
                int dwCount = 5 + dwCurrentSize; // 5: headers
                byte[] byCurrentBuffer = new byte[dwCount];
                Array.Copy(buffer, dwOffset, byCurrentBuffer, 0, dwCount);
                DataPacket dp = new DataPacket();
                dp.buffer = byCurrentBuffer;
                packets.Add(dp);
                dwOffset += dwCount;
            }
            return packets.ToArray();
        }
        /// <summary>
        /// Splits the raw packet into an array of packets, in case the raw packet was nagle'd.
        /// </summary>
        /// <param name="dp">The packet class.</param>
        /// <seealso cref="http://www.google.com/search?q=Nagle+algorithm"/>
        public static DataPacket[] SplitNaglePackets(DataPacket dp)
        {
            return SplitNaglePackets(dp.buffer);
        }

        /// Packet header verification function (contribution of Caali)
        /// <summary>
        /// Verifies the packet headers. If verfication fails, returns false -- the packet should be disregarded.
        /// </summary>
        /// <param name="dwSessionKey">The session key associated with the user.</param>
        public bool VerifyHeaders(int dwSessionKey)
        {
            Crc32 crc = new Crc32();
            int dwDataSize = dwSize - 13; // does not include headers
            int dwCalculatedLengthHash = ~(BitConverter.ToInt32(crc.ComputeHash(BitConverter.GetBytes(dwDataSize)), 0) ^ dwSessionKey);
            byte[] data = new byte[dwDataSize];
            Array.Copy(buffer, 13, data, 0, dwDataSize);
            int dwCalculatedDataHash = ~(BitConverter.ToInt32(crc.ComputeHash(data), 0) ^ dwSessionKey);

            int dwOldPointer = dwPointer;
            dwPointer = 0;
            if (Readbyte() != 0x5E)
                return false;
            int dwReadLengthHash = Readint32();
            dwPointer += 4;
            int dwReadDataHash = Readint32();
            dwPointer = dwOldPointer;
            return dwReadDataHash == dwCalculatedDataHash && dwReadLengthHash == dwCalculatedLengthHash;
        }

        /// "Read" functions
        public int Readint32()
        {
            return m_memReader.ReadInt32();
        }
        public string Readstring()
        {
            int dwLength = Readint32();
            byte[] stringBytes = m_memReader.ReadBytes(dwLength);
            string ret = "";
            for (int i = 0; i < dwLength; i++)
                ret += (char)stringBytes[i];
            return ret;
        }
        public float Readfloat()
        {
            return m_memReader.ReadSingle();
        }
        public short Readint16()
        {
            return m_memReader.ReadInt16();
        }
        public byte Readbyte()
        {
            return m_memReader.ReadByte();
        }
        public bool Readbool()
        {
            return m_memReader.ReadBoolean();
        }
        public long Readint64()
        {
            return m_memReader.ReadInt64();
        }
    }
}