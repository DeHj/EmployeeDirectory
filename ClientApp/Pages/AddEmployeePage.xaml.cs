using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ClientApp.Pages
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class AddEmployeePage : DockPanel
    {
        public AddEmployeePage()
        {
            InitializeComponent();
        }

        private void addEmployee_Click(object sender, RoutedEventArgs e)
        {
            // Add user input check!

            EmployeeDirectory.Infrastructure.ResultCode resultCode;
            int employeeId;
            MainWindow.Current.DataAccessor.AddUser(login.Text, "", firstName.Text, out employeeId, out resultCode);

            // Add resultCode handler!

            EmployeeDirectory.Models.Employee employee = new EmployeeDirectory.Models.Employee
            {
                Login = login.Text,
                Id = employeeId,
                FirstName = firstName.Text,
            };

            EmployeePage newPage = new EmployeePage(employee);
            string tabName = employee.Login;
            Elements.Tab newTab = new Elements.Tab(tabName, true, newPage);

            MainWindow.Current.CloseTab(this);
            MainWindow.Current.AddTab(tabName, newPage, newTab);
        }
    }
}
