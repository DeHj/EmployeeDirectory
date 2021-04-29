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

namespace ClientApp.Elements
{
    /// <summary>
    /// Interaction logic for EmployeeField.xaml
    /// </summary>
    public partial class EmployeeField : Grid
    {
        public EmployeeField(string firstName, string secondName, string middleName, string login, DateTime? birthday)
        {
            InitializeComponent();

            nameText.Text = $"{firstName} {secondName ?? ""} {middleName ?? ""}".Replace("  ", " ");
            loginText.Text = login;
            if (birthday != null) birthdayText.Text = birthday?.GetDateTimeFormats('D').First();
        }
    }
}
