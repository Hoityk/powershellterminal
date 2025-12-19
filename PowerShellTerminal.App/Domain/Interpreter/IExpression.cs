namespace PowerShellTerminal.App.Domain.Interpreter
{
    public interface IExpression
    {
        void Interpret(InterpreterContext context);
    }
}