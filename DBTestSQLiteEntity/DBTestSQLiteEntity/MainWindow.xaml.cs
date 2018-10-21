using DBTestSQLiteEntity.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DBTestSQLiteEntity
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ApplicationContext db;
        public MainWindow()
        {
            InitializeComponent();
            db = new ApplicationContext();
            db.Servers.Load();
            this.DataContext = db.Servers.Local.ToBindingList();
        }

        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            OnCloseClick(sender,e);
        }

        private void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

      

        private void OnAddButtonClick(object sender, RoutedEventArgs e)
        {
            Server server = GetServerFromUI();
            db.Servers.Add(server);
            db.SaveChanges();
            CleanUITBs();
        }

        private void OnUpdateButtonClick(object sender, RoutedEventArgs e)
        {
            if (ServerLB.SelectedItem == null) return;
            Server server = ServerLB.SelectedItem as Server;
            server.Name = ServerNameTB.Text;
            server.IPAdress = ServerIPTB.Text;
            server.Port = ServerPortTB.Text;
            server.Login = ServerLoginTB.Text;
            db.Entry(server).State = EntityState.Modified;
            db.SaveChanges();
        }

        private void OnRemoveButtonClick(object sender, RoutedEventArgs e)
        {
            if (ServerLB.SelectedItem == null) return;
            Server server = ServerLB.SelectedItem as Server;
            db.Servers.Remove(server);
            db.SaveChanges();
        }

        private void OnOpenClick(object sender, RoutedEventArgs e)
        {
            this.Width = this.Width + 200;
            CloseBtn.Visibility = Visibility.Visible;
            OpenBtn.Visibility = Visibility.Collapsed;
            ServerLB.Visibility = Visibility.Visible;
        }

        private void OnCloseClick(object sender, RoutedEventArgs e)
        {
            this.Width = this.Width - 200;
            ServerLB.Visibility = Visibility.Collapsed;
            CloseBtn.Visibility = Visibility.Collapsed;
            OpenBtn.Visibility = Visibility.Visible;
        }

        private void SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ServerLB.SelectedIndex < 0) ServerLB.SelectedIndex = 0;
            Server server = ServerLB.SelectedItem as Server;
            ServerNameTB.Text = server.Name;
            ServerIPTB.Text = server.IPAdress;
            ServerPortTB.Text = server.Port;
            ServerLoginTB.Text = server.Login;

        }

        private Server GetServerFromUI()
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
