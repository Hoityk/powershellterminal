using System.Diagnostics;
using System;
using System.Text;

namespace PowerShellTerminal.App.Domain.Bridge
{
    public class PowerShellEngine : IShellEngine
    {
        public string GetEngineName() => "PowerShell Core";

        public string Execute(string command, string workingDirectory)
        {
            return RunProcess("powershell.exe", command, workingDirectory);
        }

        private string RunProcess(string fileName, string commandText, string workingDir)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = fileName;
                psi.WorkingDirectory = workingDir;

                string psCommand = $"[Console]::OutputEncoding = [System.Text.Encoding]::UTF8; {commandText}";

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

                    if (!string.IsNullOrEmpty(error)) return $"[PS Error] {error}";
                    return output;
                }
            }
            catch (Exception ex) { return $"CRITICAL: {ex.Message}"; }
        }
    }
}