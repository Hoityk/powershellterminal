namespace PowerShellTerminal.App.Domain.Interfaces
{
    public interface ICommand
    {
        void Execute();
        void Undo(); 
        string GetOutput(); 
        string GetCommandText(); 
    }
}