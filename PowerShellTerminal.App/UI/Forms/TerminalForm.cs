using System;
using System.Drawing;
using System.Windows.Forms;
using PowerShellTerminal.App.Domain.Commands;
using PowerShellTerminal.App.Domain.Invokers;
using PowerShellTerminal.App.Domain.Interfaces;
using PowerShellTerminal.App.Domain.AbstractFactory;
using PowerShellTerminal.App.Domain.Bridge;

namespace PowerShellTerminal.App.UI.Forms
{
    public class TerminalForm : Form
    {
        private RichTextBox _rtbOutput;
        private TextBox _txtInput;
        private Label _lblPrompt;
        private Panel _bottomPanel;
        private Button _btnSwitchEngine;

        private CommandInvoker _invoker;
        private TerminalSystem _terminalSystem;
        private string _promptStr;
        private Color _themeColor;

        public TerminalForm(ISessionFactory factory)
        {
            IPrompt prompt = factory.CreatePrompt();
            IHeader header = factory.CreateHeader();
            _promptStr = prompt.GetText();
            _themeColor = ColorTranslator.FromHtml(prompt.GetColorHex());

            this.Text = header.GetTitle();
            this.Size = new Size(900, 600);
            this.BackColor = Color.Black;

            _invoker = new CommandInvoker();
            _terminalSystem = new TerminalSystem(new PowerShellEngine());

            _btnSwitchEngine = new Button();
            _btnSwitchEngine.Text = $"Engine: {_terminalSystem.GetCurrentEngineName()}";
            _btnSwitchEngine.Dock = DockStyle.Top;
            _btnSwitchEngine.Height = 30;
            _btnSwitchEngine.BackColor = Color.FromArgb(50, 50, 50);
            _btnSwitchEngine.ForeColor = Color.White;
            _btnSwitchEngine.FlatStyle = FlatStyle.Flat;
            _btnSwitchEngine.Click += OnSwitchEngineClick;

            _rtbOutput = new RichTextBox();
            _rtbOutput.Dock = DockStyle.Fill;
            _rtbOutput.BackColor = Color.Black;
            _rtbOutput.ForeColor = Color.LightGray;
            _rtbOutput.Font = new Font("Consolas", 12);
            _rtbOutput.ReadOnly = true;
            _rtbOutput.BorderStyle = BorderStyle.None;
            _rtbOutput.Text = $"{header.GetWelcomeMessage()}\n----------------------------\n";

            _bottomPanel = new Panel();
            _bottomPanel.Dock = DockStyle.Bottom;
            _bottomPanel.Height = 30;
            _bottomPanel.BackColor = Color.Black;
            _bottomPanel.Padding = new Padding(5);

            _lblPrompt = new Label();
            _lblPrompt.Text = _promptStr;
            _lblPrompt.ForeColor = _themeColor;
            _lblPrompt.Font = new Font("Consolas", 12, FontStyle.Bold);
            _lblPrompt.AutoSize = true;
            _lblPrompt.Dock = DockStyle.Left;
            _lblPrompt.TextAlign = ContentAlignment.MiddleLeft;

            _txtInput = new TextBox();
            _txtInput.BackColor = Color.Black;
            _txtInput.ForeColor = _themeColor;
            _txtInput.Font = new Font("Consolas", 12);
            _txtInput.BorderStyle = BorderStyle.None;
            _txtInput.Dock = DockStyle.Fill;
            _txtInput.KeyDown += OnInputKeyDown;

            _bottomPanel.Controls.Add(_txtInput);
            _bottomPanel.Controls.Add(_lblPrompt);

            this.Controls.Add(_rtbOutput);
            this.Controls.Add(_bottomPanel);
            this.Controls.Add(_btnSwitchEngine);

            this.Shown += (s, e) => _txtInput.Focus();
        }

        private void OnSwitchEngineClick(object? sender, EventArgs e)
        {
            string current = _terminalSystem.GetCurrentEngineName();

            if (current.Contains("PowerShell"))
            {
                _terminalSystem.SetEngine(new CmdEngine());
            }
            else
            {
                _terminalSystem.SetEngine(new PowerShellEngine());
            }

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