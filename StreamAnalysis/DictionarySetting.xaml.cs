using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Data;

namespace ReliefAnalysis
{
    /// <summary>
    /// DictionarySetting.xaml 的交互逻辑
    /// </summary>
    public partial class DictionarySetting : Window
    {
        DBRelief db = new DBRelief();
        public DictionarySetting()
        {
            InitializeComponent();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            
             DataTable dtSource = db.getDataByTable("dictionarysource", "");
             gridSource.ItemsSource = dtSource.DefaultView;
             
             DataTable dtSink = db.getDataByTable("dictionarysink", "");
             gridSink.ItemsSource = dtSink.DefaultView;
             

             DataTable dtCondenser = db.getDataByTable("dictionarycondenser", "");
             gridCondenser.ItemsSource = dtCondenser.DefaultView;
             

             DataTable dtReboiler = db.getDataByTable("dictionaryreboiler", "");
             gridReboiler.ItemsSource = dtReboiler.DefaultView;
            
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
           

            this.DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            
        }

       

        private void grid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString().ToLower() == "id")
            {
                // e.Cancel = true;   // For not to include 
                // e.Column.IsReadOnly = true; // Makes the column as read only
                e.Column.Visibility = Visibility.Hidden;
            }
            else if (e.Column.Header.ToString().ToLower() == "category" || e.Column.Header.ToString().ToLower() == "categoryvalue")
            {
                // e.Cancel = true;   // For not to include 
                // e.Column.IsReadOnly = true; // Makes the column as read only
                e.Column.IsReadOnly = true;
            }
           
        }
    }
}
