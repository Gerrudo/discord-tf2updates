using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace discordtf2updates
{
    public class DiscordBot
    {
        private DiscordSocketClient _client;

        private Commands _commands;

        public async Task StartAsync()
        {
            if (Configuration.AppConfig.DeveloperMode)
            {
                CustomConsole.CustomWriteLine("Running in Developer Mode!");
            }

            _client = new DiscordSocketClient();
            _client.Log += Log;
            _client.Ready += ClientReadyAsync;
            _client.SlashCommandExecuted += SlashCommandHandlerAsync;

            await _client.LoginAsync(TokenType.Bot, Configuration.AppConfig.DiscordToken);
            await _client.StartAsync();
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task ClientReadyAsync()
        {
            _commands = new Commands();
            await _commands.BuildCommandsAsync(_client);
        }

        private async Task SlashCommandHandlerAsync(SocketSlashCommand command)
        {
            try
            {
                switch (command.Data.Name)
                {
                    case "latestupdates":
                        await _commands.HandleLatestUpdatesCommand(command);
                        break;
                    case "setchannel":
                        await _commands.HandleSetUpdateChannelCommand(command);
                        break;
                    case "removechannel":
                        await _commands.HandleRemoveUpdateChannelCommand(command);
                        break;
                    default:
                        await command.RespondAsync(@$"Whoops! Something went wrong. Please report this [here](https://github.com/Gerrudo/discord-tf2updates/issues/new), please include your UserId: {command.User.Id}");
                        CustomConsole.CustomWriteLine($"{command.User.Id} Gave in an invalid command: '{command.Data.Name}'");
                        break;
                }
            }
            catch (Exception ex)
            {
                await command.RespondAsync(@$"Whoops! An error occured. Please report this [here](https://github.com/Gerrudo/discord-tf2updates/issues/new), please include your UserId: {command.User.Id}");
                CustomConsole.CustomWriteLine($"{command.User.Id} Generated an error when calling a command '{command.Data.Name}': {ex}");
            }
        }
    }
}