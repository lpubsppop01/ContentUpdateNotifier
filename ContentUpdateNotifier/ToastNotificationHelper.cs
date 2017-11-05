using System;
using System.IO;
using System.Linq;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace lpubsppop01.ContentUpdateNotifier
{
    /// <summary>
    /// Helper class that simplify to use toast notification.
    /// <para>Required: "TargetPlatformVersion" in csproj,
    /// and references to Windows.winmd and System.Runtime.WindowsRuntime (README.md for details)</para>
    /// </summary>
    class ToastNotificationHelper
    {
        #region Constructor

        string appID;
        string appName;
        string appIconPath;

        public ToastNotificationHelper(string appID, string appName, string appIconPath)
        {
            this.appID = appID;
            this.appName = appName;
            this.appIconPath = appIconPath;
        }

        #endregion

        #region Wrapper Methods

        public void ShowText(string subject, string body, Action onActivated = null)
        {
            if (!Prepare()) return;
            var content = BuildContent(subject, body, appIconPath);
            var notification = new ToastNotification(content);
            if (onActivated != null) notification.Activated += (sender, e) => onActivated();
            var notifier = ToastNotificationManager.CreateToastNotifier(appID);
            notifier.Show(notification);
        }

        bool Prepare()
        {
            bool? shortcutStatus = StartMenuShortcutInstaller.Install(appID, appName);
            bool successOrNotChanged = shortcutStatus == null || shortcutStatus.Value;
            return successOrNotChanged;
        }

        static XmlDocument BuildContent(string title, string content, string imagePath)
        {
            XmlDocument xmlDoc;
            if (File.Exists(imagePath))
            {
                xmlDoc = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02);
                var imageElem = xmlDoc.GetElementsByTagName("image").First();
                imageElem.Attributes.GetNamedItem("src").InnerText = imagePath;
            }
            else
            {
                xmlDoc = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            }
            var textElem1 = xmlDoc.GetElementsByTagName("text").First();
            textElem1.AppendChild(xmlDoc.CreateTextNode(title));
            var textElem2 = xmlDoc.GetElementsByTagName("text").Skip(1).First();
            textElem2.AppendChild(xmlDoc.CreateTextNode(content));
            return xmlDoc;
        }

        #endregion
    }
}
