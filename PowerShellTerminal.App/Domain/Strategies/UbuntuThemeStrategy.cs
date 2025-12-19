using System.Drawing;
using System.Windows.Forms;
using PowerShellTerminal.App.Domain.Interfaces;

namespace PowerShellTerminal.App.Domain.Strategies
{
    public class UbuntuThemeStrategy : IThemeStrategy
    {
        public void Apply(Form form)
        {
                var ubuntuPurple = Color.FromArgb(48, 10, 36);
            var ubuntuOrange = Color.FromArgb(221, 72, 20);

            form.BackColor = ubuntuPurple;
            form.ForeColor = ubuntuOrange;

            foreach (Control c in form.Controls)
            {
                c.BackColor = ubuntuPurple;
                c.ForeColor = Color.White; 
                if (c is Button) c.BackColor = ubuntuOrange;
            }
        }
    }
}