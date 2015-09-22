using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

namespace ApplianceTMR
{
    public class EnumStringAttribute : Attribute
    {
        private string msValue;

        public EnumStringAttribute(string Value)
        {
            this.msValue = Value;
        }

        public string Value
        {
            get { return msValue; }

            set { msValue = value; }
        }
    }

    public class EnumByteAttribute : Attribute
    {
        private byte mbValue = 0;

        public EnumByteAttribute(byte Value)
        {
            this.mbValue = Value;
        }

        public byte Value
        {
            get { return mbValue; }

            set { mbValue = value; }
        }
    }
    
    public class Appliance
    {
        private string msFullName = "Egg Timer";
        private string msPhrase = "Timer dinged";
        private byte mbMinutes = 5;
        private ApplianceType mtType = ApplianceType.EggTimer;

        public Appliance()
        { }

        public Appliance(
            ApplianceType Type, 
            string FullName, 
            string Phrase,
            byte Minutes)
        {
            this.FullName = FullName;
            this.Phrase = Phrase;
            this.Minutes = Minutes;
            this.Type = Type;
        }

        public enum ApplianceType
        {
            ClothesDryer,
            EggTimer,
            Fridge,
            Microwave,
            Oven,
            Stove,
            TV,
            WashingMachine,   
        }

        public string FullName
        {
            get { return msFullName; }

            set { msFullName = value; }
        }

        public byte Minutes
        {
            get { return mbMinutes; }

            set { mbMinutes = value; }
        }

        public ApplianceType Type
        {
            get { return mtType; }

            set { mtType = value; }
        }

        public string Phrase
        {
            get { return msPhrase; }

            set { msPhrase = value; }
        }
    }

    /// <summary>
    /// Functions that should be outside the UI.
    /// </summary>
    class ATMREngine
    {
        #region Private Members
        private byte mbTimerCount = 0;

        private List<Appliance> Appliances = new List<Appliance>();
        private bool mbHomePage = true;
        private SolidColorBrush mscbTileColor = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 49, 123, 193));
        private Windows.UI.Input.PointerPoint mStartingPoint;
        private MainPage mMainPage;
        #endregion

        #region Public Properties
        public SolidColorBrush TileColor
        {
            get {return mscbTileColor;}
        }
        #endregion
        
        public ATMREngine(MainPage mainPage)
        {
            this.mMainPage = mainPage;
            //this.mMainPage
        }

        public Appliance ApplianceByType(Appliance.ApplianceType Type)
        {
            Appliance appliance = new Appliance();

            try
            {
                bool bFound = false;
                List<string> ApplianceValues = new List<string>();
                var tempValues = ((string[])ApplicationData.Current.LocalSettings.Values["ApplianceValues"]);

                if (tempValues != null)
                {
                    ApplianceValues = tempValues.ToList();

                    if (ApplianceValues.Count > 0)
                    {
                        string sValue = ApplianceValues.Find(e => (e.IndexOf(Type.ToString()) > -1));

                        if (sValue != null)
                        {
                            appliance = ApplianceFromCSV(sValue);
                        }
                    }
                }


                if (bFound == false)
                {
                    appliance = ApplianceDefaults(Type);
                    ApplianceValues.Add(appliance.Type.ToString() + "," + appliance.Minutes.ToString() + "," + appliance.FullName + "," + appliance.Phrase);
                    ApplicationData.Current.LocalSettings.Values["ApplianceValues"] = ApplianceValues.ToArray();
                }
            }
            catch (Exception ex)
            {
                logException(ex);
            }

            return appliance;
        }

        public Image ApplianceIconFromType(Appliance.ApplianceType Type)
        {
            Image Return = new Image();
            string sIcon = "Egg_Timer";

            try
            {
                switch (Type)
                {
                    case Appliance.ApplianceType.ClothesDryer:
                        sIcon = "Clothes-Dryer";
                        break;

                    case Appliance.ApplianceType.EggTimer:
                        sIcon = "Egg_Timer";
                        break;

                    case Appliance.ApplianceType.Fridge:
                        sIcon = "Fridge";
                        break;

                    case Appliance.ApplianceType.Microwave:
                        sIcon = "Microwave";
                        break;

                    case Appliance.ApplianceType.Oven:
                        sIcon = "Oven";
                        break;

                    case Appliance.ApplianceType.Stove:
                        sIcon = "Stove";
                        break;

                    case Appliance.ApplianceType.TV:
                        sIcon = "TV";
                        break;

                    case Appliance.ApplianceType.WashingMachine:
                        sIcon = "Washing_Machine";
                        break;
                }

                Return.Source = new BitmapImage(new Uri("ms-appx:///Assets/" + sIcon + ".png", UriKind.Absolute));
            }
            catch (Exception ex)
            {
                logException(ex);
            }

            return Return;
        }

        private Appliance.ApplianceType ApplianceTypeFromType(string Type)
        {
            Appliance.ApplianceType ReturnType = Appliance.ApplianceType.EggTimer;

            try
            {
                switch (Type.ToLower())
                {
                    case "clothesdryer":
                        ReturnType= Appliance.ApplianceType.ClothesDryer;
                        break;

                    case "eggtimer":
                        ReturnType = Appliance.ApplianceType.EggTimer;
                        break;

                    case "fridge":
                        ReturnType = Appliance.ApplianceType.Fridge;
                        break;

                    case "microwave":
                        ReturnType = Appliance.ApplianceType.Microwave;
                        break;

                    case "oven":
                        ReturnType = Appliance.ApplianceType.Oven;
                        break;

                    case "stove":
                        ReturnType = Appliance.ApplianceType.Stove;
                        break;

                    case "tv":
                        ReturnType = Appliance.ApplianceType.TV;
                        break;

                    case "washingmachine":
                        ReturnType = Appliance.ApplianceType.WashingMachine;
                        break;

                }
            }
            catch (Exception ex)
            {
                logException(ex);
            }

            return ReturnType;
        }

        private Appliance ApplianceFromCSV(string Input)
        {
            Appliance appliance = new Appliance();

            try
            {
                string[] Values = Input.Split(',');

                appliance.Type = this.ApplianceTypeFromType(Values[0]);
                appliance.Minutes = Convert.ToByte(Values[1]);
                appliance.FullName = Values[2];
                appliance.Phrase = Values[3];

            }
            catch (Exception ex)
            {
                logException(ex);
            }

            return appliance;
        }

        private Appliance ApplianceDefaults(Appliance.ApplianceType Type)
        {
            Appliance appliance = new Appliance(Type, "Egg Timer", "Timer dinged", 8);

            try
            {
                switch (Type)
                {
                    case Appliance.ApplianceType.ClothesDryer:
                        appliance.FullName = "Clothes Dryer";
                        appliance.Phrase = "The clothes are dry";
                        appliance.Minutes = 38;
                        break;

                    case Appliance.ApplianceType.EggTimer:
                        appliance.FullName = "Egg Timer";
                        appliance.Phrase = "Timer dinged";
                        appliance.Minutes = 5;
                        break;

                    case Appliance.ApplianceType.Fridge:
                        appliance.FullName = "Refrigerator";
                        appliance.Phrase = "Somethings chilling";
                        appliance.Minutes = 120;
                        break;

                    case Appliance.ApplianceType.Microwave:
                        appliance.FullName = "Microwave";
                        appliance.Phrase = "Atomic Beep Beep";
                        appliance.Minutes = 10;
                        break;

                    case Appliance.ApplianceType.Oven:
                        appliance.FullName = "Oven";
                        appliance.Phrase = "Oven is hot";
                        appliance.Minutes = 25;
                        break;

                    case Appliance.ApplianceType.Stove:
                        appliance.FullName = "Stove";
                        appliance.Phrase = "Stove needs attention";
                        appliance.Minutes = 15;
                        break;

                    case Appliance.ApplianceType.TV:
                        appliance.FullName = "TV / Computer";
                        appliance.Phrase = "Pay attention to this device";
                        appliance.Minutes = 60;
                        break;

                    case Appliance.ApplianceType.WashingMachine:
                        appliance.FullName = "Washing Machine";
                        appliance.Phrase = "The wash is done";
                        appliance.Minutes = 60;
                        break;
                }


            }
            catch (Exception ex)
            {
                logException(ex);
            }

            return appliance;
        }

        public void TimersUnload()
        {
            try
            {
                Storyboard sb = new Storyboard();

                foreach(TimerTile timerTile in mMainPage.Timers.Children)
                {
                    DoubleAnimation FadeOut = new DoubleAnimation();
                    FadeOut.Duration = new Windows.UI.Xaml.Duration(TimeSpan.FromMilliseconds(400));
                    FadeOut.From = 1.0;
                    FadeOut.To = 0.0;

                    Storyboard.SetTarget(FadeOut, timerTile);
                    Storyboard.SetTargetProperty(FadeOut, "(Canvas.Opacity)");

                    sb.Children.Add(FadeOut);
                }

                if (sb.Children.Count > 0)
                {
                    sb.Begin();
                }
            }
            catch (Exception ex)
            {
                logException(ex);
            }
        }

        public void TimersReload()
        {
            try
            {
                Storyboard sb = new Storyboard();

                foreach (TimerTile timerTile in mMainPage.Timers.Children)
                {
                    DoubleAnimation FadeIn = new DoubleAnimation();
                    FadeIn.Duration = new Windows.UI.Xaml.Duration(TimeSpan.FromMilliseconds(400));
                    FadeIn.From = 0.0;
                    FadeIn.To = 1.0;

                    Storyboard.SetTarget(FadeIn, timerTile);
                    Storyboard.SetTargetProperty(FadeIn, "(Canvas.Opacity)");

                    sb.Children.Add(FadeIn);

                    mbTimerCount += 1; //Reset, temp
                }

                if (sb.Children.Count > 0)
                {
                    sb.Begin();
                }
            }
            catch (Exception ex)
            {
                logException(ex);
            }
        }

        public void TimersLoadDefault()
        {
            try
            {
                string[] types = Enum.GetNames(typeof(Appliance.ApplianceType)); 
                foreach (string type in types)
                {
                    TimerLoad(this.ApplianceTypeFromType(type));
                } 
            }
            catch (Exception ex)
            {
                logException(ex);
            }
        }

        public void TimerLoad(Appliance.ApplianceType Type)
        {
            try
            {
                this.mMainPage.NewTimer.IsEnabled = false;

                AppBar dBottomAppBar = this.mMainPage.BottomAppBar;
                double dSize = Convert.ToDouble((this.mMainPage.ActualHeight - (dBottomAppBar.ActualHeight * 2)) / 3);


                TimerTile timerTile = new TimerTile(
                    new TimeSpan(0, this.ApplianceByType(Type).Minutes, 0), 
                    this.TileColor,
                    this.ApplianceIconFromType(Type));
                timerTile.Width = this.mMainPage.ActualWidth;
                timerTile.Height = dSize;
                this.mMainPage.Timers.Children.Add(timerTile);

                
                Storyboard AddTile = new Storyboard();
                QuadraticEase ease = new QuadraticEase();
                ease.EasingMode = EasingMode.EaseIn;
                
                DoubleAnimation MoveAnimation = new DoubleAnimation();
                MoveAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(250));
                MoveAnimation.From = this.mMainPage.ActualHeight;
                MoveAnimation.To = (mbTimerCount * dSize);
                MoveAnimation.EasingFunction = ease;

                Storyboard.SetTarget(MoveAnimation, timerTile);
                Storyboard.SetTargetProperty(MoveAnimation, "(Canvas.Top)");

                AddTile.Children.Add(MoveAnimation);
                AddTile.Completed += (sendr, args) =>
                {
                    this.mMainPage.NewTimer.IsEnabled = true;
                };
                AddTile.Begin();

                mbTimerCount += 1; //TODO: need a better process
            }
            catch (Exception ex)
            {
                logException(ex);
            }
        }

        public void SettingsSelected()
        {
            try
            {
                if (mbHomePage == true)
                {
                    mMainPage.Settings.Icon = new SymbolIcon(Symbol.Home);
                    mMainPage.Settings.Label = "Home";

                    TimersUnload();

                    mbTimerCount = 0;

                    TimersLoadDefault();
                }
                else
                {
                    mMainPage.Settings.Icon = new SymbolIcon(Symbol.Setting);
                    mMainPage.Settings.Label = "Settings";

                    TimersReload();
                }

                mbHomePage = !mbHomePage;
            }
            catch (Exception ex)
            {
                logException(ex);
            }
        }

        public void TouchStarted(Windows.UI.Input.PointerPoint StartingPoint)
        {
            mStartingPoint = StartingPoint;
        }

        public void TouchCompleted(Windows.UI.Input.PointerPoint EndingPoint)
        {
            try
            {
                if (mStartingPoint.Position.X > EndingPoint.Position.X)
                {
                    System.Diagnostics.Debug.WriteLine("down");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("up"); 
                }
            }
            catch (Exception ex)
            {
                logException(ex);
            }
        }




        public static bool CanSendToasts()
        {
            bool bReturn = true;
            var notifier = ToastNotificationManager.CreateToastNotifier();

            if (notifier.Setting != NotificationSetting.Enabled)
            {
                bReturn = false;
            }

            return bReturn;
        }


        /// <summary>
        /// Code found here.  Just want to send a notification and this does it, nice and simple.
        /// https://code.msdn.microsoft.com/windowsapps/Action-Center-Quickstart-b15089f2/sourcecode?fileId=111808&pathId=298223712
        /// </summary>
        public void SentToast(string Title, string Body)
        {
            try
            {
                if (CanSendToasts() == true)
                {
                    // Use a helper method to create a new ToastNotification.
                    // Each time we run this code, we'll append the count (toastIndex in this example) to the message
                    // so that it can be seen as a unique message in the action center. This is not mandatory - we
                    // do it here for educational purposes.
                    ToastNotification toast = CreateTextOnlyToast(Title, Body); 

                    // Optional. Setting an expiration time on a toast notification defines the maximum
                    // time the notification will be displayed in action center before it expires and is removed. 
                    // If this property is not set, the notification expires after 7 days and is removed.
                    // Tapping on a toast in action center launches the app and removes it immediately from action center.
                    toast.ExpirationTime = DateTimeOffset.UtcNow.AddSeconds(3600);

                    ToastNotificationManager.CreateToastNotifier().Show(toast);
                }
            }
            catch (Exception ex)
            {
                logException(ex);
            }
        }

        public static ToastNotification CreateTextOnlyToast(string toastHeading, string toastBody)
        {
            // Using the ToastText02 toast template.
            ToastTemplateType toastTemplate = ToastTemplateType.ToastText02;

            // Retrieve the content part of the toast so we can change the text.
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);

            //Find the text component of the content
            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");

            // Set the text on the toast. 
            // The first line of text in the ToastText02 template is treated as header text, and will be bold.
            toastTextElements[0].AppendChild(toastXml.CreateTextNode(toastHeading));
            toastTextElements[1].AppendChild(toastXml.CreateTextNode(toastBody));

            // Set the duration on the toast
            IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            ((XmlElement)toastNode).SetAttribute("duration", "long");

            // Create the actual toast object using this toast specification.
            ToastNotification toast = new ToastNotification(toastXml);

            return toast;
        }

        public void SaveSetting(string Setting, string Value)
        {
            try
            {
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

                localSettings.Values[Setting] = Value;

                localSettings = null;
            }
            catch (Exception ex)
            {
                logException(ex);
            }
        }

        public void logException(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
    }
}
