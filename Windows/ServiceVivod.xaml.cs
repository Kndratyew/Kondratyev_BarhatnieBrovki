using BarhatnieBrovki.DatabaseField;
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
    /// Логика взаимодействия для ServiceVivod.xaml
    /// </summary>
    public partial class ServiceVivod : Window
    {
        Services service;
        public ServiceVivod(Services _serv, int ch)
        {
            InitializeComponent();
            service = _serv;
            VisibleAdminPanel.Content = ch.ToString();
            if (service.Title != null) Tname.Content = service.Title;
               Tcost.Content = Math.Round(service.CostSet, 2).ToString();
               Tduration.Content = service.DurationInSeconds.ToString();
            if (service.Discount != null) Tdiscount.Content = service.Discount.ToString();

            if (service.Description != null)
            {
                if(service.Description.ToString() == "")
                {
                    Tdescription.Content = "Описание отсутствует"; 
                }
                else
                Tdescription.Content = service.Description.ToString();
            }
            else Tdescription.Content = "Описание отсутствует";

            if (service.MainImagePath != null && service.MainImagePath.Length > 21) 
            {
                img.Source = new BitmapImage(new Uri(service.MainImagePathSet));
                ImgLabel.Content = "Изображение услуги";
            }
            else ImgLabel.Content = "Изображение услуги отсутствует";


            //вывод доп картинок 
            List<ServicePhotoes> listdop = service.ServicePhotoes.ToList();
            List<BitmapImage> image = new List<BitmapImage>();
            if(listdop.Count != 0)
            foreach (var item in listdop)
            {
                image.Add(new BitmapImage(new Uri(item.PhotoPathSet)));
            }
            DopPhotoListbox.ItemsSource = image;

        }

        private void ZapicKlientaButton(object sender, RoutedEventArgs e)
        {
            AddKlient addk = new AddKlient(service, int.Parse(VisibleAdminPanel.Content.ToString()));
            addk.Show();
            Close();
        }
    }
}
