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
using System.Net;
using Windows.Services.Store;
using Windows.Storage;
using TwitchModel;
using TwitchAPIlibrary;

namespace TwitchViewModel
{
    public class MediaViewModel : INotifyPropertyChanged
    {
        ApplicationDataContainer localData;
        private MediaModel selectedMedia;
        readonly ApiRequest API;
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
        public MediaModel SelectedMedia
        {
            get { return selectedMedia; }
            set
            {
                selectedMedia = value;
                OnPropertyChanged("SelectedMedia");
            }
        }
        public MediaViewModel()
        {
            localData = ApplicationData.Current.LocalSettings;
            API = new ApiRequest();
            SelectedMedia = new MediaModel();
            Login = (string)localData.Values["User_login"];
            GetStreams();
        }
        private async void GetStreams()
        {
            var streams = JsonConvert.DeserializeObject<StreamModel>(await API.GetStreamInfoAsync(Login));
            foreach (var stream in streams.data)
            {
                var users = JsonConvert.DeserializeObject<UserModel>(await API.GetUserInfoAsync(stream.user_id));
                var games = JsonConvert.DeserializeObject<GameModel>(await API.GetGameInfoAsync(stream.game_id.ToString()));
                string set_atr_size = games.data.FirstOrDefault(a => a.id == stream.game_id.ToString()).box_art_url.Replace("{width}", "600");
                set_atr_size = set_atr_size.Replace("{height}", "675");
                var video_uri = JsonConvert.DeserializeObject<VideoURIModel>(await API.UriAsync(Login));
                SelectedMedia = new MediaModel
                {
                    Atr_url = set_atr_size,
                    Game_name = games.data.FirstOrDefault(a => a.id == stream.game_id.ToString()).name,
                    Video_source = video_uri.urls.The720P,
                    Description = users.data.FirstOrDefault(a => a.id == stream.user_id).description,
                    Display_name = users.data.FirstOrDefault(a => a.id == stream.user_id).display_name,
                    Title = stream.title,
                    Viewer_count = stream.viewer_count,
                    View_count = users.data.FirstOrDefault(a => a.id == stream.user_id).view_count
                };
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}