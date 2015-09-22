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
    public sealed partial class MainPage : Page
    {
        private byte mbTimerCount = 0;
        private bool mbHomePage = true;


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

                ATMREngine engine = new ATMREngine();
                AppBar dBottomAppBar = this.BottomAppBar;
                Appliance.ApplianceType Type = Appliance.ApplianceType.Stove;
                double dSize = Convert.ToDouble((this.ActualHeight - dBottomAppBar.ActualHeight) / 3);


                TimerTile timerTile = new TimerTile(
                    new TimeSpan(0, engine.ApplianceTime(Type), 0), 
                    engine.TileColor,
                    engine.ApplianceIconFromType(Type));
                timerTile.Width = this.ActualWidth;
                timerTile.Height = dSize;
                this.Timers.Children.Add(timerTile);

                
                Storyboard AddTile = new Storyboard();
                QuadraticEase ease = new QuadraticEase();
                ease.EasingMode = EasingMode.EaseIn;
                
                DoubleAnimation MoveAnimation = new DoubleAnimation();
                MoveAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(250));
                MoveAnimation.From = this.ActualHeight;
                MoveAnimation.To = (mbTimerCount * dSize);
                MoveAnimation.EasingFunction = ease;

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
                ATMREngine engine = new ATMREngine();

                if (mbHomePage == true)
                {
                    Settings.Icon = new SymbolIcon(Symbol.Home);
                    Settings.Label = "Home";

                    engine.TimersUnload(this.Timers);
                }
                else
                {
                    Settings.Icon = new SymbolIcon(Symbol.Setting);
                    Settings.Label = "Settings";

                    engine.TimersReload(this.Timers);
                }

                mbHomePage = !mbHomePage;
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
                string appName = "Appliance TMR";
                var version = Package.Current.Id.Version;
                
                string Message =
                    "Site: Blissgig.com" + Environment.NewLine +
                    "Contact: Blissgig@gmail.com" + Environment.NewLine +
                    "Copyright 2015 James Rose" + Environment.NewLine +
                    "Version: " + String.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision); ;


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
            try
            {
                ATMREngine engine = new ATMREngine();
                engine.logException(ex);
                engine = null;
            }
            catch {}
        }


    }
}
