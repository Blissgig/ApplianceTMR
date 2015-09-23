using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace ApplianceTMR
{
    public sealed partial class MainPage : Page
    {
        public ATMREngine TimerEngine;

        
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            TimerEngine = new ATMREngine(this);
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
            TimerEngine.TimerNew(Appliance.ApplianceType.Stove);
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            TimerEngine.SettingsSelected();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            TimerEngine.AboutSelected();
        }

        private void Timers_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            TimerEngine.TouchStarted(e.GetCurrentPoint(this));
        }

        private void Timers_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            TimerEngine.TouchCompleted(e.GetCurrentPoint(this));
        }
    }
}
