using System.IO;
using System.Text.Json;

namespace discordtf2updates
{
    static class Configuration
    {
        public static AppConfig AppConfig = JsonSerializer.Deserialize<AppConfig>(File.ReadAllText("./config.json"));
    }
}