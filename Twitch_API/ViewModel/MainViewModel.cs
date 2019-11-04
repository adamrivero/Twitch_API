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

namespace Twitch_API.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        private StreamInfoModel selectedStream;
        private string _login;
        private Uri _videoUri;
        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                OnPropertyChanged("Login");
            }
        }
        public Uri VideoUri
        {
            get { return _videoUri; }
            set { _videoUri = value; OnPropertyChanged("VideoUri"); }
        }
        public StreamInfoModel SelectedStream
        {
            get { return selectedStream; }
            set { selectedStream = value;
                OnPropertyChanged("SelectedStream");
            }
        }
        public ObservableCollection<StreamInfoModel> streams { get; private set; }
        public ICommand GetCommand { get; private set; }
        public ICommand VideoCommand { get; private set; }
        public MainViewModel()
        {
            streams = new ObservableCollection<StreamInfoModel>();
            GetStreams();
            GetCommand = new DelegateCommand(GetStreams);
            VideoCommand = new DelegateCommand(GetVideo);
        }
        private async void GetVideo(Object obj)
        {
            HttpClient httpClient = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), $"https://api.twitch.tv/helix/videos?user_id={streams[0].User_id}");

            request.Headers.Add("Client-ID", "0pje11teayzq9z2najlxgdcc5d2dy1");

            var response = await httpClient.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<StreamModel>(result);
            streams.Clear();
            foreach (var item in json.data)
            {
                streams.Add(new StreamInfoModel { Title = item.title, User_name = item.user_name, User_id = item.user_id, Viewer_count = item.viewer_count });
            }
        }
        private async void GetStreams(Object obj)
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), $"https://api.twitch.tv/helix/streams?user_login={Login}");
            request.Headers.Add("Client-ID", "0pje11teayzq9z2najlxgdcc5d2dy1");
            var response = await httpClient.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<StreamModel>(result);
            streams.Clear();
            foreach (var item in json.data)
            {
                streams.Add(new StreamInfoModel { Title = item.title, User_name = item.user_name, User_id = item.user_id, Viewer_count = item.viewer_count });
            }
            request = new HttpRequestMessage(new HttpMethod("GET"), $"https://pwn.sh/tools/streamapi.py?url=twitch.tv/{Login}");
            response = await httpClient.SendAsync(request);
            result = await response.Content.ReadAsStringAsync();
            var Video_json = JsonConvert.DeserializeObject<VideoModel>(result);
            if(Video_json.success == true)
            VideoUri = new Uri(Video_json.urls._720p);
        }
        private async void GetStreams()
        {
            HttpClient httpClient = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), "https://api.twitch.tv/helix/streams");

            request.Headers.Add("Client-ID", "0pje11teayzq9z2najlxgdcc5d2dy1");

            var response = await httpClient.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<StreamModel>(result);
            foreach (var item in json.data)
            {
                streams.Add(new StreamInfoModel { Title = item.title, User_name = item.user_name, User_id = item.user_id, Viewer_count = item.viewer_count });
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
