using System;


namespace SQLiteConnections.Model
{
    interface IServer
    {
        int ID { get; set; }
        string Name { get; set; }
        string IPAdress { get; set; }
        string Port { get; set; }
        string Login { get; set; }
        Guid Guid { get; set; }
        void Update(IServer server);
    }
}
