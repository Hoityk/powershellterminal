using System.Diagnostics;
using System;

namespace PowerShellTerminal.App.Domain.Bridge
{
    public class PowerShellEngine : IShellEngine
    {
        public string GetEngineName() => "PowerShell Core";

        public string Execute(string command)
        {
            return RunProcess("powershell.exe", $"-NoProfile -ExecutionPolicy Bypass -Command \"{command}\"");
        }

        private string RunProcess(string fileName, string args)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = fileName;
                psi.Arguments = args;
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardError = true;
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;

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