using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace ApplianceTMR
{
    /// <summary>
    /// Functions that should be outside the UI.
    /// </summary>
    class ATMREngine
    {
        private int toastIndex = 1;

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
        public void SentToast(string Message)
        {
            try
            {
                if (CanSendToasts() == true)
                {
                    // Use a helper method to create a new ToastNotification.
                    // Each time we run this code, we'll append the count (toastIndex in this example) to the message
                    // so that it can be seen as a unique message in the action center. This is not mandatory - we
                    // do it here for educational purposes.
                    ToastNotification toast = CreateTextOnlyToast(Message, String.Format("message {0}", toastIndex));

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

        public void logException(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
    }
}
