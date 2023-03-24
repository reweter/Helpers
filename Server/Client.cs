using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace TcpServer
{
    class Client
    {
        protected internal string Id { get; } = Guid.NewGuid().ToString();
        protected internal StreamWriter Writer { get; }
        protected internal StreamReader Reader { get; }

        private TcpClient _Client;
        // Объект сервера
        private Server _Server;

        public Client(TcpClient tcpClient, Server serverObject)
        {
            _Client = tcpClient;
            _Server = serverObject;
            // Получаем NetworkStream для взаимодействия с сервером
            NetworkStream stream = _Client.GetStream();
            // Создаем StreamReader для чтения данных
            Reader = new StreamReader(stream);
            // Создаем StreamWriter для отправки данных
            Writer = new StreamWriter(stream);
        }

        public async Task ProcessAsync()
        {
            try
            {
                // Получаем имя пользователя
                string? userName = await Reader.ReadLineAsync();
                string message = $"{userName} вошел в чат";
                // Посылаем сообщение о входе в чат всем подключенным пользователям
                await _Server.BroadcastMessageAsync(message, Id);
                Console.WriteLine(message);
                // В бесконечном цикле получаем сообщения от клиента
                while (true)
                {
                    try
                    {
                        message = await Reader.ReadLineAsync();
                        if (message == null) continue;
                        message = $"{userName}: {message}";
                        Console.WriteLine(message);
                        await _Server.BroadcastMessageAsync(message, Id);
                    }
                    catch
                    {
                        message = $"{userName} покинул чат";
                        Console.WriteLine(message);
                        await _Server.BroadcastMessageAsync(message, Id);
                        break;
                    }
                }
            }
            finally
            {
                // В случае выхода из цикла закрываем ресурсы
                _Server.RemoveConnection(Id);
            }
        }

        // Закрытие подключения
        protected internal void Close()
        {
            Writer.Close();
            Reader.Close();
            _Client.Close();
        }
    }
}
