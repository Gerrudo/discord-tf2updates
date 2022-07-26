using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Discord;
using Discord.Net;
using Discord.WebSocket;

namespace discordtf2updates
{
    public class Commands
    {
        private static List<Guild> _guilds;

        public async Task BuildGlobalCommandsAsync(DiscordSocketClient client)
        {
            //These need to be loaded from an external file!
            string[] commandNames =
                {
                "latest-updates",
                "set-channel",
                "remove-channel"
                };

            string[] commandDescriptions =
                {
                "Post the latest news and updates.",
                "Sets the channel where the latest news and updates will be posted.",
                "Removes a channel so it will no longer receive news and updates."
                };

            for (int i = 0; i < commandNames.Length; i++)
            {
                var globalCommand = new SlashCommandBuilder();
                globalCommand.WithName(commandNames[i]);
                globalCommand.WithDescription(commandDescriptions[i]);

                try
                {
                    await client.CreateGlobalApplicationCommandAsync(globalCommand.Build());
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
            var _checkUpdates = new CheckUpdates();

            var _embedUpdates = new EmbedUpdates();
            var embed = _embedUpdates.BuildTF2Embed(_checkUpdates.latestupdate.appnews.newsitems[0]);

            await command.RespondAsync(embed: embed.Build());
        }

        public void HandleSetUpdateChannelCommand(SocketSlashCommand command)
        {
            _guilds = new List<Guild>();

            var Guild = new Guild();
            Guild.Channel = command.Channel;

            _guilds.Add(Guild);

            CustomConsole.CustomWriteLine($@"An update channel: {Guild.Channel.Name}, has been added by {command.User.Username}");
        }

        public void HandleRemoveUpdateChannelCommand(SocketSlashCommand command)
        {
            var Guild = new Guild();
            Guild.Channel = command.Channel;

            //Currently this is a limitation, as it will remove the whole Guild object, meaning all Channels that were added from a Guild will be removed.
            _guilds.Remove(Guild);

            CustomConsole.CustomWriteLine($@"An update channel: {Guild.Channel.Name}, has been removed by {command.User.Username}");
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