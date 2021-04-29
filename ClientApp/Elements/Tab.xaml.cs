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

namespace ClientApp.Elements
{
    /// <summary>
    /// Interaction logic for Tab.xaml
    /// </summary>
    public partial class Tab : DockPanel
    {
        public bool Active { get; set; } = false;
        public bool Closable { get; }

        public Tab(string tabName, RoutedEventHandler openHandler, RoutedEventHandler closeHandler)
        {
            InitializeComponent();
            openTab.Click += openHandler;
            closeTab.Click += closeHandler;
            openTab.Content = tabName;

            Closable = true;
        }

        public Tab(string tabName, RoutedEventHandler openHandler)
        {
            InitializeComponent();
            openTab.Click += openHandler;
            openTab.Content = tabName;
            
            Closable = false;
            this.Children.Remove(closeTab);
        }

        private void Tab_MouseEnter(object sender, MouseEventArgs e)
        {
            var brush = new SolidColorBrush(Colors.LightBlue);
            this.openTab.Background = brush;
            this.closeTab.Background = brush;
            this.closeTab.Content = "x";
        }

        private void Tab_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Active == false)
            {
                var brush = new SolidColorBrush(Colors.LightGray);
                this.openTab.Background = brush;
                this.closeTab.Background = brush;
                this.closeTab.Content = "";
            }
        }
    }
}
