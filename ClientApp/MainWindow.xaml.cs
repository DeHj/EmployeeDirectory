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

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly Dictionary<string, UIElement> Pages = new Dictionary<string, UIElement>();
        public Elements.Tab ActiveTab { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            // Create not closable tab
            Pages.EmployeeListPage mainPage = new Pages.EmployeeListPage();
            Pages.Add("main-page", mainPage);
            Elements.Tab mainTab = new Elements.Tab("main-page", Properties.Resources.mainPage, false, this, mainPage);
            tabs.Children.Add(mainTab);
            ActiveTab = mainTab;
        }

        private void addEmployee(object sender, RoutedEventArgs e)
        {
            Pages.AddEmployeePage addEmpPage = new Pages.AddEmployeePage();
            string pageName = "add-employee-page";
            string tabText = Properties.Resources.addNewEmployee;

            if (Pages.ContainsKey(pageName))
            {
                for (int counter = 0; ; counter++ )
                {
                    if (Pages.ContainsKey($"{pageName}({counter})") == false)
                    {
                        pageName = $"{pageName}({counter})";
                        tabText = $"{tabText}({counter})";
                        break;
                    }
                }
            }

            AddTab(pageName, addEmpPage, new Elements.Tab(pageName, tabText, true, this, addEmpPage));
        }

        public void AddTab(string pageName, UIElement page, Elements.Tab tab)
        {
            Pages.Add(pageName, page);
            pageContainer.Child = page;
            tabs.Children.Add(tab);
        }
    }
}
