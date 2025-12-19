using System.Drawing;
using System.Windows.Forms;
using PowerShellTerminal.App.Domain.Interfaces;

namespace PowerShellTerminal.App.Domain.Strategies
{
    public class MatrixThemeStrategy : IThemeStrategy
    {
        public void Apply(Form form)
        {
            form.BackColor = Color.Black;
            form.ForeColor = Color.LimeGreen;

            foreach (Control c in form.Controls)
            {
                UpdateColorRecursive(c, Color.Black, Color.LimeGreen);
            }
        }

        private void UpdateColorRecursive(Control ctrl, Color bg, Color fg)
        {
            ctrl.BackColor = bg;
            ctrl.ForeColor = fg;
            
            if (ctrl is Button)
            {
                ctrl.BackColor = Color.FromArgb(30, 30, 30);
            }
        }
    }
}