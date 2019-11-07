using Windows.UI.Xaml.Controls;

namespace Twitch_API.View
{
    public sealed partial class MediaView : Page
    {
        public MediaView()
        {
            this.InitializeComponent();
            DataContext = new TwitchViewModel.MediaViewModel();
        }

        private void AppBarButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainView));
        }
    }
}
