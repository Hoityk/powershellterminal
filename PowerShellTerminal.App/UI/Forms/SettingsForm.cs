using System;
using System.Drawing;
using System.Windows.Forms;
using PowerShellTerminal.App.Data;
using PowerShellTerminal.App.Domain.Entities;
using PowerShellTerminal.App.Domain.Context;
using PowerShellTerminal.App.Domain.Strategies;
using PowerShellTerminal.App.Domain.Interfaces;

namespace PowerShellTerminal.App.UI.Forms
{
    public class SettingsForm : Form
    {
        private UserProfile _user;
        private ComboBox _cmbThemes;
        private Button _btnSave;
        private ThemeContext _themeContext;

        public SettingsForm(UserProfile user)
        {
            _user = user;
            _themeContext = new ThemeContext();

            this.Text = $"Налаштування: {_user.ProfileName}";
            this.Size = new Size(350, 250);
            this.StartPosition = FormStartPosition.CenterScreen;

            var lblTheme = new Label() { Text = "Оберіть тему:", Top = 20, Left = 20 };

            _cmbThemes = new ComboBox() { Top = 45, Left = 20, Width = 240 };
            _cmbThemes.Items.AddRange(new object[] { "Dark Matrix", "PowerShell Blue", "Ubuntu Purple" });

            int savedIndex = _user.ThemeId - 1;

            if (savedIndex < 0 || savedIndex >= _cmbThemes.Items.Count)
            {
                savedIndex = 0;
            }

            _cmbThemes.SelectedIndexChanged += OnThemeChanged;
            _cmbThemes.SelectedIndex = savedIndex;

            _btnSave = new Button() { Text = "Зберегти та Застосувати", Top = 90, Left = 20, Width = 240, Height = 40 };
            _btnSave.Click += OnSaveClick;

            this.Controls.Add(lblTheme);
            this.Controls.Add(_cmbThemes);
            this.Controls.Add(_btnSave);
        }

        private void OnThemeChanged(object? sender, EventArgs e)
        {
            IThemeStrategy strategy = new MatrixThemeStrategy();

            switch (_cmbThemes.SelectedIndex)
            {
                case 0: strategy = new MatrixThemeStrategy(); break;
                case 1: strategy = new PowerShellBlueThemeStrategy(); break;
                case 2: strategy = new UbuntuThemeStrategy(); break;
                default: strategy = new MatrixThemeStrategy(); break;
            }

            _themeContext.SetStrategy(strategy);
            _themeContext.ApplyTheme(this);
        }

        private void OnSaveClick(object? sender, EventArgs e)
        {
            using (var db = new AppDbContext())
            {
                var userToUpdate = db.UserProfiles.Find(_user.ProfileId);
                if (userToUpdate != null)
                {
                    userToUpdate.ThemeId = _cmbThemes.SelectedIndex + 1;
                    db.SaveChanges();
                    MessageBox.Show("Налаштування збережено!");
                }
            }
            this.Close();
        }
    }
}