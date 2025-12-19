using System.Drawing;
using System.Windows.Forms;
using PowerShellTerminal.App.Domain.Interfaces;

namespace PowerShellTerminal.App.Domain.Strategies
{
    public class PowerShellBlueThemeStrategy : IThemeStrategy
    {
        public void Apply(Form form)
        {
            var blueColor = Color.FromArgb(1, 36, 86);
            var whiteColor = Color.White;

            form.BackColor = blueColor;
            form.ForeColor = whiteColor;

            foreach (Control c in form.Controls)
            {
                c.BackColor = blueColor;
                c.ForeColor = whiteColor;
                if (c is Button) c.BackColor = Color.DarkBlue;
            }
        }
    }
}