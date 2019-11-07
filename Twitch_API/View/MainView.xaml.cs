using Windows.UI.Xaml.Controls;

namespace Twitch_API.View
{
    public sealed partial class MainView : Page
    {
        public MainView()
        {
            this.InitializeComponent();
            DataContext = new TwitchViewModel.MainViewModel();
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(MediaView));
        }
    }
}
