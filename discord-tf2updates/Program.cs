﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace discordtf2updates
{
    class Program
    {
        public static Config config = JsonSerializer.Deserialize<Config>(File.ReadAllText("./config.json"));

        public static List<Guild> _guilds;

        public static async Task Main(string[] args)
        {
            var _discordBot = new DiscordBot();
            await _discordBot.StartAsync();

            var _apiHandler = new ApiHandler();

            var _checkUpdates = new CheckUpdates();
            //We store our inital updates so we have a timestamp to compare against.
            var latestUpdates = await _checkUpdates.InitalUpdates(_apiHandler);

            _guilds = new List<Guild>();

            var periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(config.PollingRateInSecs));
            while (await periodicTimer.WaitForNextTickAsync())
            {
                //The stored updates is again set, so we can compare each iteration.
                latestUpdates = await _checkUpdates.CheckForUpdatesAsync(_apiHandler, latestUpdates);
            }

            await Task.Delay(-1);
        }
    }
}