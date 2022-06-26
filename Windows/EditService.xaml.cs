using BarhatnieBrovki.DatabaseField;
using BarhatnieBrovki.Pages;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
    /// Логика взаимодействия для EditService.xaml
    /// </summary>
    public partial class EditService : Window
    {
        BarhatniyeBrovkiEntities db;
        Services service;
        string pathForImg;
        public EditService(Services _service)
        {
            InitializeComponent();
            db = new BarhatniyeBrovkiEntities();
            service = db.Services.Where(x => x.ID == _service.ID).FirstOrDefault();
            Tname.Text = service.Title;           
            Tcost.Text = Math.Round(service.CostWithoutDiscount, 2).ToString();
            Tduration.Text = (service.DurationInSeconds * 60).ToString();
            Tdiscount.Text = service.Discount.ToString();
            if (service.Description != null ) Tdescription.Text = service.Description.ToString();
           if (service.MainImagePath !=null && service.MainImagePath.Length > 21)
              imgName.Content = service.mainImagePath.Substring(22);
            else imgName.Content = "изображение отсутствует";

            //вывод доп картинок 
            List<ServicePhotoes> listdop = service.ServicePhotoes.ToList();            
            DopPhotoListbox.ItemsSource = listdop;
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveEditing(object sender, RoutedEventArgs e)
        {
            if (Tdiscount.Text == null || Tdiscount.Text == "") Tdiscount.Text = "0";
            if (Tname.Text == "" || Tname.Text == null ||
               Tcost.Text == "" || Tcost.Text == null ||
               Tduration.Text == "" || Tduration.Text == null ||
               Tdescription.Text == "" || Tdescription.Text == null
               )
            {
                MessageBox.Show("Все поля должны быть заполнены!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                db = new BarhatniyeBrovkiEntities();
                Services _serv = db.Services.Where(x => x.ID == service.ID).FirstOrDefault();

                _serv.Title = Tname.Text;

                int value2 = 0;
                if (int.TryParse(Tduration.Text, out value2))
                {
                    if (value2 > 14400)
                    {
                        throw new Exception("Длительность услуги не может быть больше 4 часов!");
                    }
                    _serv.DurationInSeconds = value2 * 60;
                }

                double value3 = 0;
                if (Double.TryParse(Tdiscount.Text, out value3))
                {
                    _serv.Discount = value3 / 10000;
                }

                decimal value = 0;
                if (decimal.TryParse(Tcost.Text, out value))
                {
                    decimal b = value * ((decimal)_serv.Discount);
                    _serv.Cost = value;
                }

                _serv.Description = Tdescription.Text;

                if(imgName.Content.ToString() == "изображение отсутствует")
                {
                    throw new Exception("Добавьте изображение!");
                }
                _serv.MainImagePath = pathForImg;
                db.SaveChanges();
                MessageBox.Show("Успешно!", "Уведомление");
            }
            catch(Exception ee)
            {
                if (ee.ToString().Contains("Длительность услуги не может быть больше"))
                {
                    MessageBox.Show("Длительность услуги не может быть больше 4 часов!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                    MessageBox.Show("Провал! Ошибка:" + ee.ToString(), "Уведомление");
            }
        }

        private void AddFotka(object sender, RoutedEventArgs e) //прикрепить фотку
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Multiselect = false;
            op.Title = "Выберите изображение";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            bool? result = op.ShowDialog();
            bool flag = false;
            if (result == true)
            {                

                string imgPath = "Услуги салона красоты\\" + op.SafeFileName; //Куда сохраняется файл         
                pathForImg = imgPath;

                //проверяем, используется ли такая картинка в базе
                if (File.Exists(imgPath))
                {
                    imgName.Content = op.SafeFileName;
                    flag = true;
                }
                if (flag != true)
                {
                    MemoryStream ms = new MemoryStream();  //Поток памяти
                    FileStream fs = new FileStream(imgPath, FileMode.Create); //Поток файла

                    var bitmap = new BitmapImage(new Uri(op.FileName));

                    PngBitmapEncoder pngEnc = new PngBitmapEncoder(); //сохраняем в формате PNG
                    pngEnc.Frames.Add(BitmapFrame.Create(bitmap));
                    pngEnc.Save(fs);
                    fs.Close();
                    imgName.Content = op.SafeFileName;
                }
            }
        }

        private void DeleteFotka(object sender, RoutedEventArgs e)
        {
            if (imgName.Content == "изображение отсутствует")
            {
                MessageBox.Show("Изображения нет!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
                MessageBoxResult pr = MessageBox.Show("Вы уверены? Данное действие необратимо","Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(pr == MessageBoxResult.Yes)
            {
                imgName.Content = "изображение отсутствует";
            }
        }

        private void DeleteDopFotka(object sender, RoutedEventArgs e)
        {
            //удалить строчку в базе, обновить список доп фоток DopPhotoListbox        
            
            MessageBoxResult pr = MessageBox.Show("Вы уверены? Данное действие необратимо", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (pr == MessageBoxResult.Yes)
            {
                //...
                db = new BarhatniyeBrovkiEntities();
                Button button = sender as Button;
                ServicePhotoes servicephotos = button.DataContext as ServicePhotoes;
                ServicePhotoes inbase = db.ServicePhotoes.Where(x => x.ID == servicephotos.ID).FirstOrDefault();               
                db.ServicePhotoes.Remove(inbase);
                db.SaveChanges();

                //обновляем вывод доп картинок 
                //вывод доп картинок 
                db = new BarhatniyeBrovkiEntities();
                Services local = db.Services.Where(x => x.ID == service.ID).FirstOrDefault();
                List<ServicePhotoes> listdop2 = local.ServicePhotoes.ToList();
                DopPhotoListbox.ItemsSource = listdop2;
                //List<ServicePhotoes> listdop2 = service.ServicePhotoes.ToList();
                //List<string> image2 = new List<string>();
                //if (listdop2.Count != 0)
                //    foreach (var item in listdop2)
                //    {
                //        image2.Add(item.PhotoPathSet.Substring(22));
                //    }
                //DopPhotoListbox.ItemsSource = image2;
            }
        }

        private void AddDopFotka(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Multiselect = false;
            op.Title = "Выберите изображение";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            bool? result = op.ShowDialog();
            bool flag = false;
            if (result == true)
            {
                db = new BarhatniyeBrovkiEntities();
                string imgPath = "Услуги салона красоты\\" + op.SafeFileName; //Куда сохраняется файл         
                pathForImg = imgPath;

                //проверяем, используется ли такая картинка в базе
                if (File.Exists(imgPath))
                {
                    db.ServicePhotoes.Add(new ServicePhotoes() { ServiceID = service.ID, PhotoPath = imgPath });
                    db.SaveChanges();

                    //обновляем список доп фоток                    
                    Services local = db.Services.Where(x => x.ID == service.ID).FirstOrDefault();
                    List<ServicePhotoes> listdop2 = local.ServicePhotoes.ToList();
                    DopPhotoListbox.ItemsSource = listdop2;

                    flag = true;
                }
                if (flag != true)
                {
                    MemoryStream ms = new MemoryStream();  //Поток памяти
                    FileStream fs = new FileStream(imgPath, FileMode.Create); //Поток файла

                    var bitmap = new BitmapImage(new Uri(op.FileName));

                    PngBitmapEncoder pngEnc = new PngBitmapEncoder(); //сохраняем в формате PNG
                    pngEnc.Frames.Add(BitmapFrame.Create(bitmap));
                    pngEnc.Save(fs);
                    fs.Close();

                    db.ServicePhotoes.Add(new ServicePhotoes() { ServiceID = service.ID, PhotoPath = imgPath });
                    db.SaveChanges();

                    //обновляем список доп фоток                    
                    Services local = db.Services.Where(x => x.ID == service.ID).FirstOrDefault();
                    List<ServicePhotoes> listdop2 = local.ServicePhotoes.ToList();
                    DopPhotoListbox.ItemsSource = listdop2;
                }
            }
        }
    }
}
