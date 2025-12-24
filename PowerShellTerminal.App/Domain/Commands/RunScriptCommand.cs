using System;
using PowerShellTerminal.App.Domain.Interfaces;
using PowerShellTerminal.App.Domain.Bridge;
using PowerShellTerminal.App.Domain.Interpreter;
using PowerShellTerminal.App.Domain.Entities;

namespace PowerShellTerminal.App.Domain.Commands
{
    public class RunScriptCommand : ICommand
    {
        private readonly TerminalSystem _system;
        private readonly string _script;
        private readonly UserProfile _user;
        private string _lastOutput;

        public RunScriptCommand(TerminalSystem system, string script, UserProfile user)
        {
            _system = system;
            _script = script;
            _user = user;
            _lastOutput = string.Empty;
        }

        public void Execute()
        {
            if (_user.Role != "Admin")
            {
                string[] forbiddenCommands = { "rm", "del", "remove", "format", "cmd" };

                foreach (var badCmd in forbiddenCommands)
                {
                    if (_script.Trim().StartsWith(badCmd, StringComparison.OrdinalIgnoreCase))
                    {
                        _lastOutput = $"[SECURITY ALERT] Command '{badCmd}' is restricted to Administrators only.";
                        return;
                    }
                }
            }

            var parser = new ExpressionParser(_system, _user);
            IExpression expressionTree = parser.Parse(_script);
            var context = new InterpreterContext(string.Empty);
            expressionTree.Interpret(context);
            _lastOutput = context.Output;
        }

        public void Undo()
        {
            _lastOutput = "Undo not supported.";
        }

        public string GetOutput()
        {
            return _lastOutput;
        }

        public string GetCommandText()
        {
            return _script;
        }
    }
}