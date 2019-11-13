using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Twitch_API.Model;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace Twitch_API.ViewModel
{
    class SearchViewModel : INotifyPropertyChanged
    {
        readonly ApiRequest API;
        private string searchParam;
        InternetConnection connection;
        ApplicationDataContainer localData;
        public ObservableCollection<StreamModel> streamModels { get; private set; }
        public SearchViewModel()
        {
            localData = ApplicationData.Current.LocalSettings;
            searchParam = (string)localData.Values["Search_param"];
            streamModels = new ObservableCollection<StreamModel>();
            connection = new InternetConnection();
            API = new ApiRequest();
            GetStreams();
        }
        public async void ClickCommand(object sender, object parameter)
        {
            var arg = parameter as ItemClickEventArgs;
            var item = arg.ClickedItem as StreamModel;
            var UserLogin = await API.GetUserInfoAsync(item.Id);
            localData.Values["User_login"] = UserLogin.data.First().login;
            NavigateService.GoTo("Media");
        }
        private async void GetStreams()
        {
            connection.CheckConnection();
            if (connection.IsConnected)
            {
                var streams = await API.SearchStream($"{searchParam}");
                foreach (var stream in streams.streams)
                {
                    streamModels.Add(new StreamModel
                    {
                        Thumbnail_url = stream.preview.large,
                        Game_name = stream.game,
                        User_name = stream.channel.display_name,
                        Viewer_count = stream.viewers,
                        Id = stream.channel._id.ToString()
                    });
                }
            }
            else
            {
                var message = new MessageDialog("Проверьте соединение с интернетом", "Упс");
                await message.ShowAsync();
                Thread.Sleep(2000);
                GetStreams();
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
