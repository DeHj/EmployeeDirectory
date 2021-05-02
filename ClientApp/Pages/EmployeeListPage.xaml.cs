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
    public partial class EmployeeListPage : DockPanel
    {
        public EmployeeListPage()
        {
            InitializeComponent();

            allEmployees_Click(this, new RoutedEventArgs());
        }

        private void find_Click(object sender, RoutedEventArgs e)
        {
            // Add user input check 

            string fn = firstName.Text == "" ? null : firstName.Text;
            string sn = secondName.Text == "" ? null : secondName.Text;
            string mn = middleName.Text == "" ? null : middleName.Text;

            EmployeeDirectory.Infrastructure.ResultCode resultCode;
            var employees = MainWindow.Current.DataAccessor.GetEmployeesByName(fn, sn, mn, 0, 10, out resultCode);

            DrawEmployeesList(employees);
        }

        private void allEmployees_Click(object sender, RoutedEventArgs e)
        {
            EmployeeDirectory.Infrastructure.ResultCode resultCode;
            var employees = MainWindow.Current.DataAccessor.GetAllEmployees(0, 10, out resultCode);

            // Add resultCode handler

            DrawEmployeesList(employees);
        }



        public void AddEmployeeField(EmployeeDirectory.Models.Employee employee)
        {
            Elements.EmployeeField employeeField = new Elements.EmployeeField(employee);
            control_employees.Items.Add(employeeField);
        }

        public void DrawEmployeesList(IEnumerable<EmployeeDirectory.Models.Employee> employees)
        {
            control_employees.Items.Clear();

            if (employees.Any())
            {
                foreach (var employee in employees)
                {
                    AddEmployeeField(employee);
                }
            }
            else
            {
                control_employees.Items.Add(new TextBlock() { Text = Properties.Resources.noResult });
            }
        }
    }
}
