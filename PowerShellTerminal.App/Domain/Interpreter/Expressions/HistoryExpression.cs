using System.Text;
using PowerShellTerminal.App.Data;
using PowerShellTerminal.App.Domain.Interpreter;

namespace PowerShellTerminal.App.Domain.Interpreter.Expressions
{
    public class HistoryExpression : IExpression
    {
        private readonly int _userId;

        public HistoryExpression(int userId)
        {
            _userId = userId;
        }

        public void Interpret(InterpreterContext context)
        {
            var sb = new StringBuilder();
            sb.AppendLine("--- COMMAND HISTORY (FROM DB) ---");

            var repo = new HistoryRepository();
            var items = repo.GetAll(); 

            var userItems = items.Where(x => x.ProfileId == _userId).OrderBy(x => x.ExecutedAt).ToList();

            foreach (var item in userItems)
            {
                string status = item.IsSuccess ? "[OK]" : "[ERR]";
                sb.AppendLine($"{item.ExecutedAt:HH:mm:ss} {status}: {item.CommandText}");
            }

            sb.AppendLine("---------------------------------");
            
            context.Output = sb.ToString();
        }
    }
}