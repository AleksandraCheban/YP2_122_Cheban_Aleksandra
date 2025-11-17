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
            var currentUsers = Entities.GetContext().ads_data.ToList();
            ListUser.ItemsSource = currentUsers;
        }

        private void clearFiltersButton_Click_1(object sender, RoutedEventArgs e)
        {
            tovarFilterTextBox.Text = "";
            sortComboBox.SelectedIndex = 0;
        }

        private void tovarFilterTextBox_TextChanged(object sender,
TextChangedEventArgs e)
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
                List<ads_data> currentAds = Entities.GetContext().ads_data.ToList();
                
                if (!string.IsNullOrWhiteSpace(tovarFilterTextBox.Text))
                {
                    currentAds = currentAds.Where(x =>
                   x.ad_title.ToString().ToLower().Contains(tovarFilterTextBox.Text.ToLower())).ToList();
                }

                //Фильтрация по убыванию/возрастанию 
                switch (sortComboBox.SelectedIndex)
                {
                    case 0: // По возрастанию цены (дешевле)
                        currentAds = currentAds.OrderBy(x => x.price).ToList();
                        break;
                    case 1: // По убыванию цены (дороже)
                        currentAds = currentAds.OrderByDescending(x => x.price).ToList();
                        break;
                    default:
                        currentAds = currentAds.OrderBy(x => x.price).ToList();
                        break;
                }

                ListUser.ItemsSource = currentAds;
            
            }
            catch (Exception)
            {
            }
        }
    }
}
