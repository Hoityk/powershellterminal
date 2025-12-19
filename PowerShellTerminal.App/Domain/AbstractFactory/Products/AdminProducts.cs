namespace PowerShellTerminal.App.Domain.AbstractFactory.Products
{
    public class AdminPrompt : IPrompt
    {
        public string GetText() => "PS ADMINISTRATOR # > ";
        public string GetColorHex() => "#FF0000"; 
    }

    public class AdminHeader : IHeader
    {
        public string GetTitle() => "ADMINISTRATOR: PowerShell Terminal";
        public string GetWelcomeMessage() => "*** WARNING: YOU HAVE ELEVATED PRIVILEGES ***";
    }
}