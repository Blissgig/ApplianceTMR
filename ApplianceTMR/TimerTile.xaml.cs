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
using Windows.UI.Xaml.Media.Imaging; //For toast

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ApplianceTMR
{
    public sealed partial class TimerTile : UserControl
    {
        private TimeSpan mTimerTime;
        private bool mbTimerRunning = false;


        public TimerTile()
        {
            this.InitializeComponent();
        }

        public TimerTile(
            TimeSpan Time, 
            SolidColorBrush TileColor,
            Image Icon)
        {
            this.InitializeComponent();

            try
            {
                mTimerTime = Time;

                this.TileBase.Background = TileColor;

                this.ApplIcon.Source = Icon.Source;

                UpdateTimerDisplay();
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        private void TimerStartStop()
        {
            try
            {
                mbTimerRunning = !mbTimerRunning;

                if (mbTimerRunning == true)
                {
                    mTimerTime = mTimerTime.Subtract(new TimeSpan(0, 0, 1));

                    if (mTimerTime.Seconds == 0)
                    {
                        mTimerTime = mTimerTime.Subtract(new TimeSpan(0, 1, 0));
                    }


                    if (mTimerTime.TotalSeconds == 0)
                    {
                        //engine.SentToast("Appliance TMR", "Washing Machine Done"); //TODO: specific device.
                        //engine = null;

                        //TODO: Stop timer
                    }
                    UpdateTimerDisplay();
                    
                }
                else
                {
                    //TODO: Stop timer
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void UpdateTimer(TimeSpan Value)
        {
            try
            {
                mTimerTime = mTimerTime.Add(Value);
                
                if (mTimerTime.TotalSeconds < 0)
                {
                    mTimerTime = new TimeSpan(0, 0, 0);
                }

                UpdateTimerDisplay();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void UpdateTimerDisplay()
        {
            try
            {
                this.ApplTime.Text = mTimerTime.Minutes.ToString();
                this.Seconds.Value = mTimerTime.Seconds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void abTimeDown_Click(object sender, RoutedEventArgs e)
        {
            UpdateTimer(new TimeSpan(0, -1, 0));
        }

        private void abTimeUp_Click(object sender, RoutedEventArgs e)
        {
            UpdateTimer(new TimeSpan(0, 1, 0));
        }

        private void ApplTime_Tapped(object sender, TappedRoutedEventArgs e)
        {
            TimerStartStop();
        }
    }
}
