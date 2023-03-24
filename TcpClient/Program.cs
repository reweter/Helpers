using System.Net.Sockets;
 
string host = "127.0.0.1";
int port = 8888;
using TcpClient client = new TcpClient();
Console.Write("Введите свое имя: ");
string? userName = Console.ReadLine();
Console.WriteLine($"Добро пожаловать, {userName}");
StreamReader? Reader = null;
StreamWriter? Writer = null;

try
{
    client.Connect(host, port); // Подключение клиента
    Reader = new StreamReader(client.GetStream());
    Writer = new StreamWriter(client.GetStream());
    if (Writer is null || Reader is null) return;
    // Запускаем новый поток для получения данных
    Task.Run(() => ReceiveMessageAsync(Reader));
    // Запускаем ввод сообщений
    await SendMessageAsync(Writer);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
Writer?.Close();
Reader?.Close();

// Отправка сообщений
async Task SendMessageAsync(StreamWriter writer)
{
    // Сначала отправляем имя
    await writer.WriteLineAsync(userName);
    await writer.FlushAsync();
    Console.WriteLine("Для отправки сообщений введите сообщение и нажмите Enter");

    while (true)
    {
        string? message = Console.ReadLine();
        await writer.WriteLineAsync(message);
        await writer.FlushAsync();
    }
}

// Получение сообщений
async Task ReceiveMessageAsync(StreamReader reader)
{
    while (true)
    {
        try
        {
            // Считываем ответ в виде строки
            string? message = await reader.ReadLineAsync();
            // Если пустой ответ, ничего не выводим на консоль
            if (string.IsNullOrEmpty(message)) continue;
            Print(message); // Вывод сообщения
        }
        catch
        {
            break;
        }
    }
}
// Чтобы полученное сообщение не накладывалось на ввод нового сообщения
void Print(string message)
{
    if (OperatingSystem.IsWindows())    // Если ОС Windows
    {
        var position = Console.GetCursorPosition(); // Получаем текущую позицию курсора
        int left = position.Left;   // Смещение в символах относительно левого края
        int top = position.Top;     // Смещение в строках относительно верха
        // Копируем ранее введенные символы в строке на следующую строку
        Console.MoveBufferArea(0, top, left, 1, 0, top + 1);
        // Устанавливаем курсор в начало текущей строки
        Console.SetCursorPosition(0, top);
        // В текущей строке выводит полученное сообщение
        Console.WriteLine(message);
        // Переносим курсор на следующую строку
        // И пользователь продолжает ввод уже на следующей строке
        Console.SetCursorPosition(left, top + 1);
    }
    else Console.WriteLine(message);
}