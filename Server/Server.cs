using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace TcpServer
{
    class Server
    {
        private TcpListener Listener = new TcpListener(IPAddress.Any, 8888); // сервер для прослушивания
        private List<Client> Сlients = new List<Client>(); // все подключения

        protected internal void RemoveConnection(string id)
        {
            // получаем по id закрытое подключение
            Client? client = Сlients.FirstOrDefault(c => c.Id == id);
            // и удаляем его из списка подключений
            if (client != null) Сlients.Remove(client);
            client?.Close();
        }

        // прослушивание входящих подключений
        protected internal async Task ListenAsync()
        {
            try
            {
                // Запуск сервера
                Listener.Start();

                while (true)
                {
                    TcpClient tcpClient = await Listener.AcceptTcpClientAsync();

                    Client clientObject = new Client(tcpClient, this);
                    Сlients.Add(clientObject);
                    Task.Run(clientObject.ProcessAsync);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Disconnect();
            }
        }

        // Трансляция сообщения подключенным клиентам
        protected internal async Task BroadcastMessageAsync(string message, string id)
        {
            foreach (var client in Сlients)
            {
                if (client.Id != id) // Если id клиента не равно id отправителя
                {
                    await client.Writer.WriteLineAsync(message); // Передача данных
                    await client.Writer.FlushAsync();
                }
            }
        }

        // Отключение всех клиентов
        protected internal void Disconnect()
        {
            foreach (var client in Сlients)
                client.Close(); // Отключение клиента

            Listener.Stop(); // Остановка сервера
        }
    }
}
