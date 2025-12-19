using System;
using System.Linq;
using System.Windows.Forms;
using PowerShellTerminal.App.Data;
using PowerShellTerminal.App.Domain.Entities;
using PowerShellTerminal.App.UI.Forms;

namespace PowerShellTerminal.App
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (var db = new AppDbContext())
            {
                db.Database.EnsureCreated();

                if (!db.Themes.Any())
                {
                    db.Themes.AddRange(
                        new Theme 
                        { 
                            ThemeId = 1, 
                            ThemeName = "Dark Matrix", 
                            BackgroundColor = "#000000", 
                            ForegroundColor = "#00FF00", 
                            CursorColor = "#00FF00" 
                        },
                        new Theme 
                        { 
                            ThemeId = 2, 
                            ThemeName = "PowerShell Blue", 
                            BackgroundColor = "#012456", 
                            ForegroundColor = "#FFFFFF", 
                            CursorColor = "#FFFFFF" 
                        },
                        new Theme 
                        { 
                            ThemeId = 3, 
                            ThemeName = "Ubuntu Purple", 
                            BackgroundColor = "#300A24", 
                            ForegroundColor = "#DD4814", 
                            CursorColor = "#FFFFFF" 
                        }
                    );
                    db.SaveChanges();
                }
            }

            Application.EnableVisualStyles();
            
            LoginForm loginForm = new LoginForm();
            
            if (loginForm.ShowDialog() == DialogResult.OK && loginForm.LoggedInUser != null)
            {
                var user = loginForm.LoggedInUser;
                SettingsForm settingsForm = new SettingsForm(user);
                settingsForm.ShowDialog();
            }
        }
    }
}