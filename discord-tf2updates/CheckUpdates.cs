using System;
using System.Threading;
using System.Threading.Tasks;

namespace discordtf2updates
{
    public class CheckUpdates
    {
        public static Updates latestupdate;

        private async Task<Updates> InitalUpdates(ApiHandler ApiHandler)
        {
            CustomConsole.CustomWriteLine("Getting initial updates from Steam...");

            return await ApiHandler.GetAppNewsAsync();
        }

        public async Task StartBackgroundCheckAsync()
        {
            var _apiHandler = new ApiHandler();

            //We store our inital updates so we have a timestamp to compare against.
            var latestUpdates = await InitalUpdates(_apiHandler);

            var periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(Configuration.AppConfig.PollingRateInSecs));
            while (await periodicTimer.WaitForNextTickAsync())
            {
                //latest is again set, so we can compare each iteration.
                latestUpdates = await CheckForUpdatesAsync(_apiHandler, latestUpdates);
            }
        }

        private async Task<Updates> CheckForUpdatesAsync(ApiHandler apiHandler, Updates storedUpdates)
        {
            CustomConsole.CustomWriteLine("Checking Steam for updates...");

            latestupdate = await apiHandler.GetAppNewsAsync();

            if (latestupdate.appnews.newsitems[0].date > storedUpdates.appnews.newsitems[0].date)
            {
                CustomConsole.CustomWriteLine("Updates found, posting to Discord.");

                var _embedUpdates = new EmbedUpdates();
                var embed = _embedUpdates.BuildTF2Embed(latestupdate.appnews.newsitems[0]);

                var _commands = new Commands();
                await _commands.GlobalPostUpdatesAsync(embed);

                return latestupdate;
            }
            else
            {
                CustomConsole.CustomWriteLine("No updates.");

                return latestupdate;
            }
        }
    }
}