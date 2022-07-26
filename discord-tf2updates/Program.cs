using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace discordtf2updates
{
    class Program
    {
        public static Config config = JsonSerializer.Deserialize<Config>(File.ReadAllText("./config.json"));

        public static async Task Main(string[] args)
        {
            var _discordBot = new DiscordBot();
            await _discordBot.StartAsync();

            var _checkUpdates = new CheckUpdates();
            await _checkUpdates.StartBackgroundCheckAsync();

            await Task.Delay(-1);
        }
    }
}