using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Twitch_API.Model
{
    class StreamInfoModel : INotifyPropertyChanged
    {
        private string user_name;
        private string title;
        private int user_id;
        private int viewer_count;
        public string User_name
        {
            get { return user_name; }
            set
            {
                user_name = value;
                OnPropertyChanged("User_name");
            }
        }
        public int User_id
        {
            get { return user_id; }
            set
            {
                user_id = value;
                OnPropertyChanged("User_id");
            }
        }
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }
        public int Viewer_count
        {
            get { return viewer_count; }
            set
            {
                viewer_count = value;
                OnPropertyChanged("Viewer_count");
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
