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

namespace YP2_122_Cheban_Aleksandra.Pages
{
    /// <summary>
    /// Логика взаимодействия для MyPage.xaml
    /// </summary>
    public partial class MyPage : Page
    {
        private string _currentUserLogin;
        public MyPage(string userLogin)
        {
            InitializeComponent();
            _currentUserLogin = userLogin;
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                using (var db = new Entities())
                {
                    // 1. Находим ID пользователя по логину
                    var user = db.User_login.FirstOrDefault(u => u.user_login1 == _currentUserLogin);

                    if (user != null)
                    {
                        // 2. Ищем объявления по ID пользователя с правильными Include
                        var currentAds = db.ads_data
                            .Include("Ad_title1")
                            .Include("Ad_description1")
                            .Include("Ad_post_date1")
                            .Include("City1")
                            .Include("Category1")
                            .Include("Ad_type1")
                            .Include("Ad_status1")
                            .Include("User_login1")  // Добавляем связь с пользователем
                            .Where(x => x.user_login == user.id) // используем числовой ID пользователя
                            .ToList();

                        ListActiv.ItemsSource = currentAds;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AddNewTovarPage(_currentUserLogin));
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            if (ListActiv.SelectedItem is ads_data selectedAd)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить объявление #{selectedAd.id}?",
                                           "Подтверждение удаления",
                                           MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (var db = new Entities())
                        {
                            var adToDelete = db.ads_data.Find(selectedAd.id);
                            if (adToDelete != null)
                            {
                                db.ads_data.Remove(adToDelete);
                                db.SaveChanges();
                                MessageBox.Show("Объявление удалено");
                                LoadData();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите объявление для удаления");
            }
        }

        private void BtChange_Click(object sender, RoutedEventArgs e)
        {
            if (ListActiv.SelectedItem is ads_data selectedAd)
            {
                NavigationService?.Navigate(new AddNewTovarPage(_currentUserLogin, selectedAd));
            }
            else
            {
                MessageBox.Show("Выберите объявление для редактирования");
            }
        }

        private void Clidk_Finished(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void sortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void Update()
        {
            if (!IsInitialized)
            {
                return;
            }
            {
                using (var db = new Entities())
                {
                    var user = db.User_login.FirstOrDefault(u => u.user_login1 == _currentUserLogin);

                    if (user != null)
                    {
                        List<ads_data> currentAds = db.ads_data
                            .Include("Ad_title1")
                            .Include("Ad_description1")
                            .Include("Ad_post_date1")
                            .Include("City1")
                            .Include("Category1")
                            .Include("Ad_type1")
                            .Include("Ad_status1")
                            .Include("User_login1")
                            .Where(x => x.user_login == user.id)
                            .ToList();


                        switch (sortComboBox.SelectedIndex)
                        {
                            case 0: // Активный
                                currentAds = currentAds.Where(x =>
                                    x.Ad_status1 != null &&
                                    x.Ad_status1.ad_status1.ToLower() == "активно").ToList();
                                break;
                            case 1: // Завершенный
                                currentAds = currentAds.Where(x =>
                                    x.Ad_status1 != null &&
                                    x.Ad_status1.ad_status1.ToLower() == "завершено").ToList();
                                break;
                            default:
                                break;
                        }

                        ListActiv.ItemsSource = currentAds;
                    }
                }
            }
        }
    }
}
