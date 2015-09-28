//using System;
//using System.Collections.Generic;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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

            Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            TimerEngine = new ATMREngine(this);
        }

        private void NewTimer_Click(object sender, RoutedEventArgs e)
        {
            TimerEngine.TimerAdd(Appliance.ApplianceType.ClothesDryer);
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
