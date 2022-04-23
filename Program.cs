using System.Net;
using System.Net.Sockets;
using System.Text;

Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Unspecified);
s.Bind(new IPEndPoint(IPAddress.Any, 8080));
s.Listen(100);
while (true)
{
    byte[] bytes = new byte[1024];
    Socket c = s.Accept();
    int bytesReceived = c.Receive(bytes);
    string msg = Encoding.UTF8.GetString(bytes);
    if (bytesReceived <= 0)
        Console.WriteLine("No bytes to read");
    else
        Console.WriteLine(msg);
    try
    {
        string body = File.ReadAllText(Directory.GetCurrentDirectory() + msg.Split(' ')[1]);
        c.Send(Encoding.UTF8.GetBytes("HTTP/1.1 200 OK\nContent-Type: text/html\nContent-Length: " + body.Length + "\n\n" + body));
    }
    catch (FileNotFoundException e)
    {
        c.Send(Encoding.UTF8.GetBytes("HTTP/1.1 404 Not Found"));
    }
    c.Close();
}