using System.Net.Sockets;
using System.Text;

namespace PowerShellTerminal.App.Domain.Bridge
{

    public class RemoteEngine : IShellEngine
    {
        private const string SERVER_IP = "127.0.0.1"; 
        private const int SERVER_PORT = 5000;

        public string GetEngineName() => "Remote Server (TCP)";

        public string Execute(string command)
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    client.Connect(SERVER_IP, SERVER_PORT);
                    NetworkStream stream = client.GetStream();

                    byte[] data = Encoding.UTF8.GetBytes(command);
                    stream.Write(data, 0, data.Length);

                    byte[] buffer = new byte[8192]; 
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    return response;
                }
            }
            catch (SocketException)
            {
                return "[Network Error] Не вдалося підключитися до сервера. Запустіть TerminalServer.exe!";
            }
            catch (Exception ex)
            {
                return $"[Client Error] {ex.Message}";
            }
        }
    }
}