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

namespace YP2_122_Cheban_Aleksandra
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        
        public AuthPage()
        {
            InitializeComponent();
            
        }

        private void ButtonEnter_OnClick(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(TextBoxLogin.Text) || string.IsNullOrEmpty(PasswordBox.Password))
            {
                MessageBox.Show("Введите логин или пароль");
                return;
            }
            using (var db = new Entities())
            {
                var user = db.ads_data
                            .AsNoTracking()
                            .FirstOrDefault(u => u.User_login1.user_login1 == TextBoxLogin.Text &&
                                                u.User_password1.user_password1 == PasswordBox.Password);

                if (user == null)
                {
                    MessageBox.Show("Пользователь с такими данными не  найден!");
                }
                else
                {
                    MessageBox.Show("Пользователь успешно найден!");
                    string userLogin = TextBoxLogin.Text;
                    NavigationService?.Navigate(new MainPage(userLogin));
                }
            }
        }
    }
}
