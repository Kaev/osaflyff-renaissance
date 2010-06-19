using System;
using System.IO;
using System.Net.Sockets;
using System.Collections.Generic;

namespace FlyffCluster
{
    public class Packet : PacketBase
    {
        public int dwMergedCount = 0;
        private BinaryWriter m_memWriter;
        public Packet()
        {
            m_memBuffer = new MemoryStream();
            m_memWriter = new BinaryWriter(m_memBuffer);
            Addbyte(0x5E);
            Addint32(0); // SIZE.
        }

        public override byte[] buffer
        {
            get
            {
                return base.m_memBuffer.ToArray();
            }
        }

        public int dwDataSize
        {
            get
            {
                return dwSize - 5;
            }
        }

        /// Regular "add" functions
        public void Addbyte(byte b)
        {
            m_memWriter.Write(b);
        }
        public void Addint32(int i)
        {
            m_memWriter.Write(i);
        }
        public void Addfloat(float f)
        {
            m_memWriter.Write(f);
        }
        public void Addfloat(double d)
        {
            m_memWriter.Write((float)d);
        }
        public void Addint16(short s)
        {
            m_memWriter.Write(s);
        }
        public void Addint64(long l)
        {
            m_memWriter.Write(l);
        }
        public void Addstring(string str)
        {
            Addint32(str.Length);
            for (int i = 0; i < str.Length; i++)
                Addbyte(str[i]);
        }
        public void Addbytes(params byte[] bytes)
        {
            m_memWriter.Write(bytes);
        }
        /// Typecast functions
        public void Addbyte(short s)
        {
            Addbyte((byte)s);
        }
        public void Addbyte(int i)
        {
            Addbyte((byte)i);
        }
        public void Addbyte(uint i)
        {
            Addbyte((byte)i);
        }
        public void Addint32(uint i)
        {
            Addint32((int)i);
        }
        public void Addint32(long l)
        {
            Addint32((int)l);
        }
        public void Addint16(int i)
        {
            Addint16((short)i);
        }
        public void Addint16(uint i)
        {
            Addint16((short)i);
        }
        public void Addint16(long l)
        {
            Addint16((short)l);
        }

        /// Merged packet functions
        public void StartNewMergedPacket(int dwMoverID, uint dwCommand)
        {
            StartNewMergedPacket(dwMoverID, dwCommand, 0xFFFFFF00);
        }
        public void StartNewMergedPacket(int dwMoverID, uint dwCommand, uint dwMainCommand)
        {
            if (dwMergedCount == 0)
            {
                Addint32(dwMainCommand);
                Addint32(0);
                Addint16(1);
                dwMergedCount++;
            }
            else
            {
                if (dwMoverID != -1)
                {
                    m_memBuffer.Seek(13, SeekOrigin.Begin);
                    Addint16(dwMergedCount);
                    m_memBuffer.Seek(0, SeekOrigin.End);
                }
            }
            Addint32(dwMoverID);
            Addint16((short)dwCommand);
        }
        public int Send(Client c)
        {
            return Send(c.c_socket);
        }
        public int Send(Socket s)
        {
            try
            {
                int dwOldPointer = dwPointer;
                dwPointer = 1;
                Addint32(dwDataSize);
                dwPointer = dwOldPointer;
                if (s.Connected)
                    return s.Send(buffer);
                else
                    return -1;
            }
            catch
            {
                Log.Write(Log.MessageType.error, "Packet::Send(): @ exception caught..");
                return -1;
            }
        }
        public void SendTo(List<Client> list)
        {
            for (int i = 0; i < list.Count; i++)
                Send(list[i]);
        }
    }
}