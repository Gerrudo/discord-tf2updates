using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Discord;
using Discord.Net;
using Discord.WebSocket;

namespace discordtf2updates
{
    public class Commands
    {
        private List<Guild> _guilds;

        public async Task BuildCommandsAsync(DiscordSocketClient client)
        {
            var _slashCommands = JsonSerializer.Deserialize<List<SlashCommands>>(File.ReadAllText("./SlashCommands.json"));

            for (int i = 0; i < _slashCommands.Count; i++)
            {
                var newCommand = new SlashCommandBuilder();
                newCommand.WithName(_slashCommands[i].CommandName);
                newCommand.WithDescription(_slashCommands[i].CommandDescription);

                try
                {
                    if (_slashCommands[i].GlobalCommand)
                    {
                        await client.CreateGlobalApplicationCommandAsync(newCommand.Build());
                    }
                    else
                    {
                        var guild = client.GetGuild(Configuration.AppConfig.DeveloperGuildId);
                        await guild.CreateApplicationCommandAsync(newCommand.Build());
                    }
                }
                catch (HttpException exception)
                {
                    var json = JsonSerializer.Serialize(exception.Errors);

                    Console.WriteLine(json);
                }
            }
        }

        public async Task HandleLatestUpdatesCommand(SocketSlashCommand command)
        {
            var _embedUpdates = new EmbedUpdates();
            var embed = _embedUpdates.BuildTF2Embed(CheckUpdates.latestupdate.appnews.newsitems[0]);

            await command.RespondAsync(embed: embed.Build());

            CustomConsole.CustomWriteLine($"{command.User.Id} Requested latest updates in channel: {command.Channel.Id}");
        }

        public async Task HandleSetUpdateChannelCommand(SocketSlashCommand command)
        {
            _guilds = new List<Guild>();

            var Guild = new Guild();
            Guild.Channel = command.Channel;

            _guilds.Add(Guild);

            string response = $@"An update channel: {Guild.Channel.Name}, has been added by {command.User.Username}";

            await command.RespondAsync(response);

            CustomConsole.CustomWriteLine($"{command.User.Id} Added an update channel with Id: {command.Channel.Id}");
        }

        public async Task HandleRemoveUpdateChannelCommand(SocketSlashCommand command)
        {
            var Guild = new Guild();
            Guild.Channel = command.Channel;

            //Currently this is a limitation, as it will remove the whole Guild object, meaning all Channels that were added from a Guild will be removed.
            _guilds.Remove(Guild);

            string response = $@"An update channel: {Guild.Channel.Name}, has been removed by {command.User.Username}";

            await command.RespondAsync(response);

            CustomConsole.CustomWriteLine($"{command.User.Id} Removed an update channel with Id: {command.Channel.Id}");
        }

        public async Task GlobalPostUpdatesAsync(EmbedBuilder embed)
        {
            foreach (Guild Guild in _guilds)
            {
                await Guild.Channel.SendMessageAsync(embed: embed.Build());

                CustomConsole.CustomWriteLine($@"Sending Embed to {Guild.Channel.Name}");
            }
        }
    }
}