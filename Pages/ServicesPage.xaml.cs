using BarhatnieBrovki.DatabaseField;
using BarhatnieBrovki.Windows;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для ServicesPage.xaml
    /// </summary>
    public partial class ServicesPage : Page
    {
        BarhatniyeBrovkiEntities db;
        int UserOrAdmin = 0;
        int allZapic = 0; //сколько всего данных в таблице (сколько всего услуг в бд) 
        public ServicesPage(int ch)
        {
            InitializeComponent();            
            UserOrAdmin = ch;
            VisibleAdminPanel.Content = UserOrAdmin;
            db = new BarhatniyeBrovkiEntities();
       
            listboxServices.ItemsSource = db.Services.ToList();
            //DataContext = Connection.Database.Services.ToList();
            allZapic = listboxServices.Items.Count;
            CountVisibleData.Content = allZapic + "/" + allZapic;
        }


        int k = 0;
        private void SelectedService(object sender, SelectionChangedEventArgs e)
        {
            k++;
            if (k == 1)
            {
                ServiceVivod servv = new ServiceVivod((Services)listboxServices.SelectedItem, UserOrAdmin);
                servv.ShowDialog();
                listboxServices.SelectedItem = null;
            }
            else k = 0;
        }

        private void SearchingTextbox(object sender, KeyEventArgs e)
        {
            Sorting();
        }

        private void Skidka(object sender, SelectionChangedEventArgs e)
        {
            Sorting();
        }

        private void Sorting()
        {
            if (PriceCombobox == null || SkidkaCombobox == null || listboxServices == null) return; //при инициализации чтоб ошибку не давало
            db = new BarhatniyeBrovkiEntities();


            List<Services> local = new List<Services>();

            if (SearchTextBox.Text != null && SearchTextBox.Text != "" && SearchTextBox.Text != " ")
                local = db.Services.ToList().Where(x => x.Title.ToLower().Contains(SearchTextBox.Text.ToLower())).ToList(); //поиск по полю ввода 

            else local = db.Services.ToList(); //выводим все услуги, если в поиск ничего не введено
            allZapic = db.Services.ToList().Count;

            if (SkidkaCombobox.SelectedIndex == 0) { } // все
            else if (SkidkaCombobox.SelectedIndex == 1) // 0-5%
            {
                local = local.Where(x => x.Discount >= 0 && x.Discount < 5 || x.Discount == null).ToList();
            }
            else if (SkidkaCombobox.SelectedIndex == 2) //5-15%
            {
                local = local.Where(x => x.Discount >= 5 && x.Discount < 15).ToList();
            }
            else if (SkidkaCombobox.SelectedIndex == 3) //15-30%
            {
                local = local.Where(x => x.Discount >= 15 && x.Discount < 30).ToList();
            }
            else if (SkidkaCombobox.SelectedIndex == 4) //30-70%
            {
                local = local.Where(x => x.Discount >= 30 && x.Discount < 70).ToList();
            }
            else if (SkidkaCombobox.SelectedIndex == 5) //70-100%
            {
                local = local.Where(x => x.Discount >= 70 && x.Discount < 100).ToList();
            }



            if (PriceCombobox.SelectedIndex == 0) //все
            {
                var local2 = local;
                listboxServices.ItemsSource = local2;
              
                if (local2.Count() == 0) { VisibleNotRezult.Visibility = Visibility.Visible;  } //если результатов нет, то сообщаем об этом
                else { VisibleNotRezult.Visibility = Visibility.Collapsed; }
               
                CountVisibleData.Content = local2.Count() + "/" + allZapic;
            }

            else if (PriceCombobox.SelectedIndex == 1) //по возрастанию
            {
                var local2 = (from p in local orderby p.CostSet ascending select p);
                listboxServices.ItemsSource = local2;

                if (local2.Count() == 0) { VisibleNotRezult.Visibility = Visibility.Visible; } //если результатов нет, то сообщаем об этом
                else { VisibleNotRezult.Visibility = Visibility.Collapsed; }
              
                CountVisibleData.Content = local2.Count() + "/" + allZapic;
            }

            else if (PriceCombobox.SelectedIndex == 2) //по убыванию
            {
                var local2 =(from p in local orderby p.CostSet descending select p);
                listboxServices.ItemsSource = local2;

                if (local2.Count() == 0) { VisibleNotRezult.Visibility = Visibility.Visible; } //если результатов нет, то сообщаем об этом
                else { VisibleNotRezult.Visibility = Visibility.Collapsed; }

                CountVisibleData.Content = local2.Count() + "/" + allZapic;
            }           
        }

        private void EditServiceButton(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Services service = button.DataContext as Services;
            EditService ed = new EditService(service);
            ed.Closing += Updating;
            ed.ShowDialog();
        }

        private void Updating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            db = new BarhatniyeBrovkiEntities();
            listboxServices.ItemsSource = db.Services.ToList();
            Sorting();
        }


        private void DeleteService(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Services service = button.DataContext as Services;
            MessageBoxResult rez = MessageBox.Show("Вы точно хотите удалить данную услугу? Данное действие необратимо", "Уведомление", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                      
            if(rez == MessageBoxResult.Yes)
            {
                //если фотка больше нигде не используется, удаляем её при след запуске приложения
                db = new BarhatniyeBrovkiEntities();

                Services forRemove = db.Services.Where(x => x.ID == service.ID).First();
                if(forRemove.ClientServices.Count != 0)// если эта услуга пользовалась, то НЕ удаляем её
                {
                    MessageBox.Show("Данная услуга пользовалась, поэтому её удаление невозможно!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                db.Services.Remove(forRemove); //удаляем услугу
                db.SaveChanges();
                Updating(null,null);

                MessageBox.Show("Успешно!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                Sorting(); //обновляем список услуг
            }
        }
    }
}
