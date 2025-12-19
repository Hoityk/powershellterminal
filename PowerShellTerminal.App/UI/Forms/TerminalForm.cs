using System;
using System.Drawing;
using System.Windows.Forms;
using PowerShellTerminal.App.Domain.Core;
using PowerShellTerminal.App.Domain.Commands;
using PowerShellTerminal.App.Domain.Invokers;
using PowerShellTerminal.App.Domain.Interfaces;

namespace PowerShellTerminal.App.UI.Forms
{
    public class TerminalForm : Form
    {
        private RichTextBox _rtbOutput;
        private TextBox _txtInput;
        
        private CommandInvoker _invoker;
        private PowerShellReceiver _receiver;

        public TerminalForm()
        {
            this.Text = "PowerShell Terminal (Pattern: Command)";
            this.Size = new Size(800, 600);
            this.BackColor = Color.Black;

            _invoker = new CommandInvoker();
            _receiver = new PowerShellReceiver();

            _rtbOutput = new RichTextBox();
            _rtbOutput.Dock = DockStyle.Top;
            _rtbOutput.Height = 500;
            _rtbOutput.BackColor = Color.Black;
            _rtbOutput.ForeColor = Color.LightGray;
            _rtbOutput.Font = new Font("Consolas", 12);
            _rtbOutput.ReadOnly = true;
            _rtbOutput.Text = "PowerShell Terminal v1.0\nType a command and press Enter...\n----------------------------\n";

            _txtInput = new TextBox();
            _txtInput.Dock = DockStyle.Bottom;
            _txtInput.BackColor = Color.FromArgb(30, 30, 30);
            _txtInput.ForeColor = Color.White;
            _txtInput.Font = new Font("Consolas", 12);
            _txtInput.KeyDown += OnInputKeyDown;

            this.Controls.Add(_rtbOutput);
            this.Controls.Add(_txtInput);
            
            this.Shown += (s, e) => _txtInput.Focus();
        }

        private void OnInputKeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string text = _txtInput.Text.Trim();
                if (string.IsNullOrWhiteSpace(text)) return;

                ICommand cmd = new RunScriptCommand(_receiver, text);

                AppendText($"> {text}", Color.Yellow);

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