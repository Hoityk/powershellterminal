namespace PowerShellTerminal.App.Domain.AbstractFactory
{
    public interface IPrompt
    {
        string GetText();
        string GetColorHex(); 
    }

    public interface IHeader
    {
        string GetTitle();
        string GetWelcomeMessage();
    }

    public interface ISessionFactory
    {
        IPrompt CreatePrompt();
        IHeader CreateHeader();
    }
}