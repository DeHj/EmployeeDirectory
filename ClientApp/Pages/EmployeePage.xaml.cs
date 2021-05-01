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
        public EmployeeDirectory.Models.Employee AssociatedEmployee { get; }
        public IEnumerable<EmployeeDirectory.Models.Phone> Phones {
            get;
            private set;
        }

        public EmployeePage(EmployeeDirectory.Models.Employee employee)
        {
            InitializeComponent();

            AssociatedEmployee = employee;

            nameText.Text = $"{employee.FirstName} {employee.SecondName ?? ""} {employee.MiddleName ?? ""}".Replace("  ", " ");
            loginText.Text = employee.Login;
            if (employee.BirthDay != null) birthdayText.Text = employee.BirthDay?.GetDateTimeFormats('D').First();

            Phones = UpdatePhones();
            RedrawPhones();
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

        public IEnumerable<EmployeeDirectory.Models.Phone> UpdatePhones()
        {
            EmployeeDirectory.Infrastructure.ResultCode resultCode;
            return MainWindow.Current.DataAccessor.GetPhonesById(AssociatedEmployee.Id, out resultCode);

            // Add resultCode handler!
        }

        public void RedrawPhones()
        {
            foreach (var phone in Phones)
            {
                Elements.PhoneField phoneField = new Elements.PhoneField(phone);
                phonesList.Items.Add(phoneField);
            }

            if (Phones.Any())
            {
                phonesListText.Text = Properties.Resources.employeePhonesList;
                phonesListText.Visibility = Visibility.Visible;
            }
            else
            {
                phonesListText.Text = Properties.Resources.emptyEmployeePhonesList;
                phonesListText.Visibility = Visibility.Collapsed;
            }
        }
    }
}
