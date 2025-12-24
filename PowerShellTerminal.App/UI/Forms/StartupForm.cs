using System;
using System.Drawing;
using System.Windows.Forms;
using PowerShellTerminal.App.Domain.Entities;

namespace PowerShellTerminal.App.UI.Forms
{
    public class StartupForm : Form
    {
        public UserProfile? SelectedUser { get; private set; }

        public StartupForm()
        {
            this.Text = "PowerShell Terminal Launcher";
            this.Size = new Size(400, 250);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(30, 30, 30); 

            var lblTitle = new Label()
            {
                Text = "Оберіть режим запуску:",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Top = 30,
                Left = 90
            };

            var btnAdmin = CreateButton("Запуск як ADMIN", Color.Crimson, 70);
            btnAdmin.Click += (s, e) => SelectRole("Admin");

            var btnUser = CreateButton("Запуск як USER", Color.SeaGreen, 130);
            btnUser.Click += (s, e) => SelectRole("User");

            this.Controls.Add(lblTitle);
            this.Controls.Add(btnAdmin);
            this.Controls.Add(btnUser);
        }

        private Button CreateButton(string text, Color color, int top)
        {
            return new Button()
            {
                Text = text,
                Top = top,
                Left = 50,
                Width = 280,
                Height = 45,
                FlatStyle = FlatStyle.Flat,
                BackColor = color,
                ForeColor = Color.White,
                Font = new Font("Consolas", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
        }

        private void SelectRole(string role)
        {

            SelectedUser = new UserProfile 
            { 
                ProfileName = role.ToLower(), 
                Role = role 
            };
            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}