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
    /// Interaction logic for ChangeEmployeePage.xaml
    /// </summary>
    public partial class ChangeEmployeePage : DockPanel
    {
        public EmployeeDirectory.Models.Employee AssociatedEmployee { get; }

        public ChangeEmployeePage(EmployeeDirectory.Models.Employee employee)
        {
            InitializeComponent();

            AssociatedEmployee = employee;

            firstName.Text = employee.FirstName;
            secondName.Text = employee.SecondName ?? "";
            middleName.Text = employee.MiddleName ?? "";
            if (employee.BirthDay != null)
                birthday.DisplayDate = (DateTime)employee.BirthDay;
        }

        private void changeEmployee_Click(object sender, RoutedEventArgs e)
        {
            // Add user input data check 

            EmployeeDirectory.Infrastructure.ResultCode resultCode;
            MainWindow.Current.DataAccessor.ChangeUser(AssociatedEmployee.Id, "", 
                firstName.Text == "" ? null : firstName.Text, 
                secondName.Text == "" ? null : secondName.Text,
                middleName.Text == "" ? null : middleName.Text,
                birthday.SelectedDate,
                out resultCode);

            // Add resultCode handler!

            // If all is OK:

            // Search the corresponding tab:
            Predicate<Elements.Tab> predicate = (Elements.Tab tab) =>
            {
                EmployeePage page = tab.AssociatedPage as EmployeePage;
                if (page == null)
                    return false;
                if (page.AssociatedEmployee == AssociatedEmployee)
                    return true;
                return false;
            };

            Elements.Tab tab = MainWindow.Current.FindTab(predicate);
            MainWindow.Current.ActiveTab = tab ?? MainWindow.Current.MainTab;
        }
    }
}
