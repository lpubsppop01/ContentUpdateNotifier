using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;

namespace lpubsppop01.ContentUpdateNotifier
{
    public class ContentUpdatedEventArgs : EventArgs
    {
        #region Constructor

        public ContentUpdatedEventArgs(string location, bool removed)
        {
            Location = location;
            Removed = removed;
        }

        #endregion

        #region Properties

        public string Location { get; private set; }
        public bool Removed { get; private set; }

        #endregion
    }

    sealed class ContentWatcher
    {
        #region Constructor

        ContentList contentList;

        public ContentWatcher(ContentList contentList)
        {
            this.contentList = contentList;
        }

        #endregion

        #region Events

        public event EventHandler<ContentUpdatedEventArgs> ContentUpdated;

        void OnContentUpdated(string location, bool removed)
        {
            ContentUpdated?.Invoke(this, new ContentUpdatedEventArgs(location, removed));
        }

        #endregion

        #region Check Update

        void CheckUpdate()
        {
            // Collect updated/removed content locations
            var updated = new List<(string location, int timestamp, bool removed)>();
            foreach (var path in contentList.Keys)
            {
                if (!contentList.TryGetValue(path, out int timestamp, out bool removed)) continue;
                if (removed)
                {
                    if (File.Exists(path))
                    {
                        var currTimestamp = File.GetLastWriteTime(path).ToUnixTimestamp();
                        updated.Add((path, currTimestamp, removed: false));
                    }
                }
                else
                {
                    if (File.Exists(path))
                    {
                        try
                        {
                            var currTimestamp = File.GetLastWriteTime(path).ToUnixTimestamp();
                            if (currTimestamp > timestamp)
                            {
                                updated.Add((path, currTimestamp, removed: false));
                            }
                        }
                        catch { continue; }
                    }
                    else
                    {
                        updated.Add((path, timestamp, removed: true));
                    }
                }
            }
            if (!updated.Any()) return;

            // Update content list
            contentList.BeginEdit();
            try
            {
                foreach (var t in updated)
                {
                    contentList.Update(t.location, t.timestamp, t.removed);
                }
            }
            finally
            {
                contentList.EndEdit();
            }

            // Raise events
            foreach (var t in updated)
            {
                OnContentUpdated(t.location, t.removed);
            }
        }

        #endregion

        #region Timer

        Timer timer;

        void timer_ElapsedEventHandler(object sender, ElapsedEventArgs e)
        {
            CheckUpdate();
        }

        public void Start()
        {
            if (timer == null)
            {
                timer = new Timer(5000);
                timer.Elapsed += timer_ElapsedEventHandler;
            }
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        #endregion
    }
}
