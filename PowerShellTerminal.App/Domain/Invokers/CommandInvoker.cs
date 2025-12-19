using System.Collections.Generic;
using PowerShellTerminal.App.Domain.Interfaces;

namespace PowerShellTerminal.App.Domain.Invokers
{
    public class CommandInvoker
    {
        private readonly List<ICommand> _history = new List<ICommand>();

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            
            _history.Add(command);
        }

        public List<string> GetHistoryLog()
        {
            List<string> log = new List<string>();
            foreach (var cmd in _history)
            {
                log.Add(cmd.GetCommandText());
            }
            return log;
        }
    }
}