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
    /// frmMessage.xaml 的交互逻辑
    /// </summary>
    public partial class frmMessage : Window
    {
        public int status = -1;
        public frmMessage()
        {
            InitializeComponent();
        }

        private void yes_Click(object sender, RoutedEventArgs e)
        {
            status = 1;
            DialogResult = true;
        }

        private void no_Click(object sender, RoutedEventArgs e)
        {
            status = 0;
            DialogResult = true;
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            status = -1;
            DialogResult = true;
        }
    }
}
