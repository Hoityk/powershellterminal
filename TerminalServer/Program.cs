using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        int port = 5000;
        TcpListener server = new TcpListener(IPAddress.Any, port);
        server.Start();

        Console.WriteLine($"[SERVER] Running on port {port}. Waiting for commands...");

        while (true)
        {
            try
            {
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("[SERVER] Client connected!");

                Thread t = new Thread(() => HandleClient(client));
                t.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SERVER ERROR] {ex.Message}");
            }
        }
    }

    static void HandleClient(TcpClient client)
    {
        try
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[4096];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            
            string command = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"[CMD] Received: {command}");

            string output = ExecutePowerShell(command);

            byte[] responseBytes = Encoding.UTF8.GetBytes(output);
            stream.Write(responseBytes, 0, responseBytes.Length);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Handler Error] {ex.Message}");
        }
        finally
        {
            client.Close();
        }
    }

    static string ExecutePowerShell(string command)
    {
        try
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "powershell.exe";
            string psCommand = $"[Console]::OutputEncoding = [System.Text.Encoding]::UTF8; {command}";
            psi.Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{psCommand}\"";
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.StandardOutputEncoding = Encoding.UTF8;
            psi.StandardErrorEncoding = Encoding.UTF8;

            using (Process process = Process.Start(psi))
            {
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (!string.IsNullOrEmpty(error)) return $"[Remote Error] {error}";
                return output;
            }
        }
        catch (Exception ex)
        {
            return $"CRITICAL SERVER ERROR: {ex.Message}";
        }
    }
}