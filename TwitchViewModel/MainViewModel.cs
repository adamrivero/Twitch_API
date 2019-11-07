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
using TwitchModel;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using TwitchAPIlibrary;

namespace TwitchViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        ApplicationDataContainer localData;
        readonly ApiRequest API;
        public ObservableCollection<ToGamesModel> topGameModels { get; private set; }
        public ObservableCollection<StreamModel> streamModels { get; private set; }
        public MainViewModel()
        {
            localData = ApplicationData.Current.LocalSettings;
            API = new ApiRequest();
            topGameModels = new ObservableCollection<ToGamesModel>();
            streamModels = new ObservableCollection<StreamModel>();
            GetStreams();
        }
        public async void ClickCommand(object sender, object parameter)
        {
            var arg = parameter as ItemClickEventArgs;
            var item = arg.ClickedItem as StreamModel;
            var UserLogin = JsonConvert.DeserializeObject<UserModel>(await API.GetUserInfoAsync(item.Id));
            localData.Values["User_login"] = UserLogin.data.FirstOrDefault().login;
        }
        private async void GetStreams()
        {
            string param = "";
            var streams = JsonConvert.DeserializeObject<StreamModel>(await API.GetStreamInfoAsync());
            var TopGames = JsonConvert.DeserializeObject<ToGamesModel>(await API.GetTopGameAsync());
            foreach (var item in streams.data)
            {
                param += item.game_id + "&id=";
            }
            var games = JsonConvert.DeserializeObject<GameModel>(await API.GetGameInfoAsync(param));
            foreach (var stream in streams.data)
            {
                        string set_preview_size = stream.thumbnail_url.Replace("{width}", "350");
                        set_preview_size = set_preview_size.Replace("{height}", "200");
                        streamModels.Add(new StreamModel { Thumbnail_url = set_preview_size, 
                            Game_name = games.data.FirstOrDefault(item => item.id == stream.game_id.ToString()).name, 
                            User_name = stream.user_name, 
                            Viewer_count = stream.viewer_count, 
                            Id = stream.user_id });
            }
            foreach (var TopGame in TopGames.data)
            {
                string set_atr_size = TopGame.box_art_url.Replace("{width}", "600");
                set_atr_size = set_atr_size.Replace("{height}", "675");
                topGameModels.Add(new ToGamesModel{ Box_art_url = set_atr_size, Id = TopGame.id, Name = TopGame.name });
            }
            
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
