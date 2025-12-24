using PowerShellTerminal.App.Domain.Bridge;

namespace PowerShellTerminal.App.Domain.Interpreter.Expressions
{
    public class ChangeDirectoryExpression : IExpression
    {
        private readonly TerminalSystem _system;
        private readonly string _path;

        public ChangeDirectoryExpression(TerminalSystem system, string path)
        {
            _system = system;
            _path = path.Trim();
        }

        public void Interpret(InterpreterContext context)
        {
            string result = _system.ChangeDirectory(_path);
            
            context.Output = result;
        }
    }
}