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
using YP2_122_Cheban_Aleksandra.Pages;

namespace YP2_122_Cheban_Aleksandra
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private string _currentUserLogin;
        public MainPage(string userLogin)
        {
            InitializeComponent();
            _currentUserLogin = userLogin;
        }

        private void Btn_ClickActiv(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ActivPage());
        }
        private void Btn_ClicFinished(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new FinishedPage());
        }
        private void Btn_ClicMy(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new MyPage(_currentUserLogin));
        }
    }
}
