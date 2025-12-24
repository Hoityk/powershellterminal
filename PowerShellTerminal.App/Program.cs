using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerShellTerminal.App.Data;
using PowerShellTerminal.App.Domain.Entities;
using PowerShellTerminal.App.UI.Forms;
using PowerShellTerminal.App.Domain.AbstractFactory;
using PowerShellTerminal.App.Domain.AbstractFactory.Factories;
using Microsoft.EntityFrameworkCore;

namespace PowerShellTerminal.App
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.OutputEncoding = Encoding.UTF8;
            Application.EnableVisualStyles();

            using (var db = new AppDbContext())
            {
                db.Database.EnsureCreated();

                if (!db.Themes.Any())
                {
                    db.Themes.AddRange(
                        new Theme { ThemeId = 1, ThemeName = "Dark Matrix", BackgroundColor = "#000000", ForegroundColor = "#00FF00", CursorColor = "#00FF00" },
                        new Theme { ThemeId = 2, ThemeName = "PowerShell Blue", BackgroundColor = "#012456", ForegroundColor = "#FFFFFF", CursorColor = "#FFFFFF" },
                        new Theme { ThemeId = 3, ThemeName = "Ubuntu Purple", BackgroundColor = "#300A24", ForegroundColor = "#DD4814", CursorColor = "#FFFFFF" }
                    );
                    db.SaveChanges();
                }

                if (!db.UserProfiles.Any(u => u.Role == "Admin"))
                {
                    db.UserProfiles.Add(new UserProfile { ProfileName = "admin", Role = "Admin", CreatedAt = DateTime.Now, ThemeId = 1 });
                }

                if (!db.UserProfiles.Any(u => u.Role == "User"))
                {
                    db.UserProfiles.Add(new UserProfile { ProfileName = "user", Role = "User", CreatedAt = DateTime.Now, ThemeId = 2 });
                }

                db.SaveChanges();
            }

            StartupForm startup = new StartupForm();

            if (startup.ShowDialog() == DialogResult.OK && startup.SelectedUser != null)
            {
                string selectedRole = startup.SelectedUser.Role;

                UserProfile realUser;
                using (var db = new AppDbContext())
                {
                    realUser = db.UserProfiles
                        .Include(u => u.Theme)
                        .First(u => u.Role == selectedRole);
                }

                ISessionFactory factory;
                if (realUser.Role == "Admin")
                {
                    factory = new AdminSessionFactory();
                }
                else
                {
                    factory = new UserSessionFactory();
                }

                TerminalForm terminal = new TerminalForm(factory, realUser);
                Application.Run(terminal);
            }
        }
    }
}