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
using ReliefAnalysis;
using System.IO;

namespace ReliefAnalysis
{
    /// <summary>
    /// PSV.xaml 的交互逻辑
    /// </summary>
    public partial class PSV : Window
    {
        public string vsdFile;
        public string dbFile;
        public DataTable dtpsv=new DataTable();
        public PSV()
        {
            InitializeComponent();
        }
        
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text.ToString().Trim() == string.Empty)
            {
                CustomMessageBox msg = new CustomMessageBox();
                msg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                msg.tipcontent.Text="PSV Name could not be empty";
                msg.Show();               
                return;
            }
            string pressure=txtPress.Text.ToString().Trim();
            if ( pressure== string.Empty)
            {
                CustomMessageBox msg = new CustomMessageBox();
                msg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                msg.tipcontent.Text = "Pressure Name could not be empty";
                msg.Show();         
                return;
            }
            else if (isNumber(pressure) == false)
            {
                CustomMessageBox msg = new CustomMessageBox();
                msg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                msg.tipcontent.Text = "Pressure  could not be empty";
                msg.Show();         
                return;
            }

            string relief = txtPrelief.Text.ToString().Trim();
            if (relief == string.Empty)
            {
                CustomMessageBox msg = new CustomMessageBox();
                msg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                msg.tipcontent.Text = "Prelief's Value could not be empty";
                msg.Show();
                return;
            }
            else if (isNumber(pressure) == false)
            {
                CustomMessageBox msg = new CustomMessageBox();
                msg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                msg.tipcontent.Text = "Prelief's Value must be a number";
                msg.Show();
                return;
            }
            

            DBRelief dbR = new DBRelief(dbFile);
            dtpsv = dtpsv.Clone();
            DataRow dr = dtpsv.NewRow();
            if (dtpsv.Rows.Count == 0)
            {
                dtpsv.Rows.Add(dr);
            }
            else
            {
                dr = dtpsv.Rows[0];
            }
            dr["visiofile"] = vsdFile;
            dr["psvname"] = txtName.Text;
            dr["description"] = txtDescription.Text;
            dr["valvetype"] = cbxValueType.SelectedItem.ToString();
            dr["valvenumber"] = txtValveNumber.Text;
            dr["pressure"] = txtPress.Text;
            
            dr["reliefmultiple"] = txtPrelief.Text;
            dr["location"] =cbxLocation.SelectedItem.ToString() ;
            dr["locationdescription"] = txtLocationDescription.Text;
            dbR.saveDataByTable(dtpsv,vsdFile);


            this.DialogResult = true;
            
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {            
            this.DialogResult = false;
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            DBRelief dbR = new DBRelief(dbFile);
            dtpsv = dbR.getDataByVsdFile("frmpsv", vsdFile);
            if (dtpsv != null)
            {
                if (dtpsv.Rows.Count > 0)
                {
                    DataRow dr = dtpsv.Rows[0];
                    vsdFile = dr["visiofile"].ToString();
                    txtName.Text = dr["psvname"].ToString();
                    txtDescription.Text = dr["description"].ToString();
                    cbxValueType.SelectedItem = dr["valvetype"].ToString();
                    txtValveNumber.Text = dr["valvenumber"].ToString();
                    txtPress.Text = dr["pressure"].ToString();
                    txtPrelief.Text = dr["reliefmultiple"].ToString();
                    cbxLocation.SelectedItem = dr["location"].ToString();
                    txtLocationDescription.Text = dr["locationdescription"].ToString();
                }
            }
        }


        public bool isNumber(string strNumber)
        {
            try
            {
                decimal d = decimal.Parse(strNumber);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
