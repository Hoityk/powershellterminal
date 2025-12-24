namespace PowerShellTerminal.App.Domain.Bridge
{
    public interface IShellEngine
    {
        string Execute(string command, string workingDirectory);
        string GetEngineName();
    }
}