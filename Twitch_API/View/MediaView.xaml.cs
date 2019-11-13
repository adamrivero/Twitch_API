using Windows.Devices.Enumeration;
using Windows.Media.Casting;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;
using System.Diagnostics;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using ScreenCasting.Util;
using ScreenCasting;
using Windows.UI.ViewManagement;
using Windows.Foundation;
using Windows.UI.Xaml.Media;

namespace Twitch_API.View
{
    public sealed partial class MediaView : Page
    {
        public static MediaView Current;
        DeviceInformation selectedDevice;
        private DevicePicker picker;
        ProjectionViewBroker pvb = new ProjectionViewBroker();
        public ViewLifetimeControl ProjectionViewPageControl;
        DeviceInformation activeDevice = null;
        int thisViewId;
        public MediaView()
        {
            this.InitializeComponent();
            picker = new DevicePicker();
            picker.Filter.SupportedDeviceSelectors.Add(ProjectionManager.GetDeviceSelector());
            picker.DeviceSelected += Picker_DeviceSelected;
            picker.DisconnectButtonClicked += Picker_DisconnectButtonClicked;
        }

        private async void Picker_DeviceSelected(DevicePicker sender, DeviceSelectedEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                try
                {
                    picker.SetDisplayStatus(args.SelectedDevice, "Connecting", DevicePickerDisplayStatusOptions.ShowProgress);

                    // Set status to Connecting

                    // Getting the selected device improves debugging
                    selectedDevice = args.SelectedDevice;

                    thisViewId = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().Id;

                    // If projection is already in progress, then it could be shown on the monitor again
                    // Otherwise, we need to create a new view to show the presentation

                    // First, create a new, blank view
                    var thisDispatcher = Window.Current.Dispatcher;
                    await CoreApplication.CreateNewView().Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        // ViewLifetimeControl is a wrapper to make sure the view is closed only
                        // when the app is done with it
                        ProjectionViewPageControl = ViewLifetimeControl.CreateForCurrentView();

                        // Assemble some data necessary for the new page
                        pvb.MainPageDispatcher = thisDispatcher;
                        pvb.ProjectionViewPageControl = ProjectionViewPageControl;
                        pvb.MainViewId = thisViewId;

                        // Display the page in the view. Note that the view will not become visible
                        // until "StartProjectingAsync" is called
                        var rootFrame = new Frame();
                        rootFrame.Navigate(typeof(ProjectionViewPage), pvb);
                        Window.Current.Content = rootFrame;

                        Window.Current.Activate();
                    });


                    try
                    {
                        // Start/StopViewInUse are used to signal that the app is interacting with the
                        // view, so it shouldn't be closed yet, even if the user loses access to it
                        ProjectionViewPageControl.StartViewInUse();

                        try
                        {
                            await ProjectionManager.StartProjectingAsync(ProjectionViewPageControl.Id, thisViewId, selectedDevice);

                        }
                        catch (Exception ex)
                        {
                            if (!ProjectionManager.ProjectionDisplayAvailable || pvb.ProjectedPage == null)
                                throw ex;
                        }

                        // ProjectionManager currently can throw an exception even when projection has started.\
                        // Re-throw the exception when projection has not been started after calling StartProjectingAsync 
                        if (ProjectionManager.ProjectionDisplayAvailable && pvb.ProjectedPage != null)
                        {
                            this.player.Pause();
                            await pvb.ProjectedPage.SetMediaSource(this.player.Source, this.player.Position);
                            activeDevice = selectedDevice;
                            // Set status to Connected
                            picker.SetDisplayStatus(args.SelectedDevice, "Connected", DevicePickerDisplayStatusOptions.ShowDisconnectButton);
                            picker.Hide();
                        }
                        else
                        {
                          
                            // Set status to Failed
                            picker.SetDisplayStatus(args.SelectedDevice, "Connection Failed", DevicePickerDisplayStatusOptions.ShowRetryButton);
                        }
                    }
                    catch (Exception)
                    {
                        // Set status to Failed
                        try { picker.SetDisplayStatus(args.SelectedDevice, "Connection Failed", DevicePickerDisplayStatusOptions.ShowRetryButton); } catch { }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            });
        }

        private  void startWatcherButton_Click(object sender, RoutedEventArgs e)
        {
            picker.Show(new Rect(100, 100, 100, 100), Windows.UI.Popups.Placement.Above);
        }
        private async void Picker_DevicePickerDismissed(DevicePicker sender, object args)
        {
            //Casting must occur from the UI thread.  This dispatches the casting calls to the UI thread.
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (activeDevice == null)
                {
                    player.Play();
                }
            });
        }
        private async void Picker_DisconnectButtonClicked(DevicePicker sender, DeviceDisconnectButtonClickedEventArgs args)
        {
            //Casting must occur from the UI thread.  This dispatches the casting calls to the UI thread.
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                //Update the display status for the selected device.
                sender.SetDisplayStatus(args.Device, "Disconnecting", DevicePickerDisplayStatusOptions.ShowProgress);

                if (this.pvb.ProjectedPage != null)
                    this.pvb.ProjectedPage.StopProjecting();

                //Update the display status for the selected device.
                sender.SetDisplayStatus(args.Device, "Disconnected", DevicePickerDisplayStatusOptions.None);

                // Set the active device variables to null
                activeDevice = null;
            });
        }
    }
}
