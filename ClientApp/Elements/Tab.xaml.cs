﻿using System;
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
        public MainWindow MainWindow { get; set; }
        public UIElement AssociatedPage { get; set; }

        public Tab(string tabName, string tabText, bool closable, MainWindow mainWindow, UIElement page)
        {
            InitializeComponent();

            TabName = tabName;
            text.Text = tabText;

            MainWindow = mainWindow;
            AssociatedPage = page;
            Closable = closable;

            if (closable == false)
                closeTab.Visibility = Visibility.Collapsed;
            else
                closeTab.Visibility = Visibility.Hidden;

            Background = Application.Current.Resources["nonActiveTab"] as SolidColorBrush;
        }

        private void tab_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Closable)
                closeTab.Visibility = Visibility.Visible;

            Background = Application.Current.Resources["activeTab"] as SolidColorBrush;
        }

        private void tab_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Closable)
                closeTab.Visibility = Visibility.Hidden;

            if (MainWindow.ActiveTab != this)
                Background = Application.Current.Resources["nonActiveTab"] as SolidColorBrush;
        }

        private void closeTab_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ActiveTab = MainWindow.MainTab;

            MainWindow.Pages.Remove(TabName);
            MainWindow.tabs.Children.Remove(this);
        }

        private void openTab_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ActiveTab = this;
        }
    }
}
