using System;
using System.ComponentModel;


namespace SQLiteConnections.Model
{
    class Server : IServer, INotifyPropertyChanged
    {
        private int _ID;
        private string _name;
        private string _ipAdress;
        private string _port;
        private string _login;
        private Guid _guid;

        public int ID
        {
            get { return _ID; }
            set { _ID = value; NotifyPropertyChanged("ID"); }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyPropertyChanged("Name"); }
        }

        public string IPAdress
        {
            get { return _ipAdress; }
            set { _ipAdress = value; NotifyPropertyChanged("IPAdress"); }
        }

        public string Port
        {
            get { return _port; }
            set { _port = value; NotifyPropertyChanged("Port"); }
        }

        public string Login
        {
            get { return _login; }
            set { _login = value; NotifyPropertyChanged("Login"); }
        }

        public string DisplayMember
        {
            get { return string.Format("{0} ({1})", Name, IPAdress); }

        }

        public Guid Guid
        {
            get { return _guid; }
            set { _guid = value; NotifyPropertyChanged("Guid"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
                PropertyChanged(this, new PropertyChangedEventArgs("DisplayMember"));
            }
        }

        public void Update(IServer server)
        {
            Name = server.Name;
            IPAdress = server.IPAdress;
            Port = server.Port;
            Login = server.Login;
        }
    }
}
