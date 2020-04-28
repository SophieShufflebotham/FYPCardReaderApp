using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading.Tasks;
using FYPCardReaderApp.Models;
using FYPCardReaderApp.Responses;

namespace FYPCardReaderApp.Models
{
    public class RestService
    {
        private HttpClient client;

        private const string BaseUrl = "https://fypazureapp.azurewebsites.net/";

        public RestService()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(BaseUrl);
            client.MaxResponseContentBufferSize = 256000;
        }

        public async void PostTimeRequest<TResult>(object data)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync(client.BaseAddress + "/accessTimes/setAccessTime", content);
            //response.EnsureSuccessStatusCode();
            string responseString = await response.Content.ReadAsStringAsync();
        }

        public void GetResetStatus()
        {
            //StringContent content = new StringContent(JsonConvert.SerializeObject(data));
            //content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = client.GetAsync(client.BaseAddress + "/accessTimes/clearAllSafety");
            //response.EnsureSuccessStatusCode();
        }

        public async Task<Person[]> GetMissingUsers()
        {

            var response = await client.GetAsync(client.BaseAddress + "/accessTimes/findUnsafeUsers").ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            string responseString = await response.Content.ReadAsStringAsync();
            Person[] result = JsonConvert.DeserializeObject<Person[]>(responseString);
            return result;
        }

        public async Task<LocationsResponse[]> PostUserPermissions(object data)
        {

            StringContent content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync(client.BaseAddress + "/users/userPermissions", content);
            //response.EnsureSuccessStatusCode();
            string responseString = await response.Content.ReadAsStringAsync();
            LocationsResponse[] result = JsonConvert.DeserializeObject<LocationsResponse[]>(responseString);
            return result;
        }
    }
}
