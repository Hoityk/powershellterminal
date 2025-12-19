namespace PowerShellTerminal.App.Domain.Bridge
{
    public class TerminalSystem
    {
        protected IShellEngine _engine;

        public TerminalSystem(IShellEngine engine)
        {
            _engine = engine;
        }

        public void SetEngine(IShellEngine engine)
        {
            _engine = engine;
        }

        public string RunCommand(string cmd)
        {
            if (string.IsNullOrWhiteSpace(cmd)) return "";
            
            return _engine.Execute(cmd);
        }

        public string GetCurrentEngineName()
        {
            return _engine.GetEngineName();
        }
    }
}