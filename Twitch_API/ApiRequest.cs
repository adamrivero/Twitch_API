﻿using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Twitch_API.Model;

namespace Twitch_API
{
    class ApiRequest
    {
        public async Task<Uri> UriAsync(string login)
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), $"https://pwn.sh/tools/streamapi.py?url=twitch.tv/{login}");
            var response = await httpClient.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<VideoURIModel>(result);
            return json.urls.The720P;
        }
        public async Task<UserModel> GetUserInfoAsync(string id)
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), $"https://api.twitch.tv/helix/users?id={id}");
            request.Headers.Add("Client-ID", "0pje11teayzq9z2najlxgdcc5d2dy1");
            var response = await httpClient.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<UserModel>(result);
            return json;
        }
        public async Task<StreamModel> GetStreamInfoAsync()
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), "https://api.twitch.tv/helix/streams");
            request.Headers.Add("Client-ID", "0pje11teayzq9z2najlxgdcc5d2dy1");
            var response = await httpClient.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<StreamModel>(result);
            return json;
        }
        public async Task<StreamModel> GetStreamInfoAsync(string param)
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), $"https://api.twitch.tv/helix/streams?{param}");
            request.Headers.Add("Client-ID", "0pje11teayzq9z2najlxgdcc5d2dy1");
            var response = await httpClient.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<StreamModel>(result);
            return json;
        }
        public async Task<GameModel> GetGameInfoAsync(string gameID)
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), $"https://api.twitch.tv/helix/games?id={gameID}");
            request.Headers.Add("Client-ID", "0pje11teayzq9z2najlxgdcc5d2dy1");
            var response = await httpClient.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<GameModel>(result);
            return json;
        }
        public async Task<TopGamesModel> GetTopGameAsync()
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), $"https://api.twitch.tv/helix/games/top");
            request.Headers.Add("Client-ID", "0pje11teayzq9z2najlxgdcc5d2dy1");
            var response = await httpClient.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<TopGamesModel>(result);
            return json;
        }
        public async Task<SearchStreamModel> SearchStream(string param)
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), $"https://api.twitch.tv/kraken/search/streams?query={param}");
            request.Headers.Add("Client-ID", "0pje11teayzq9z2najlxgdcc5d2dy1");
            request.Headers.Add("Accept", "application/vnd.twitchtv.v5+json");
            var response = await httpClient.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<SearchStreamModel>(result);
            return json;
        }
    }
}
