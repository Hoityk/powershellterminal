using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PowerShellTerminal.App.Data;
using PowerShellTerminal.App.Domain.Entities;

namespace PowerShellTerminal.App.UI.Forms
{
    public class LoginForm : Form
    {
        private TextBox _txtUsername;
        private Button _btnLogin;
        public UserProfile? LoggedInUser { get; private set; }

        public LoginForm()
        {
            this.Text = "Вхід у термінал";
            this.Size = new Size(300, 150);
            this.StartPosition = FormStartPosition.CenterScreen;

            var lblName = new Label() { Text = "Ім'я профілю:", Top = 20, Left = 20, Width = 240 };
            _txtUsername = new TextBox() { Top = 45, Left = 20, Width = 240 };
            _btnLogin = new Button() { Text = "Увійти / Створити", Top = 75, Left = 20, Width = 240, BackColor = Color.LightGray };
            _btnLogin.Click += OnLoginClick;

            this.Controls.Add(lblName);
            this.Controls.Add(_txtUsername);
            this.Controls.Add(_btnLogin);
        }

        private void OnLoginClick(object? sender, EventArgs e)
        {
            var name = _txtUsername.Text;
            if (string.IsNullOrWhiteSpace(name)) return;

            using (var db = new AppDbContext())
            {
                var user = db.UserProfiles.FirstOrDefault(u => u.ProfileName == name);
                
                if (user == null)
                {
                    user = new UserProfile { ProfileName = name, CreatedAt = DateTime.Now, ThemeId = 1 };
                    db.UserProfiles.Add(user);
                    db.SaveChanges(); 
                    MessageBox.Show("Створено новий профіль!");
                }
                else
                {
                    MessageBox.Show($"З поверненням, {user.ProfileName}!");
                }

                LoggedInUser = user;
                this.DialogResult = DialogResult.OK; 
            }
        }
    }
}