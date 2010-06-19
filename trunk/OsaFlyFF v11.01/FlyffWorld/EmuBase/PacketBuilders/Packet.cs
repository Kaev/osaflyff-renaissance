using System;
using System.IO;
using System.Net.Sockets;
using System.Collections.Generic;

namespace FlyffWorld
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
        public void AddUint64(ulong l)
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

        public void AddItemData(Item item)
        {
            // [Divinepunition] structure fixed by me
            this.Addint32(item.dwItemID);
            this.Addint32(0); //unique id
            this.Addint32(0); //need to add charname if it's linked object in place of this
            this.Addint16(item.dwQuantity);
            this.Addbyte(0); //seled character level ? but why ? need to see packet for a linked object
            if (item.Data.itemkind[2] == 36) //if the item is a collector, we send the actual charge
            {
                this.Addint16(item.dwCharge);
                this.Addint16(0);
                this.Addint32(0);
            }
            else if (item.Data.itemkind[2] > 24 && item.Data.itemkind[2] != 57 || item.Data.itemkind[2] != 58)//if it's an item that don't need endurance value
            {
                this.Addint32(-1);
                this.Addint32(0);
            }
            else
                this.Addint64(item.Data.endurance);
            this.Addbyte(item.bExpired ? 1 : 0);
            this.Addint32(item.dwRefine);
            this.Addint32(0);
            this.Addbyte(item.dwElement);
            this.Addint32(item.dwEleRefine);
            this.Addint32(0);
            this.Addbyte(item.c_sockets.Length);
            for (int i = 0; i < item.c_sockets.Length; i++)
            {
                this.Addint16(item.c_sockets[i]);
            }
            this.Addbyte((item.qwLastUntil > -1 && !item.bExpired) ? 1 : 0);
            this.Addint32(0);
            this.AddUint64(item.c_awakening);
            if (item.qwLastUntil > -1 && !item.bExpired)
            {
                this.Addint32((uint)DLL.time());
                this.Addint32((int)(item.qwLastUntil - DLL.time()));
            }
            else
                this.Addint32(0);
            

            /// [Adidishen] full structure = contribution of Caali

            /*this.Addint32(item.dwItemID);
            this.Addint32(0); /// [Caali] The unique ID of the item. [Adidishen] not to be confused with the slot's unique ID!!! [Caali] this ID is different for every item in the server.
            this.Addstring(""); // Sealed character name. o_O?
            this.Addint16(item.dwQuantity);
            this.Addbyte(0); // Sealed character's level?..
            this.Addint32(0); // durability. [Caali] still built in, but doesn't matter if it's 0
            this.Addint32(0); // Sealed character job?!... what the fuck?
            this.Addbyte(item.bExpired ? 1 : 0);
            this.Addint32(item.dwRefine);
            this.Addint32(0);
            this.Addbyte(item.dwElement);
            this.Addint32(item.dwEleRefine);
            this.Addint32(0);
            this.Addbyte(item.c_sockets.Length);
            for (int i = 0; i < item.c_sockets.Length; i++)
                this.Addint16(item.c_sockets[i]);
            this.Addint32((item.qwLastUntil > -1 && !item.bExpired) ? 1 : 0);
            this.AddUint64(item.c_awakening);
            if (item.qwLastUntil > -1 && !item.bExpired)
            {
                this.Addint32((uint)DLL.time());
                this.Addint32((int)(item.qwLastUntil - DLL.time()));
            }
            else
                this.Addint32(0);
            this.Addbyte(0);*/
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
                Addint32(dwMoverID);
                Addint16(++dwMergedCount);
            }
            else
            {
                if (dwMoverID != -1)
                {
                    int dwPointer = this.dwPointer;
                    m_memBuffer.Seek(13, SeekOrigin.Begin);
                    Addint16(++dwMergedCount);
                    m_memBuffer.Seek(0, SeekOrigin.End);
                }
            }
            Addint32(dwMoverID);
            Addint16(dwCommand);
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
                Log.Write(Log.MessageType.error, "Packet::Send(): @ exception caught");
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