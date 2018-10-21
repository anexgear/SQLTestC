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
        private static readonly string databaseName = "servers.db";
        private static readonly string tableName = "srv";
        private static readonly string id = "id";
        private static readonly string guid = "guid";
        private static readonly string name = "name";
        private static readonly string ipv4 = "ipv4";
        private static readonly string port = "port";
        private static readonly string login = "login";
        private static readonly string ConnectionString = "Data Source = " + databaseName + "; Version=3;";
        private static readonly string NewTableQuery = new StringBuilder("DROP TABLE IF EXISTS " + tableName + ";")
            .Append("CREATE TABLE " + tableName + "(")
            .Append(id + " INTEGER PRIMARY KEY AUTOINCREMENT, ")
            .Append(guid + " TEXT UNIQUE, ")
            .Append(name + " TEXT, ")
            .Append(ipv4 + " TEXT, ")
            .Append(port + " TEXT, ")
            .Append(login + " TEXT);")
            .ToString();



        public DBHelper()
        {
            if (!File.Exists(databaseName))
            {
                CreateDB();
            }
        }

        public void CreateDB()
        {
            SQLiteConnection.CreateFile(databaseName);
            CreateTable();
        }


        public void CreateTable()
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
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
            string deleteServerString = "DELETE FROM " + tableName + " WHERE guid = @guid";
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
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
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                Server server;
                string GetServerString = "SELECT * FROM " + tableName + " WHERE guid LIKE @guid";
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
                                ID = int.Parse(reader[id].ToString()),
                                Name = (string)reader[name],
                                IPAdress = (string)reader[ipv4],
                                Port = (string)reader[port],
                                Login = (string)reader[login]
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
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                string GetServerString = "SELECT * FROM " + tableName + " WHERE name LIKE @name";
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
                                ID = int.Parse(reader[id].ToString()),
                                Name = (string)reader[name],
                                IPAdress = (string)reader[ipv4],
                                Port = (string)reader[port],
                                Login = (string)reader[login]
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
            string GetServersString = "SELECT DISTINCT * FROM " + tableName;
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (SQLiteCommand sqlCommand = new SQLiteCommand(GetServersString, connection))
                {
                    using (SQLiteDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Server server = new Server
                            {
                                ID = int.Parse(reader[id].ToString()),
                                Name = (string)reader[name],
                                IPAdress = (string)reader[ipv4],
                                Port = (string)reader[port],
                                Login = (string)reader[login],
                                Guid = Guid.Parse(reader[guid].ToString())                               
                            };

                            servers.Add(server);

                        }
                        return servers;
                    }
                }
            }
        }

        public void InsertToDB(Server server)
        {
            string InsertToDBString = "insert into " + tableName + " (name, ipv4, port, login, guid) values (@name, @ipv4, @port, @login, @guid)";

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
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

        public void UpdateServer(Server server)
        {
            string UpdateServerString = "UPDATE " + tableName + " SET name = @name, ipv4 = @ipv4, port = @port, login = @login WHERE guid LIKE @guid";

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
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
