using BarhatnieBrovki.Pages;
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

namespace BarhatnieBrovki
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int typeAccount; //0-пользователь, 1-администратор
        public MainWindow(int _typeAccount)
        {
            InitializeComponent();
            typeAccount = _typeAccount;
            MainFrame.Navigate(new ServicesPage(typeAccount)); //стартовое окно-список услуг
          
            if (typeAccount == 0) //если зашел пользователь, то прятаем админскую панель
            {
                StepToAddServices.Visibility = Visibility.Hidden;
                StepToUpcomingEntries.Visibility = Visibility.Hidden;
            }
            else //показываем админскую панель для админа
            {
                StepToAddServices.Visibility = Visibility.Visible;
                StepToUpcomingEntries.Visibility = Visibility.Visible;
            }
        }

        private void StepToServicesButton(object sender, RoutedEventArgs e) //услуги
        {
            StepToServices.Background = new SolidColorBrush(Color.FromRgb(255, 74, 109));
            StepToAddServices.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            StepToUpcomingEntries.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            MainFrame.Navigate(new ServicesPage(typeAccount)); 
        }

        private void StepToAddServicesButton(object sender, RoutedEventArgs e) //добавить услугу
        {
            StepToServices.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            StepToAddServices.Background = new SolidColorBrush(Color.FromRgb(255, 74, 109)); 
            StepToUpcomingEntries.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            MainFrame.Navigate(new AddServicePage()); //!!
        }

        private void StepToUpcomingEntriesButton(object sender, RoutedEventArgs e) //ближайшие записи
        {
            StepToServices.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            StepToAddServices.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            StepToUpcomingEntries.Background = new SolidColorBrush(Color.FromRgb(255, 74, 109));
            MainFrame.Navigate(new NearestEntry());
          //  MainFrame.Navigate(new ServicesPage()); //!!
        }
    }
}
