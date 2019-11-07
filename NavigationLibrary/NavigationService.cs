using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twitch_API.View;

namespace NavigationLibrary
{
    public static class NavigateService
    {
        static NavigationService navigationService;

        static NavigateService()
        {
            navigationService = new NavigationService();
            navigationService.Configure("Media", typeof(MediaView));
            navigationService.Configure("Home", typeof(MainView));
        }
        public static void GoTo(string Tag)
        {
            navigationService.NavigateTo(Tag);
        }
    }
}
