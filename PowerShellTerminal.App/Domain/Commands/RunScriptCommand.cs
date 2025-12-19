using PowerShellTerminal.App.Domain.Interfaces;
using PowerShellTerminal.App.Domain.Bridge;

namespace PowerShellTerminal.App.Domain.Commands
{
    public class RunScriptCommand : ICommand
    {
        private readonly TerminalSystem _system;
        private readonly string _script;
        private string _lastOutput;

        public RunScriptCommand(TerminalSystem system, string script)
        {
            _system = system;
            _script = script;
            _lastOutput = string.Empty;
        }

        public void Execute()
        {
            _lastOutput = _system.RunCommand(_script);
        }

        public void Undo()
        {
            _lastOutput = "Undo not supported.";
        }

        public string GetOutput()
        {
            return _lastOutput;
        }

        public string GetCommandText()
        {
            return _script;
        }
    }
}