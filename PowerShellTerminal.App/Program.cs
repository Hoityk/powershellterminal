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
                    db.Themes.Add(new Theme 
                    { 
                        ThemeId = 1, 
                        ThemeName = "Default",
                        BackgroundColor = "#000000",
                        ForegroundColor = "#FFFFFF",
                        CursorColor = "#FFFFFF"
                    });
                    db.SaveChanges();
                }
                // -----------------------------------------------------
            }

            Application.EnableVisualStyles();
            
            // 2. Запуск Форми
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