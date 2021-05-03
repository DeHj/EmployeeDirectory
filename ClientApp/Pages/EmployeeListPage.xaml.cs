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
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class EmployeeListPage : DockPanel, IPage
    {
        private EventArgs lastChange;

        ScrollViewer scrollViewer;
        int pageSize = 10;


        public EmployeeListPage()
        {
            InitializeComponent();

            firstName.txtUserEntry.TextChanged += employeeListPage_TextChanged;
            secondName.txtUserEntry.TextChanged += employeeListPage_TextChanged;
            middleName.txtUserEntry.TextChanged += employeeListPage_TextChanged;
        }

        private void find_Click(object sender, RoutedEventArgs e)
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

                EmployeeDirectory.Infrastructure.ResultCode resultCode;
                var employees = MainWindow.Current.DataAccessor.GetEmployeesByName(fn, sn, mn, 0, pageSize, out resultCode);

                if (resultCode == EmployeeDirectory.Infrastructure.ResultCode.OK)
                {
                    lastChange = MainWindow.Current.LastEmployeeChange;
                    RedrawEmployeesList(employees);
                }
                else if (resultCode == EmployeeDirectory.Infrastructure.ResultCode.InternalError)
                    new Windows.MessageWindow(Properties.Resources.serverErrorMessage).ShowDialog();

                else
                    throw new Exception("Unexpected resultCode value");
            }
        }

        private void allEmployees_Click(object sender, RoutedEventArgs e)
        {
            EmployeeDirectory.Infrastructure.ResultCode resultCode;
            var employees = MainWindow.Current.DataAccessor.GetAllEmployees(0, pageSize, out resultCode);

            if (resultCode == EmployeeDirectory.Infrastructure.ResultCode.OK)
            {
                lastChange = MainWindow.Current.LastEmployeeChange;
                RedrawEmployeesList(employees);
            }
            else if (resultCode == EmployeeDirectory.Infrastructure.ResultCode.InternalError)
                new Windows.MessageWindow(Properties.Resources.serverErrorMessage).ShowDialog();

            else
                throw new Exception("Unexpected resultCode value");
        }

        private void employeeListPage_TextChanged(object sender, TextChangedEventArgs e)
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



        public void AddEmployeeField(EmployeeDirectory.Models.Employee employee)
        {
            Elements.EmployeeField employeeField = new Elements.EmployeeField(employee);
            control_employees.Items.Add(employeeField);
        }

        public void RedrawEmployeesList(IEnumerable<EmployeeDirectory.Models.Employee> employees)
        {
            control_employees.Items.Clear();

            if (employees.Any())
            {
                AddEmployeesToScreen(employees);
            }
            else
                control_employees.Items.Add(new TextBlock() { Text = Properties.Resources.noResult });
        }

        public void AddEmployeesToScreen(IEnumerable<EmployeeDirectory.Models.Employee> employees)
        {
            if (employees != null)
            {
                foreach (var employee in employees)
                {
                    AddEmployeeField(employee);
                }
            }
        }



        public void Update()
        {
            warningMessage_TextBlock.Visibility = Visibility.Hidden;

            if (lastChange != MainWindow.Current.LastEmployeeChange)
            {
                if (firstName.Text != "" ||
                secondName.Text != "" ||
                middleName.Text != "")
                {
                    find_Click(this, new RoutedEventArgs());
                }
                else
                    allEmployees_Click(this, new RoutedEventArgs());
            }
        }





        static ScrollViewer findScrollViewer(DependencyObject parent)
        {
            var childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < childCount; i++)
            {
                var elt = VisualTreeHelper.GetChild(parent, i);
                if (elt is ScrollViewer) return (ScrollViewer)elt;
                var result = findScrollViewer(elt);
                if (result != null) return result;
            }
            return null;
        }
        
        private void control_employees_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (scrollViewer == null)
            {
                scrollViewer = findScrollViewer(control_employees);
                
                scrollViewer.ScrollChanged += control_employees_Scroll;
            }
        }

        private void control_employees_Scroll(object sender, ScrollChangedEventArgs e)
        {


            double posBefore = scrollViewer.VerticalOffset;

            MainWindow.Current.Title = $"{ scrollViewer.VerticalOffset } { scrollViewer.ScrollableHeight } " +
                $"{control_employees.Items.Count} {scrollViewer.ViewportHeight}";

            //return;

            // Подгрузка
            if (scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight)
            {
                EmployeeDirectory.Infrastructure.ResultCode resultCode;
                var employees = MainWindow.Current.DataAccessor.GetAllEmployees(control_employees.Items.Count, pageSize, out resultCode);

                if (resultCode == EmployeeDirectory.Infrastructure.ResultCode.OK)
                {
                    lastChange = MainWindow.Current.LastEmployeeChange;
                    AddEmployeesToScreen(employees);

                    scrollViewer.ScrollToVerticalOffset(posBefore);
                }
                else if (resultCode == EmployeeDirectory.Infrastructure.ResultCode.InternalError)
                    new Windows.MessageWindow(Properties.Resources.serverErrorMessage).ShowDialog();

                else
                    throw new Exception("Unexpected resultCode value");
            }
        }
    }
}
