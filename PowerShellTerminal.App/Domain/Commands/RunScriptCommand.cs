using PowerShellTerminal.App.Domain.Core;
using PowerShellTerminal.App.Domain.Interfaces;

namespace PowerShellTerminal.App.Domain.Commands
{
    public class RunScriptCommand : ICommand
    {
        private readonly PowerShellReceiver _receiver;
        private readonly string _script;
        private string _lastOutput;

        public RunScriptCommand(PowerShellReceiver receiver, string script)
        {
            _receiver = receiver;
            _script = script;
            _lastOutput = string.Empty;
        }

        public void Execute()
        {
            _lastOutput = _receiver.RunProcess(_script);
        }

        public void Undo()
        {

            _lastOutput = "Undo not supported for this command.";
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