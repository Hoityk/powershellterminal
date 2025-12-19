using System.Linq;
using PowerShellTerminal.App.Domain.Bridge;
using PowerShellTerminal.App.Domain.Interpreter.Expressions;

namespace PowerShellTerminal.App.Domain.Interpreter
{
    public class ExpressionParser
    {
        private readonly TerminalSystem _system;

        public ExpressionParser(TerminalSystem system)
        {
            _system = system;
        }

        public IExpression Parse(string commandLine)
        {
            if (commandLine.Contains("|"))
            {
                var parts = commandLine.Split('|', 2); 
                var leftStr = parts[0];
                var rightStr = parts[1];

                IExpression leftExpr = ParseTerminal(leftStr);
                IExpression rightExpr = ParseTerminal(rightStr);

                return new PipeExpression(leftExpr, rightExpr);
            }
            
            return ParseTerminal(commandLine);
        }

        private IExpression ParseTerminal(string commandPart)
        {
            var trimmed = commandPart.Trim();

            if (trimmed.Equals("upper", System.StringComparison.OrdinalIgnoreCase))
            {
                return new ToUpperExpression();
            }

            return new SystemCommandExpression(_system, trimmed);
        }
    }
}