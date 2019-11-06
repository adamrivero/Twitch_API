using GalaSoft.MvvmLight.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Twitch_API.Model;
using Twitch_API.View;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Twitch_API.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        private AllInfo selectedInfo;
        public AllInfo SelectedInfo
        {
            get { return selectedInfo; }
            set
            {
                selectedInfo = value;
                OnPropertyChanged("SelectedInfo");
            }
        }
        public ObservableCollection<AllInfo> allInfos { get; private set; }
        public MainViewModel()
        {
            allInfos = new ObservableCollection<AllInfo>();
            //SelectedStream.StreamVideoUri = await Uri2Async("csruhub");
            GetStreams();
            SelectedInfo = new AllInfo();
        }
        public async void ClickCommand(object sender, object parameter)
        {
            ApplicationDataContainer localData = ApplicationData.Current.LocalSettings;
            var arg = parameter as Windows.UI.Xaml.Controls.ItemClickEventArgs;
            var item = arg.ClickedItem as Model.AllInfo;
            var UserLogin = await GetUserInfoAsync(item.id);
            localData.Values["User_login"] = UserLogin.data[0].login;
            NavigateService.GoTo("Media");
        }
        private async void GetStreams()
        {
            HttpClient httpClient = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), "https://api.twitch.tv/helix/streams");

            request.Headers.Add("Client-ID", "0pje11teayzq9z2najlxgdcc5d2dy1");

            var response = await httpClient.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            var streams = JsonConvert.DeserializeObject<StreamModel>(result);
            foreach (var stream in streams.data)
            {
                var games = await GetGameInfoAsync(stream.game_id.ToString());
                    foreach (var game in games.data)
                    {
                        string set_preview_size = stream.thumbnail_url.Replace("{width}", "350");
                        set_preview_size = set_preview_size.Replace("{height}", "200");
                        allInfos.Add(new AllInfo { thumbnail_url = set_preview_size, game_name = game.name, user_name = stream.user_name, viewer_count = stream.viewer_count, id = stream.user_id });
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
