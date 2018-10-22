using SQLiteConnections.Model;
using System;
using System.Collections.ObjectModel;


namespace SQLiteConnections
{
    interface IDBHelper
    {
        void CreateTable();
        void CreateDB();
        void InsertToDB(IServer server);
        void UpdateServer(IServer server);
        void RemoveServer(Guid Guid);
        Server GetServer(Guid Guid);
        Server GetServer(string Name);
        ObservableCollection<Server> GetServers();
    }
}
