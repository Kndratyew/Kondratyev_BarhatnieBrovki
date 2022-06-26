using BarhatnieBrovki.DatabaseField;
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
using System.Windows.Shapes;

namespace BarhatnieBrovki.Windows
{
    /// <summary>
    /// Логика взаимодействия для Autorization.xaml
    /// </summary>
    /// 
    public partial class Autorization : Window
    {
        public Autorization()
        {
            InitializeComponent();

            //удаляем все фотки, которые не используются в услугах
            BarhatniyeBrovkiEntities db = new BarhatniyeBrovkiEntities();

            //из папки
            List<string> OnDebug = (from a in Directory.GetFiles(Environment.CurrentDirectory + "/Услуги салона красоты") select System.IO.Path.GetFileName(a)).ToList();

            //из базы
            List<Services> OnBase = new List<Services>();

            //из базы 
            List<ServicePhotoes> OnBasePhotos = new List<ServicePhotoes>();
            OnBase = db.Services.ToList();
            OnBasePhotos = db.ServicePhotoes.ToList();

            foreach (var item in OnDebug)
            {
                bool b = false;
                foreach (var item2 in OnBase)
                {
                    if (item2.mainImagePath != null && item2.mainImagePath.Length > 24)
                    {
                        string loc = item2.mainImagePath.Substring(22, item2.mainImagePath.Length - 22).ToString();
                        int idd = item2.ID;
                        if(item2.mainImagePath.Contains("\r\n"))
                        {
                            loc = loc.Substring(0, loc.Length - 2);
                        }
                        if (item.ToString() == loc)
                        {
                            b = true;
                            break;
                        } 
                    }
                }

                if (b == false)//если фотка не нашлась в базе  твблице service, то проверяем её в таблице servicephotos
                {
                    bool flag = false;
                    foreach (var items in OnDebug)
                    {
                        foreach (var item2 in OnBasePhotos)
                        {
                           
                                string loc = item2.photoPath.Substring(22, item2.photoPath.Length - 22).ToString();
                                int idd = item2.ID;
                                if (item.Contains("\r\n"))
                                {
                                    loc = loc.Substring(0, loc.Length - 2);
                                }                          
                                if (item.ToString() == loc)
                                {
                                    flag = true;
                                    break;
                                }
                            
                        }
                    }

                        if(flag == false)
                        if (item != null && item != "" && item != " ") //если строчка в базе не пустая
                        {
                            File.Delete(Environment.CurrentDirectory + "\\Услуги салона красоты\\" + item); //удаляем фотку
                        }

                }
            }
        }
       

        private void StepToUser(object sender, RoutedEventArgs e)
        {
           
            MainWindow mainWindow = new MainWindow(0);
            mainWindow.Show();
            Close();
        }

        private void StepToAdmin(object sender, RoutedEventArgs e)
        {
            AdminVhod adminVhod = new AdminVhod();
            adminVhod.Show();
            Close();
        }
    }

  
}
