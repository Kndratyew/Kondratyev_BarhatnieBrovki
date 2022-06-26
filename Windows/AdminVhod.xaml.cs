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
using System.Windows.Shapes;

namespace BarhatnieBrovki.Windows
{
    /// <summary>
    /// Логика взаимодействия для AdminVhod.xaml
    /// </summary>
    public partial class AdminVhod : Window
    {
        public AdminVhod()
        {
            InitializeComponent();
        }

        private void AutorizationClick(object sender, RoutedEventArgs e)
        {
            if(InputField.Password != "0000")
            {
                MessageBox.Show("Неверный пароль!","Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else
            {
                MessageBox.Show("Вход выполнен успешно!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                MainWindow mainWindow = new MainWindow(1);
                mainWindow.Show();
                Close();

            }
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            Autorization autorization = new Autorization();
            autorization.Show();
            Close();
        }
        private void EnterDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                AutorizationClick(null, null);
            }

            else if (e.Key == Key.Escape)
            {
                Back(null, null);
            }
        }
    }
}
