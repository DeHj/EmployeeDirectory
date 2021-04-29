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
    /// Interaction logic for PromptTextBox.xaml
    /// </summary>
    public partial class HintTextBox : Grid
    {
        public string Hint { set { promptBlock.Text = value; } }
        public string Text { get { return txtUserEntry.Text; } }

        public HintTextBox()
        {
            InitializeComponent();
        }

        private void txtUserEntry_KeyUp(object sender, KeyEventArgs e)
        {
            promptBlock.Visibility = (txtUserEntry.Text.Length == 0) ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
