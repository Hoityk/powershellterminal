using System;
using System.Drawing;
using System.Windows.Forms;
using PowerShellTerminal.App.Domain.Core;
using PowerShellTerminal.App.Domain.Commands;
using PowerShellTerminal.App.Domain.Invokers;
using PowerShellTerminal.App.Domain.Interfaces;
using PowerShellTerminal.App.Domain.AbstractFactory;
using PowerShellTerminal.App.Domain.Bridge;
using PowerShellTerminal.App.Domain.Interpreter;

namespace PowerShellTerminal.App.UI.Controls
{
    public class TerminalControl : UserControl
    {
        private RichTextBox _rtbOutput;
        private TextBox _txtInput;
        private Label _lblPrompt;
        private Panel _bottomPanel;
        private Button _btnSwitchEngine;

        private CommandInvoker _invoker;
        private TerminalSystem _terminalSystem;
        private string _promptStr;

        public TerminalControl(ISessionFactory factory)
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.Black;

            IPrompt prompt = factory.CreatePrompt();
            IHeader header = factory.CreateHeader();
            _promptStr = prompt.GetText();

            _invoker = new CommandInvoker();
            _terminalSystem = new TerminalSystem(new PowerShellEngine());

            Panel topBar = new Panel();
            topBar.Dock = DockStyle.Top;
            topBar.Height = 25;
            topBar.BackColor = Color.FromArgb(30, 30, 30);

            _btnSwitchEngine = new Button();
            _btnSwitchEngine.Text = $"Engine: PS";
            _btnSwitchEngine.Dock = DockStyle.Right;
            _btnSwitchEngine.Width = 120;
            _btnSwitchEngine.FlatStyle = FlatStyle.Flat;
            _btnSwitchEngine.ForeColor = Color.White;
            _btnSwitchEngine.Font = new Font("Consolas", 8);
            _btnSwitchEngine.Click += OnSwitchEngineClick;

            topBar.Controls.Add(_btnSwitchEngine);

            _rtbOutput = new RichTextBox();
            _rtbOutput.Dock = DockStyle.Fill;
            _rtbOutput.BorderStyle = BorderStyle.None;
            _rtbOutput.Font = new Font("Consolas", 12);
            _rtbOutput.ReadOnly = true;
            _rtbOutput.BackColor = Color.Black;
            _rtbOutput.ForeColor = Color.LightGray;
            _rtbOutput.Text = $"{header.GetWelcomeMessage()}\n----------------------------\n";

            _bottomPanel = new Panel();
            _bottomPanel.Dock = DockStyle.Bottom;
            _bottomPanel.Height = 30;
            _bottomPanel.Padding = new Padding(5);
            _bottomPanel.BackColor = Color.Black;

            _lblPrompt = new Label();
            _lblPrompt.Text = _promptStr;
            _lblPrompt.ForeColor = ColorTranslator.FromHtml(prompt.GetColorHex());
            _lblPrompt.Font = new Font("Consolas", 12, FontStyle.Bold);
            _lblPrompt.AutoSize = true;
            _lblPrompt.Dock = DockStyle.Left;
            _lblPrompt.TextAlign = ContentAlignment.MiddleLeft;

            _txtInput = new TextBox();
            _txtInput.BackColor = Color.Black;
            _txtInput.ForeColor = _lblPrompt.ForeColor;
            _txtInput.Font = new Font("Consolas", 12);
            _txtInput.BorderStyle = BorderStyle.None;
            _txtInput.Dock = DockStyle.Fill;
            _txtInput.KeyDown += OnInputKeyDown;

            _bottomPanel.Controls.Add(_txtInput);
            _bottomPanel.Controls.Add(_lblPrompt);

            this.Controls.Add(_rtbOutput);
            this.Controls.Add(_bottomPanel);
            this.Controls.Add(topBar);

            _txtInput.Focus();
        }

        public void ApplyTheme(Color bg, Color fg, Color buttonBg)
        {
            this.BackColor = bg;

            _bottomPanel.BackColor = bg;

            _txtInput.BackColor = bg;
            _txtInput.ForeColor = fg;
            _lblPrompt.ForeColor = fg;

            _rtbOutput.BackColor = bg;
            _rtbOutput.ForeColor = fg;

            _btnSwitchEngine.BackColor = buttonBg;
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

                var parser = new ExpressionParser(_terminalSystem);

                ICommand cmd = new RunScriptCommand(_terminalSystem, text);

                AppendText($"{_promptStr}{text}", _txtInput.ForeColor);
                _invoker.ExecuteCommand(cmd);

                string result = cmd.GetOutput();
                AppendText(result, _rtbOutput.ForeColor);

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