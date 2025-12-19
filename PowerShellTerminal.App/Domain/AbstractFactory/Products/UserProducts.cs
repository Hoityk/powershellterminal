namespace PowerShellTerminal.App.Domain.AbstractFactory.Products
{
    public class UserPrompt : IPrompt
    {
        public string GetText() => "PS User $ > ";
        public string GetColorHex() => "#00FF00"; 
    }

    public class UserHeader : IHeader
    {
        public string GetTitle() => "PowerShell Terminal [User Mode]";
        public string GetWelcomeMessage() => "Welcome back! Type 'help' for commands.";
    }
}