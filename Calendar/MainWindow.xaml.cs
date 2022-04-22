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
using System.Dynamic;

namespace Calendar
{
    /// ToDo List window
    public partial class MainWindow : Window
    {
        /// variable that used for remembering date from calendar window
        static DateTime chosendate;
        /// variable for connecting with database
        ToDoEntities database = new ToDoEntities();
        /// variable that passes to database member (has id, time, date, properties) to allow updating database in code
        Task modelT = new Task();
        /// ToDoList window
        public MainWindow()
        {
            InitializeComponent();
        }
        /// Clear txtTime label and txtProperties label fields
        ///
        /// Uses Clear() function
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }
        /// Clear modelT variable content, txtTime label and txtProperties label fields
        /// 
        /// Also changes Save button content from "Update" to "Save".
        /// Desables Delete button.
        /// Actually does not clear modelT, just change its id to 0 to mark it as not existing in datetable member
        void Clear()
        {
            txtTime.Text = txtProperties.Text = "";
            btnDelete.IsEnabled = false;
            modelT.id = 0;
            btnSave.Content = "Save";
        }
        /// Updates datagrid table which consists of datetable members from chosen day 
        void Update()
        {
            int chosenday = Int32.Parse(UserControlDays.st_day.Substring(30));
            int chosenmonth = MonthWindow.st_month;
            int chosenyear = MonthWindow.st_year;
            
            chosendate = new DateTime(chosenyear, chosenmonth, chosenday, 0, 0, 0).Date;

            modelT.date = chosendate;

            var query =
                from Task in database.Tasks
                where Task.date == chosendate
                select new { Task.id, Task.date, Task.time, Task.prop };

            dataGrid1.ItemsSource = query.ToList();

        }
        /// Loading ToDoList window
        /// 
        /// Just uses Clear() and Update() functions
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {            
            Clear();
            Update();   
        }
        /// Saves data from txtTime and txtProperties labels as new datetable member or updates old, if it already has id
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (modelT.id == 0)
            {
                modelT.date = chosendate;
            }

            if (txtTime.Text == "")             //TIME
            {
                modelT.time = DateTime.Now;
            }
            else
            {
                DateTime ts = DateTime.Parse(txtTime.Text);
                modelT.time = ts;
            }
             
            modelT.prop = txtProperties.Text;   //PROPERTIES
            
            
            using (ToDoEntities db = new ToDoEntities())
            {
                if (modelT.id == 0)
                {
                    db.Tasks.Add(modelT);
                }
                else
                    db.Entry(modelT).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            Clear();
            MessageBox.Show("Saved Succesfully");
            Update();
        }
        /// Servise double click on member in datagrid table to allow updating datetable member
        /// 
        /// Changes txtTime and txtProperties labels according to chosen member.
        /// Record member date (id, time, date, properties) to variable modelT.
        /// Changes Save button content from "Save" to "Update", enables Delete button
        private void dataGrid1_MouseDoubleClick(object sender, MouseButtonEventArgs e) 
        {
            string index = Convert.ToString(dataGrid1.CurrentCell.Item);
            /////////////
            int timeoccure = index.IndexOf("time = ") + 18;
            string topaste = index.Substring(timeoccure, 5);

            int propoccure = index.IndexOf("prop = ") + 7;
            string topaste1 = index.Substring(propoccure);

            int dateoccure = index.IndexOf("date = ") - 2;
            string strid = index.Remove(dateoccure);
            strid = strid.Remove(0, 7);
            int intid = Int32.Parse(strid);

            int lngth = topaste1.Length;
            lngth = lngth - 2;
            topaste1 = topaste1.Remove(lngth);

            dateoccure = index.IndexOf("time = ") - 2;
            string strdate = index.Remove(dateoccure);
            dateoccure = strdate.IndexOf("date = ") + 7;
            strdate = strdate.Substring(dateoccure);

            int int_day = int.Parse(strdate.Remove(2));

            string strpart = strdate.Remove(5);
            strpart = strpart.Substring(3);
            int int_month = int.Parse(strpart);

            strpart = strdate.Substring(6);
            int int_year = int.Parse(strpart.Remove(4));

            modelT.id = intid;
            DateTime dt1 = new DateTime(int_year, int_month, int_day);

            txtTime.Text = topaste;
            txtProperties.Text = topaste1;

            btnSave.Content = "Update";
            btnDelete.IsEnabled = true;
        }
        /// Removes datetable member from database and update ToDoList window
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            using (ToDoEntities db = new ToDoEntities())
            {
                var entry = db.Entry(modelT);
                if (entry.State == System.Data.Entity.EntityState.Detached)
                    db.Tasks.Attach(modelT);
                db.Tasks.Remove(modelT);
                db.SaveChanges();
                Update();
                Clear();
                MessageBox.Show("Delete Succesfully");
            }
        }
    }
}