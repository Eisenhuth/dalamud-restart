using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace restart
{
    using Dalamud.Game.Command;
    using Dalamud.IoC;
    using Dalamud.Plugin;

    public class RestartPlugin : IDalamudPlugin
    {
        public string Name =>"Dalamud Restart";
        private const string commandName = "/xlrestart";
        
        [PluginService] private static CommandManager CommandManager { get; set; } = null!;
        
        
        public RestartPlugin([RequiredVersion("1.0")] CommandManager commandManager)
        {
            commandManager.AddHandler(commandName, new CommandInfo(RestartCommand)
            {
                HelpMessage = "quickly restarts the game",
                ShowInHelp = true
            });
        }

        private void RestartCommand(string command, string args)
        {
            //taken from https://github.com/goatcorp/Dalamud/blob/a24af3e9218369cc6295c90d7f43d3647a2c205e/Dalamud/Interface/Internal/DalamudInterface.cs#L623
            [DllImport("kernel32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            static extern void RaiseException(uint dwExceptionCode, uint dwExceptionFlags, uint nNumberOfArguments, IntPtr lpArguments);

            RaiseException(0x12345678, 0, 0, IntPtr.Zero);
            Process.GetCurrentProcess().Kill();
        }

        public void Dispose()
        {
            CommandManager.RemoveHandler(commandName);
        }
    }
}