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
            try
            {
                var result = Client.PostAsync($"{requestUri}?format=json", content).Result;
            }
            catch (Exception)
            {
                await new MessageDialog("Error occured while sending data.").ShowAsync();
            }
        }

        public async Task<T> PostAsync<T>(string requestUri, IEnumerable<KeyValuePair<string, string>> nameValueCollection = null, JsonConverter converter = null)
        {
            T result = default(T);
            try
            { 
                string jsonResponse = await PostStringAsync($"{requestUri}?format=json", nameValueCollection);
                result = converter != null ?
                    JsonConvert.DeserializeObject<T>(jsonResponse, converter) : JsonConvert.DeserializeObject<T>(jsonResponse);
            }
            catch (Exception)
            {
                await new MessageDialog("Error occured while sending data.").ShowAsync();
            }
            return result;
        }

        public async Task<T> GetAsync<T>(string requestUri, JsonConverter converter = null)
        {
            T result = default(T);
            try
            {
                string jsonResponse = await GetStringAsync($"{requestUri}?format=json");
                result = converter != null ?
                    JsonConvert.DeserializeObject<T>(jsonResponse, converter) : JsonConvert.DeserializeObject<T>(jsonResponse);
            }
            catch (Exception)
            {
                await new MessageDialog("Error occured while getting data.").ShowAsync();
            }
            return result;
        }

        public async Task<string> GetStringAsync(string requestUri)
        {
            string response = null;
            try
            {
                response = await Client.GetAsync(requestUri).Result.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                await new MessageDialog("Error occured while getting data.").ShowAsync();
            }
            return response;
        }

        public async Task<string> PostStringAsync(string requestUri, IEnumerable<KeyValuePair<string, string>> nameValueCollection = null)
        {
            string response = null;
            var content = new FormUrlEncodedContent(nameValueCollection ?? new List<KeyValuePair<string, string>>());
            try
            {
                response = await Client.PostAsync(requestUri, content).Result.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                await new MessageDialog("Error occured while sending data.").ShowAsync();
            }
            return response;
        }

    }
}
