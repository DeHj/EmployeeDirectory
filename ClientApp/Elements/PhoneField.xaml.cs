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

        public PhoneField(EmployeeDirectory.Models.Phone phone)
        {
            InitializeComponent();

            AssociatedPhone = phone;
            string ch = phone.PhoneValue;
            control_PhoneNumber.Text = $"{ch[0]}({ch[1]}{ch[2]}{ch[3]}){ch[4]}{ch[5]}{ch[6]}-{ch[7]}{ch[8]}-{ch[9]}{ch[10]}";
        }

        private void changePhone_Click(object sender, RoutedEventArgs e)
        {
            Pages.AddPhonePage page = new Pages.AddPhonePage(AssociatedPhone);
            string tabName = $"{AssociatedPhone.PhoneValue} - {Properties.Resources.changePhoneTab}";
            Tab tab = new Tab(MainWindow.Current.GiveFreeTabName(tabName), true, page);

            MainWindow.Current.AddTab(page, tab);
        }

        private void deletePhone_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
