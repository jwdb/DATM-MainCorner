using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DATM_MainCenter.Services
{

    public class MineStat
    {
        const ushort dataSize = 512; // this will hopefully suffice since the MotD should be <=59 characters
        const ushort numFields = 6;  // number of values expected from server

        public string Address { get; set; }
        public ushort Port { get; set; }
        public string Motd { get; set; }
        public string Version { get; set; }
        public string CurrentPlayers { get; set; }
        public string MaximumPlayers { get; set; }
        public bool ServerUp { get; set; }
        public long Delay { get; set; }

        public MineStat(string address, ushort port)
        {

            Address = address;
            Port = port;

        }

        public async Task Check()
        {
            var rawServerData = new byte[dataSize];

            try
            {
                // ToDo: Add timeout
                var stopWatch = new Stopwatch();
                var tcpclient = new TcpClient();
                stopWatch.Start();
                await tcpclient.ConnectAsync(Address, Port);
                stopWatch.Stop();
                var stream = tcpclient.GetStream();
                var payload = new byte[] { 0xFE, 0x01 };
                stream.Write(payload, 0, payload.Length);
                stream.Read(rawServerData, 0, dataSize);
                tcpclient.Dispose();
                Delay = stopWatch.ElapsedMilliseconds;
            }
            catch (Exception ex)
            {
                ServerUp = false;
                return;
            }

            if (rawServerData == null || rawServerData.Length == 0)
            {
                ServerUp = false;
            }
            else
            {
                var serverData = Encoding.Unicode.GetString(rawServerData).Split("\u0000\u0000\u0000".ToCharArray());
                if (serverData != null && serverData.Length >= numFields)
                {
                    ServerUp = true;
                    Version = serverData[2];
                    Motd = serverData[3];
                    CurrentPlayers = serverData[4];
                    MaximumPlayers = serverData[5];
                }
                else
                {
                    ServerUp = false;
                }
            }
        }
    }
}
