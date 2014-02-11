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
    /// Sink.xaml 的交互逻辑
    /// </summary>
    public partial class Sink : Window
    {
       
         public string vsdFile;
        public string dbFile;
        private int op = 0;
        private  DataTable dtsink = new DataTable();
        private DataRow dr;
        private int loadstatus = 0;
        public Sink()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            dr["sinkname"] = txtName.Text;
            dr["visiofile"] = vsdFile;
            dr["description"] = txtDescription.Text;
            dr["sinktype"] = cbxType.Text;
            dr["maxpossiblepressure"] = txtMaxPressure.Text;
            dr["ismaintained"] = chkIsMaintained.IsChecked;
            getColor(ref dr, "sinkname_color", txtName);
            getColor(ref dr, "sinktype_color", cbxType);
            getColor(ref dr, "maxpossiblepressure_color", txtMaxPressure);
            getColor(ref dr, "ismaintained_color", chkIsMaintained);
            DBRelief dbR = new DBRelief(dbFile);
            dbR.saveDataByRow(dr, op);
            this.DialogResult = true;
        }

        private void MetroWindow_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {
                DBRelief dbR = new DBRelief(dbFile);
                string sinkName = txtName.Text;
                dtsink = dbR.getDataByVsdFile("frmsink", vsdFile, "sinkname='" + sinkName + "'");
                if (dtsink != null)
                {
                    if (dtsink.Rows.Count > 0)
                    {
                        op = 1;
                        dr = dtsink.Rows[0];
                        txtName.Text = sinkName;
                        txtDescription.Text = dr["description"].ToString();
                        cbxType.Text = dr["sinktype"].ToString();
                        txtMaxPressure.Text = dr["maxpossiblepressure"].ToString();
                        chkIsMaintained.IsChecked = bool.Parse(dr["ismaintained"].ToString());

                        setColor(dr, "sinkname_color", txtName);
                        setColor(dr, "sinktype_color", cbxType);
                        setColor(dr, "maxpossiblepressure_color", txtMaxPressure);
                        setColor(dr, "ismaintained_color", chkIsMaintained);
                        loadstatus = 1;

                    }
                    else
                    {
                        dr = dtsink.NewRow();
                    }
                }
            }
            
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Action");
            }
        }

        private void getColor(ref DataRow dr, string dcName, TextBox txtBox)
        {
            if (txtBox.BorderBrush == Brushes.Green)
            {
                dr[dcName] = "green";
            }
            else
            {
                dr[dcName] = "blue";
            }
        }

        private void getColor(ref DataRow dr, string dcName, CheckBox chkBox)
        {
            if (chkBox.BorderBrush == Brushes.Green)
            {
                dr[dcName] = "green";
            }
            else
            {
                dr[dcName] = "blue";
            }
        }

        private void getColor(ref DataRow dr, string dcName, ComboBox cbx)
        {
            if (cbx.BorderBrush == Brushes.Green)
            {
                dr[dcName] = "green";
            }
            else
            {
                dr[dcName] = "blue";
            }
        }




        private void setColor(DataRow dr, string dcName, TextBox txtBox)
        {
            string color = dr[dcName].ToString();
            if (color == "blue")
            {
                txtBox.BorderBrush = Brushes.Blue;
                txtBox.BorderThickness = new Thickness(2, 2, 2, 2);
            }
            else
            {
                txtBox.BorderBrush = Brushes.Green;
                txtBox.BorderThickness = new Thickness(2, 2, 2, 2);
            }
        }

        private void setColor(DataRow dr, string dcName, CheckBox chkBox)
        {
            string color = dr[dcName].ToString();
            if (color == "blue")
            {
                chkBox.BorderBrush = Brushes.Blue;
                chkBox.BorderThickness = new Thickness(2, 2, 2, 2);
            }
            else
            {
                chkBox.BorderBrush = Brushes.Green;
                chkBox.BorderThickness = new Thickness(2, 2, 2, 2);
            }
        }

        private void setColor(DataRow dr, string dcName, ComboBox cbx)
        {
            string color = dr[dcName].ToString();
            if (color == "blue")
            {
                cbx.BorderBrush = Brushes.Blue;
                cbx.BorderThickness = new Thickness(2, 2, 2, 2);
            }
            else
            {
                cbx.BorderBrush = Brushes.Green;
                cbx.BorderThickness = new Thickness(2, 2, 2, 2);
            }
        }


        private void txtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (loadstatus == 1)
            {
                TextBox txtBox = (TextBox)sender;
                txtBox.BorderBrush = Brushes.Blue;
                txtBox.BorderThickness = new Thickness(2, 2, 2, 2);
            }
        }
        private void chkIsContinued_Click(object sender, RoutedEventArgs e)
        {
            if (loadstatus == 1)
            {
                CheckBox txtBox = (CheckBox)sender;
                txtBox.BorderBrush = Brushes.Blue;
                txtBox.BorderThickness = new Thickness(2, 2, 2, 2);
            }
        }

        private void cbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (loadstatus == 1)
            {
                ComboBox txtBox = (ComboBox)sender;
                txtBox.BorderBrush = Brushes.Blue;
                txtBox.BorderThickness = new Thickness(2, 2, 2, 2);
            }
        }
    
    
    
    }
}

