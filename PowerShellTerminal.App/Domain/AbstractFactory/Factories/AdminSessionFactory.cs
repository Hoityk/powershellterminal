using PowerShellTerminal.App.Domain.AbstractFactory.Products;

namespace PowerShellTerminal.App.Domain.AbstractFactory.Factories
{
    public class AdminSessionFactory : ISessionFactory
    {
        public IPrompt CreatePrompt()
        {
            return new AdminPrompt();
        }

        public IHeader CreateHeader()
        {
            return new AdminHeader();
        }
    }
}