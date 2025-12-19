using System;
using System.Collections.Generic;

namespace PowerShellTerminal.App.Domain.Entities
{
    public class TerminalSession
    {
        public Guid SessionId { get; set; }
        public string CurrentDirectory { get; set; }
        public bool IsActive { get; set; }

        public TerminalSession()
        {
            SessionId = Guid.NewGuid();
            CurrentDirectory = Environment.CurrentDirectory;
            IsActive = true;
        }

        public void RunCommand(string command)
        {
            Console.WriteLine($"Executing: {command}...");
        }
    }
}