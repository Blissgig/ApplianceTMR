using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace ApplianceTMR
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private byte mbTimerCount = 0;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void NewTimer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.NewTimer.IsEnabled = false; //To insure that that multiple timers are started at the same time... may remove this.

                AppBar dBottomAppBar = this.BottomAppBar; 

                double dSize = Convert.ToDouble((this.ActualHeight - dBottomAppBar.ActualHeight) / 3);

                TimerTile timerTile = new TimerTile();

                timerTile.Width = this.ActualWidth;
                timerTile.Height = dSize;
                timerTile.SetDefaults(new TimeSpan(0, 8, 0));
                this.Timers.Children.Add(timerTile);

                
                Storyboard AddTile = new Storyboard();
                DoubleAnimation MoveAnimation = new DoubleAnimation();
                MoveAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(400));
                MoveAnimation.From = this.ActualHeight;
                MoveAnimation.To = (mbTimerCount * dSize);

                Storyboard.SetTarget(MoveAnimation, timerTile);
                Storyboard.SetTargetProperty(MoveAnimation, "(Canvas.Top)");

                AddTile.Children.Add(MoveAnimation);
                AddTile.Completed += (sendr, args) =>
                    {
                        this.NewTimer.IsEnabled = true;
                    };
                AddTile.Begin();
                

                mbTimerCount += 1;
            }
            catch (Exception ex)
            {
                logException(ex);
                this.NewTimer.IsEnabled = true; //Just in case
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //TODO
            }
            catch (Exception ex)
            {
                logException(ex);
            }
        }

        private async void About_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string appName = "ApplianceTMR";
                var version = Package.Current.Id.Version;
                string appVersion = String.Format("{0}.{1}.{2}.{3}",
                    version.Major, version.Minor, version.Build, version.Revision);
                

                string Message =
                    "Site: Blissgig.com" + Environment.NewLine +
                    "Contact: Blissgig@gmail.com" + Environment.NewLine +
                    "Copyright 2015 James Rose" + Environment.NewLine +
                    "Version: " + appVersion;


                MessageDialog messageDialog = new MessageDialog(Message, appName);
                await messageDialog.ShowAsync();
            }
            catch (Exception ex)
            {
                logException(ex);
            }
        }

        private void logException(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }


    }
}
