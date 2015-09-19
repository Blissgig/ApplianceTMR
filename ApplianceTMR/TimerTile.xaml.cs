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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ApplianceTMR
{
    public sealed partial class TimerTile : UserControl
    {
        private TimeSpan mTimerTime;

        public TimerTile()
        {
            this.InitializeComponent();
        }

        public void SetDefaults(TimeSpan Time)
        {
            try
            {
                mTimerTime = Time;

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

                UpdateTimerDisplay();
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


    }
}
