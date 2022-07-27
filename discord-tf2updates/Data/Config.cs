namespace discordtf2updates
{
    class Config
    {
        public string DiscordToken { get; set; }
        public string SteamToken { get; set; }
        public int PollingRateInSecs { get; set; }
        public string SteamWebApiUri { get; set; }
        public string NewsEndpoint { get; set; }
        public int AppId { get; set; }
        public string Feeds { get; set; }
        public bool UseDevGuild { get; set; }
        public ulong DevGuildId { get; set; }
    }
}