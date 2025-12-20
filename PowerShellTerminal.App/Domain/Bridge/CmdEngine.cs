using System.Diagnostics;
using System;
using System.Text;

namespace PowerShellTerminal.App.Domain.Bridge
{
    public class CmdEngine : IShellEngine
    {
        public string GetEngineName() => "Windows CMD";

        public string Execute(string command)
        {
            // Просто запускаємо cmd, не чіпаючи chcp
            return RunProcess("cmd.exe", command);
        }

        private string RunProcess(string fileName, string commandText)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = fileName;

                // ВИПРАВЛЕННЯ:
                // 1. Прибираємо "chcp 65001".
                // 2. "/C" виконує команду і закривається.
                psi.Arguments = $"/C {commandText}";

                psi.RedirectStandardOutput = true;
                psi.RedirectStandardError = true;
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;

                // ВИПРАВЛЕННЯ:
                // Читаємо у "рідному" кодуванні Windows CMD (CP 866 - Кирилиця DOS)
                // Завдяки тому, що ти додав RegisterProvider у Program.cs, це спрацює.
                psi.StandardOutputEncoding = Encoding.GetEncoding(866);
                psi.StandardErrorEncoding = Encoding.GetEncoding(866);

                using (Process process = Process.Start(psi))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    // Якщо є помилка, повертаємо її
                    if (!string.IsNullOrEmpty(error)) return $"[CMD Error] {error}";
                    
                    return output;
                }
            }
            catch (Exception ex) { return $"CRITICAL: {ex.Message}"; }
        }
    }
}