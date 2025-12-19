using System.Diagnostics;
using System;

namespace PowerShellTerminal.App.Domain.Core
{
    public class PowerShellReceiver
    {
        public string RunProcess(string script)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "powershell.exe";
                psi.Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{script}\"";
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardError = true;
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;

                using (Process process = Process.Start(psi))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (!string.IsNullOrEmpty(error))
                        return $"Error: {error}";
                    
                    return output;
                }
            }
            catch (Exception ex)
            {
                return $"Critical Error: {ex.Message}";
            }
        }
    }
}