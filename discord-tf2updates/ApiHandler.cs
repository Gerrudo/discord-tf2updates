using System.Threading.Tasks;
using RestSharp;

namespace discordtf2updates
{
    public class ApiHandler
    {
        public async Task<Updates> GetAppNewsAsync()
        {
            var client = new RestClient(Configuration.AppConfig.SteamWebApiUri);

            var request = new RestRequest(Configuration.AppConfig.NewsEndpoint);

            request.AddHeader("token", Configuration.AppConfig.SteamToken);

            request.AddParameter("appid", Configuration.AppConfig.AppId);
            request.AddParameter("feeds", Configuration.AppConfig.Feeds);
            request.AddParameter("count", 1);

            var response = await client.GetAsync<Updates>(request);

            return response;
        }
    }
}