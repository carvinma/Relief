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

namespace ReliefAnalysis
{
    /// <summary>
    /// TowerCondenser.xaml 的交互逻辑
    /// </summary>
    public partial class TowerCondenser : Window
    {
        public TowerCondenser()
        {
            InitializeComponent();
        }
        public int categoryTag=1;  //1 Condenser 2 HxCondenser 3 Reboiler 4 HxReboiler
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (categoryTag == 1 || categoryTag == 2)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = "Air";
                cbxType.Items.Add(item);

                item = new ComboBoxItem();
                item.Content = "Water";
                cbxType.Items.Add(item);
                lbDriven.Visibility = Visibility.Hidden;
                cbxDriven.Visibility = Visibility.Hidden;

            }
            if (categoryTag == 3 || categoryTag==4)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = "Steam";
                cbxType.Items.Add(item);

                item = new ComboBoxItem();
                item.Content = "Process Oil";
                cbxType.Items.Add(item);
          

                item = new ComboBoxItem();
                item.Content = "Pump-Electrical";
                cbxDriven.Items.Add(item);

            }
            cbxType.SelectedIndex = 0;
            cbxDriven.SelectedIndex = 0;
        }
    }
}
