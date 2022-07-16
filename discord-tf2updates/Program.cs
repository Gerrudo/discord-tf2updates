using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using RestSharp;

namespace discordtf2updates
{
    class Program
    {
        private DiscordSocketClient _client;

        public static async Task Main(string[] args)
        {
            var _client = new DiscordSocketClient();
            _client.MessageReceived += CommandHandlerAsync;
            _client.Log += Log;

            string discordToken = JsonConvert.DeserializeObject<Config>(File.ReadAllText("./config.json")).discordToken;

            await _client.LoginAsync(TokenType.Bot, discordToken);
            await _client.StartAsync();

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
                case "latest":
                    var updates = await GetAppNewsAsync();
                    var embed = EmbedUpdates(updates.appnews.newsitems[0]);
                    await message.Channel.SendMessageAsync(embed: embed.Build());
                    break;
                case "help":
                    await message.Channel.SendMessageAsync($@"{message.Author.Mention}, here are the current commands: !latest");
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
            string steamToken = JsonConvert.DeserializeObject<Config>(File.ReadAllText("./config.json")).steamToken;

            string uri = "https://api.steampowered.com";

            var client = new RestClient(uri);

            var request = new RestRequest("/ISteamNews/GetNewsForApp/v0002");

            request.AddHeader("token", steamToken);

            request.AddParameter("appid", 440);
            request.AddParameter("feeds", "tf2_blog");
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
    }
}