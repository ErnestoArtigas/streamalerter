using Newtonsoft.Json;
using StreamAlerter.Core.Interfaces.HttpsRepository;
using StreamAlerter.Entities;
using StreamAlerter.HttpsRepository.Model;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace StreamAlerter.HttpsRepository
{
    public class TwitchApiRepository : IStreamAlerterHttpsRepository
    {
        private HttpClient HttpClient;

        public TwitchApiRepository()
        {
            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Add("client-id", "82ffn0yqwo8kc4gfbxlb6molj61cr2");
            HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "8amd6egnvznyxmrnpf7jxh3ttzdj3u");
        }

        public async Task<IEnumerable<Streamer>> GetTwitchUsers(string streamerName)
        {
            try
            {
                HttpResponseMessage response = await HttpClient.GetAsync($"https://api.twitch.tv/helix/search/channels?query={streamerName}");
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStreamAsync();

                using var streamReader = new StreamReader(responseBody);
                using var jsonReader = new JsonTextReader(streamReader);

                JsonSerializer serializer = new JsonSerializer();

                // We developped TwitchDataModel and TwitchStreamerModel for easily transform the data from Twitch as proper Streamer instances.
                try
                {
                    var twitchData = serializer.Deserialize<TwitchDataModel>(jsonReader);
                    var streamerArray = new List<Streamer>();

                    foreach (TwitchStreamerModel element in twitchData.data)
                    {
                        streamerArray.Add(new Streamer(element.broadcaster_login, element.is_live));
                    }

                    return streamerArray;
                }
                catch (JsonReaderException jsonReaderEx)
                {
                    throw new JsonReaderException(jsonReaderEx.Message);
                }

            }
            catch (HttpRequestException httpRequestEx)
            {
                throw new HttpRequestException(httpRequestEx.Message);
            }
        }
    }

}
