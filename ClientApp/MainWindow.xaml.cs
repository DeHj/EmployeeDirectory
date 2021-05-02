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

        public EmployeeDirectory.Infrastructure.IDataAccessor DataAccessor { get; set; } = new Infrastructure.StubAccessor();

        //public Dictionary<string, UIElement> Pages { get; } = new Dictionary<string, UIElement>();

        private Elements.Tab activeTab;
        public Elements.Tab ActiveTab { 
            get { return activeTab; }
            set {
                if (activeTab != null)
                    activeTab.Background = Application.Current.Resources["nonActiveTab"] as SolidColorBrush;

                activeTab = value;
                pageContainer.Child = value.AssociatedPage as UIElement;
                value.AssociatedPage.Update();
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
            MainTab = new Elements.Tab(Properties.Resources.mainPage, false, MainPage);
            AddTab(MainPage, MainTab);
        }

        private void addEmployee(object sender, RoutedEventArgs e)
        {
            Pages.AddEmployeePage addEmpPage = new Pages.AddEmployeePage();
            string tabText = GiveFreeTabName(Properties.Resources.addNewEmployee);

            AddTab(addEmpPage, new Elements.Tab(tabText, true, addEmpPage));
        }

        public void AddTab(UIElement page, Elements.Tab tab)
        {
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
            tabs.Children.Remove(tab);
        }

        public void CloseTab(UIElement page)
        {
            foreach (var tab in tabs.Children)
            {
                if ((tab as Elements.Tab).AssociatedPage == page)
                {
                    CloseTab(tab as Elements.Tab);
                    break;
                }
            }
        }

        public string GiveFreeTabName(string tabName)
        {
            Predicate<string> nameIsTaken = (string name) =>
            {
                foreach (var tab in tabs.Children)
                {
                    if ((tab as Elements.Tab).TabName == name)
                        return true;
                    else
                        continue;
                }
                return false;
            };

            if (nameIsTaken(tabName) == false)
                return tabName;

            for(int i = 0; ; i++)
            {
                string curName = $"{tabName} ({i})";
                if (nameIsTaken(curName) == false)
                    return curName;
            }

            throw new Exception("All name is taken");
        }
    }
}
