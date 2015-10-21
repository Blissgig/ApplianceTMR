using System;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace ApplianceTMR
{
    public sealed partial class TimerTile : UserControl
    {
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

                this.TileBorder.Background = TileColor;

                this.ApplianceIcon.Source = Icon.Source;
            }
            catch (Exception)
            {
                throw;
            }
        }
             
        public SolidColorBrush TileColor
        {
            set { this.TileBase.Background = value; }
        }

        private void Icon_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            mMainPage.TimerEngine.TimerApplianceChange(this);
        }

        private void Rewind_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            mMainPage.TimerEngine.TimerRewind(this);
        }

        private void Play_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            mMainPage.TimerEngine.TimerPlayStop(this);
        }

        private void FastForward_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            mMainPage.TimerEngine.TimerFastForward(this);
        }

        private void Close_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            mMainPage.TimerEngine.TimerUnload(this);
        }
    }
}
