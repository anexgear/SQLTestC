using SQLiteConnections.Model;
using System;
using System.Collections.ObjectModel;


namespace SQLiteConnections
{
    interface IDBHelper
    {
        void CreateTable();
        void CreateDB();
        void InsertToDB(Server server);
        void UpdateServer(Server server);
        void RemoveServer(Guid Guid);
        Server GetServer(Guid Guid);
        Server GetServer(string Name);
        ObservableCollection<Server> GetServers();
    }
}
