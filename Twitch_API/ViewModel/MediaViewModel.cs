using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Web;
using Twitch_API.Model;
using System.Net;
using Windows.Services.Store;
using Windows.Storage;

namespace Twitch_API.ViewModel
{
    class MediaViewModel : INotifyPropertyChanged
    {
        private AllInfo selectedInfo;
        private string _login;
        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                OnPropertyChanged("Login");
            }
        }
        public AllInfo SelectedInfo
        {
            get { return selectedInfo; }
            set
            {
                selectedInfo = value;
                OnPropertyChanged("SelectedInfo");
            }
        }
        public ICommand GetCommand { get; private set; }
        public ICommand VideoCommand { get; private set; }
        public MediaViewModel()
        {
            SelectedInfo = new AllInfo();
            ApplicationDataContainer localData = ApplicationData.Current.LocalSettings;
            Login = (string)localData.Values["User_login"];
            //SelectedStream.StreamVideoUri = await Uri2Async("csruhub");
            //GetStreams();
            if(Login != null)
            {
                GetStreams();
            }
            GetCommand = new DelegateCommand(GetStreams);
        }
        private async void GetStreams(Object obj)
        {
            await FindByLoginAsync();
        }
        private async void GetStreams()
        {
            HttpClient httpClient = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), $"https://api.twitch.tv/helix/streams?user_login={Login}");

            request.Headers.Add("Client-ID", "0pje11teayzq9z2najlxgdcc5d2dy1");

            var response = await httpClient.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            var streams = JsonConvert.DeserializeObject<StreamModel>(result);
            foreach(var stream in streams.data)
            {
                var users = await GetUserInfoAsync(stream.user_id);
                var games = await GetGameInfoAsync(stream.game_id.ToString());
                foreach (var user in users.data)
                {
                    foreach (var game in games.data)
                    {
                        string set_atr_size = game.box_art_url.Replace("{width}", "600");
                        set_atr_size = set_atr_size.Replace("{height}", "675");
                        SelectedInfo = new AllInfo { box_art_url = set_atr_size, game_name = game.name, streamVideoURI = await UriAsync(Login), broadcaster_type = user.broadcaster_type, user_id = stream.user_id, description = user.description, display_name = user.display_name, email = user.email, game_id = stream.game_id, id = user.id, language = stream.language, login = user.login, offline_image_url = user.offline_image_url, profile_image_url = user.profile_image_url, started_at = stream.started_at, thumbnail_url = stream.thumbnail_url, title = stream.title, type = stream.type, user_name = stream.user_name, viewer_count = stream.viewer_count, view_count = user.view_count };
                    }
                }
            }
        }
        private async Task<Uri> UriAsync(string login)
        {
            using (var httpClient = new HttpClient())
            {
                //WebUtility.UrlEncode(login);
                
                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), $"https://api.twitch.tv/api/channels/{login}/access_token?client_id=0pje11teayzq9z2najlxgdcc5d2dy1");
                var response = await httpClient.SendAsync(request);
                var result = await response.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<RootObject>(result);
                string token = json.token;
                string sig = json.sig;
                return new Uri($"http://usher.twitch.tv/api/channel/hls/{login}.m3u8?player=twitchweb&token={token}&sig={sig}");
            }
        }
        public class RootObject
        {
            public string token { get; set; }
            public string sig { get; set; }
            public bool mobile_restricted { get; set; }
            public DateTime expires_at { get; set; }
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
        public async Task FindByLoginAsync()
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), $"https://api.twitch.tv/helix/streams?user_login={Login}");
            request.Headers.Add("Client-ID", "0pje11teayzq9z2najlxgdcc5d2dy1");
            var response = await httpClient.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<StreamModel>(result);
            foreach (var stream in json.data)
            {
                var users = await GetUserInfoAsync(stream.user_id);
                var games = await GetGameInfoAsync(stream.game_id.ToString());
                foreach (var user in users.data)
                {
                    foreach (var game in games.data)
                    {
                        string set_atr_size = game.box_art_url.Replace("{width}", "600");
                        set_atr_size = set_atr_size.Replace("{height}", "675");
                      //  allInfos.Add(new AllInfo { box_art_url = set_atr_size, game_name = game.name, streamVideoURI = await UriAsync(user.login), broadcaster_type = user.broadcaster_type, user_id = stream.user_id, description = user.description, display_name = user.display_name, email = user.email, game_id = stream.game_id, id = user.id, language = stream.language, login = user.login, offline_image_url = user.offline_image_url, profile_image_url = user.profile_image_url, started_at = stream.started_at, thumbnail_url = stream.thumbnail_url, title = stream.title, type = stream.type, user_name = stream.user_name, viewer_count = stream.viewer_count, view_count = user.view_count });
                    }
                }
            }
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}