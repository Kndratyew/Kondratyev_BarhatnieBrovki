using BarhatnieBrovki.DatabaseField;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для AddKlient.xaml
    /// </summary>
    public partial class AddKlient : Window
    {
        BarhatniyeBrovkiEntities db;
        Services service;
        DateTime dt = DateTime.Now.AddHours(-DateTime.Now.Hour).AddMinutes(-DateTime.Now.Minute).AddSeconds(-DateTime.Now.Second);
        Clients Selectedclient;
        List<Clients> clients = new List<Clients>(); //список клиентов из базы
        int ch = 0;
        public AddKlient(Services _serv, int _ch) 
        {
            InitializeComponent();
            ch = _ch;
            service = _serv;            
            Tname.Content = service.Title;
            Tduration.Content = service.DurationInSeconds;
            Data.DisplayDateStart = DateTime.Now;
            Data.SelectedDate = dt;
            //заполняем список клиентов из бд
            db = new BarhatniyeBrovkiEntities();

            ObservableCollection<ComboBoxItem> combos = new ObservableCollection<ComboBoxItem>();

            clients = db.Clients.ToList();
            Brush color = new SolidColorBrush(Color.FromRgb(255, 74, 109));
            Brush colorRed = new SolidColorBrush(Color.FromRgb(255, 74, 109));
            Brush colorBlue = new SolidColorBrush(Color.FromRgb(255, 228, 255));
            foreach (var item in clients)
            {
                if (color.ToString() == colorRed.ToString()) color = colorBlue;
                else if (color.ToString() == colorBlue.ToString()) color = colorRed;

                combos.Add(new ComboBoxItem { Content = (item.FirstName + " " + item.LastName.Substring(0, 1) + ". " + item.Patronymic.Substring(0, 1) + "."), Background = color });
            }
            KlientCombobox.ItemsSource = combos;

        }

        private void SelectedKlientCombobox(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cbItem = sender as ComboBox;
            int index = cbItem.SelectedIndex;
            Selectedclient = clients[index];
        }

        private void Save(object sender, RoutedEventArgs e)
        {

            if (timeH.Text == "" || timeH.Text == null ||
                timeM.Text == "" || timeM.Text == null ||
                timeS.Text == "" || timeS.Text == null
               )
            {
                MessageBox.Show("Все поля должны быть заполнены (кроме комментария, он необязателен) !", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Selectedclient == null)
            {
                MessageBox.Show("Выберите клиента!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            dt = dt.AddHours(double.Parse(timeH.Text));
            dt = dt.AddMinutes(double.Parse(timeM.Text));
            dt = dt.AddMinutes(double.Parse(Tduration.Content.ToString()));
            dt = dt.AddSeconds(double.Parse(timeS.Text));


            try
            {
                db = new BarhatniyeBrovkiEntities();
                ClientServices zapic = new ClientServices();
           
                
                zapic.ClientID = Selectedclient.ID;
                zapic.ServiceID = service.ID;
                zapic.StartTime = dt;
                zapic.Comment = commentarii.Text;




                db.ClientServices.Add(zapic);
                db.SaveChanges();
                MessageBox.Show("Успешно!", "Уведомление");
            }
            catch (Exception)
            {
                MessageBox.Show("Провал! Ошибка:" + e.ToString(), "Уведомление");
            }
        }

        private void ValidationTimeH(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0) && timeH.Text.Length != 2 &&
                timeH.Text != "9" &&
                timeH.Text != "8" &&
                timeH.Text != "7" &&
                timeH.Text != "6" &&
                timeH.Text != "5" &&
                timeH.Text != "4" &&
                timeH.Text != "3"))
            {
                e.Handled = true;
            }
            else if (timeH.Text == "2")
            {
                if (e.Text == "4" ||
                   e.Text == "5" ||
                   e.Text == "6" ||
                   e.Text == "7" ||
                   e.Text == "8" ||
                   e.Text == "9"
                   )
                    e.Handled = true;
            }         
        }
        private void ValidationTimeM(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0) &&
                timeM.Text.Length != 2 &&
                timeM.Text != "9" &&
                timeM.Text != "8" &&
                timeM.Text != "7" &&
                timeM.Text != "6"))
            {
                e.Handled = true;
            }
        }
        private void ValidationTimeS(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0) &&
                timeS.Text.Length != 2 &&
                timeS.Text != "9" &&
                timeS.Text != "8" &&
                timeS.Text != "7" &&
                timeS.Text != "6"))
            {
                e.Handled = true;
            }
        }

        private void SelectedDate(object sender, SelectionChangedEventArgs e)
        {
            dt = Data.SelectedDate.Value;
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            ServiceVivod servivod = new ServiceVivod(service, ch);
            servivod.Show();
            Close();

        }

        private void CalculatePMS(object sender, RoutedEventArgs e)
        {
            if (timeH.Text == "" || timeH.Text == null ||
                timeM.Text == "" || timeM.Text == null ||
                timeS.Text == "" || timeS.Text == null ||
                timeH.Text == " " || timeH.Text == "  " ||
                timeM.Text == " " || timeM.Text == "  " ||
                timeS.Text == " " || timeS.Text == "  "
               )
            {
                MessageBox.Show("Все поля времени должны быть заполнены!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
          DateTime dt2 = dt.AddHours(double.Parse(timeH.Text));
            dt2 = dt2.AddMinutes(double.Parse(timeM.Text));
            dt2 = dt2.AddMinutes(double.Parse(Tduration.Content.ToString()));
            dt2 = dt2.AddSeconds(double.Parse(timeS.Text));

           TimeEnd.Content = dt2.Date + dt2.TimeOfDay + "";

        }
    }
}
