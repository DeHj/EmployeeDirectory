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

namespace ClientApp.Pages
{
    /// <summary>
    /// Interaction logic for AddPhonePage.xaml
    /// </summary>
    public partial class AddPhonePage : StackPanel
    {
        int employeeId;

        public AddPhonePage(int id)
        {
            InitializeComponent();

            employeeId = id;
        }

        private void addPhone_Click(object sender, RoutedEventArgs e)
        {
            if (phoneNumberBox.Number.Length == 11)
            {
                EmployeeDirectory.Infrastructure.ResultCode resultCode;
                MainWindow.Current.DataAccessor.AddPhone(employeeId, phoneNumberBox.Number, out resultCode);

                // Add resultCode handler!

                Elements.Tab tab = MainWindow.Current.FindTab((Elements.Tab tab) =>
                {
                    EmployeePage page = tab.AssociatedPage as EmployeePage;
                    if (page == null)
                        return false;
                    if (page.AssociatedEmployee.Id == employeeId)
                        return true;
                    return false;
                });

                MainWindow.Current.ActiveTab = tab ?? MainWindow.Current.MainTab;
            }
            else
            {
                // Add incorrect input notice!
            }

        }
    }
}
