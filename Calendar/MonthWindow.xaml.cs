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
using System.Net;
using Newtonsoft.Json;

namespace Calendar
{
    public partial class MonthWindow : Window
    {
        ///APIKey
        /// 
        ///to get acces to json weather file
        string APIKey = "75ad8a1de4bb3ccfd018b8830657b902";
        /// Gets weather from website openweathermap.org using APIKey.
        /// 
        /// Uses txtCity label content to get access to wanted weather properties.
        /// Update labDetails, labWind, labPressure, labHumidity, labTemperature labels contents
        void getWeather()
        {
            using (WebClient web = new WebClient())
            {
                try
                {
                    string url = String.Format("https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}", txtCity.Text, APIKey);
                    var json = web.DownloadString(url);
                    Weatherinfo.root Info = JsonConvert.DeserializeObject<Weatherinfo.root>(json);

                    labDetails.Content = Info.weather[0].description;
                    labWind.Content = Info.wind.speed.ToString("F1") + " m/s";
                    labPressure.Content = Info.main.pressure.ToString() + " hPa";
                    labHumidity.Content = Info.main.humidity.ToString() + "%";
                    double TempF = Info.main.temp;
                    double TempC = TempF - 273.15;
                    labTemperature.Content = TempC.ToString("F1") + " °C";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// global variable to use it for database of todo lists as a date
        /// 
        /// public static int st_day is in UserControlDays class
        public static int st_year;
        /// global variable to use it for database of todo lists as a date
        /// 
        /// public static int st_day is in UserControlDays class
        public static int st_month;
        /// global variable to use it as current displayed month
        int month;
        /// global variable of displayed year
        int year;
        /// array for months names depending on their number
        string[] months = { "January","February", "March", " April", "May","June","July","August", "September", " October", "November", "December" };
        ///
        public MonthWindow()
        {
            InitializeComponent();
            
        }
        /// Loading application window with calendar and weather
        private void MonthWindow_Loaded(object sender, RoutedEventArgs e)
        {
            displaDays();
            getWeather();
        }
        /// Displays month in application window. Create table of days depends on day of week
        /// 
        /// @param neededmonth month that will be used
        /// @param neededyear year that will be used
        void monthload(int neededmonth, int neededyear)
        {
            DateTime startofthemonth = new DateTime(neededyear, neededmonth, 1);
            //get the count of days of the month
            int days = DateTime.DaysInMonth(neededyear, neededmonth);

            int dayoftheweek = Convert.ToInt32(startofthemonth.DayOfWeek.ToString("d"));

            //create a blank usercontrol for days out of this month but in same week as fist day of month
            for (int i = 0; i < dayoftheweek; i++)
            {
                UserControlBlank ucblank = new UserControlBlank();
                daycontainer.Children.Add(ucblank);
            }

            //create usercontrol for days
            for (int i = 1; i < days + 1; i++)
            {
                UserControlDays ucdays = new UserControlDays();
                ucdays.days(i);
                daycontainer.Children.Add(ucdays);
            }
            String monthname = months[month - 1] + " ";
            MonthYear.Content = monthname + year.ToString();

            st_year = neededyear;
            st_month = neededmonth;
        }
        /// Displays current month in calendar window.
        /// 
        /// Uses monthload() function with current year and month parameters
        private void displaDays()
        {
            daycontainer.Children.Clear();
            DateTime now = DateTime.Now;
            month = now.Month;
            year = now.Year;

            monthload(month, year);

        }
        /// Displays previous month
        /// 
        /// Using global variables int month, year, displays month precending to what was displayed before
        private void PrevBut_Click(object sender, RoutedEventArgs e)
        {
            daycontainer.Children.Clear();

            if (month > 1)
                month--;
            else
            {
                month = 12;
                year--;
            }
            monthload(month, year);
        }
        /// Service button reload weather properties for another city
        /// 
        /// Just uses getWeather function
        private void ChangeCity_Click(object sender, RoutedEventArgs e)
        {
            getWeather();
        }
        /// Displays next month
        /// 
        /// Using global variables int month, year, displays month next to what was displayed before
        private void NextBut_Click(object sender, RoutedEventArgs e)
        {
            daycontainer.Children.Clear();

            if(month < 12)
                month++;
            else
            {
                month = 1;
                year++;
            }
            monthload(month, year);
        }


    }
}
