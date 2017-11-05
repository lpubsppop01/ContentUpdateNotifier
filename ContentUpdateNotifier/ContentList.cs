using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace lpubsppop01.ContentUpdateNotifier
{
    class ContentList : IDisposable
    {
        #region Constructor

        SQLiteConnection conn;

        public ContentList(string path)
        {
            conn = SQLiteExtension.Open(path);
            if (!HasMyTable) CreateMyTable();
        }

        #endregion

        #region DB

        const string MyTableName = "content_list";

        bool HasMyTable => conn.TableNames().Any(n => n == MyTableName);

        void CreateMyTable()
        {
            using (SQLiteCommand command = conn.CreateCommand())
            {
                command.CommandText = $"create table {MyTableName}(path TEXT PRIMARY KEY, timestamp INTEGER, removed TEXT)";
                command.ExecuteNonQuery();
            }
        }

        SQLiteTransaction transaction;

        public void BeginEdit()
        {
            if (transaction != null) throw new InvalidOperationException();
            transaction = conn.BeginTransaction();
        }

        public void EndEdit()
        {
            if (transaction == null) throw new InvalidOperationException();
            transaction.Commit();
            transaction = null;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            conn.Close();
        }

        #endregion

        #region Dictionary Like Members

        public ICollection<string> Keys => conn.StringValues(MyTableName, "path").ToArray();

        public int Count => (int)conn.Count(MyTableName);

        public (int timestamp, bool removed) this[string path]
        {
            get
            {
                if (!TryGetValue(path, out int timestamp, out bool removed)) throw new KeyNotFoundException();
                return (timestamp, removed);
            }
            set
            {
                if (ContainsKey(path))
                {
                    Update(path, value.timestamp, value.removed);
                }
                else
                {
                    Add(path, value.timestamp, value.removed);
                }
            }
        }

        public bool Add(string path, int timestamp, bool removed)
        {
            using (var command = conn.CreateCommand())
            {
                command.CommandText = $"INSERT INTO {MyTableName} VALUES ('{path}', {timestamp}, '{removed}')";
                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool Update(string path, int timestamp, bool removed)
        {
            using (var command = conn.CreateCommand())
            {
                command.CommandText = $"UPDATE {MyTableName} SET timestamp = {timestamp}, removed = '{removed}' WHERE path = '{path}'";
                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool Clear()
        {
            using (var command = conn.CreateCommand())
            {
                command.CommandText = $"DELETE FROM {MyTableName}";
                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool ContainsKey(string path) => conn.Count(MyTableName, $"path = '{path}'") > 0;

        public bool Remove(string path)
        {
            using (var command = conn.CreateCommand())
            {
                command.CommandText = $"DELETE FROM {MyTableName} WHERE path = '{path}'";
                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool TryGetValue(string path, out int timestamp, out bool removed)
        {
            timestamp = 0;
            removed = false;
            var values = conn.StringValuePairs(MyTableName, "timestamp", "removed", $"path = '{path}'");
            if (!values.Any()) return false;
            if (!int.TryParse(values.First().str1, out timestamp)) return false;
            if (!bool.TryParse(values.First().str2, out removed)) return false;
            return true;
        }

        #endregion
    }
}
