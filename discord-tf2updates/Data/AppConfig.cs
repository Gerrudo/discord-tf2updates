namespace discordtf2updates
{
    class AppConfig
    {
        public string DiscordToken { get; set; }
        public string SteamToken { get; set; }
        public int PollingRateInSecs { get; set; }
        public string SteamWebApiUri { get; set; }
        public string NewsEndpoint { get; set; }
        public int AppId { get; set; }
        public string Feeds { get; set; }
        public bool DeveloperMode { get; set; }
        public ulong DeveloperGuildId { get; set; }
    }
}