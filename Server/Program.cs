using TcpServer;

Server server = new Server(); // создаем сервер
await server.ListenAsync(); // запускаем сервер