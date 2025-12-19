namespace PowerShellTerminal.App.Domain.Interpreter
{
    public class InterpreterContext
    {
        public string Input { get; set; } = string.Empty;
        
        public string Output { get; set; } = string.Empty;

        public InterpreterContext(string input)
        {
            Input = input;
        }
    }
}