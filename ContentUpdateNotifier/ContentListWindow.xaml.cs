using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace lpubsppop01.ContentUpdateNotifier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    partial class ContentListWindow : Window
    {
        #region Constructor

        public ContentListWindow()
        {
            InitializeComponent();

            FilePaths = new ObservableCollection<string>();
        }

        #endregion

        #region Properties

        ObservableCollection<string> FilePaths
        {
            get { return lstFilePaths.ItemsSource as ObservableCollection<string>; }
            set { lstFilePaths.ItemsSource = value; }
        }

        ContentList m_ContentList;
        public ContentList ContentList
        {
            get { return m_ContentList; }
            set
            {
                m_ContentList = value;
                FilePaths.Clear();
                foreach (var path in m_ContentList.Keys)
                {
                    FilePaths.Add(path);
                }
            }
        }

        #endregion

        #region Event Handlers

        void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (FilePaths == null) return;
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() ?? false)
            {
                FilePaths.Add(dialog.FileName);
                int timestamp = File.GetLastWriteTime(dialog.FileName).ToUnixTimestamp();
                ContentList.Add(dialog.FileName, timestamp, removed: false);
            }
        }

        void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (FilePaths == null) return;
            FilePaths.Remove((string)lstFilePaths.SelectedValue);
        }

        #endregion
    }
}
