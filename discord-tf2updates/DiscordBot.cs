using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace discordtf2updates
{
    public class DiscordBot
    {
        //These two are null after StartAsync() finishes, do we need await Task.Delay(-1);?
        private DiscordSocketClient _client;

        private Commands _commands;

        public async Task StartAsync()
        {
            var _client = new DiscordSocketClient();
            _client.Log += Log;
            _client.Ready += ClientReadyAsync;
            _client.SlashCommandExecuted += SlashCommandHandlerAsync;

            var _commands = new Commands();

            await _client.LoginAsync(TokenType.Bot, Program.config.DiscordToken);
            await _client.StartAsync();
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task ClientReadyAsync()
        {
            await _commands.BuildGlobalCommandsAsync(_client);
        }

        private async Task SlashCommandHandlerAsync(SocketSlashCommand command)
        {
            switch (command.Data.Name)
            {
                case "latest-updates":
                    await _commands.HandleLatestUpdatesCommand(command);
                    break;
                case "set-update-channel":
                    _commands.HandleSetUpdateChannelCommand(command);
                    break;
                case "remove-update-channel":
                    _commands.HandleRemoveUpdateChannelCommand(command);
                    break;
            }
        }
    }
}