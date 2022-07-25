using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace discordtf2updates
{
    public class DiscordBot
    {
        private DiscordSocketClient _client;

        public async Task StartAsync()
        {
            var _client = new DiscordSocketClient();
            _client.MessageReceived += CommandHandlerAsync;
            _client.Log += Log;

            await _client.LoginAsync(TokenType.Bot, Program.config.DiscordToken);
            await _client.StartAsync();
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task<Task> CommandHandlerAsync(SocketMessage message)
        {
            if (!message.Content.StartsWith("!") || message.Author.IsBot)
            {
                return Task.CompletedTask;
            }

            string command = ValidateCommand(message);

            switch (command)
            {
                case "setchannel":
                    SetChannel(message);
                    break;
                case "latest":
                    var _apiHandler = new ApiHandler();
                    var updates = await _apiHandler.GetAppNewsAsync();

                    var _embedBuilder = new EmbedUpdates();
                    var embed = _embedBuilder.BuildTF2Embed(updates.appnews.newsitems[0]);

                    await message.Channel.SendMessageAsync(embed: embed.Build());
                    break;
                case "help":
                    await message.Channel.SendMessageAsync($@"{message.Author.Mention}, here are the current commands: {Program.config.CommandList}");
                    break;
            }

            return Task.CompletedTask;
        }

        private string ValidateCommand(SocketMessage message)
        {
            int lengthofCommand = -1;
            string command;

            if (message.Content.Contains(" "))
            {
                lengthofCommand = message.Content.IndexOf(" ");
            }
            else
            {
                lengthofCommand = message.Content.Length;
            }

            command = message.Content.Substring(1, lengthofCommand - 1).ToLower();

            return command;
        }

        private void SetChannel(SocketMessage message)
        {
            var Guild = new Guild();

            Guild.Message = message;
            Guild.Channel = message.Channel;

            Program._guilds.Add(Guild);

            CustomConsole.CustomWriteLine($@"An update channel has been added by {message.Author.Username}");
        }

        public static async Task PostUpdatesAsync(EmbedBuilder embed)
        {
            foreach (Guild Guild in Program._guilds)
            {
                await Guild.Channel.SendMessageAsync(embed: embed.Build());

                CustomConsole.CustomWriteLine($@"Sending Embed to {Guild.Channel.Name}");
            }
        }
    }
}