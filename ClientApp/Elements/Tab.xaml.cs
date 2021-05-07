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
using ClientApp;

namespace ClientApp.Elements
{
    /// <summary>
    /// Interaction logic for Tab.xaml
    /// </summary>
    public partial class Tab : Button
    {
        public bool Closable { get; }
        public string TabName { get; set; }
        public Pages.IPage AssociatedPage { get; set; }


        public Tab(string tabName, bool closable, Pages.IPage page)
        {
            InitializeComponent();

            TabName = tabName;
            text.Text = tabName;

            AssociatedPage = page;
            Closable = closable;

            if (closable == false)
                closeTab.Visibility = Visibility.Collapsed;
            else
                closeTab.Visibility = Visibility.Hidden;

            Background = Application.Current.Resources["nonActiveTab"] as SolidColorBrush;
        }

        private void Tab_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Closable)
                closeTab.Visibility = Visibility.Visible;

            Background = Application.Current.Resources["activeTab"] as SolidColorBrush;
        }

        private void Tab_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Closable)
                closeTab.Visibility = Visibility.Hidden;

            if (MainWindow.Current.ActiveTab != this)
                Background = Application.Current.Resources["nonActiveTab"] as SolidColorBrush;
        }

        private void CloseTab_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Current.CloseTab(this);
            return;
        }

        private void OpenTab_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Current.ActiveTab = this;
        }
    }
}
