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
        /// <param name="id"></param>
        public AddPhonePage(int id)
        {
            InitializeComponent();

            employeeId = id;
            addChangePhone.Content = Properties.Resources.addPhoneNumber_Button;
            message_TextBox.Text = Properties.Resources.addPhoneNumber_TextBlock;
        }

        /// <summary>
        /// Using to change existing phone number
        /// </summary>
        /// <param name="id"></param>
        /// <param name="changingPhone"></param>
        public AddPhonePage(EmployeeDirectory.Models.Phone changingPhone)
        {
            InitializeComponent();

            employeeId = changingPhone.IdEmployee;
            this.changingPhone = changingPhone.PhoneNumber;
            addChangePhone.Content = Properties.Resources.change_Button;
            phoneNumberBox.Number = changingPhone.PhoneNumber;
            message_TextBox.Text = Properties.Resources.changePhoneNumber_TextBlock;
        }

        private void addPhone_Click(object sender, RoutedEventArgs e)
        {
            if (phoneNumberBox.Number.Length == 11)
            {
                EmployeeDirectory.Infrastructure.ResultCode resultCode;

                if (changingPhone == null)
                {
                    MainWindow.Current.DataAccessor.AddPhone(employeeId, phoneNumberBox.Number, out resultCode);
                    // Add resultCode handler!
                }
                else
                {
                    MainWindow.Current.DataAccessor.RemovePhone(changingPhone, out resultCode);
                    // Add resultCode handler!
                    MainWindow.Current.DataAccessor.AddPhone(employeeId, phoneNumberBox.Number, out resultCode);
                    // Add resultCode handler! 
                }

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
                // Add incorrect input notice!
            }

        }
    }
}
