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
using System.Windows.Shapes;

namespace ClientApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для EntryWindow.xaml
    /// </summary>
    public partial class EntryWindow : Window
    {
        public bool ToClose { get; set; } = false;

        public EntryWindow()
        {
            InitializeComponent();
            
            applicationUrl.Text = Properties.Settings.Default.ApplicationUrl;
        }

        private void TryConnect_Click(object sender, RoutedEventArgs e)
        {
            string url = applicationUrl.Text;

            MainWindow.Current.DataAccessor = new Infrastructure.ServerAccessor(url);

            MainWindow.Current.DataAccessor.GetAllEmployees(0, 1, out EmployeeDirectory.Infrastructure.ResultCode resultCode);

            if (resultCode == EmployeeDirectory.Infrastructure.ResultCode.OK)
            {
                Properties.Settings.Default.ApplicationUrl = url;
                Properties.Settings.Default.Save();

                this.DialogResult = true;
            }

            else if (resultCode == EmployeeDirectory.Infrastructure.ResultCode.ConnectionError)
                new MessageWindow(Properties.Resources.connectionErrorMessage).ShowDialog();

            else
                new MessageWindow(Properties.Resources.serverErrorMessage).ShowDialog();
        }
    }
}
