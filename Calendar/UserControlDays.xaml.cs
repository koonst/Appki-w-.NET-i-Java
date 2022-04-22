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


namespace Calendar
{
    /// <summary>
    /// Logika interakcji dla klasy UserControlDays.xaml
    /// </summary>
    public partial class UserControlDays : UserControl
    {
        /// global variable to use it for database of todo lists as a date
        public static string st_day;
        ///
        public UserControlDays()
        {
            InitializeComponent();
        }

        private void UserControlDays_load(object sender, RoutedEventArgs e)
        {

        }
        /// Display day box according to a day number
        /// 
        /// @param numday
        public void days(int numday)
        {
            lbdays.Content = numday.ToString();
        }
        /// Servise mouse double click on day box to open ToDoList window
        private void UserControlDays_doubleClick(object sender, MouseButtonEventArgs e)
        {
            st_day = lbdays.ToString();
            MainWindow calendar = new MainWindow();
            calendar.Show();
        }
    }
}
