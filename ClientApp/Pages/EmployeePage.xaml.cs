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
    /// Interaction logic for EmployeePage.xaml
    /// </summary>
    public partial class EmployeePage : DockPanel
    {
        public EmployeeDirectory.Models.Employee AssociatedEmployee { get; set; }
        //public IEnumerable<EmployeeDirectory.Models.Phone> GetPhones {
        //    get {
        //        return phonesList.Items.
        //    } 
        //}

        public EmployeePage(EmployeeDirectory.Models.Employee employee)
        {
            InitializeComponent();

            AssociatedEmployee = employee;

            nameText.Text = $"{employee.FirstName} {employee.SecondName ?? ""} {employee.MiddleName ?? ""}".Replace("  ", " ");
            loginText.Text = employee.Login;
            if (employee.BirthDay != null) birthdayText.Text = employee.BirthDay?.GetDateTimeFormats('D').First();
        }

        private void changeEmployee_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addPhoneNumber_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteEmployee_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
