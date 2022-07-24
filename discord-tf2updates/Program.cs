using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using Discord;
using Discord.WebSocket;
using RestSharp;

namespace discordtf2updates
{
    class Program
    {
        public static Config config = JsonSerializer.Deserialize<Config>(File.ReadAllText("./config.json"));

        private DiscordSocketClient _client;

        private static List<Guild> _guilds;

        public static async Task Main(string[] args)
        {
            var _client = new DiscordSocketClient();
            _client.MessageReceived += CommandHandlerAsync;
            _client.Log += Log;

            await _client.LoginAsync(TokenType.Bot, config.DiscordToken);
            await _client.StartAsync();

            _guilds = new List<Guild>();

            await CheckForUpdatesAsync(TimeSpan.FromSeconds(config.PollingRateInSecs));

            await Task.Delay(-1);
        }

        private static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private static async Task<Task> CommandHandlerAsync(SocketMessage message)
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
                    var updates = await GetAppNewsAsync();
                    var embed = EmbedUpdates(updates.appnews.newsitems[0]);
                    await message.Channel.SendMessageAsync(embed: embed.Build());
                    break;
                case "help":
                    await message.Channel.SendMessageAsync($@"{message.Author.Mention}, here are the current commands: {config.CommandList}");
                    break;
            }

            return Task.CompletedTask;
        }

        private static string ValidateCommand(SocketMessage message)
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

        private static async Task<Updates> GetAppNewsAsync()
        {
            var client = new RestClient(config.SteamWebApiUri);

            var request = new RestRequest(config.NewsEndpoint);

            request.AddHeader("token", config.SteamToken);

            request.AddParameter("appid", config.AppId);
            request.AddParameter("feeds", config.Feeds);
            request.AddParameter("count", 1);

            var response = await client.GetAsync<Updates>(request);

            return response;
        }

        private static EmbedBuilder EmbedUpdates(Newsitem newsitem)
        {
            var converter = new ReverseMarkdown.Converter();

            string contentsToMd = converter.Convert(newsitem.contents);

            var embed = new EmbedBuilder
            {
                Title = newsitem.title,
                Description = contentsToMd,
                Color = Color.Orange,
                Url = newsitem.url,
            };
            return embed;
        }

        private static void SetChannel(SocketMessage message)
        {
            var Guild = new Guild();

            Guild.Message = message;
            Guild.Channel = message.Channel;

            _guilds.Add(Guild);

            CustomConsole.CustomWriteLine($@"An update channel has been added by {message.Author.Username}");
        }

        private static async Task CheckForUpdatesAsync(TimeSpan timeSpan)
        {
            CustomConsole.CustomWriteLine("Getting initial updates from Steam...");
            var storedUpdates = await GetAppNewsAsync();

            var periodicTimer = new PeriodicTimer(timeSpan);
            while (await periodicTimer.WaitForNextTickAsync())
            {
                CustomConsole.CustomWriteLine("Checking Steam for updates...");
                var updates = await GetAppNewsAsync();

                if (updates.appnews.newsitems[0].date > storedUpdates.appnews.newsitems[0].date)
                {
                    CustomConsole.CustomWriteLine("Updates found, posting to Discord.");
                    storedUpdates = updates;
                    var embed = EmbedUpdates(updates.appnews.newsitems[0]);
                    await PostUpdatesAsync(embed);
                }
                else
                {
                    CustomConsole.CustomWriteLine("No updates.");
                }
            }
        }

        private static async Task PostUpdatesAsync(EmbedBuilder embed)
        {
            foreach (Guild Guild in _guilds)
            {
                await Guild.Channel.SendMessageAsync(embed: embed.Build());
                CustomConsole.CustomWriteLine($@"Sending Embed to {Guild.Channel.Name}");
            }
        }
    }
}
