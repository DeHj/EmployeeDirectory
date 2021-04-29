using System;
using System.Collections.Generic;
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

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int count = 0;
        private readonly Dictionary<string, Panel> pages = new Dictionary<string, Panel>();

        public MainWindow()
        {
            InitializeComponent();

            // Create not closable tabs
            Tabs.Children.Add(new Elements.Tab("UserList", openUserList));
            Tabs.Children.Add(new Elements.Tab("AddUser", addUser));
        }

        private void addUser(object sender, RoutedEventArgs e)
        {


            int tmp = count;
            RoutedEventHandler handler = (object sender, RoutedEventArgs e) => { Title = $"{tmp}"; };

            Tabs.Children.Add(new Elements.Tab($"tab {count++}", handler, handler));
        }

        private void openUserList(object sender, RoutedEventArgs e)
        {

        }
    }
}
