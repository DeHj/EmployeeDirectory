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
    /// Interaction logic for element.xaml
    /// </summary>
    public partial class PhoneNumberBox : Grid
    {
        string number = "";

        public PhoneNumberBox()
        {
            InitializeComponent();

            phoneNumber.Text = ConvertPhoneNumber(number.ToString());
            txtUserEntry.MaxLength = phoneNumber.Text.Length;
        }

        private void txtUserEntry_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 && number.Length < 10)
                number += (e.Key - Key.D0).ToString();

            if (e.Key == Key.Back)
                number = number.Substring(0, Math.Max(0, number.Length - 1));

            phoneNumber.Text = ConvertPhoneNumber(number.ToString());
            txtUserEntry.Text = "8(888)888-88-88";
        }

        private string ConvertPhoneNumber(string number)
        {
            char[] ch = new char[10];
            for (int i = 0; i < 10; i++)
                ch[i] = number.Length > i ? number[i] : ' ';

            return $"8({ch[0]}{ch[1]}{ch[2]}){ch[3]}{ch[4]}{ch[5]}-{ch[6]}{ch[7]}-{ch[8]}{ch[9]}";
        }
    }
}
