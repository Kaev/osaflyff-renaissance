using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace FlyffLogin
{
    public class ClientManager
    {
        public static List<Client> c_clients = new List<Client>();

        public static object srClientListRoot = new object();

        public static readonly int MANAGERS_COUNT = 5;
        private static Thread[] m_managers;
        static ClientManager()
        {
            // Initialize threads
            m_managers = new Thread[MANAGERS_COUNT];
            for (int i = 0; i < MANAGERS_COUNT; i++)
                m_managers[i] = new Thread(new ThreadStart(ManagerProcess));
            try
            {
                for (int i = 0; i < MANAGERS_COUNT; i++)
                    m_managers[i].Start();
            }
            catch (Exception e)
            {
                Log.Write(Log.MessageType.fatal, "Could not start one or more client managers: " + e.Message);
                Environment.Exit(50800);
            }
            Log.Write(Log.MessageType.info, "Initialized {0} client managers.", MANAGERS_COUNT);
        }
        public static void DeleteClient(Client c)
        {
            lock (srClientListRoot)
                c_clients.Remove(c);
        }
        public static void ManagerProcess()
        {
            try
            {
                List<Client> readyclients = new List<Client>();
                while (true)
                {
                    readyclients.Clear();
                    lock (srClientListRoot)
                    {
                        for (int i = 0; i < c_clients.Count; i++)
                        {
                            if (!c_clients[i].bIsBusy)
                            {
                                c_clients[i].bIsBusy = true; // prevents the rest of the threads from accessing this client until the current thread sets this as false later on
                                if (c_clients[i].c_socket.Poll(100, SelectMode.SelectRead))
                                {
                                    readyclients.Add(c_clients[i]);
                                }
                                else
                                {
                                    c_clients[i].bIsBusy = false; // allows other threads to use this socket.
                                }
                            }
                        }
                    }
                    // Reason why we're using while loop here is because once we destroy the client, if in a for loop, it'll miss the client that's supposed to come after the destroyed client
                    // This way is safer
                    while (readyclients.Count > 0)
                    {
                        byte[] buffer = new byte[1500];
                        try
                        {
                            if (readyclients[0].c_socket.Receive(buffer) < 1)
                                throw new DisconnectedException();
                        }
                        catch (DisconnectedException)
                        {
                            readyclients[0].Destruct("Client finished session");
                            readyclients.RemoveAt(0);
                            continue;
                        }
                        catch (Exception e)
                        {
                            if (!readyclients[0].c_socket.Connected)
                            {
                                readyclients[0].Destruct("Client finished session");
                                readyclients.RemoveAt(0);
                                continue;
                            }
                            else
                            {
                                readyclients[0].Write("failed to receive packet: {0}", e.Message);
                            }
                        }
                        DataPacket[] dps = DataPacket.SplitNaglePackets(buffer);
                        for (int j = 0; j < dps.Length; j++)
                        {
                            readyclients[0].HandlePacket(dps[j]);
                        }
                        readyclients[0].bIsBusy = false;
                        readyclients.RemoveAt(0);
                    }
                    Thread.Sleep(50);
                }
            }
            catch (Exception e)
            {
                Log.Write(Log.MessageType.fatal, "ClientManager::ManagerProcess: manager crashed: {0}", e.Message);
            }
        }
    }
    public class DisconnectedException : Exception { }
}
