using PowerShellTerminal.App.Domain.Bridge;

namespace PowerShellTerminal.App.Domain.Interpreter.Expressions
{
    public class SystemCommandExpression : IExpression
    {
        private readonly TerminalSystem _system; 
        private readonly string _command;

        public SystemCommandExpression(TerminalSystem system, string command)
        {
            _system = system;
            _command = command.Trim();
        }

        public void Interpret(InterpreterContext context)
        {

            context.Output = _system.RunCommand(_command);
        }
    }
}