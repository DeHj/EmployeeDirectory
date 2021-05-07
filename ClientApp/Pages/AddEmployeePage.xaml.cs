using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClientApp.Pages
{
    /// <summary>
    /// Interaction logic for AddEmployeePage.xaml
    /// </summary>
    public partial class AddEmployeePage : DockPanel, IPage
    {
        public AddEmployeePage()
        {
            InitializeComponent();

            login.txtUserEntry.TextChanged += AddEmployeePage_TextChanged;
            firstName.txtUserEntry.TextChanged += AddEmployeePage_TextChanged;
            secondName.txtUserEntry.TextChanged += AddEmployeePage_TextChanged;
            middleName.txtUserEntry.TextChanged += AddEmployeePage_TextChanged;
        }

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            var employee = new EmployeeDirectory.Models.Employee
            {
                Login = login.Text,
                FirstName = firstName.Text,
                SecondName = secondName.Text == "" ? null : secondName.Text,
                MiddleName = middleName.Text == "" ? null : middleName.Text,
                BirthDay = birthday.SelectedDate
            };

            if (employee.IsValid())
            {
                MainWindow.Current.DataAccessor.AddEmployee(login.Text, "", firstName.Text, out int employeeId, out EmployeeDirectory.Infrastructure.ResultCode resultCode);

                if (resultCode == EmployeeDirectory.Infrastructure.ResultCode.OK)
                {
                    employee.Id = employeeId;

                    if (employee.SecondName != null ||
                        employee.MiddleName != null ||
                        employee.BirthDay != null)
                    {
                        MainWindow.Current.DataAccessor.ChangeEmployee(employeeId, "", null,
                            employee.SecondName,
                            employee.MiddleName,
                            employee.BirthDay,
                            out resultCode);
                    }

                    MainWindow.Current.LastEmployeeChange = new EventArgs();
                    EmployeePage newPage = new EmployeePage(employee);
                    string tabName = employee.Login;
                    var newTab = new Elements.Tab(tabName, true, newPage);

                    new Windows.MessageWindow(Properties.Resources.successfulEmployeeAddingMessage).ShowDialog();

                    MainWindow.Current.CloseTab(this);
                    MainWindow.Current.AddTab(newPage, newTab);
                }

                else if (resultCode == EmployeeDirectory.Infrastructure.ResultCode.AlreadyExist)
                    new Windows.MessageWindow(Properties.Resources.errorEmployeeAlreadyExist).ShowDialog();

                else if (resultCode == EmployeeDirectory.Infrastructure.ResultCode.InternalError)
                    new Windows.MessageWindow(Properties.Resources.serverErrorMessage).ShowDialog();

                else
                    throw new Exception("Unexpected resultCode value");
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

        private void AddEmployeePage_TextChanged(object sender, TextChangedEventArgs e)
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
