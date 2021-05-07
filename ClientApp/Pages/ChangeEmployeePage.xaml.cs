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
    public partial class ChangeEmployeePage : DockPanel, IPage
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

            firstName.txtUserEntry.TextChanged += AddEmployeePage_TextChanged;
            secondName.txtUserEntry.TextChanged += AddEmployeePage_TextChanged;
            middleName.txtUserEntry.TextChanged += AddEmployeePage_TextChanged;
        }

        private void ChangeEmployee_Click(object sender, RoutedEventArgs e)
        {
            var employee = new EmployeeDirectory.Models.Employee
            {
                Id = AssociatedEmployee.Id,
                Login = "login",
                FirstName = firstName.Text,
                SecondName = secondName.Text,
                MiddleName = middleName.Text,
                BirthDay = birthday.SelectedDate
            };

            if (employee.IsValid())
            {
                MainWindow.Current.DataAccessor.ChangeEmployee(AssociatedEmployee.Id, "",
                    firstName.Text == "" ? null : firstName.Text,
                    secondName.Text == "" ? null : secondName.Text,
                    middleName.Text == "" ? null : middleName.Text,
                    birthday.SelectedDate,
                    out EmployeeDirectory.Infrastructure.ResultCode resultCode);

                if (resultCode == EmployeeDirectory.Infrastructure.ResultCode.OK)
                {
                    MainWindow.Current.LastEmployeeChange = new EventArgs();

                    // Search the corresponding tab:
                    bool predicate(Elements.Tab tab)
                    {
                        EmployeePage page = tab.AssociatedPage as EmployeePage;
                        if (page == null)
                            return false;
                        if (page.AssociatedEmployee == AssociatedEmployee)
                            return true;
                        return false;
                    }

                    Elements.Tab tab = MainWindow.Current.FindTab(predicate);
                    MainWindow.Current.ActiveTab = tab ?? MainWindow.Current.MainTab;
                }

                else if (resultCode == EmployeeDirectory.Infrastructure.ResultCode.NotExist)
                    new Windows.MessageWindow(Properties.Resources.employeeNotExist).ShowDialog();

                else if (resultCode == EmployeeDirectory.Infrastructure.ResultCode.InternalError)
                    new Windows.MessageWindow(Properties.Resources.serverErrorMessage).ShowDialog();

                else
                    throw new Exception("Unexpected resultCode value");
            }
            else
            {
                if (EmployeeDirectory.Models.Employee.StringIsValid(firstName.Text) == false ||
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

            if (EmployeeDirectory.Models.Employee.StringIsValid(firstName.Text) == false ||
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
