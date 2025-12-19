using System.Windows.Forms;
using PowerShellTerminal.App.Domain.Interfaces;
using PowerShellTerminal.App.Domain.Strategies;

namespace PowerShellTerminal.App.Domain.Context
{
    public class ThemeContext
    {
        private IThemeStrategy _strategy;

        public ThemeContext()
        {
            _strategy = new MatrixThemeStrategy();
        }

        public void SetStrategy(IThemeStrategy strategy)
        {
            _strategy = strategy;
        }

        public void ApplyTheme(Form form)
        {
            _strategy.Apply(form);
        }
    }
}