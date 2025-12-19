namespace PowerShellTerminal.App.Domain.Interpreter.Expressions
{
    public class PipeExpression : IExpression
    {
        private readonly IExpression _left;
        private readonly IExpression _right;

        public PipeExpression(IExpression left, IExpression right)
        {
            _left = left;
            _right = right;
        }

        public void Interpret(InterpreterContext context)
        {
            _left.Interpret(context);

            var pipeContext = new InterpreterContext(context.Output);
            
            pipeContext.Output = context.Output; 

            _right.Interpret(pipeContext);

            context.Output = pipeContext.Output;
        }
    }
}