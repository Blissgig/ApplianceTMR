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
        private ATMREngine mEngine;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            mEngine = new ATMREngine(this);
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
            mEngine.TimerLoad(Appliance.ApplianceType.Stove);
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            mEngine.SettingsSelected();
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

        private void Timers_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            mEngine.TouchStarted(e.GetCurrentPoint(this));
        }

        private void Timers_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            mEngine.TouchCompleted(e.GetCurrentPoint(this));
        }

        private void logException(Exception ex)
        {
            mEngine.logException(ex);
        }

    }
}
