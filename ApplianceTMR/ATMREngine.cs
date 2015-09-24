﻿using System;
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
using Windows.UI.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Popups;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Background; //For toast
using System.Threading;
using Windows.UI.Core;
using System.Xml.Linq;


namespace ApplianceTMR
{
    public class Appliance
    {
        #region Private Members
        private string msName = "GUID";
        private string msFullName = "Egg Timer";
        private string msPhrase = "Timer dinged";
        private TimeSpan mtsTime = new TimeSpan(0, 5, 0);
        private bool mbIsRunning = false;
        private ApplianceType mtType = ApplianceType.EggTimer;
        #endregion

        public Appliance()
        {
            this.Name = "TimerTile" + Guid.NewGuid().ToString().Replace("-", "");
        }

        public Appliance(
            ApplianceType Type, 
            string FullName, 
            string Phrase,
            TimeSpan Time)
        {
            this.Name = "TimerTile" + Guid.NewGuid().ToString().Replace("-", "");
            this.FullName = FullName;
            this.Phrase = Phrase;
            this.mtsTime = Time;
            this.Type = Type;
        }

        #region Public Properties
        public string Name
        {
            get { return msName; }

            set { msName = value; }
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

        public string Phrase
        {
            get { return msPhrase; }

            set { msPhrase = value; }
        }

        public TimeSpan Time
        {
            get { return mtsTime; }

            set { mtsTime = value; }
        }

        public bool IsRunning
        {
            get { return mbIsRunning; }

            set { mbIsRunning = value; }
        }

        public ApplianceType Type
        {
            get { return mtType; }

            set { mtType = value; }
        }
        #endregion
    }

    /// <summary>
    /// Functions that should be outside the UI.
    /// </summary>
    public class ATMREngine
    {
        #region Private Members
        private List<Appliance> Appliances = new List<Appliance>();
        private bool mbHomePage = true;
        private bool mbIsRunning = false;
        private SolidColorBrush mTileColor = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 49, 123, 193));
        private Windows.UI.Input.PointerPoint mStartingPoint;
        private MainPage mMainPage;
        #endregion

        #region Public Properties
        public SolidColorBrush TileColor
        {
            get {return mTileColor;}
        }
        #endregion
        
        public ATMREngine(MainPage mainPage)
        {
            this.mMainPage = mainPage;
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
                            bFound = true;
                        }
                    }
                }


                if (bFound == false)
                {
                    appliance = ApplianceDefaults(Type);
                    ApplianceValues.Add(appliance.Type.ToString() + "," + appliance.Time.TotalMinutes.ToString() + "," + appliance.FullName + "," + appliance.Phrase);
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
                appliance.Time = new TimeSpan(0, Convert.ToInt16(Values[1]), 0);
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
            Appliance appliance = new Appliance();
            appliance.Type = Type;

            try
            {
                switch (Type)
                {
                    case Appliance.ApplianceType.ClothesDryer:
                        appliance.FullName = "Clothes Dryer";
                        appliance.Phrase = "The clothes are dry";
                        appliance.Time = new TimeSpan(0, 38, 0);
                        break;

                    case Appliance.ApplianceType.EggTimer:
                        appliance.FullName = "Egg Timer";
                        appliance.Phrase = "Timer dinged";
                        appliance.Time = new TimeSpan(0, 5, 0);
                        break;

                    case Appliance.ApplianceType.Fridge:
                        appliance.FullName = "Refrigerator";
                        appliance.Phrase = "Somethings chilling";
                        appliance.Time = new TimeSpan(0, 120, 0);
                        break;

                    case Appliance.ApplianceType.Microwave:
                        appliance.FullName = "Microwave";
                        appliance.Phrase = "Atomic Beep Beep";
                        appliance.Time = new TimeSpan(0, 10, 0);
                        break;

                    case Appliance.ApplianceType.Oven:
                        appliance.FullName = "Oven";
                        appliance.Phrase = "Oven is hot";
                        appliance.Time = new TimeSpan(0, 25, 0);
                        break;

                    case Appliance.ApplianceType.Stove:
                        appliance.FullName = "Stove";
                        appliance.Phrase = "Stove needs attention";
                        appliance.Time = new TimeSpan(0, 15, 0);
                        break;

                    case Appliance.ApplianceType.TV:
                        appliance.FullName = "TV / Computer";
                        appliance.Phrase = "Pay attention to this device";
                        appliance.Time = new TimeSpan(1, 0, 0);
                        break;

                    case Appliance.ApplianceType.WashingMachine:
                        appliance.FullName = "Washing Machine";
                        appliance.Phrase = "The wash is done";
                        appliance.Time = new TimeSpan(1, 0, 0);
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
                    TimerNew(this.ApplianceTypeFromType(type));
                } 
            }
            catch (Exception ex)
            {
                logException(ex);
            }
        }

        public void TimerNew(Appliance.ApplianceType Type)
        {
            try
            {
                this.mMainPage.NewTimer.IsEnabled = false;

                AppBar dBottomAppBar = this.mMainPage.BottomAppBar;
                Appliance newAppliance = ApplianceByType(Type);
                double dSize = Convert.ToDouble((this.mMainPage.ActualHeight - (dBottomAppBar.ActualHeight * 2)) / 3);
                TimerTile timerTile = new TimerTile(this.TileColor, this.ApplianceIconFromType(Type), mMainPage);
                timerTile.Width = this.mMainPage.ActualWidth;
                timerTile.Height = dSize;
                timerTile.Name = newAppliance.Name;
                this.mMainPage.Timers.Children.Add(timerTile);

                TimerSetTime(timerTile, newAppliance.Time);

                Appliances.Add(newAppliance);
                
                Storyboard AddTile = new Storyboard();
                QuadraticEase ease = new QuadraticEase();
                ease.EasingMode = EasingMode.EaseIn;
                
                DoubleAnimation MoveAnimation = new DoubleAnimation();
                MoveAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(250));
                MoveAnimation.From = this.mMainPage.ActualHeight;
                MoveAnimation.To = (Appliances.Count * dSize);
                MoveAnimation.EasingFunction = ease;

                Storyboard.SetTarget(MoveAnimation, timerTile);
                Storyboard.SetTargetProperty(MoveAnimation, "(Canvas.Top)");

                AddTile.Children.Add(MoveAnimation);
                AddTile.Completed += (sendr, args) =>
                {
                    this.mMainPage.NewTimer.IsEnabled = true;
                };
                AddTile.Begin();
            }
            catch (Exception ex)
            {
                logException(ex);
            }
        }

        public void TimerSwipe(
            TimerTile timerTile,
            PointerPoint StartingPoint,
            PointerPoint EndingPoint)
        {
            try
            {
                //Make sure the Y value is not too large. If so the user is scrolling up/down.
                if ((StartingPoint.Position.Y + EndingPoint.Position.Y) > timerTile.ActualHeight)
                {
                    return;
                }

                //var v = Math.Abs(StartingPoint.Position.X - EndingPoint.Position.X);

                if (Math.Abs(StartingPoint.Position.X - EndingPoint.Position.X) < 25)
                {
                    //Small amount of moment, must be just a press
                    TimerStartPause(timerTile);
                }
                else
                {
                    Int16 iValue = 1;
                    Appliance applFind = Appliances.Find(e => (e.Name == timerTile.Name));

                    if (StartingPoint.Position.X < (timerTile.ActualWidth / 2))
                    {
                        //Affecting Icon
                        if (EndingPoint.Position.X < StartingPoint.Position.X)
                        {
                            iValue = -1;
                        }

                        string[] types = Enum.GetNames(typeof(Appliance.ApplianceType));
                        string type = "";

                        for (Byte b = 0; b < types.Count(); b++)
                        {
                            type = types[b];

                            if (type == applFind.Type.ToString())
                            {
                                if ((b + iValue) > type.Count())
                                {
                                    type = types[0];
                                }
                                else if ((b + iValue) < 0)
                                {
                                    type = types[(types.Count() - 1)];
                                }
                                else
                                {
                                    type = types[b + iValue];
                                }
                                break;
                            }
                        }

                        Appliances.Remove(applFind);  //Remove the current item, then add the new one below.
                        applFind.Type = ApplianceTypeFromType(type);
                        applFind = ApplianceByType(applFind.Type);
                        timerTile.Name = applFind.Name;
                        Appliances.Add(applFind);
                        TimerSetIcon(timerTile, applFind);
                        TimerSetTime(timerTile, applFind.Time);
                    }
                    else
                    {
                        //Affecting Time
                        if (EndingPoint.Position.X < StartingPoint.Position.X)
                        {
                            iValue = -1;
                        }
                        TimeSpan timeSpan = applFind.Time.Add(new TimeSpan(0, iValue, 0));

                        //To insure that the value never gets set below zero
                        if (timeSpan.TotalMinutes > -1)
                        {
                            applFind.Time = timeSpan;
                            TimerSetTime(timerTile, timeSpan);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logException(ex);
            }
        }
        
        public void TimerSetIcon(TimerTile timerTile, Appliance appl)
        {
            try
            {
                Storyboard sb = new Storyboard();

                DoubleAnimation FadeOut = new DoubleAnimation();
                FadeOut.Duration = new Duration(TimeSpan.FromMilliseconds(400));
                FadeOut.From = timerTile.ApplianceIcon.Opacity; ;
                FadeOut.To = 0.0;

                Storyboard.SetTarget(FadeOut, timerTile.ApplianceIcon);
                Storyboard.SetTargetProperty(FadeOut, "(Image.Opacity)");
                sb.Children.Add(FadeOut);
                sb.Completed += (sendr, e) =>
                {
                   
                    Storyboard sbFadeIn = new Storyboard();
                    DoubleAnimation FadeIn = new DoubleAnimation();
                    FadeIn.Duration = new Duration(TimeSpan.FromMilliseconds(400));
                    FadeIn.From = 0.0;
                    FadeIn.To = 1.0;
                    timerTile.ApplianceIcon.Source = ApplianceIconFromType(appl.Type).Source;

                    Storyboard.SetTarget(FadeIn, timerTile.ApplianceIcon);
                    Storyboard.SetTargetProperty(FadeIn, "(Image.Opacity)");
                    sbFadeIn.Children.Add(FadeIn);
                    sbFadeIn.Begin();
                };
                sb.Begin();
            }
            catch (Exception ex)
            {
                logException(ex);
            }
        }

        public void TimerSetTime(TimerTile timerTile, TimeSpan Time)
        { 
            try
            {
                string sValue = "";
      
                //HOURS
                if (Time.Hours.ToString() != timerTile.TimeHours.Text)
                {
                    if (Time.Hours > 0)
                    {
                        sValue = Time.Hours.ToString();
                        TextChange(timerTile.TimeHoursColon, ":");
                        timerTile.TimeGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star); 
                    }
                    else
                    {
                        TextChange(timerTile.TimeHoursColon, "");
                        timerTile.TimeGrid.ColumnDefinitions[1].Width = new GridLength(0);   
                    }
                    TextChange(timerTile.TimeHours, sValue);
                }

                //MINUTES - TENS
                if (Time.Minutes.ToString("00").Substring(0, 1) != timerTile.TimeMinutesTen.Text)
                {
                    if (Time.Minutes.ToString("00").Substring(0, 1) == "0")
                    {
                        //If there are hours, show the minutes
                        if (Time.Hours > 0)
                        {
                            sValue = "0";
                            timerTile.TimeGrid.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star); 
                        }
                        else
                        {
                            sValue = "";
                            timerTile.TimeGrid.ColumnDefinitions[2].Width = new GridLength(0);  
                        }    
                    }
                    else
                    {
                        sValue = Time.Minutes.ToString("00").Substring(0, 1);
                        timerTile.TimeGrid.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star); 
                    }
                    TextChange(timerTile.TimeMinutesTen, sValue);
                }
                else if (Time.Minutes.ToString("00").Substring(0, 1) == timerTile.TimeMinutesTen.Text && Time.Hours == 0)
                {
                    TextChange(timerTile.TimeMinutesTen, "");
                    timerTile.TimeGrid.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star); 
                }

                //MINUTES - ONES
                if (Time.Minutes.ToString("00").Substring(1, 1) != timerTile.TimeMinutesOne.Text)
                {
                    if (Time.Minutes.ToString("00").Substring(1, 1) == "0")
                    {
                        if (Time.Hours > 0)
                        {
                            sValue = "0";
                        }
                        else
                        {
                            sValue = "";
                        }  
                    }
                    else
                    {
                        sValue = Time.Minutes.ToString("00").Substring(1, 1);
                    }
                    TextChange(timerTile.TimeMinutesOne, sValue);
                }

                //SECONDS - TENS
                if (Time.Seconds.ToString("00").Substring(0, 1) != timerTile.TimeSecondsTen.Text)
                {
                    TextChange(timerTile.TimeSecondsTen, Time.Seconds.ToString("00").Substring(0, 1));
                }

                //SECONDS - ONES
                if (Time.Seconds.ToString("00").Substring(1, 1) != timerTile.TimeSecondsOne.Text)
                {
                    TextChange(timerTile.TimeSecondsOne, Time.Seconds.ToString("00").Substring(1, 1));
                }
            }
            catch (Exception ex)
            {
                logException(ex);
            }
        }

        public void TimerStartPause(TimerTile timerTile)
        {
            try
            {
                Appliance appl = Appliances.Find(e => (e.Name == timerTile.Name));

                //Just in case
                if (appl != null)
                {
                    appl.IsRunning = !appl.IsRunning;

                    if (mbIsRunning == false)
                    {
                        mbIsRunning = true;
                        TimerRun();
                    }
                }
            }
            catch (Exception ex)
            {
                logException(ex);
            }
        }

        public async void TimerRun()
        {
            try 
	        {
                bool bRunning = false;
                Appliance applTemp = new Appliance();
                IProgress<object> progress = new Progress<object>(_ => ApplyTime(applTemp));
                await Task.Run(async () =>
                {
                    while (mbIsRunning == true)
                    {
                        bRunning = false; //Reset
                        await Task.Delay(1000);
                        foreach (Appliance appl in Appliances)
                        {
                            if (appl.IsRunning == true)
                            {
                                appl.Time = appl.Time.Add(new TimeSpan(0, 0, -1));
                                applTemp = appl;
                                progress.Report(null);
                                bRunning = true;
                            }
                        }

                        //All timers have completed.  Why waste the cycles if we don't need to.... That's just rude.
                        if (bRunning == false) { mbIsRunning = false; }
                    }
                });
	        }
	        catch (Exception ex)
	        {
		        logException(ex);
	        }
        }

        private void ApplyTime(Appliance appl)
        {
            try
            {
                TimerTile timerTile = (TimerTile)mMainPage.FindName(appl.Name);

                if (appl.Time.TotalSeconds == 0)
                {
                    appl.IsRunning = false;
                    SentToast(appl.FullName, appl.Phrase);
                    appl = ApplianceDefaults(appl.Type);
                    TimerSetTime(timerTile, appl.Time);
                }
                else
                {
                    TimerSetTime(timerTile, appl.Time);
                }
            }
            catch (Exception ex)
            {
                logException(ex);
            }
        }

        private void TextChange(TextBlock UITextBlock, string Text)
        {
            try
            {
                Storyboard sbFadeOut = new Storyboard();
                DoubleAnimation animFadeOut = new DoubleAnimation();
                TimeSpan tsTextFade = new TimeSpan(0, 0, 0, 0, 300);

                animFadeOut.Duration = new Duration(tsTextFade);
                animFadeOut.From = 1.0;
                animFadeOut.To = 0.0;

                Storyboard.SetTarget(animFadeOut, UITextBlock);
                Storyboard.SetTargetProperty(animFadeOut, "(TextBlock.Opacity)");

                sbFadeOut.Children.Add(animFadeOut);

                sbFadeOut.Completed +=
                    (sndr, evtArgs) =>
                    {
                        UITextBlock.Text = Text;
                        Storyboard sbFadeIn = new Storyboard();
                        DoubleAnimation animFadeIn = new DoubleAnimation();

                        animFadeIn.Duration = new Duration(tsTextFade);
                        animFadeIn.From = 0.0;
                        animFadeIn.To = 1.0;

                        Storyboard.SetTarget(animFadeIn, UITextBlock);
                        Storyboard.SetTargetProperty(animFadeIn, "(TextBlock.Opacity)");

                        sbFadeIn.Children.Add(animFadeIn);
                        sbFadeIn.Begin();
                    };
                sbFadeOut.Begin();
            }
            catch (Exception)
            {
                throw;
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

        public async void AboutSelected()
        {
            try 
	        {
                var version = Package.Current.Id.Version;
                StorageFile file = await Package.Current.InstalledLocation.GetFileAsync("AppxManifest.xml");
                string manifestXml = await FileIO.ReadTextAsync(file);
                XDocument doc = XDocument.Parse(manifestXml);
                XNamespace packageNamespace = "http://schemas.microsoft.com/appx/2010/manifest";
                var displayName = (from name in doc.Descendants(packageNamespace + "DisplayName")
                                   select name.Value).First();

                string Message =
                    "Site: Blissgig.com" + Environment.NewLine +
                    "Contact: Blissgig@gmail.com" + Environment.NewLine +
                    "Copyright 2015 James Rose" + Environment.NewLine +
                    "Version: " + String.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision); ;


                MessageDialog messageDialog = new MessageDialog(Message, displayName);
                await messageDialog.ShowAsync();
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


        /// <summary>
        /// Code found here.  Just want to send a notification and this does it, nice and simple.
        /// https://code.msdn.microsoft.com/windowsapps/Action-Center-Quickstart-b15089f2/sourcecode?fileId=111808&pathId=298223712
        /// </summary>
        public void SentToast(string Title, string Body)
        {
            try
            {
                var notifier = ToastNotificationManager.CreateToastNotifier();

                if (notifier.Setting == NotificationSetting.Enabled)
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

        public ToastNotification CreateTextOnlyToast(string toastHeading, string toastBody)
        {
            ToastNotification toastReturn = null;

            try
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
                toastReturn = new ToastNotification(toastXml);
            }
            catch (Exception ex)
            {
                logException(ex);
            }

            return toastReturn;
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
