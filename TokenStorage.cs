using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Elvy.Result;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ElvyAuthorizationSDK
{
    public class TokenStorage
    {
        private readonly string _refreshToken;
        private string _accessToken;
        private readonly string _refreshUrl;
        private readonly HttpClient _client;
        public TokenStorage(string refreshUrl, string refreshToken)
        {
            _refreshToken = refreshToken;
            _refreshUrl = refreshUrl;
            _client = new HttpClient();
        }
        public string GetRefreshToken()
        {
            return _refreshToken;
        }
       
        public ElvyAccessToken GetAccessToken()
        {
            Result checkResult = TokenHandler.TokenNotExpired(_accessToken, out DateTime expiration);
            if (checkResult.Succes())
            {
                return new ElvyAccessToken()
                {
                    access_token = _accessToken,
                    expires_in = (expiration - DateTime.Now).Seconds,
                    token_type = "access_token",
                    result = "OK"
                };
            }
            JObject json = JObject.FromObject(new
            {
                grant_type = "refresh_token",
                refresh_token = _refreshToken
            });
            var body = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            HttpRequestMessage refreshRequest = new(HttpMethod.Post, _refreshUrl) { Content = body };
            HttpResponseMessage refreshResponse = _client.Send(refreshRequest);
            if (refreshResponse.StatusCode != HttpStatusCode.OK)
            {
                return new ElvyAccessToken()
                {
                    result = refreshResponse.StatusCode.ToString()
                };
            }

            string responseBody = refreshResponse.Content.ReadAsStringAsync().Result;
            ElvyAccessToken elvyAccessToken = JsonConvert.DeserializeObject<ElvyAccessToken>(responseBody);
            _accessToken = elvyAccessToken.access_token;
            return elvyAccessToken;
        }
    }
}
