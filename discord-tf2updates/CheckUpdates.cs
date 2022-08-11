using System;
using System.Threading;
using System.Threading.Tasks;

namespace discordtf2updates
{
    public class CheckUpdates
    {
        public static Updates latestUpdate;

        public async Task StartBackgroundCheckAsync()
        {
            var _apiHandler = new ApiHandler();

            //We store our inital updates so we have a timestamp to compare against.
            CustomConsole.CustomWriteLine("Getting initial updates from Steam...");

            latestUpdate = await _apiHandler.GetAppNewsAsync();

            var periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(Configuration.AppConfig.PollingRateInSecs));
            while (await periodicTimer.WaitForNextTickAsync())
            {
                //Current is again set, so we can compare each iteration.
                latestUpdate = await CheckForUpdatesAsync(_apiHandler, latestUpdate);
            }
        }

        private async Task<Updates> CheckForUpdatesAsync(ApiHandler apiHandler, Updates currentUpdate)
        {
            CustomConsole.CustomWriteLine("Checking Steam for updates...");

            latestUpdate = await apiHandler.GetAppNewsAsync();

            if (latestUpdate.appnews.newsitems[0].date > currentUpdate.appnews.newsitems[0].date)
            {
                CustomConsole.CustomWriteLine("Updates found, posting to Discord.");

                var _embedUpdates = new EmbedUpdates();
                var embed = _embedUpdates.BuildTF2Embed(latestUpdate.appnews.newsitems[0]);

                var _commands = new Commands();
                await _commands.PostUpdatesAsync(embed);
            }

            return latestUpdate;
        }
    }
}