using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace lpubsppop01.ContentUpdateNotifier
{
    static class SQLiteExtension
    {
        public static SQLiteConnection Open(string path)
        {
            if (!File.Exists(path))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.Create(path).Close();
            }
            string connStr = string.Format("Data Source={0};Version=3;", path);
            var conn = new SQLiteConnection(connStr);
            conn.Open();
            return conn;
        }

        public static IEnumerable<string> TableNames(this SQLiteConnection conn)
        {
            return conn.StringValues("sqlite_master", "tbl_name", "type = 'table'");
        }

        public static IEnumerable<string> StringValues(this SQLiteConnection conn, string tableName, string columnName, string where = null)
        {
            using (var command = conn.CreateCommand())
            {
                command.CommandText = $"SELECT {columnName} FROM {tableName}";
                if (!string.IsNullOrEmpty(where)) command.CommandText += $" WHERE {where}";
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return reader[columnName].ToString();
                    }
                }
            }
        }

        public static IEnumerable<(string str1, string str2)> StringValuePairs(this SQLiteConnection conn,
            string tableName, string columnName1, string columnName2, string where = null)
        {
            using (var command = conn.CreateCommand())
            {
                command.CommandText = $"SELECT {columnName1}, {columnName2} FROM {tableName}";
                if (!string.IsNullOrEmpty(where)) command.CommandText += $" WHERE {where}";
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return (reader[columnName1].ToString(), reader[columnName2].ToString());
                    }
                }
            }
        }

        public static long Count(this SQLiteConnection conn, string tableName, string where = null)
        {
            using (var command = conn.CreateCommand())
            {
                command.CommandText = $"SELECT COUNT(*) FROM {tableName}";
                if (!string.IsNullOrEmpty(where)) command.CommandText += $" WHERE {where}";
                long count = (long)command.ExecuteScalar();
                return count;
            }
        }
    }
}
