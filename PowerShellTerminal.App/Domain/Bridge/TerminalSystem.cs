using System;
using System.IO;

namespace PowerShellTerminal.App.Domain.Bridge
{
    public class TerminalSystem
    {
        protected IShellEngine _engine;

        public string CurrentDirectory { get; private set; }

        public TerminalSystem(IShellEngine engine)
        {
            _engine = engine;
            CurrentDirectory = Directory.GetCurrentDirectory();
        }

        public void SetEngine(IShellEngine engine)
        {
            _engine = engine;
        }

        public string ChangeDirectory(string newPath)
        {
            try
            {
                string combinedPath = Path.GetFullPath(Path.Combine(CurrentDirectory, newPath));

                if (Directory.Exists(combinedPath))
                {
                    CurrentDirectory = combinedPath;
                    return string.Empty;
                }
                else
                {
                    return $"Error: Path '{combinedPath}' not found.";
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public string RunCommand(string cmd)
        {
            if (string.IsNullOrWhiteSpace(cmd)) return "";

            return _engine.Execute(cmd, CurrentDirectory);
        }

        public string GetCurrentEngineName()
        {
            return _engine.GetEngineName();
        }
    }
}