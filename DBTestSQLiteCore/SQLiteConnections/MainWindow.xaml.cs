using System;
using System.Windows;
using System.Windows.Controls;
using SQLiteConnections.Database;
using SQLiteConnections.Model;
using System.Collections.ObjectModel;

namespace SQLiteConnections
{
    
    public partial class MainWindow : Window
    {
        private IDBHelper dBHelper;
        private ObservableCollection<Server> servers = new ObservableCollection<Server>();
        

        public MainWindow()
        {
           
            InitializeComponent();
        }

        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {

            dBHelper = new DBHelper();

            servers = dBHelper.GetServers();
            ServerLB.Items.Clear();
            ServerLB.ItemsSource = servers;
                      
        }

       
        private void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
           
        }

        private void OnAddButtonClick(object sender, RoutedEventArgs e)
        {

            Server server = SetNewServerFromUI();
            dBHelper.InsertToDB(server);
            servers.Add(dBHelper.GetServer(server.Name.ToString()));
            CleanUITBs();
        }



        private void SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ServerLB.SelectedIndex < 0)
            {
                AddBtn.IsEnabled = true;
                UpdateBtn.IsEnabled = false;
                RemoveBtn.IsEnabled = false;
                ServerLB.SelectedIndex = 0;

            } else
            {
                AddBtn.IsEnabled = true;
                UpdateBtn.IsEnabled = true;
                RemoveBtn.IsEnabled = true;
            }
            
               

            CounterTB.Text = "Index: " + ServerLB.SelectedIndex.ToString();
            Server server = servers[ServerLB.SelectedIndex];
            ServerNameTB.Text = server.Name;
            ServerIPTB.Text = server.IPAdress;
            ServerPortTB.Text = server.Port;
            ServerLoginTB.Text = server.Login;
        }

        private void OnUpdateButtonClick(object sender, RoutedEventArgs e)
        {
            int index = ServerLB.SelectedIndex;
            //string oldServerName = servers[index].Name;
            Server server = servers[index];
            server.Update(GetServerUpdateFromUI());
            dBHelper.UpdateServer(server);
        }
        
        private void OnRemoveButtonClick(object sender, RoutedEventArgs e)
        {
            int index = ServerLB.SelectedIndex;
            Guid guid = servers[index].Guid;
            dBHelper.RemoveServer(guid);
            servers.Remove(servers[index]);
        }

        private Server SetNewServerFromUI()
        {
            return new Server
            {
                Name = ServerNameTB.Text.ToUpper(),
                IPAdress = ServerIPTB.Text.ToUpper(),
                Port = ServerPortTB.Text.ToUpper(),
                Login = ServerLoginTB.Text.ToUpper(),
                Guid = Guid.NewGuid()
            };
        }

        private Server GetServerUpdateFromUI()
        {
            return new Server
            {
                Name = ServerNameTB.Text.ToUpper(),
                IPAdress = ServerIPTB.Text.ToUpper(),
                Port = ServerPortTB.Text.ToUpper(),
                Login = ServerLoginTB.Text.ToUpper(),
            };
        }

        private void CleanUITBs()
        {
            ServerNameTB.Text = "";
            ServerIPTB.Text = "";
            ServerPortTB.Text = "";
            ServerLoginTB.Text = "";
        }
                
    }
}
