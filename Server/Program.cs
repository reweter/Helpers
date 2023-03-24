using TcpServer;

Server server = new Server(); // Создаем сервер
await server.ListenAsync(); // Запускаем сервер