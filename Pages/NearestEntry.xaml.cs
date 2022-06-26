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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BarhatnieBrovki.Pages
{
    /// <summary>
    /// Логика взаимодействия для NearestEntry.xaml
    /// </summary>
    public partial class NearestEntry : Page
    {
        BarhatniyeBrovkiEntities db;      
        public NearestEntry()
        {
            InitializeComponent();
            timer_tick(null, null);
            var timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_tick);
            timer.Interval = new TimeSpan(0, 0, 30);
            timer.Start();
          
        }

        public void timer_tick(object sender, EventArgs e)
        {
            db = new BarhatniyeBrovkiEntities();
            DateTime onedaylater = DateTime.Now.AddDays(1);
            var spisok = db.ClientServices.Where(x => (x.StartTime.Year == DateTime.Now.Year &&
            x.StartTime.Month == DateTime.Now.Month &&
            x.StartTime.Day == DateTime.Now.Day)
            ||
            (x.StartTime.Year == onedaylater.Year &&
            x.StartTime.Month == onedaylater.Month &&
            x.StartTime.Day == onedaylater.Day)).ToList();

            var spisokforBinding = new List<ClientServices>();
            foreach (var item in spisok)
            {
                spisokforBinding.Add(item);                
            }
            foreach (var item in spisok)
            {
                if (item.StartTime.Day == DateTime.Now.Day)
                    if (
                       item.StartTime.TimeOfDay > DateTime.Now.TimeOfDay
                      )
                    {
                    }
                    else
                    {
                        spisokforBinding.Remove(item);
                    }
            }

            //spisok = spisok.Where(x => (x.StartTime.Hour > DateTime.Now.Hour &&
            //x.StartTime.Minute >= DateTime.Now.Minute &&
            //x.StartTime.Second >= DateTime.Now.Second)).ToList();

            listboxZapic.ItemsSource = spisokforBinding.OrderBy(o => o.StartTime).ToList();
        }
    }
}
