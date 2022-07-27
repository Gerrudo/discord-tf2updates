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
            _client = new DiscordSocketClient();
            _client.Log += Log;
            _client.Ready += ClientReadyAsync;
            _client.SlashCommandExecuted += SlashCommandHandlerAsync;

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
            _commands = new Commands();
            await _commands.BuildCommandsAsync(_client, Program.config.UseDevGuild);
        }

        private async Task SlashCommandHandlerAsync(SocketSlashCommand command)
        {
            //The command object is readonly.
            string filteredCommand = command.Data.Name;

            //This will remove the 'test-' prefix from the front of a guild command, so it is treated the same as if it's global command.
            if (Program.config.UseDevGuild)
            {
                filteredCommand = command.Data.Name.Remove(0, 5);
            }

            try
            {
                switch (filteredCommand)
                {
                    case "latest-updates":
                        await _commands.HandleLatestUpdatesCommand(command);
                        break;
                    case "set-channel":
                        await _commands.HandleSetUpdateChannelCommand(command);
                        break;
                    case "remove-channel":
                        await _commands.HandleRemoveUpdateChannelCommand(command);
                        break;
                }
            }
            catch (Exception ex)
            {
                await command.RespondAsync(@$"Whoops! An error occured. Please report this [here](https://github.com/Gerrudo/discord-tf2updates/issues/new). {ex.Message}");
            }
        }
    }
}