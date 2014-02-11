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
using System.Data.OleDb;
using System.IO;
using SharpCompress;
using SharpCompress.Reader;
using SharpCompress.Common;
using ReliefAnalysis;
using P2Wrap91;

namespace ReliefAnalysis
{
    /// <summary>
    /// FlashCalculation.xaml 的交互逻辑
    /// </summary>
    public partial class FlashCalculation : Window
    {
        public FlashCalculation()
        {
            InitializeComponent();
        }
        public string dbFile;
        private DataSet dsStreamInfo;
        private string przFile = string.Empty;
        int iFirst = 1;
        int iSecond = 3;
        string firstValue = "";
        string secondValue = "";
        public P2Wrap91.CP2ServerClass cp2Srv;

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            rbPressure.IsChecked = true;
            rbPressure2.IsEnabled = false;
            rbBubble.IsChecked = true;
            dsStreamInfo = new DataSet();
            
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            OptionStream frm = new OptionStream();
            frm.dbFile = dbFile;
            frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frm.Owner = this;
            if (frm.ShowDialog() == true)
            {
                 dsStreamInfo = (DataSet)Application.Current.Properties["streaminfo"];
                Application.Current.Properties.Remove("streaminfo");
                Guid guid = Guid.NewGuid();
                DataTable dt = this.dsStreamInfo.Tables[0];
                przFile = dt.Rows[0]["sourcefile"].ToString();
                this.txtName.Text = dt.Rows[0]["streamname"].ToString() + "_" + guid.ToString().Substring(0, 5).ToLower();
                string temperature = dt.Rows[0]["Temperature"].ToString();
                this.txtTemp.Text = UnitConverter.unitConv(temperature, "K", "C", "{0:0.0000}");

                string pressure = dt.Rows[0]["Pressure"].ToString();
                if (pressure != "")
                {
                    this.txtPressure.Text = UnitConverter.unitConv(pressure, "KPA", "MPAG", "{0:0.0000}");
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            if (rbPressure.IsChecked == true)
            {
                iFirst = 1;
                firstValue = txtPressure.Text.Trim();
            }
            else
            {
                iFirst = 2;
                firstValue = txtTemp.Text.Trim();
            }

            DataTable dt = this.dsStreamInfo.Tables[0];
            if (rbPressure2.IsChecked == true)
            {
                iSecond = 1;
                secondValue = txtPressure2.Text.Trim();
            }
            if (this.rbTemp2.IsChecked == true)
            {
                iSecond = 2;
                secondValue = txtTemp.Text.Trim();
            }
            if (this.rbDew.IsChecked == true)
            {
                iSecond = 3;
                secondValue = "";
            }
            if (this.rbBubble.IsChecked == true)
            {
                iSecond = 4;
                secondValue = "";
            }
            if (this.rbDuty.IsChecked == true)
            {
                iSecond = 5;
                secondValue = txtDuty.Text.Trim();
            }
            FlashCompute fc = new FlashCompute();
            //DataTable dt2=fc.compute(przFile,txtName.Text.Trim().ToUpper(),iFirst,firstValue,iSecond,secondValue,dt);
           
            
            //newPrzFile = fc.compute(cp2Srv,przFile, txtName.Text.Trim().ToUpper(), iFirst, firstValue, iSecond, secondValue, dt.Rows[0], ref l, ref v);
            
            
            
            
        }
        string newPrzFile = string.Empty;
        string l = string.Empty;
        string v = string.Empty;
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            PRZReader przReader = new PRZReader(newPrzFile);
            DataSet ds= przReader.getDataFromFile();
            DBRelief dbreader = new DBRelief();

            string[] feedH = dbreader.computeH(ds.Tables[0], txtName.Text.Trim().ToUpper());
            string[] vaporH = dbreader.computeH(ds.Tables[0], v);
            string[] liqidH = dbreader.computeH(ds.Tables[0], l);
            
           
            lblFeedH.Content = "FEED= " + feedH[0]+" KJ/hr";
            lblVaporH.Content = "VAPOR= " + vaporH[0] + " KJ/hr";
            lblLiqidH.Content = "LIQID= " + liqidH[0] + " KJ/hr";
            lblTest.Content = (double.Parse(vaporH[1]) - double.Parse(liqidH[1])).ToString() + " KJ/Kg";
        }

        private void rbFirst_Click(object sender, RoutedEventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Content.ToString() == "Pressure")
            {
                rbPressure2.IsEnabled = false;
                rbTemp2.IsEnabled = true;               
            }
            if (rb.Content.ToString() == "Temp")
            {
                rbTemp2.IsEnabled = false;
                rbPressure2.IsEnabled = true;              
            }

        }

        
    }
}
