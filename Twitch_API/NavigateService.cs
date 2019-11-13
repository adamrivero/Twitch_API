using GalaSoft.MvvmLight.Views;
using Twitch_API.View;

namespace Twitch_API
{
    public static class NavigateService
    {
        static NavigationService navigationService;

        static NavigateService()
        {
            navigationService = new NavigationService();
            navigationService.Configure("Media", typeof(MediaView));
            navigationService.Configure("Home", typeof(MainView));
            navigationService.Configure("Search", typeof(SearchView));
        }
        public static void GoTo(string Key)
        {
            navigationService.NavigateTo(Key);
        }
    }
}
