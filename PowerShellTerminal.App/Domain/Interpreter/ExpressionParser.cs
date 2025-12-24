using System.Linq;
using PowerShellTerminal.App.Domain.Bridge;
using PowerShellTerminal.App.Domain.Interpreter.Expressions;
using PowerShellTerminal.App.Domain.Entities;

namespace PowerShellTerminal.App.Domain.Interpreter
{
    public class ExpressionParser
    {
        private readonly TerminalSystem _system;
        private readonly UserProfile _user;

        public ExpressionParser(TerminalSystem system, UserProfile user)
        {
            _system = system;
            _user = user;
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
                return new ToUpperExpression();

            if (trimmed.Equals("history", System.StringComparison.OrdinalIgnoreCase))
                return new HistoryExpression(_user.ProfileId);

            if (trimmed.Equals("help", System.StringComparison.OrdinalIgnoreCase))
                return new HelpExpression();

            return new SystemCommandExpression(_system, trimmed);
        }
    }
}