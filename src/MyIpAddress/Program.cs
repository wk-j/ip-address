using System;
using System.Net;
using System.Net.Sockets;

namespace MyIp {
    class Program {
        static void Main(string[] args) {
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0)) {
                socket.Connect("8.8.8.8", 65530);
                var endPoint = socket.LocalEndPoint as IPEndPoint;
                var ip = endPoint.Address.ToString();
                Console.WriteLine($"{ip}");
            }
        }
    }
}
