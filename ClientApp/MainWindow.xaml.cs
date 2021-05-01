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
        public static MainWindow Current { get; private set; }

        public EmployeeDirectory.Infrastructure.IDataAccessor DataAccessor { get; set; } = new Infrastructure.ServerAccessor();

        public Dictionary<string, UIElement> Pages { get; } = new Dictionary<string, UIElement>();

        private Elements.Tab activeTab;
        public Elements.Tab ActiveTab { 
            get { return activeTab; }
            set {
                if (activeTab != null)
                    activeTab.Background = Application.Current.Resources["nonActiveTab"] as SolidColorBrush;

                activeTab = value;
                pageContainer.Child = value.AssociatedPage;
                //pageContainer.UpdateLayout();
                //this.UpdateLayout();
                activeTab.Background = Application.Current.Resources["activeTab"] as SolidColorBrush;
            } 
        }

        public Pages.EmployeeListPage MainPage { get; }
        public Elements.Tab MainTab { get; }


        public MainWindow()
        {
            InitializeComponent();

            Current = this;

            // Create not closable tab
            MainPage = new Pages.EmployeeListPage();
            MainTab = new Elements.Tab("main-page", Properties.Resources.mainPage, false, MainPage);
            AddTab("main-page", MainPage, MainTab);
            


            EmployeeDirectory.Models.Employee emp1 = new EmployeeDirectory.Models.Employee
            {
                FirstName = "Denis",
                Login = "dehabs",
                BirthDay = new DateTime(1997, 8, 30)
            };
            MainPage.AddEmployeeField(emp1);
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

            AddTab(pageName, addEmpPage, new Elements.Tab(pageName, tabText, true, addEmpPage));
        }

        public void AddTab(string pageName, UIElement page, Elements.Tab tab)
        {
            Pages.Add(pageName, page);
            pageContainer.Child = page;
            tabs.Children.Add(tab);
            ActiveTab = tab;
        }

        public Elements.Tab FindTab(Predicate<Elements.Tab> predicate)
        {
            foreach (var tab in tabs.Children)
            {
                if (predicate(tab as Elements.Tab))
                    return tab as Elements.Tab;
            }
            return null;
        }
        
        public void CloseTab(Elements.Tab tab)
        {
            ActiveTab = MainTab;
            Pages.Remove(tab.TabName);
            tabs.Children.Remove(tab);
        }
    }
}
