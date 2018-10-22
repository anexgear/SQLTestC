using System;
using System.Text;
using System.Data.SQLite;
using SQLiteConnections.Model;
using System.Collections.ObjectModel;
using System.IO;

namespace SQLiteConnections.Database
{
    class DBHelper : IDBHelper
    {
        private const string DATABASE_NAME = "servers.db";
        private const string TABLE_NAME = "srv";
        private const string ID = "id";
        private const string GUID = "guid";
        private const string NAME = "name";
        private const string IPV4 = "ipv4";
        private const string PORT = "port";
        private const string LOGIN = "login";
        private const string CONNECTION_STRING = "Data Source = " + DATABASE_NAME + "; Version=3;";
        private static readonly string NewTableQuery = new StringBuilder("DROP TABLE IF EXISTS " + TABLE_NAME + ";")
            .Append("CREATE TABLE " + TABLE_NAME + "(")
            .Append(ID + " INTEGER PRIMARY KEY AUTOINCREMENT, ")
            .Append(GUID + " TEXT UNIQUE, ")
            .Append(NAME + " TEXT, ")
            .Append(IPV4 + " TEXT, ")
            .Append(PORT + " TEXT, ")
            .Append(LOGIN + " TEXT);")
            .ToString();



        public DBHelper()
        {
            if (!File.Exists(DATABASE_NAME))
            {
                CreateDB();
            }
        }

        public void CreateDB()
        {
            SQLiteConnection.CreateFile(DATABASE_NAME);
            CreateTable();
        }


        public void CreateTable()
        {
            using (SQLiteConnection connection = new SQLiteConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (SQLiteCommand sqlCommand = new SQLiteCommand(NewTableQuery, connection))
                {
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public void RemoveServer(Guid Guid)
        {
            var deleteServerString = "DELETE FROM " + TABLE_NAME + " WHERE guid = @guid";
            using (SQLiteConnection connection = new SQLiteConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (SQLiteCommand sqlCommand = new SQLiteCommand(deleteServerString, connection))
                {
                    sqlCommand.Parameters.Add("@guid", System.Data.DbType.String).Value = Guid.ToString().ToUpper();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public Server GetServer(Guid Guid)
        {
            using (SQLiteConnection connection = new SQLiteConnection(CONNECTION_STRING))
            {
                Server server;
                var GetServerString = "SELECT * FROM " + TABLE_NAME + " WHERE guid LIKE @guid";
                connection.Open();
                using (SQLiteCommand sqlCommand = new SQLiteCommand(GetServerString, connection))
                {
                    sqlCommand.Parameters.Add("@guid", System.Data.DbType.String).Value = Guid.ToString().ToUpper();
                    using (SQLiteDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            server = new Server
                            {
                                ID = int.Parse(reader[ID].ToString()),
                                Name = (string)reader[NAME],
                                IPAdress = (string)reader[IPV4],
                                Port = (string)reader[PORT],
                                Login = (string)reader[LOGIN]
                            };

                            return server;
                        }
                        return null;
                    }
                }
            }
        }

        public Server GetServer(string Name)
        {
            using (SQLiteConnection connection = new SQLiteConnection(CONNECTION_STRING))
            {
                var GetServerString = "SELECT * FROM " + TABLE_NAME + " WHERE name LIKE @name";
                Server server;
                connection.Open();
                using (SQLiteCommand sqlCommand = new SQLiteCommand(GetServerString, connection))
                {
                    sqlCommand.Parameters.Add("@name", System.Data.DbType.String).Value = Name.ToUpper();
                    using (SQLiteDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            server = new Server
                            {
                                ID = int.Parse(reader[ID].ToString()),
                                Name = (string)reader[NAME],
                                IPAdress = (string)reader[IPV4],
                                Port = (string)reader[PORT],
                                Login = (string)reader[LOGIN]
                            };

                            return server;
                        }
                        return null;
                    }
                }
            }
        }

        public ObservableCollection<Server> GetServers()
        {
            ObservableCollection<Server> servers = new ObservableCollection<Server>();
            var GetServersString = "SELECT DISTINCT * FROM " + TABLE_NAME;
            using (SQLiteConnection connection = new SQLiteConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (SQLiteCommand sqlCommand = new SQLiteCommand(GetServersString, connection))
                {
                    using (SQLiteDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var server = new Server
                            {
                                ID = int.Parse(reader[ID].ToString()),
                                Name = (string)reader[NAME],
                                IPAdress = (string)reader[IPV4],
                                Port = (string)reader[PORT],
                                Login = (string)reader[LOGIN],
                                Guid = Guid.Parse(reader[GUID].ToString())                               
                            };

                            servers.Add(server);

                        }
                        return servers;
                    }
                }
            }
        }

        public void InsertToDB(IServer server)
        {
            var InsertToDBString = "insert into " + TABLE_NAME + " (name, ipv4, port, login, guid) values (@name, @ipv4, @port, @login, @guid)";

            using (SQLiteConnection connection = new SQLiteConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (SQLiteCommand sqlCommand = new SQLiteCommand(InsertToDBString, connection))
                {
                    sqlCommand.Parameters.Add("@name", System.Data.DbType.String).Value = server.Name.ToUpper();
                    sqlCommand.Parameters.Add("@ipv4", System.Data.DbType.String).Value = server.IPAdress.ToUpper();
                    sqlCommand.Parameters.Add("@port", System.Data.DbType.String).Value = server.Port.ToUpper();
                    sqlCommand.Parameters.Add("@login", System.Data.DbType.String).Value = server.Login.ToUpper();
                    sqlCommand.Parameters.Add("@guid", System.Data.DbType.String).Value = server.Guid.ToString().ToUpper();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public void UpdateServer(IServer server)
        {
            var UpdateServerString = "UPDATE " + TABLE_NAME + " SET name = @name, ipv4 = @ipv4, port = @port, login = @login WHERE guid LIKE @guid";

            using (SQLiteConnection connection = new SQLiteConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (SQLiteCommand sqlCommand = new SQLiteCommand(UpdateServerString, connection))
                {
                    sqlCommand.Parameters.Add("@name", System.Data.DbType.String).Value = server.Name.ToUpper();
                    sqlCommand.Parameters.Add("@ipv4", System.Data.DbType.String).Value = server.IPAdress.ToUpper();
                    sqlCommand.Parameters.Add("@port", System.Data.DbType.String).Value = server.Port.ToUpper();
                    sqlCommand.Parameters.Add("@login", System.Data.DbType.String).Value = server.Login.ToUpper();
                    sqlCommand.Parameters.Add("@guid", System.Data.DbType.String).Value = server.Guid.ToString().ToUpper();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

    }
}
