using MobilePlatformsProject.Converters.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace MobilePlatformsProject.Rest
{
    public class RequestHelper
    {
        public HttpClient Client { get; set; }

        public RequestHelper(string baseUri)
        {
            var baseAddress = new Uri(baseUri);
            var handler = new HttpClientHandler();
            Client = new HttpClient(handler) { BaseAddress = baseAddress };
        }

        public async void PostAsync(string requestUri, IEnumerable<KeyValuePair<string, string>> nameValueCollection = null)
        {
            var content = new FormUrlEncodedContent(nameValueCollection ?? new List<KeyValuePair<string, string>>());
            await Client.PostAsync($"{requestUri}?format=json", content);
        }

        public async Task<T> PostAsync<T>(string requestUri, IEnumerable<KeyValuePair<string, string>> nameValueCollection = null, JsonConverter converter = null)
        {
            T result = default(T);
            string jsonResponse = await PostStringAsync($"{requestUri}?format=json", nameValueCollection);
            result = converter != null ?
                JsonConvert.DeserializeObject<T>(jsonResponse, converter) : JsonConvert.DeserializeObject<T>(jsonResponse);
            return result;
        }

        public async Task<T> GetAsync<T>(string requestUri, JsonConverter converter = null)
        {
            T result = default(T);
            string jsonResponse = await GetStringAsync($"{requestUri}?format=json");
            result = converter != null ?
                JsonConvert.DeserializeObject<T>(jsonResponse, converter) : JsonConvert.DeserializeObject<T>(jsonResponse);
            return result;
        }

        public async Task<string> GetStringAsync(string requestUri)
        {
            HttpResponseMessage response = null;
            response = await Client.GetAsync(requestUri);
            if (!response.IsSuccessStatusCode)
                throw new ArgumentException(await response.Content.ReadAsStringAsync());
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PostStringAsync(string requestUri, IEnumerable<KeyValuePair<string, string>> nameValueCollection = null)
        {
            string response = null;
            var content = new FormUrlEncodedContent(nameValueCollection ?? new List<KeyValuePair<string, string>>());
                response = await Client.PostAsync(requestUri, content).Result.Content.ReadAsStringAsync();
            return response;
        }

    }
}
