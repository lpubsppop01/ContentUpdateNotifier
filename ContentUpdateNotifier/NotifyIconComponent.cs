using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;

namespace lpubsppop01.ContentUpdateNotifier
{
    public partial class NotifyIconComponent : Component
    {
        #region Constructors

        const string MyAppID = "lpubsppop01.ContentUpdateNotifier";
        const string MyAppName = "lpubsppop01.ContentUpdateNotifier";

        static string MyIconPath
        {
            get
            {
                string appDirPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string myIconPath = Path.Combine(appDirPath, @"_Images\ContentUpdateNotifier_64x64.png");
                return myIconPath;
            }
        }

        static string MyDBPath
        {
            get
            {
                string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string myDBPath = Path.Combine(localAppDataPath, MyAppID, "db.sqlite");
                return myDBPath;
            }
        }

        ToastNotificationHelper toast;
        ContentList contentList;
        ContentWatcher contentWatcher;

        public NotifyIconComponent()
        {
            InitializeComponent();

            toolStripMenuItem_Open.Click += toolStripMenuItem_Open_Click;
            toolStripMenuItem_Exit.Click += toolStripMenuItem_Exit_Click;

            toast = new ToastNotificationHelper(MyAppID, MyAppName, MyIconPath);

            contentList = new ContentList(MyDBPath);
            contentWatcher = new ContentWatcher(contentList);
            contentWatcher.ContentUpdated += watcher_FileUpdated;
            contentWatcher.Start();
        }

        public NotifyIconComponent(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        void notifyIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left) return;
            ShowContentListWindow();
        }

        void toolStripMenuItem_Open_Click(object sender, EventArgs e)
        {
            ShowContentListWindow();
        }

        void toolStripMenuItem_Exit_Click(object sender, EventArgs e)
        {
            contentWatcher.Stop();
            Application.Current.Shutdown();
        }

        void watcher_FileUpdated(object sender, ContentUpdatedEventArgs e)
        {
            string subject = e.Removed ? "Removed" : "Updated";
            string filename = Path.GetFileName(e.Location);
            string dirPath = Path.GetDirectoryName(e.Location);
            Action onActivated = () => Process.Start(dirPath);
            toast.ShowText(subject, filename, onActivated);
        }

        #endregion

        #region Window

        ContentListWindow window;

        void ShowContentListWindow()
        {
            if (window == null)
            {
                window = new ContentListWindow { ContentList = contentList };
                window.Closed += (sender, e) => { window = null; };
            }
            if (window.IsVisible)
            {
                window.Activate();
            }
            else
            {
                window.Show();
            }
        }

        #endregion
    }
}
