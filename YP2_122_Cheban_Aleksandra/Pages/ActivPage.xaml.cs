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
    /// Логика взаимодействия для ActivPage.xaml
    /// </summary>
    public partial class ActivPage : Page
    {

        public ActivPage()
        {
            InitializeComponent();
            LoadAdsData();
        }

        private void LoadAdsData()
        {
            try
            {
                using (var db = new Entities())
                {
                    var currentAds = db.ads_data
                        .Include("Ad_title1")
                        .Include("User_login1")
                        .Include("Ad_description1")
                        .Include("Ad_post_date1")
                        .Include("City1")
                        .Include("Category1")
                        .Include("Ad_type1")
                        .Include("Ad_status1")
                        .Where(x => x.Ad_status1 != null &&
                                   x.Ad_status1.ad_status1.ToLower() == "активно")
                        .ToList();

                    ListActiv.ItemsSource = currentAds;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
            }
        }

        private void clearFiltersButton_Click_1(object sender, RoutedEventArgs e)
        {
            tovarFilterTextBox.Text = "";
            sortComboBox.SelectedIndex = 0;
        }

        private void Clidk_Finished(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void tovarFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
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
            try
            {
                using (var db = new Entities())
                {
                    List<ads_data> currentAds = db.ads_data
                        .Include("Ad_title1")
                        .Include("User_login1")
                        .Include("Ad_description1")
                        .Include("Ad_post_date1")
                        .Include("City1")
                        .Include("Category1")
                        .Include("Ad_type1")
                        .Include("Ad_status1")
                        .Where(x => x.Ad_status1 != null &&
                                   x.Ad_status1.ad_status1.ToLower() == "активно")
                        .ToList();

                    // Фильтрация по названию услуги через навигационное свойство Ad_title1
                    if (!string.IsNullOrWhiteSpace(tovarFilterTextBox.Text))
                    {
                        currentAds = currentAds.Where(x =>
                           x.Ad_title1 != null &&
                           x.Ad_title1.ad_title1.ToLower().Contains(tovarFilterTextBox.Text.ToLower())).ToList();
                    }

                    // Сортировка по цене и фильтрация по статусу
                    switch (sortComboBox.SelectedIndex)
                    {
                        case 0: 
                            currentAds = currentAds.OrderByDescending(x => x.price).ToList();
                            break;
                        case 1: 
                            currentAds = currentAds.OrderBy(x => x.price).ToList();
                            break;
                   
                        default:
                            currentAds = currentAds.OrderBy(x => x.price).ToList();
                            break;
                    }

                    // Обновляем ListView вместо DataGrid
                    ListActiv.ItemsSource = currentAds;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении данных: {ex.Message}");
            }
        }
    }
}