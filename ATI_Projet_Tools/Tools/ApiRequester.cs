using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace ATI_Projet_App.Tools
{
    public class ApiRequester( HttpClient http)
    {
        private readonly HttpClient httpClient = http;
        public TResult Get<TResult>(string url, string token = "")
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }
            using (HttpResponseMessage response = httpClient.GetAsync(url).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    string json = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<TResult>(json);
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString());
                }
            }
        }

        public bool Post<TModel>(TModel objet, string url, string token = "", string mediatype = "application/json")
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }

            string jsonToSend = JsonConvert.SerializeObject(objet);
            HttpContent content = new StringContent(jsonToSend, Encoding.UTF8, mediatype);

            using (HttpResponseMessage response = httpClient.PostAsync(url, content).Result)
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.StatusCode.ToString());
                }
                return true;


            }
        }

        public bool Delete(string url, string token = "")
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }

            using (HttpResponseMessage response = httpClient.DeleteAsync(url).Result)
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.StatusCode.ToString());
                }
                return response.StatusCode == System.Net.HttpStatusCode.OK;

            }
        }

        public bool Patch<TModel>(TModel objet, string url, string token = "", string mediatype = "application/json")
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }

            string jsonToSend = JsonConvert.SerializeObject(objet);
            HttpContent content = new StringContent(jsonToSend, Encoding.UTF8, mediatype);

            using (HttpResponseMessage response = httpClient.PatchAsync(url, content).Result)
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.StatusCode.ToString());
                }
                return true;


            }
        }
    }
}
