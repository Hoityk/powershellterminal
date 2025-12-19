using System.Windows.Forms;

namespace PowerShellTerminal.App.Domain.Interfaces
{
    public interface IThemeStrategy
    {
        void Apply(Form form);
    }
}