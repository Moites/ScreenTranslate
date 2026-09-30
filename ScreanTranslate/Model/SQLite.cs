using Microsoft.Data.Sqlite;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ScreanTranslate.Model
{
    public class SQLite
    {
        // DataSort (false = по возрастанию, true = по убыванию)
        public static bool DataSort { get; set; } = false;
        private static readonly string db = "Data Source=" +
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TranslateDB.db");
        public static bool TableExists(string tableName)
        {
            using (var connection = new SqliteConnection(db))
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT count(*) FROM sqlite_master WHERE type='table' AND name=@TableName;";

                command.Parameters.AddWithValue("@TableName", tableName);
                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar()) == 1;
            }
        }
        public static Task InsertDB(string _text, string _translate_text, string _screenshot, DateTime _date_time)
        {
            return Task.Run(() =>
            {
                if (TableExists("TranslateHistory"))
                {
                    using (var connection = new SqliteConnection(db))
                    {
                        connection.Open();

                        var command = connection.CreateCommand();
                        command.CommandText = @"
                                                INSERT INTO TranslateHistory (text, translate_text, screenshot, data_time)
                                                VALUES
                                                (@text, @translate_text, @screenshot, @data_time);";

                        command.Parameters.AddWithValue("text", _text);
                        command.Parameters.AddWithValue("translate_text", _translate_text);
                        command.Parameters.AddWithValue("screenshot", _screenshot);
                        command.Parameters.AddWithValue("data_time", _date_time);

                        command.ExecuteNonQuery();
                    }
                }
                else
                {
                    using (var connection = new SqliteConnection(db))
                    {
                        connection.Open();

                        var command = connection.CreateCommand();
                        command.CommandText = @"CREATE TABLE TranslateHistory (id INTEGER  PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL, 
                                            text TEXT NOT NULL, translate_text TEXT NOT NULL, 
                                            screenshot BASE64 NOT NULL, data_time DATATIME NOT NULL );";

                        command.ExecuteNonQuery();
                    }

                    InsertDB(_text, _translate_text, _screenshot, _date_time);
                }
            });
        }

        public static List<DataDB> SelectDB()
        {
            if (TableExists("TranslateHistory"))
            {
                string sort = string.Empty;
                if (DataSort)
                {
                    sort = "DESC";
                }
                else
                {
                    sort = " ";
                }
                using (var connection = new SqliteConnection(db))
                {
                    var result = new List<DataDB>();

                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText = @$"
                    SELECT * FROM TranslateHistory ORDER BY data_time {sort}";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var screan = Convert.FromBase64String(reader.GetString(3));

                            result.Add(new DataDB()
                            {
                                Text = reader.GetString(1),
                                TranslateText = reader.GetString(2),
                                Screenshot = screan,
                                DataTime = reader.GetDateTime(4),
                            });
                        }
                    }
                    return result;
                }
            }
            else
            {
                using (var connection = new SqliteConnection(db))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText = @"CREATE TABLE TranslateHistory (id INTEGER  PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL, 
                                            text TEXT NOT NULL, translate_text TEXT NOT NULL, 
                                            screenshot BASE64 NOT NULL, data_time DATATIME NOT NULL );";

                    command.ExecuteNonQuery();
                }

                return SelectDB();
            }
        }
        public void DeleteDB()
        {
            using (var connection = new SqliteConnection(db))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"DELETE FROM TranslateHistory";

                command.ExecuteNonQuery();
            }
        }
    }
}