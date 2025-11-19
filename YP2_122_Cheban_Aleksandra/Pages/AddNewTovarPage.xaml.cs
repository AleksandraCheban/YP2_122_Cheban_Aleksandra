using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Логика взаимодействия для AddNewTovarPage.xaml
    /// </summary>
    public partial class AddNewTovarPage : Page
    {
        private ads_data _currentAd = new ads_data();
        private string _currentUserLogin;

        public AddNewTovarPage(string userLogin)
        {
            InitializeComponent();
            _currentUserLogin = userLogin;
            InitializePage();
        }

       
        // Конструктор для РЕДАКТИРОВАНИЯ существующего объявления
        public AddNewTovarPage(string userLogin, ads_data selectedAd)
        {
            InitializeComponent();
            _currentUserLogin = userLogin;
            _currentAd = selectedAd; // Устанавливаем выбранное объявление для редактирования
            InitializePage();
        }

        private void InitializePage()
        {
            CBCity.ItemsSource = new Entities().City.ToList();
            CBCity.DisplayMemberPath = "city1";
            CBCity.SelectedValuePath = "id";

            CBCategory.ItemsSource = new Entities().Category.ToList();
            CBCategory.DisplayMemberPath = "category1";
            CBCategory.SelectedValuePath = "id";

            DataContext = _currentAd;
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentAd.ad_title.ToString()))
                errors.AppendLine("Укажите название объявления!");
            if (string.IsNullOrWhiteSpace(_currentAd.ad_description.ToString()))
                errors.AppendLine("Укажите описание объявления!");
            if (_currentAd.price <= 0)
                errors.AppendLine("Укажите корректную цену!");
            if (_currentAd.city <= 0)
                errors.AppendLine("Выберите город!");
            if (_currentAd.category <= 0)
                errors.AppendLine("Выберите категорию!");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }



            try
            {
                using (var db = new Entities())
                {
                    // Находим пользователя по логину
                    var user = db.User_login.FirstOrDefault(u => u.user_login1 == _currentUserLogin);

                    if (user != null)
                    {
                        if (_currentAd.id == 0)
                        {
                            int currentDate = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
                            // Создаем новое объявление с ID пользователя
                            var newAd = new ads_data()
                            {
                                ad_title = _currentAd.ad_title,
                                ad_description = _currentAd.ad_description,
                                price = _currentAd.price,
                                city = _currentAd.city,
                                category = _currentAd.category,
                                user_login = user.id, // Используем числовой ID пользователя
                               // user_password = user.pass,
                                ad_post_date = currentDate,
                                ad_status = 1,
                                ad_type = 1
                            };

                            db.ads_data.Add(newAd);
                        }
                        else
                        {
                            // Редактируем существующее объявление
                            var existingAd = db.ads_data.Find(_currentAd.id);
                            if (existingAd != null)
                            {
                                existingAd.ad_title = _currentAd.ad_title;
                                existingAd.ad_description = _currentAd.ad_description;
                                existingAd.price = _currentAd.price;
                                existingAd.city = _currentAd.city;
                                existingAd.category = _currentAd.category;
                            }
                        }

                        db.SaveChanges();
                        MessageBox.Show("Объявление успешно сохранено!");
                        NavigationService?.Navigate(new Pages.MyPage(_currentUserLogin));
                    }
                    else
                    {
                        MessageBox.Show("Пользователь не найден");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }

        private void ButtonClean_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            
            TBTitle.Text = "";
            TBDescription.Text = "";
            TBPrice.Text = "";
            CBCity.SelectedIndex = -1;
            CBCategory.SelectedIndex = -1;
            _currentAd = new ads_data();
            DataContext = _currentAd;
        }
    }
}