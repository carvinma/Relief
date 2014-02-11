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
    /// Accumulator.xaml 的交互逻辑
    /// </summary>
    public partial class Accumulator : Window
    {
        public string vsdFile;
        public string dbFile;
        private int op = 0;
        private DataTable dt = new DataTable();
        private DataRow dr;
        public string AccumulationName;
        public Accumulator()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            dr["accumulatorname"] = txtName.Text.Trim();
            if (voH.IsChecked==true)
            {
                dr["orientation"] = voH.Content.ToString();
            }
            else
            {
                dr["orientation"] = voV.Content.ToString();
            }

            dr["diameter"] = this.txtDiameter.Text.Trim();
            dr["length"]=this.txtLength.Text.Trim();
            dr["numberofheads"] = this.txtNumberofHeads.Text.Trim();
            dr["headratio"]=this.txtHeadRatio.Text.Trim();
           
            dr["visiofile"] = vsdFile;
            DBRelief dbR = new DBRelief(dbFile);
            dbR.saveDataByRow(dr, op);
            AccumulationName = txtName.Text.Trim();
            this.DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void MetroWindow_Loaded_1(object sender, RoutedEventArgs e)
        {
            DBRelief dbR = new DBRelief(dbFile);
            string Name = txtName.Text;
            dt = dbR.getDataByVsdFile("frmaccumulator", vsdFile, "accumulatorname='" + Name + "'");
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    op = 1;
                    dr = dt.Rows[0];

                    txtName.Text = Name;
                    string orientation=voH.Content.ToString();
                    string orientation_db=dr["orientation"].ToString();
                    if(string.Compare(orientation_db,orientation)==0)
                    {
                        voH.IsChecked=true;
                    }
                    else{
                        voV.IsChecked=true;
                    }
                    
                    this.txtDiameter.Text = dr["diameter"].ToString();
                    this.txtLength.Text = dr["length"].ToString();
                    this.txtNumberofHeads.Text = dr["numberofheads"].ToString();
                    this.txtHeadRatio.Text = dr["headratio"].ToString();
                    
                }
                else
                {
                    dr = dt.NewRow();
                }
            }
        }
    }
}
