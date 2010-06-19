using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
namespace FlyffWorld
{
    public class Packet : PacketCommands
    {
        bool sizeAdded = false;
        private int worldPakCount = 0;
        public string Buffer = "";
        public int Size { get { return Buffer.Length; } set { } }
        public int FlyffSize { get { return Buffer.Length - (sizeAdded ? 5 : 1); } set { } }
        public Packet()
        {
            Addbyte(0x5E);
        }
        public void Addpad(int i)
        {
            for (int l = 0; l < i; l++)
                Addbyte(0);
        }
        public void AddDropItemData(Item item)
        {
            Addint(-1);
            Addint(item.dwItemID);
            Addint(NULL);
            Addint(NULL);
            Addshort(item.dwQuantity);
            Addshort(NULL);
            Addint(item.bExpired ? -1 : 2100000000);
            Addint(NULL);
            //Addbyte(item.expired ? 1 : 0);
            Addint(item.dwRefine);
            Addint(NULL);
            Addbyte(item.dwElement);
            Addint(item.dwEleRefine);
            Addint(NULL);
            Addbyte(item.c_sockets.Length);
            for (int i = 0; i < item.c_sockets.Length; i++)
                Addshort(item.c_sockets[i]);
            Addint(0);
            Addlong(item.c_awakening);
            Addint(0);
        }
        public void Addbyte(byte b)
        {
            Buffer+=(char)b;
        }
        public void AddItemData(Item item)
        {
            Addint(item.dwItemID);
            Addint(NULL);
            Addint(NULL);
            Addshort(item.dwQuantity);
            Addbyte(NULL);
            Addint(item.bExpired ? -1 : 2100000000);
            Addint(NULL);
            Addbyte(item.bExpired ? 1 : 0);
            Addint(item.dwRefine);
            Addint(NULL);
            Addbyte(item.dwElement);
            Addint(item.dwEleRefine);
            Addint(NULL);
            Addbyte(item.c_sockets.Length);
            for (int i = 0; i < item.c_sockets.Length; i++)
                Addshort(item.c_sockets[i]);
            Addint((item.qwLastUntil > -1 && !item.bExpired) ? 1 : 0);
            Addlong(item.c_awakening);
            if (item.qwLastUntil > -1 && !item.bExpired)
            {
                Addint((uint)DLL.time());
                Addint((int)(item.qwLastUntil - DLL.time()));
            }
            else
                Addint(NULL);
            Addbyte(0);
        }
        public void Addbyte(int i)
        {
            Buffer += (char)(byte)i;
        }
        public void Addfloat(float f)
        {
            byte[] fbyte = BitConverter.GetBytes(f);
            Addbyte(fbyte[0]);
            Addbyte(fbyte[1]);
            Addbyte(fbyte[2]);
            Addbyte(fbyte[3]);
        }
        public void Addfloat(double d)
        {
            byte[] fbyte = BitConverter.GetBytes((float)d);
            Addbyte(fbyte[0]);
            Addbyte(fbyte[1]);
            Addbyte(fbyte[2]);
            Addbyte(fbyte[3]);
        }
        public void Setbyte(int pos, byte newbyte)
        {
            Buffer=Buffer.Remove(pos, 1).Insert(pos, Convert.ToString((char)newbyte));
        }
        public void StartNewMergedPacketLUA(object moverID, object command)
        {
            StartNewMergedPacket(Convert.ToInt32(moverID), Convert.ToUInt32(command));
        }
        public void StartNewMergedPacket(int moverID, uint command)
        {
            if (worldPakCount == 0) // packet started so we add (int)FFFFFF00 (int)0 (short)count (int)id
            {
                Addint(0xFFFFFF00);
                Addint(0);
                Addshort(1);
                worldPakCount++;
            }
            else
                if (moverID != -1)
                {
                    byte[] shortbyte = BitConverter.GetBytes((short)++worldPakCount);
                    Setbyte(9 + (sizeAdded ? 4 : 0), shortbyte[0]);
                    Setbyte(10 + (sizeAdded ? 4 : 0), shortbyte[1]);
                }
            Addint(moverID);
            Addshort((int)command);
        }
        [Obsolete("ArrayList with Client are no longer in use. Therefore, Packet::SendTo(ArrayList) is now deprecated. Please use Packet::SendTo(List<Client>) instead.", false)]
        public void SendTo(System.Collections.ArrayList clients)
        {
            for (int i = 0; i < clients.Count; i++)
                if (clients[i] is Client)
                    Send((Client)clients[i]);
        }
        public void SendTo(List<Client> clients)
        {
            for (int i = 0; i < clients.Count; i++)
                Send(clients[i]);
        }

        public void StartNewMergedPacket(int moverID, uint command, int otherStart)
        {
            if (worldPakCount == 0)
            {
                Addint(otherStart);
                Addint(0);
                Addshort(1);
                worldPakCount++;
            }
            else
                if (moverID != -1)
                {
                    byte[] shortbyte = BitConverter.GetBytes((short)++worldPakCount);
                    Setbyte(13, shortbyte[0]);
                    Setbyte(14, shortbyte[1]);
                }
            Addint(moverID);
            Addshort((short)command);
        }
        public void Addfloat(int i)
        {
            byte[] fbyte = BitConverter.GetBytes((float)i);
            Addbyte(fbyte[0]);
            Addbyte(fbyte[1]);
            Addbyte(fbyte[2]);
            Addbyte(fbyte[3]);
        }
        public void Addlong(long l)
        {
            byte[] lbyte = BitConverter.GetBytes(l);
            Addbyte(lbyte[0]);
            Addbyte(lbyte[1]);
            Addbyte(lbyte[2]);
            Addbyte(lbyte[3]);
            Addbyte(lbyte[4]);
            Addbyte(lbyte[5]);
            Addbyte(lbyte[6]);
            Addbyte(lbyte[7]);
        }
        public void Addshort(short s)
        {
            byte[] _sbyte = BitConverter.GetBytes(s);
            Addbyte(_sbyte[0]);
            Addbyte(_sbyte[1]);
        }
        public void Addshort(int i)
        {
            byte[] ibyte = BitConverter.GetBytes((short)i);
            Addbyte(ibyte[0]);
            Addbyte(ibyte[1]);
        }
        public void Addsize()
        {
            if (sizeAdded)
                return;
            byte[] ibyte = BitConverter.GetBytes(FlyffSize);
            sizeAdded = true;
            Addheader(ibyte);
        }
        public void Addheader(byte[] ba)
        {
            string data = Buffer.Substring(1);
            Buffer = "^";
            for (int i = 0; i < ba.Length; i++)
                Buffer += (char)ba[i];
            Buffer += data;
        }
        public void Addstring(string str)
        {
            if (str == null)
            {
                Addint(0);
                return;
            }
            Addint(str.Length);
            foreach (char c in str)
                Addbyte((byte)c);
        }
        public void Addint(int i)
        {
            byte[] ibyte = BitConverter.GetBytes(i);
            Addbyte(ibyte[0]);
            Addbyte(ibyte[1]);
            Addbyte(ibyte[2]);
            Addbyte(ibyte[3]);
        }
        public void Addint(uint ui)
        {
            byte[] uibyte = BitConverter.GetBytes(ui);
            Addbyte(uibyte[0]);
            Addbyte(uibyte[1]);
            Addbyte(uibyte[2]);
            Addbyte(uibyte[3]);
        }
        public void Addhex(string hex)
        {
            hex = hex.Replace(" ", "");
            for (int i = 0; i < hex.Length; i = i + 2)
                this.Addbyte(Convert.ToByte(hex.Substring(i, 2), 16));
        }
        public int Send(Client c)
        {
            try
            {
                if (!sizeAdded)
                    Addsize();
                char[] ca = Buffer.ToCharArray();
                byte[] data = new byte[ca.Length];
                for (int i = 0; i < ca.Length; i++)
                    data[i] = (byte)ca[i];
                Log.Write(Log.MessageType.packet, "{0}{1}{2}", "0xunknown", ca.Length, Packets.MakePString(data, ca.Length));
                int sent = c.c_socket.Send(data, Buffer.Length, SocketFlags.None);
                if (sent != Buffer.Length)
                    Log.Write(Log.MessageType.error, "Unexpected send() result: lost {0} bytes while sending packet.", Size - sent);
                return sent;
            }
            catch (Exception)
            {
                c.Destruct("Failed to send packet - client probably disconnected");
                return -1;
            }
        }
        public int Send(Socket s)
        {
            try
            {
                if (!sizeAdded)
                    Addsize();
                char[] ca = Buffer.ToCharArray();
                byte[] data = new byte[ca.Length];
                for (int i = 0; i < ca.Length; i++)
                    data[i] = (byte)ca[i];
                int sent = s.Send(data, Buffer.Length, SocketFlags.None);
                if (sent != Buffer.Length)
                    Log.Write(Log.MessageType.error, "Unexpected send() result: lost {0} bytes while sending packet.", Size - sent);
                return sent;
            }
            catch (Exception e)
            {
                Log.Write(Log.MessageType.fatal, "Error sending packet: {0}", e.Message);
                return -1;
            }
        }
    }
}
