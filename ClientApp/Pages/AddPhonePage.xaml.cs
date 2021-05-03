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
    /// Interaction logic for AddPhonePage.xaml
    /// </summary>
    public partial class AddPhonePage : StackPanel, IPage
    {
        int employeeId;
        string changingPhone;

        /// <summary>
        /// Using to add new phone number
        /// </summary>
        public AddPhonePage(int id, string login)
        {
            InitializeComponent();

            employeeId = id;

            addChangePhone.Content = Properties.Resources.addPhoneNumber_Button;
            message_TextBox.Text = $"{Properties.Resources.addPhoneNumber_TextBlock} {login}";
        }

        /// <summary>
        /// Using to change existing phone number
        /// </summary>
        public AddPhonePage(EmployeeDirectory.Models.Phone changingPhone, string login)
        {
            InitializeComponent();

            employeeId = changingPhone.IdEmployee;

            this.changingPhone = changingPhone.PhoneNumber;
            phoneNumberBox.Number = changingPhone.PhoneNumber;

            addChangePhone.Content = Properties.Resources.changePhoneNumber_Button;
            message_TextBox.Text = $"{Properties.Resources.changePhoneNumber_TextBlock} {login}";
        }


        private void addPhone_Click(object sender, RoutedEventArgs e)
        {
            var phone = new EmployeeDirectory.Models.Phone
            {
                IdEmployee = employeeId,
                PhoneNumber = phoneNumberBox.Number
            };

            if (phone.IsValid())
            {
                EmployeeDirectory.Infrastructure.ResultCode resultCode;

                if (changingPhone == null)
                {
                    MainWindow.Current.DataAccessor.AddPhone(employeeId, phoneNumberBox.Number, out resultCode);
                    // Add resultCode handler!

                    // If all is OK:
                    MainWindow.Current.LastPhoneChange = new EventArgs();
                }
                else if(phoneNumberBox.Number != changingPhone)
                {
                    MainWindow.Current.DataAccessor.RemovePhone(changingPhone, out resultCode);
                    // Add resultCode handler!
                    MainWindow.Current.DataAccessor.AddPhone(employeeId, phoneNumberBox.Number, out resultCode);
                    // Add resultCode handler!

                    // If all is OK:
                    MainWindow.Current.LastPhoneChange = new EventArgs();
                }

                // Search existing tab:
                Elements.Tab tab = MainWindow.Current.FindTab((Elements.Tab tab) =>
                {
                    EmployeePage page = tab.AssociatedPage as EmployeePage;
                    if (page == null)
                        return false;
                    if (page.AssociatedEmployee.Id == employeeId)
                        return true;
                    return false;
                });

                MainWindow.Current.CloseTab(this);
                MainWindow.Current.ActiveTab = tab ?? MainWindow.Current.MainTab;
            }
            else
            {
                warningMessage_TextBlock.Text = Properties.Resources.warningMessagePhoneField;
                warningMessage_TextBlock.Visibility = Visibility.Visible;
            }
        }

        private void addPhonePage_KeyDown(object sender, KeyEventArgs e)
        {
            Update();
        }


        public void Update()
        {
            warningMessage_TextBlock.Visibility = Visibility.Hidden;
        }
    }
}
