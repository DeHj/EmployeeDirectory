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
using System.Configuration;
using System.Collections.ObjectModel;

namespace ClientApp.Pages
{
    /// <summary>
    /// Interaction logic for EmployeeListPage.xaml
    /// </summary>
    public partial class EmployeeListPage : DockPanel, IPage
    {
        private EventArgs lastChange;
        bool max = false;
        Button loadMoreButton;

        // If equal true last server request was GetAllEmployees.
        // Otherwise last server request was GetEmployeesByName.

        class SearchParams
        {
            public string FirstName, SecondName, MiddleName;
        }

        SearchParams searchParams;

        int PageSize { get; set; }


        public EmployeeListPage()
        {
            InitializeComponent();

            firstName.txtUserEntry.TextChanged += EmployeeListPage_TextChanged;
            secondName.txtUserEntry.TextChanged += EmployeeListPage_TextChanged;
            middleName.txtUserEntry.TextChanged += EmployeeListPage_TextChanged;

            PageSize = Properties.Settings.Default.PageSize;

            loadMoreButton = new Button();
            loadMoreButton.Click += LoadMore_Click;
            loadMoreButton.Content = Properties.Resources.loadMore_Button;

            control_employees.Children.Add(loadMoreButton);
        }


        #region event handlers

        private void Find_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeeDirectory.Models.Employee.StringIsValid(firstName.Text) == false ||
                EmployeeDirectory.Models.Employee.StringIsValid(secondName.Text) == false ||
                EmployeeDirectory.Models.Employee.StringIsValid(middleName.Text) == false)
            {
                warningMessage_TextBlock.Text = $"{Properties.Resources.warningMessageEmployeeInvalidSymbols} " +
                   $"{EmployeeDirectory.Models.Employee.InvalidSymbols}";
                warningMessage_TextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                string fn = firstName.Text == "" ? null : firstName.Text;
                string sn = secondName.Text == "" ? null : secondName.Text;
                string mn = middleName.Text == "" ? null : middleName.Text;

                var employees = MainWindow.Current.DataAccessor.GetEmployeesByName(fn, sn, mn, 0, PageSize, out EmployeeDirectory.Infrastructure.ResultCode resultCode);

                HandleServerGetRequest(employees, resultCode, true);

                if (resultCode == EmployeeDirectory.Infrastructure.ResultCode.OK)
                    searchParams = new SearchParams { FirstName = fn, SecondName = sn, MiddleName = mn };
            }
        }

        private void AllEmployees_Click(object sender, RoutedEventArgs e)
        {
            var employees = MainWindow.Current.DataAccessor.GetAllEmployees(0, PageSize, out EmployeeDirectory.Infrastructure.ResultCode resultCode);

            HandleServerGetRequest(employees, resultCode, true);

            if (resultCode == EmployeeDirectory.Infrastructure.ResultCode.OK)
                searchParams = null;
        }

        private void LoadMore_Click(object sender, RoutedEventArgs e)
        {
            EmployeeDirectory.Infrastructure.ResultCode resultCode;
            IEnumerable<EmployeeDirectory.Models.Employee> employees;
            
            if (searchParams == null)
                employees = MainWindow.Current.DataAccessor.GetAllEmployees(control_employees.Children.Count - 1, PageSize, out resultCode);
            else
                employees = MainWindow.Current.DataAccessor.GetEmployeesByName(
                    searchParams.FirstName,
                    searchParams.SecondName,
                    searchParams.MiddleName,
                    control_employees.Children.Count - 1, PageSize, out resultCode);

            HandleServerGetRequest(employees, resultCode, false);
        }

        private void EmployeeListPage_TextChanged(object sender, TextChangedEventArgs e)
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

        #endregion




        private void HandleServerGetRequest(IEnumerable<EmployeeDirectory.Models.Employee> employees, EmployeeDirectory.Infrastructure.ResultCode resultCode, bool toClear)
        {
            if (resultCode == EmployeeDirectory.Infrastructure.ResultCode.OK)
            {
                lastChange = MainWindow.Current.LastEmployeeChange;
                max = employees.Count() < PageSize;

                if (toClear)
                    RedrawEmployeesList(employees);
                else
                    AddEmployeesToScreen(employees);
            }
            else if (resultCode == EmployeeDirectory.Infrastructure.ResultCode.InternalError)
                new Windows.MessageWindow(Properties.Resources.serverErrorMessage).ShowDialog();

            else
                throw new Exception("Unexpected resultCode value");
        }




        private void AddEmployeeField(EmployeeDirectory.Models.Employee employee)
        {
            var employeeField = new Elements.EmployeeField(employee);
            control_employees.Children.Add(employeeField);
        }

        private void RedrawEmployeesList(IEnumerable<EmployeeDirectory.Models.Employee> employees)
        {
            if (employees.Any())
            {
                control_employees.Children.Clear();
                control_employees.Children.Add(loadMoreButton);

                AddEmployeesToScreen(employees);
            }
            else
            {
                control_employees.Children.Clear();

                TextBlock noResult = new TextBlock();
                noResult.Text = Properties.Resources.noResult;
                control_employees.Children.Add(noResult);
            }
        }

        private void AddEmployeesToScreen(IEnumerable<EmployeeDirectory.Models.Employee> employees)
        {
            if (control_employees.Children[control_employees.Children.Count - 1] is Button)
                control_employees.Children.RemoveAt(control_employees.Children.Count - 1);

            if (employees != null)
            {
                foreach (var employee in employees) 
                {
                    AddEmployeeField(employee);
                }
            }

            if (max == false)
                control_employees.Children.Add(loadMoreButton);
        }




        public void Update()
        {
            max = false;

            warningMessage_TextBlock.Visibility = Visibility.Hidden;

            if (lastChange != MainWindow.Current.LastEmployeeChange)
            {
                if (firstName.Text != "" ||
                secondName.Text != "" ||
                middleName.Text != "")
                {
                    Find_Click(this, new RoutedEventArgs());
                }
                else
                    AllEmployees_Click(this, new RoutedEventArgs());
            }
        }
    }
}
