using System.Text;

namespace PowerShellTerminal.App.Domain.Interpreter.Expressions
{
    public class HelpExpression : IExpression
    {
        public void Interpret(InterpreterContext context)
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== PowerShell Terminal Help ===");
            sb.AppendLine("INTERNAL COMMANDS (C#):");
            sb.AppendLine("  history  - Show command history from DB");
            sb.AppendLine("  upper    - Convert text to UPPERCASE (use with |)");
            sb.AppendLine("  help     - Show this message");
            sb.AppendLine("");
            sb.AppendLine("SUPPORTED SHELL COMMANDS:");
            sb.AppendLine("  Any PowerShell or CMD command (dir, cd, echo, ipconfig...)");
            sb.AppendLine("  Any system executable (git, dotnet, notepad...)");
            sb.AppendLine("================================");
            
            context.Output = sb.ToString();
        }
    }
}