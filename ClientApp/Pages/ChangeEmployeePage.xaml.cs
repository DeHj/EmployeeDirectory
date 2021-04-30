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

namespace ClientApp.Pages
{
    /// <summary>
    /// Interaction logic for ChangeEmployeePage.xaml
    /// </summary>
    public partial class ChangeEmployeePage : DockPanel
    {
        public ChangeEmployeePage()
        {
            InitializeComponent();
        }

        public void SetValues(Employee employee)
        {
            firstName.Text = employee.FirstName;
            secondName.Text = employee.SecondName ?? "";
            middleName.Text = employee.MiddleName ?? "";
            if (employee.BirthDay != null)
                birthday.DisplayDate = (DateTime)employee.BirthDay;
        }

        private void changeEmployee_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
