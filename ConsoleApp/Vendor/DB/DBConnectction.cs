using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting_Time_Student_Team_HSU.Vendor.DB
{
    public class DBConnectction
    {
        private string _connectionString;
        private SQLiteConnection _connection;

        public DBConnectction(string dbFilePath)
        {
            _connectionString = $"Data Source={dbFilePath};Version=3;";
            _connection = new SQLiteConnection(_connectionString);

            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            try
            {
                _connection.Open();

                // Проверяем существование таблицы
                string checkTableSql = "SELECT name FROM sqlite_master WHERE type='table' AND name='Users';";
                using (var command = new SQLiteCommand(checkTableSql, _connection))
                {
                    var result = command.ExecuteScalar();

                    if (result == null)
                    {
                        // Таблица не существует, создаем
                        string createTableSql = @"CREATE TABLE Users (
                                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            Name TEXT NOT NULL,
                                            Email TEXT UNIQUE NOT NULL,
                                            Age INTEGER
                                        );";

                        using (var createCommand = new SQLiteCommand(createTableSql, _connection))
                        {
                            createCommand.ExecuteNonQuery();
                            Console.WriteLine("Таблица Users создана");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при инициализации БД: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }
        }

        public SQLiteConnection GetConnection()
        {
            if (_connection.State != System.Data.ConnectionState.Open)
                _connection.Open();

            return _connection;
        }

        public void CloseConnection()
        {
            if (_connection.State != System.Data.ConnectionState.Closed)
                _connection.Close();
        }
    }
}
