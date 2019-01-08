using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace IranianMusic.Instruments.Shared
{
    public static class HttpUtility
    {
        static string endpoint= "/api/Instrument";
        public static async Task<HttpResponseMessage> GetAllInstruments(string baseUri)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(baseUri) })
            {
                try
                {
                    var response = await client.GetAsync(endpoint);
                    return response;
                }
                catch (Exception ex)
                {
                    throw new Exception("There was a problem connecting to Provider API.", ex);
                }
            }
        }
        public static async Task<HttpResponseMessage> GetInstrumentDesc(string name,string baseUri)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(baseUri) })
            {
                try
                {
                    var response = await client.GetAsync($"{endpoint}/{name}");
                    return response;
                }
                catch (Exception ex)
                {
                    throw new Exception("There was a problem connecting to Provider API.", ex);
                }
            }
        }
    }
}
