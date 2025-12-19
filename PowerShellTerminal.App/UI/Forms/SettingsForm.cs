using System;
using System.Drawing;
using System.Windows.Forms;
using PowerShellTerminal.App.Data;
using PowerShellTerminal.App.Domain.Entities;

namespace PowerShellTerminal.App.UI.Forms
{
    public class SettingsForm : Form
    {
        private UserProfile _user;
        private ComboBox _cmbThemes;
        private Button _btnSave;

        public SettingsForm(UserProfile user)
        {
            _user = user;
            this.Text = $"Налаштування: {_user.ProfileName}";
            this.Size = new Size(300, 200);
            this.StartPosition = FormStartPosition.CenterScreen;

            var lblTheme = new Label() { Text = "Оберіть тему:", Top = 20, Left = 20 };
            
            _cmbThemes = new ComboBox() { Top = 45, Left = 20, Width = 240 };
            _cmbThemes.Items.AddRange(new object[] { "Dark Matrix", "PowerShell Blue", "Ubuntu Purple" });
            _cmbThemes.SelectedIndex = 0; 

            _btnSave = new Button() { Text = "Зберегти в БД", Top = 80, Left = 20, Width = 240, BackColor = Color.LightBlue };
            _btnSave.Click += OnSaveClick;

            this.Controls.Add(lblTheme);
            this.Controls.Add(_cmbThemes);
            this.Controls.Add(_btnSave);
        }

        private void OnSaveClick(object? sender, EventArgs e)
        {
            using (var db = new AppDbContext())
            {
                var userToUpdate = db.UserProfiles.Find(_user.ProfileId);
                if (userToUpdate != null)
                {

                    userToUpdate.CreatedAt = DateTime.Now; 
                    
                    db.SaveChanges(); 
                    MessageBox.Show($"Тему '{_cmbThemes.SelectedItem}' збережено для {userToUpdate.ProfileName}!");
                }
            }
            this.Close();
        }
    }
}