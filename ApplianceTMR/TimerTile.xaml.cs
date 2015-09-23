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
            }
            catch (Exception)
            {
                throw;
            }
        }
                
        private void TileBase_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            mStartingPoint = e.GetCurrentPoint(this);
        }

        private void TileBase_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            mMainPage.TimerEngine.TimerSwipe(this, mStartingPoint, e.GetCurrentPoint(this));
        }
    }
}
