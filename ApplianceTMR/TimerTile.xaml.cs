using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Notifications;


// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ApplianceTMR
{
    public sealed partial class TimerTile : UserControl
    {
        private Windows.UI.Input.PointerPoint mStartingPoint;
        private MainPage mMainPage;

        public TimerTile()
        {
            this.InitializeComponent();
        }

        public TimerTile(
            SolidColorBrush TileColor,
            Image Icon,
            MainPage mainPage)
        {
            this.InitializeComponent();

            try
            {
                this.mMainPage = mainPage;

                this.TileBase.Background = TileColor;

                this.ApplIcon.Source = Icon.Source;

                //UpdateTimerDisplay();
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        //private void TimerStartStop()
        //{
        //    try
        //    {
        //        mbTimerRunning = !mbTimerRunning;

        //        if (mbTimerRunning == true)
        //        {
        //            mTimerTime = mTimerTime.Subtract(new TimeSpan(0, 0, 1));

        //            if (mTimerTime.Seconds == 0)
        //            {
        //                mTimerTime = mTimerTime.Subtract(new TimeSpan(0, 1, 0));
        //            }


        //            if (mTimerTime.TotalSeconds == 0)
        //            {
        //                //engine.SentToast("Appliance TMR", "Washing Machine Done"); //TODO: specific device.
        //                //engine = null;

        //                //TODO: Stop timer
        //            }
        //            UpdateTimerDisplay();
                    
        //        }
        //        else
        //        {
        //            //TODO: Stop timer
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //private void UpdateTimer(TimeSpan Value)
        //{
        //    try
        //    {
        //        mTimerTime = mTimerTime.Add(Value);
                
        //        if (mTimerTime.TotalSeconds < 0)
        //        {
        //            mTimerTime = new TimeSpan(0, 0, 0);
        //        }

        //        UpdateTimerDisplay();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //private void UpdateTimerDisplay()
        //{
        //    try
        //    {
        //        this.ApplTime.Text = mTimerTime.Minutes.ToString();
        //        this.Seconds.Value = mTimerTime.Seconds;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        private void TileBase_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            mStartingPoint = e.GetCurrentPoint(this);
        }

        private void TileBase_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                ATMREngine engine = new ATMREngine(mMainPage);

                engine.TimerSwipe(this, mStartingPoint, e.GetCurrentPoint(this));

            }
            catch (Exception)
            {
                throw;
            }
            
        }
    }
}
