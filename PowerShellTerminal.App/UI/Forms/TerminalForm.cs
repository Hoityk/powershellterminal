using System;
using System.Drawing;
using System.Windows.Forms;
using PowerShellTerminal.App.Domain.Commands;
using PowerShellTerminal.App.Domain.Invokers;
using PowerShellTerminal.App.Domain.Interfaces;
using PowerShellTerminal.App.Domain.AbstractFactory;
using PowerShellTerminal.App.Domain.Bridge;
using PowerShellTerminal.App.Domain.Entities;
using PowerShellTerminal.App.Domain.Context;
using PowerShellTerminal.App.Domain.Strategies;

namespace PowerShellTerminal.App.UI.Forms
{
    public class TerminalForm : Form
    {
        private RichTextBox _rtbOutput;
        private TextBox _txtInput;
        private Label _lblPrompt;
        private Panel _bottomPanel;
        private Button _btnSwitchEngine;
        private Button _btnSettings;

        private CommandInvoker _invoker;
        private TerminalSystem _terminalSystem;
        private string _promptStr;
        private UserProfile _currentUser;

        public TerminalForm(ISessionFactory factory, UserProfile user)
        {
            _currentUser = user;

            IPrompt prompt = factory.CreatePrompt();
            IHeader header = factory.CreateHeader();
            _promptStr = prompt.GetText();

            this.Text = header.GetTitle();
            this.Size = new Size(900, 600);

            _invoker = new CommandInvoker();
            _terminalSystem = new TerminalSystem(new PowerShellEngine());

            Panel topPanel = new Panel();
            topPanel.Dock = DockStyle.Top;
            topPanel.Height = 30;
            topPanel.BackColor = Color.FromArgb(40, 40, 40);

            _btnSwitchEngine = new Button();
            _btnSwitchEngine.Text = $"Engine: PS";
            _btnSwitchEngine.Dock = DockStyle.Left;
            _btnSwitchEngine.Width = 150;
            _btnSwitchEngine.FlatStyle = FlatStyle.Flat;
            _btnSwitchEngine.ForeColor = Color.White;
            _btnSwitchEngine.Click += OnSwitchEngineClick;

            _btnSettings = new Button();
            _btnSettings.Text = "⚙ Settings (Strategy)";
            _btnSettings.Dock = DockStyle.Right;
            _btnSettings.Width = 150;
            _btnSettings.FlatStyle = FlatStyle.Flat;
            _btnSettings.ForeColor = Color.White;
            _btnSettings.Click += OnSettingsClick;

            topPanel.Controls.Add(_btnSwitchEngine);
            topPanel.Controls.Add(_btnSettings);

            _rtbOutput = new RichTextBox();
            _rtbOutput.Dock = DockStyle.Fill;
            _rtbOutput.BorderStyle = BorderStyle.None;
            _rtbOutput.Font = new Font("Consolas", 12);
            _rtbOutput.ReadOnly = true;
            _rtbOutput.Text = $"{header.GetWelcomeMessage()}\n----------------------------\n";

            _bottomPanel = new Panel();
            _bottomPanel.Dock = DockStyle.Bottom;
            _bottomPanel.Height = 30;
            _bottomPanel.Padding = new Padding(5);

            _lblPrompt = new Label();
            _lblPrompt.Text = _promptStr;
            _lblPrompt.Font = new Font("Consolas", 12, FontStyle.Bold);
            _lblPrompt.AutoSize = true;
            _lblPrompt.Dock = DockStyle.Left;
            _lblPrompt.TextAlign = ContentAlignment.MiddleLeft;

            _txtInput = new TextBox();
            _txtInput.Font = new Font("Consolas", 12);
            _txtInput.BorderStyle = BorderStyle.None;
            _txtInput.Dock = DockStyle.Fill;
            _txtInput.KeyDown += OnInputKeyDown;

            _bottomPanel.Controls.Add(_txtInput);
            _bottomPanel.Controls.Add(_lblPrompt);

            this.Controls.Add(_rtbOutput);
            this.Controls.Add(_bottomPanel);
            this.Controls.Add(topPanel);

            ApplyThemeStrategy();

            this.Shown += (s, e) => _txtInput.Focus();
        }

        private void OnSettingsClick(object? sender, EventArgs e)
        {
            SettingsForm settings = new SettingsForm(_currentUser);
            settings.ShowDialog();
            ApplyThemeStrategy();
        }

        private void ApplyThemeStrategy()
        {
            IThemeStrategy strategy = new MatrixThemeStrategy();

            switch (_currentUser.ThemeId)
            {
                case 1: strategy = new MatrixThemeStrategy(); break;
                case 2: strategy = new PowerShellBlueThemeStrategy(); break;
                case 3: strategy = new UbuntuThemeStrategy(); break;
            }

            ThemeContext context = new ThemeContext();
            context.SetStrategy(strategy);
            context.ApplyTheme(this);

            _rtbOutput.BackColor = this.BackColor;
            _rtbOutput.ForeColor = this.ForeColor;

            _bottomPanel.BackColor = this.BackColor;
            _txtInput.BackColor = this.BackColor;
            _txtInput.ForeColor = this.ForeColor;
        }

        private void OnSwitchEngineClick(object? sender, EventArgs e)
        {
            string current = _terminalSystem.GetCurrentEngineName();
            if (current.Contains("PowerShell"))
                _terminalSystem.SetEngine(new CmdEngine());
            else
                _terminalSystem.SetEngine(new PowerShellEngine());

            _btnSwitchEngine.Text = $"Engine: {_terminalSystem.GetCurrentEngineName()}";
            _txtInput.Focus();
        }

        private void OnInputKeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string text = _txtInput.Text;
                if (string.IsNullOrWhiteSpace(text)) return;

                ICommand cmd = new RunScriptCommand(_terminalSystem, text);

                AppendText($"{_promptStr}{text}", _txtInput.ForeColor);
                _invoker.ExecuteCommand(cmd);

                string result = cmd.GetOutput();
                AppendText(result, Color.White);

                _txtInput.Clear();
                e.SuppressKeyPress = true;
            }
        }

        private void AppendText(string text, Color color)
        {
            _rtbOutput.SelectionStart = _rtbOutput.TextLength;
            _rtbOutput.SelectionLength = 0;
            _rtbOutput.SelectionColor = color;
            _rtbOutput.AppendText(text + Environment.NewLine);
            _rtbOutput.ScrollToCaret();
        }
    }
}