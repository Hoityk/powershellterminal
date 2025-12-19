using PowerShellTerminal.App.Domain.AbstractFactory.Products;

namespace PowerShellTerminal.App.Domain.AbstractFactory.Factories
{
    public class UserSessionFactory : ISessionFactory
    {
        public IPrompt CreatePrompt()
        {
            return new UserPrompt();
        }

        public IHeader CreateHeader()
        {
            return new UserHeader();
        }
    }
}