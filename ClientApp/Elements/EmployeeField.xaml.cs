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
using EmployeeDirectory.Models;

namespace ClientApp.Elements
{
    /// <summary>
    /// Interaction logic for EmployeeField.xaml
    /// </summary>
    public partial class EmployeeField : Border
    {
        public Employee AssociatedEmployee { get; set; }

        public EmployeeField(Employee employee)
        {
            InitializeComponent();

            AssociatedEmployee = employee;

            nameText.Text = $"{employee.FirstName} {employee.SecondName?? ""} {employee.MiddleName?? ""}".Replace("  ", " ");
            loginText.Text = employee.Login;
            if (employee.BirthDay != null) birthdayText.Text = employee.BirthDay?.GetDateTimeFormats('D').First();
        }

        
        private void nameText_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Predicate<Tab> predicate = (Tab tab) =>
            {
                Pages.EmployeePage page = (tab as Tab).AssociatedPage as Pages.EmployeePage;
                if (page == null)
                    return false;
                if (page.AssociatedEmployee == AssociatedEmployee)
                    return true;
                return false;
            };

            Tab existingTab = MainWindow.Current.FindTab(predicate);

            if (existingTab != null)
                MainWindow.Current.ActiveTab = existingTab;
            else
            {
                Pages.EmployeePage newPage = new Pages.EmployeePage(AssociatedEmployee);
                string tabName = AssociatedEmployee.Login;
                Tab newTab = new Tab(tabName, true, newPage);

                MainWindow.Current.AddTab(newPage, newTab);
            }
        }
    }
}
