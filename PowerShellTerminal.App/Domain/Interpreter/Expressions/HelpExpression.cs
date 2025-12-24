using System.Text;

namespace PowerShellTerminal.App.Domain.Interpreter.Expressions
{
    public class HelpExpression : IExpression
    {
        public void Interpret(InterpreterContext context)
        {
            var sb = new StringBuilder();
            sb.AppendLine("========================================================");
            sb.AppendLine("           POWERSHELL TERMINAL [PRO] v1.0               ");
            sb.AppendLine("========================================================");
            sb.AppendLine("");
            sb.AppendLine("[ INTERNAL COMMANDS (C# INTERPRETER) ]");
            sb.AppendLine("  cd <path>      : Змінити поточну папку (підтримує ..)");
            sb.AppendLine("  history        : Показати історію команд (з SQLite БД)");
            sb.AppendLine("  help           : Показати цю довідку");
            sb.AppendLine("  <cmd> | upper  : Конвертувати вивід у ВЕРХНІЙ РЕГІСТР");
            sb.AppendLine("");
            sb.AppendLine("[ KEYBOARD SHORTCUTS ]");
            sb.AppendLine("  UP / DOWN      : Навігація по історії команд");
            sb.AppendLine("  ENTER          : Виконати команду");
            sb.AppendLine("");
            sb.AppendLine("[ UI FEATURES ]");
            sb.AppendLine("  Live Syntax    : Жива підсвітка ключових слів (Gold/Blue)");
            sb.AppendLine("  Tabs           : Підтримка декількох вкладок з ізольованим станом");
            sb.AppendLine("  Themes         : Зміна тем (Settings -> Matrix / Ubuntu / Blue)");
            sb.AppendLine("");
            sb.AppendLine("[ ENGINES (BRIDGE PATTERN) ]");
            sb.AppendLine("  1. PowerShell  : Стандартний режим (.NET Core)");
            sb.AppendLine("  2. CMD         : Режим командного рядка Windows");
            sb.AppendLine("  3. Remote      : Відправка команд на TCP сервер (порт 5000)");
            sb.AppendLine("");
            sb.AppendLine("========================================================");
            
            context.Output = sb.ToString();
        }
    }
}