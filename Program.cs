using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscordBot
{
	class Program
    { 

		static void Main(string[] args)
		=> new Program().StartAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private CommandHandler _handler;

        public async Task StartAsync()
		{
			_client = new DiscordSocketClient();

			await _client.LoginAsync(TokenType.Bot, "");

			await _client.StartAsync();

            _handler = new CommandHandler(_client);

            Console.WriteLine("The bot is running !");

            await Task.Delay(-1);

        }

        public static async Task Log(string message, LogSeverity severity = LogSeverity.Info, [System.Runtime.CompilerServices.CallerMemberName] string source = "", Exception exception = null)
        {
            await Console.Error.WriteLineAsync($"[{severity}][{source}] {message}");
            if (exception != null)
            {
                await Console.Error.WriteLineAsync(exception.ToString());
                await Console.Error.WriteLineAsync();
            }
        }
    }
}
