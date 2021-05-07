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
    /// Логика взаимодействия для PhoneField.xaml
    /// </summary>
    public partial class PhoneField : Border
    {
        public EmployeeDirectory.Models.Phone AssociatedPhone { get; }
        public EmployeeDirectory.Models.Employee AssociatedEmployee { get; }


        public PhoneField(EmployeeDirectory.Models.Phone phone, EmployeeDirectory.Models.Employee employee)
        {
            InitializeComponent();

            AssociatedPhone = phone;
            AssociatedEmployee = employee;

            string ch = phone.PhoneNumber;
            control_PhoneNumber.Text = $"{ch[0]}({ch[1]}{ch[2]}{ch[3]}){ch[4]}{ch[5]}{ch[6]}-{ch[7]}{ch[8]}-{ch[9]}{ch[10]}";
        }

        private void ChangePhone_Click(object sender, RoutedEventArgs e)
        {
            var page = new Pages.AddPhonePage(AssociatedPhone, AssociatedEmployee.Login);
            string tabName = $"{AssociatedPhone.PhoneNumber} - {Properties.Resources.changePhoneTab}";
            var tab = new Tab(MainWindow.Current.GiveFreeTabName(tabName), true, page);

            MainWindow.Current.AddTab(page, tab);
        }

        private void DeletePhone_Click(object sender, RoutedEventArgs e)
        {
            Windows.ConfirmDialog dialog = new Windows.ConfirmDialog(Properties.Resources.deletePhoneConfirmMessage);
            if (dialog.ShowDialog() == true)
            {
                MainWindow.Current.DataAccessor.RemovePhone(AssociatedPhone.PhoneNumber, out EmployeeDirectory.Infrastructure.ResultCode resultCode);

                if (resultCode == EmployeeDirectory.Infrastructure.ResultCode.OK)
                {
                    new Windows.MessageWindow(Properties.Resources.successfulPhoneRemovingMessage).ShowDialog();
                    MainWindow.Current.LastPhoneChange = new EventArgs();

                    // Search the existing tab:
                    Tab tab = MainWindow.Current.FindTab((Tab t) =>
                    {
                        Pages.EmployeePage page = t.AssociatedPage as Pages.EmployeePage;
                        if (page == null)
                            return false;
                        return page.AssociatedEmployee.Id == AssociatedPhone.IdEmployee;
                    });

                    if (tab != null)
                        MainWindow.Current.ActiveTab = tab;
                }
                else
                    HandleResultCode(resultCode);
            }
        }

        private static void HandleResultCode(EmployeeDirectory.Infrastructure.ResultCode resultCode)
        {
            if (resultCode == EmployeeDirectory.Infrastructure.ResultCode.NotExist)
                new Windows.MessageWindow(Properties.Resources.phoneErrorEmployeeOrPhoneNotExist).ShowDialog();

            else if (resultCode == EmployeeDirectory.Infrastructure.ResultCode.InternalError)
                new Windows.MessageWindow(Properties.Resources.serverErrorMessage).ShowDialog();

            else
                throw new Exception("Unexpected resultCode value");
        }
    }
}
