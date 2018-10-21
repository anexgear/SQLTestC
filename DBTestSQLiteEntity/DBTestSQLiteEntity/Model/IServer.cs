using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBTestSQLiteEntity.Model
{
    interface IServer
    {
        int ID { get; set; }
        string Name { get; set; }
        string IPAdress { get; set; }
        string Port { get; set; }
        string Login { get; set; }     
        void Update(IServer server);
    }
}
