using RestSharp;
using Newtonsoft.Json;

namespace RestSharpReqnroll.Helpers
{
    public class RestClientHelper
    {
        private readonly RestClient _client;
        public HttpResponseMessage? LastResponse { get; set; }
        public string? ResponseContent { get; set; }

        public RestClientHelper(string baseUrl)
        {
            _client = new RestClient(baseUrl);
        }

        public async Task<T?> GetAsync<T>(string endpoint, Dictionary<string, string>? headers = null)
        {
            var request = new RestRequest(endpoint, Method.Get);
            AddHeaders(request, headers);

            var response = await _client.ExecuteAsync(request);
            ResponseContent = response.Content;

            if (!response.IsSuccessful)
                throw new HttpRequestException($"Request failed with status {response.StatusCode}: {response.Content}");

            return JsonConvert.DeserializeObject<T>(response.Content ?? "");
        }

        public async Task<T?> PostAsync<T>(string endpoint, object? body = null, Dictionary<string, string>? headers = null)
        {
            var request = new RestRequest(endpoint, Method.Post);
            AddHeaders(request, headers);

            if (body != null)
            {
                request.AddJsonBody(body);
            }

            var response = await _client.ExecuteAsync(request);
            ResponseContent = response.Content;

            if (!response.IsSuccessful)
                throw new HttpRequestException($"Request failed with status {response.StatusCode}: {response.Content}");

            return JsonConvert.DeserializeObject<T>(response.Content ?? "");
        }

        public async Task<T?> PutAsync<T>(string endpoint, object? body = null, Dictionary<string, string>? headers = null)
        {
            var request = new RestRequest(endpoint, Method.Put);
            AddHeaders(request, headers);

            if (body != null)
            {
                request.AddJsonBody(body);
            }

            var response = await _client.ExecuteAsync(request);
            ResponseContent = response.Content;

            if (!response.IsSuccessful)
                throw new HttpRequestException($"Request failed with status {response.StatusCode}: {response.Content}");

            return JsonConvert.DeserializeObject<T>(response.Content ?? "");
        }

        public async Task<T?> DeleteAsync<T>(string endpoint, Dictionary<string, string>? headers = null)
        {
            var request = new RestRequest(endpoint, Method.Delete);
            AddHeaders(request, headers);

            var response = await _client.ExecuteAsync(request);
            ResponseContent = response.Content;

            if (!response.IsSuccessful)
                throw new HttpRequestException($"Request failed with status {response.StatusCode}: {response.Content}");

            return JsonConvert.DeserializeObject<T>(response.Content ?? "");
        }

        public async Task<string> GetRawAsync(string endpoint, Dictionary<string, string>? headers = null)
        {
            var request = new RestRequest(endpoint, Method.Get);
            AddHeaders(request, headers);

            var response = await _client.ExecuteAsync(request);
            ResponseContent = response.Content;

            if (!response.IsSuccessful)
                throw new HttpRequestException($"Request failed with status {response.StatusCode}: {response.Content}");

            return response.Content ?? "";
        }

        private void AddHeaders(RestRequest request, Dictionary<string, string>? headers)
        {
            if (headers == null) return;

            foreach (var header in headers)
            {
                request.AddHeader(header.Key, header.Value);
            }
        }
    }
}
