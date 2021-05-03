using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClientApp.Pages
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class AddEmployeePage : DockPanel, IPage
    {
        public AddEmployeePage()
        {
            InitializeComponent();

            login.txtUserEntry.TextChanged += addEmployeePage_TextChanged;
            firstName.txtUserEntry.TextChanged += addEmployeePage_TextChanged;
            secondName.txtUserEntry.TextChanged += addEmployeePage_TextChanged;
            middleName.txtUserEntry.TextChanged += addEmployeePage_TextChanged;
        }

        private void addEmployee_Click(object sender, RoutedEventArgs e)
        {
            var employee = new EmployeeDirectory.Models.Employee
            {
                Login = login.Text,
                FirstName = firstName.Text,
                SecondName = secondName.Text,
                MiddleName = middleName.Text,
                BirthDay = birthday.SelectedDate
            };

            if (employee.IsValid())
            {
                EmployeeDirectory.Infrastructure.ResultCode resultCode;
                int employeeId;
                MainWindow.Current.DataAccessor.AddEmployee(login.Text, "", firstName.Text, out employeeId, out resultCode);

                // Add resultCode handler!

                // If all is OK:
                MainWindow.Current.LastEmployeeChange = new EventArgs();
                employee.Id = employeeId;
                EmployeePage newPage = new EmployeePage(employee);
                string tabName = employee.Login;
                Elements.Tab newTab = new Elements.Tab(tabName, true, newPage);

                MainWindow.Current.CloseTab(this);
                MainWindow.Current.AddTab(newPage, newTab);
            }
            else
            {
                if (EmployeeDirectory.Models.Employee.StringIsValid(login.Text) == false ||
                EmployeeDirectory.Models.Employee.StringIsValid(firstName.Text) == false ||
                EmployeeDirectory.Models.Employee.StringIsValid(secondName.Text) == false ||
                EmployeeDirectory.Models.Employee.StringIsValid(middleName.Text) == false)
                {
                    warningMessage_TextBlock.Text = $"{Properties.Resources.warningMessageEmployeeInvalidSymbols} " +
                        $"{EmployeeDirectory.Models.Employee.InvalidSymbols}";
                }
                else
                    warningMessage_TextBlock.Text = $"{Properties.Resources.warningMessageEmployeeInvalidModel}";
                warningMessage_TextBlock.Visibility = Visibility.Visible;
            }
            
        }

        private void addEmployeePage_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
            
            if (EmployeeDirectory.Models.Employee.StringIsValid(login.Text) == false ||
                EmployeeDirectory.Models.Employee.StringIsValid(firstName.Text) == false ||
                EmployeeDirectory.Models.Employee.StringIsValid(secondName.Text) == false ||
                EmployeeDirectory.Models.Employee.StringIsValid(middleName.Text) == false)
            {
                warningMessage_TextBlock.Text = $"{Properties.Resources.warningMessageEmployeeInvalidSymbols} " +
                    $"{EmployeeDirectory.Models.Employee.InvalidSymbols}";
                warningMessage_TextBlock.Visibility = Visibility.Visible;
            }
        }



        public void Update()
        {
            warningMessage_TextBlock.Visibility = Visibility.Hidden;
        }
    }
}
