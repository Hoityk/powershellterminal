using PowerShellTerminal.App.Domain.Interfaces;
using PowerShellTerminal.App.Domain.Bridge;
using PowerShellTerminal.App.Domain.Interpreter;

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
            var parser = new ExpressionParser(_system);
            IExpression expressionTree = parser.Parse(_script);
            var context = new InterpreterContext(string.Empty);
            expressionTree.Interpret(context);
            _lastOutput = context.Output;
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