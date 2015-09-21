using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
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
        public enum ApplianceType
        {
            //TODO: Get common times for these devices
            [EnumStringAttribute("Clothes Dryer")]
            [EnumByteAttribute(60)]
            ClothesDryer,
            [EnumStringAttribute("Egg Timer")]
            [EnumByteAttribute(5)]
            EggTimer,
            [EnumStringAttribute("Refrigorator")]
            [EnumByteAttribute(120)]
            Fridge,
            [EnumStringAttribute("Microwave")]
            [EnumByteAttribute(15)]
            Microwave,
            [EnumStringAttribute("Oven")]
            [EnumByteAttribute(25)]
            Oven,
            [EnumStringAttribute("Stove")]
            [EnumByteAttribute(30)]
            Stove,
            [EnumStringAttribute("Television")]
            [EnumByteAttribute(30)]
            TV,
            [EnumStringAttribute("Washing Machine")]
            WashingMachine,   
        }
    }

    /// <summary>
    /// Functions that should be outside the UI.
    /// </summary>
    class ATMREngine
    {
        private int toastIndex = 1;
        private SolidColorBrush mscbTileColor = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 49, 123, 193));


        public SolidColorBrush TileColor
        {
            get {return mscbTileColor;}
        }

        public Image ApplianceByType(Appliance.ApplianceType Type)
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

        public byte ApplianceTime(Appliance.ApplianceType Type)
        {
            byte bReturn = 0;

            try
            {
                Type type = Type.GetType();
                
                
               //List<System.Reflection.FieldInfo> fieldInfo = type.GetType().GetRuntimeFields().ToList(); //Close but still wrong

                //List<PropertyInfo> fields = type.GetRuntimeProperties().ToList(); // nothing came back

                //System.Reflection.PropertyInfo fieldInfo = type.GetType().GetRuntimeProperty("EnumByteAttribute");

                //DisplayAttribute attribute = value.GetType()
                //            .GetField(value.ToString())

                
            }
            catch (Exception ex)
            {
                logException(ex);
            }

            return bReturn;
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
                    toastIndex++;
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
