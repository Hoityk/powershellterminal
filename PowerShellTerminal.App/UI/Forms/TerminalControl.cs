using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using PowerShellTerminal.App.Domain.Core;
using PowerShellTerminal.App.Domain.Commands;
using PowerShellTerminal.App.Domain.Invokers;
using PowerShellTerminal.App.Domain.Interfaces;
using PowerShellTerminal.App.Domain.AbstractFactory;
using PowerShellTerminal.App.Domain.Bridge;
using PowerShellTerminal.App.Domain.Interpreter;
using PowerShellTerminal.App.Data;
using PowerShellTerminal.App.Domain.Entities;

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
        private UserProfile _currentUser;
        private HistoryRepository _historyRepository;
        private List<string> _localHistory = new List<string>();
        private int _historyIndex = 0;

        public TerminalControl(ISessionFactory factory, UserProfile user)
        {
            _currentUser = user;
            _historyRepository = new HistoryRepository();

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
            _btnSwitchEngine.Text = $"Engine: {_terminalSystem.GetCurrentEngineName()}";
            _btnSwitchEngine.Dock = DockStyle.Right;
            _btnSwitchEngine.AutoSize = true;
            _btnSwitchEngine.AutoSizeMode = AutoSizeMode.GrowAndShrink;
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

            var dbHistory = _historyRepository.GetAll()
                .Where(x => x.ProfileId == _currentUser.ProfileId)
                .OrderBy(x => x.ExecutedAt)
                .Select(x => x.CommandText)
                .ToList();

            _localHistory.AddRange(dbHistory);
            _historyIndex = _localHistory.Count;

            _txtInput.Focus();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _txtInput.Select();
            this.ActiveControl = _txtInput;
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
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
            {
                _terminalSystem.SetEngine(new CmdEngine());
            }
            else if (current.Contains("CMD"))
            {
                _terminalSystem.SetEngine(new RemoteEngine());
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
            if (e.KeyCode == Keys.Up)
            {
                if (_historyIndex > 0)
                {
                    _historyIndex--;
                    _txtInput.Text = _localHistory[_historyIndex];
                    _txtInput.SelectionStart = _txtInput.Text.Length;
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }

            if (e.KeyCode == Keys.Down)
            {
                if (_historyIndex < _localHistory.Count - 1)
                {
                    _historyIndex++;
                    _txtInput.Text = _localHistory[_historyIndex];
                    _txtInput.SelectionStart = _txtInput.Text.Length;
                }
                else
                {
                    _historyIndex = _localHistory.Count;
                    _txtInput.Clear();
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }

            if (e.KeyCode == Keys.Enter)
            {
                string text = _txtInput.Text;
                if (string.IsNullOrWhiteSpace(text)) return;

                _localHistory.Add(text);
                _historyIndex = _localHistory.Count;

                ICommand cmd = new RunScriptCommand(_terminalSystem, text, _currentUser);

                AppendText($"{_promptStr}{text}", _txtInput.ForeColor);
                _invoker.ExecuteCommand(cmd);

                string result = cmd.GetOutput();
                AppendText(result, _rtbOutput.ForeColor);

                var historyItem = new CommandHistoryItem
                {
                    CommandText = text,
                    ExecutedAt = DateTime.Now,
                    IsSuccess = !result.Contains("Error"),
                    ProfileId = _currentUser.ProfileId
                };

                _historyRepository.Add(historyItem);

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