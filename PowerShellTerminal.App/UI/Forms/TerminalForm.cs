using System;
using System.Drawing;
using System.Windows.Forms;
using PowerShellTerminal.App.Domain.Entities;
using PowerShellTerminal.App.Domain.AbstractFactory;
using PowerShellTerminal.App.UI.Controls;
using PowerShellTerminal.App.Data;

namespace PowerShellTerminal.App.UI.Forms
{
    public class TerminalForm : Form
    {
        private TabControl _tabControl;
        private Button _btnAddTab;
        private Button _btnSettings;
        private ContextMenuStrip _tabContextMenu;
        private UserProfile _currentUser;
        private ISessionFactory _factory;

        public TerminalForm(ISessionFactory factory, UserProfile user)
        {
            _currentUser = user;
            _factory = factory;

            IHeader header = factory.CreateHeader();
            this.Text = header.GetTitle();
            this.Size = new Size(900, 600);
            this.BackColor = Color.Black;

            _tabContextMenu = new ContextMenuStrip();
            var closeItem = _tabContextMenu.Items.Add("Close Tab");
            closeItem.Click += (s, e) => CloseTab(_tabControl.SelectedIndex);

            Panel topPanel = new Panel();
            topPanel.Dock = DockStyle.Top;
            topPanel.Height = 35;
            topPanel.BackColor = Color.FromArgb(45, 45, 48);

            _btnAddTab = new Button();
            _btnAddTab.Text = " + New Tab ";
            _btnAddTab.Dock = DockStyle.Left;
            _btnAddTab.Width = 100;
            _btnAddTab.FlatStyle = FlatStyle.Flat;
            _btnAddTab.ForeColor = Color.White;
            _btnAddTab.Click += (s, e) => AddNewTab();

            _btnSettings = new Button();
            _btnSettings.Text = "⚙ Settings";
            _btnSettings.Dock = DockStyle.Right;
            _btnSettings.Width = 100;
            _btnSettings.FlatStyle = FlatStyle.Flat;
            _btnSettings.ForeColor = Color.White;
            _btnSettings.Click += OnSettingsClick;

            topPanel.Controls.Add(_btnAddTab);
            topPanel.Controls.Add(_btnSettings);

            _tabControl = new TabControl();
            _tabControl.Dock = DockStyle.Fill;
            _tabControl.Appearance = TabAppearance.Normal;
            _tabControl.Padding = new Point(10, 5);
            _tabControl.MouseClick += OnTabMouseClick;

            this.Controls.Add(_tabControl);
            this.Controls.Add(topPanel);

            AddNewTab();
            ApplyThemeStrategy();
        }

        private void OnTabMouseClick(object? sender, MouseEventArgs e)
        {
            for (int i = 0; i < _tabControl.TabCount; i++)
            {
                Rectangle r = _tabControl.GetTabRect(i);

                if (r.Contains(e.Location))
                {
                    if (e.Button == MouseButtons.Middle)
                    {
                        CloseTab(i);
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        _tabControl.SelectedIndex = i;
                        _tabContextMenu.Show(_tabControl, e.Location);
                    }
                }
            }
        }

        private void CloseTab(int index)
        {
            if (index < 0 || index >= _tabControl.TabCount) return;

            TabPage tabToRemove = _tabControl.TabPages[index];

            foreach (Control c in tabToRemove.Controls)
            {
                c.Dispose();
            }

            _tabControl.TabPages.RemoveAt(index);

            if (_tabControl.TabCount == 0)
            {
                AddNewTab();
            }
        }

        private void AddNewTab()
        {
            TabPage page = new TabPage($"Terminal {_tabControl.TabPages.Count + 1}");

            TerminalControl terminal = new TerminalControl(_factory, _currentUser);
            page.Controls.Add(terminal);

            _tabControl.TabPages.Add(page);
            _tabControl.SelectedTab = page;

            ApplyThemeStrategy();

            terminal.Focus();
        }

        private void OnSettingsClick(object? sender, EventArgs e)
        {
            SettingsForm settings = new SettingsForm(_currentUser);
            settings.ShowDialog();

            using (var db = new AppDbContext())
            {
                var updatedUser = db.UserProfiles.Find(_currentUser.ProfileId);
                if (updatedUser != null)
                {
                    _currentUser = updatedUser;
                }
            }

            ApplyThemeStrategy();
        }

        private void ApplyThemeStrategy()
        {
            Color bg = Color.Black;
            Color fg = Color.White;
            Color btn = Color.Gray;

            switch (_currentUser.ThemeId)
            {
                case 1:
                    bg = Color.Black;
                    fg = Color.LimeGreen;
                    btn = Color.FromArgb(30, 30, 30);
                    break;
                case 2:
                    bg = Color.FromArgb(1, 36, 86);
                    fg = Color.White;
                    btn = Color.DarkBlue;
                    break;
                case 3:
                    bg = Color.FromArgb(48, 10, 36);
                    fg = Color.FromArgb(221, 72, 20);
                    btn = Color.FromArgb(221, 72, 20);
                    break;
            }

            this.BackColor = bg;
            _tabControl.BackColor = bg;

            foreach (TabPage page in _tabControl.TabPages)
            {
                page.BackColor = bg;
                foreach (Control c in page.Controls)
                {
                    if (c is TerminalControl term)
                    {
                        term.ApplyTheme(bg, fg, btn);
                    }
                }
            }
        }
    }
}