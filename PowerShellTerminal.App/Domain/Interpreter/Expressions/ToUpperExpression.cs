namespace PowerShellTerminal.App.Domain.Interpreter.Expressions
{
    public class ToUpperExpression : IExpression
    {
        public void Interpret(InterpreterContext context)
        {

            string textToProcess = !string.IsNullOrEmpty(context.Output) ? context.Output : context.Input;
            
            context.Output = textToProcess.ToUpper();
        }
    }
}