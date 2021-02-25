using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Polly;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Prism_App.Services.Request
{
    public class RequestService : IRequestService
    {
        private readonly HttpStatusCode[] _httpStatusCodesToRetry =
        {
            HttpStatusCode.RequestTimeout, // 408
            HttpStatusCode.InternalServerError, // 500
            HttpStatusCode.BadGateway, // 502
            HttpStatusCode.ServiceUnavailable, // 503
            HttpStatusCode.GatewayTimeout // 504
        };

        public Task<TResult> GetAsync<TResult>(string uri, string token = "", CancellationTokenSource cancellationToken = null)
        {
            return SendRequest<TResult>(HttpMethod.Get, uri, null, token, cancellationToken);
        }

        public Task<TResult> PostAsync<TResult>(string uri, string token = "", CancellationTokenSource cancellationToken = null)
        {
            return SendRequest<TResult>(HttpMethod.Post, uri, null, token, cancellationToken);
        }

        public Task<TResult> PostAsync<TResult>(string uri, Dictionary<string, string> data, string token = "", CancellationTokenSource cancellationToken = null)
        {
            return SendRequest<TResult>(HttpMethod.Post, uri, data, token, cancellationToken);
        }

        private async Task<TResult> SendRequest<TResult>(HttpMethod method, string uri,
           Dictionary<string, string> data = null,
           string token = "",
           CancellationTokenSource cancellationToken = default(CancellationTokenSource))
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                throw new Exception("There is no Internet connection");
            if (cancellationToken == null)
                cancellationToken = new CancellationTokenSource();

            using (var response = await Policy
                .Handle<TimeoutException>()
                .Or<HttpRequestException>()
                .OrInner<OperationCanceledException>(ex => ex.CancellationToken != cancellationToken?.Token)
                .OrResult<HttpResponseMessage>(r => _httpStatusCodesToRetry.Contains(r.StatusCode))
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                .ExecuteAsync(async cancelToken =>
                {
                    using (var httpClient = CreateHttpClient(token))
                    {
                        var request = new HttpRequestMessage(method, uri);
                        if (data != null && data.Any())
                        {
                            var httpContent = new MultipartFormDataContent();
                            foreach (var item in data) 
                                httpContent.Add(new StringContent(item.Value), item.Key);
                            request.Content = httpContent;
                        }
                        return await httpClient.SendAsync(request, cancelToken);
                    }
                }, cancellationToken.Token))
            {
                if (!response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    throw new ExtendedHttpRequestException(response.StatusCode, content);
                }
                var stringr = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TResult>(stringr);
                return result;
            }
        }
        private static HttpClient CreateHttpClient(string token = "")
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Add(GlobalSettings.AuthenticationTokenHeaderKey, token);
            }

            return httpClient;
        }      
    }
}

