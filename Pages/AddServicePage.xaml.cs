using BarhatnieBrovki.DatabaseField;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BarhatnieBrovki.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddServicePage.xaml
    /// </summary>
    public partial class AddServicePage : Page
    {
        BarhatniyeBrovkiEntities db;
        string pathForImg;
        public AddServicePage()
        {
            InitializeComponent();
            imgName.Content = "изображение отсутствует";
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
            db = new BarhatniyeBrovkiEntities();
            if (db.Services.Where(x =>x.Title == Tname.Text).FirstOrDefault() != null)
            {
                MessageBox.Show("Данная услуга уже существует!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        
            try
            {
                db = new BarhatniyeBrovkiEntities();
                Services _serv = new Services();

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
                else
                {
                    MessageBox.Show("Неверные входные данные!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                double value3 = 0;
                if (Double.TryParse(Tdiscount.Text, out value3))
                {
                    if(value3 < 0 || value3 > 100)
                    {
                        MessageBox.Show("Скидка не может быть меньше 0% и больше 100% !", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    _serv.Discount = value3 / 10000;
                }
                else
                {
                    MessageBox.Show("Неверные входные данные!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                decimal value = 0;
                if (decimal.TryParse(Tcost.Text, out value))
                {
                    decimal b = value * ((decimal)_serv.Discount);
                    _serv.Cost = value;
                }
                else
                {
                    MessageBox.Show("Неверные входные данные!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                _serv.Description = Tdescription.Text;
                _serv.MainImagePath = pathForImg;
                db.Services.Add(_serv);
                db.SaveChanges();
                MessageBox.Show("Успешно!", "Уведомление");
            }
            catch (Exception ee)
            {
                if(ee.ToString().Contains("Длительность услуги не может быть больше"))
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
            MessageBoxResult pr = MessageBox.Show("Вы уверены? Данное действие необратимо", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (pr == MessageBoxResult.Yes)
            {
                imgName.Content = "изображение отсутствует";
            }
        }
    }
}
