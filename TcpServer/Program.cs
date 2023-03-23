using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServerStart();
        }

        private static async void ServerStart()
        {
            Server server = new Server(); // создаем сервер
            await server.ListenAsync(); // запускаем сервер
        }
    }
}
