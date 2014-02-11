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

namespace ReliefAnalysis
{
    /// <summary>
    /// Source.xaml 的交互逻辑
    /// </summary>
    public partial class Source : Window
    {
        public string vsdFile;
        public string dbFile;
        private int op = 0;
        private  DataTable dtsource = new DataTable();
        private DataRow dr;
        private int loadstatus = 0;
        public Source()
        {
            InitializeComponent();
        }

        DBRelief dbR = new DBRelief();
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtName.Text.Trim() == string.Empty)
                {
                    txtName.BorderBrush = Brushes.Red;
                    txtName.BorderThickness = new Thickness(2, 2, 2, 2);
                    txtName.Focus();
                    return;
                }

                if (txtMaxPressure.Text.Trim() == string.Empty)
                {
                    txtMaxPressure.BorderBrush = Brushes.Red;
                    txtMaxPressure.BorderThickness = new Thickness(2, 2, 2, 2);
                    txtMaxPressure.Focus();
                    return;
                }
                //string lastSourceName = dr["sourcename"].ToString();
                dr["sourcename"] = txtName.Text;
                dr["description"] = txtDescription.Text;
                dr["sourcetype"] = cbxType.Text;
                dr["maxpossiblepressure"] = txtMaxPressure.Text;
                dr["ismaintained"] = chkIsMaintained.IsChecked;
                dr["visiofile"] = vsdFile;

                getColor(ref dr, "sourcename_color", txtName);
                getColor(ref dr, "sourcetype_color", cbxType);
                getColor(ref dr, "maxpossiblepressure_color", txtMaxPressure);
                getColor(ref dr, "ismaintained_color", chkIsMaintained);
                dbR.saveDataByRow(dr, op);                
            }
            
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Action");
            }
            this.DialogResult = true;
        }

        private void MetroWindow_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {
                txtName.BorderBrush = Brushes.Red;
                txtName.BorderThickness = new Thickness(2, 2, 2, 2);
                txtMaxPressure.BorderBrush = Brushes.Red;
                txtMaxPressure.BorderThickness = new Thickness(2, 2, 2, 2);

                dbR = new DBRelief(dbFile);
                string sourceName = txtName.Text;
                dtsource = dbR.getDataByVsdFile("frmsource", vsdFile, "sourcename='" + sourceName + "'");
                if (dtsource != null)
                {
                    if (dtsource.Rows.Count > 0)
                    {
                        op = 1;
                        dr = dtsource.Rows[0];

                        txtName.Text = sourceName;
                        txtDescription.Text = dr["description"].ToString();
                        cbxType.Text = dr["sourcetype"].ToString();
                        txtMaxPressure.Text = dr["maxpossiblepressure"].ToString();
                        chkIsMaintained.IsChecked = bool.Parse(dr["ismaintained"].ToString());
                        setColor(dr, "sourcename_color", txtName);
                        setColor(dr, "sourcetype_color", cbxType);
                        setColor(dr, "maxpossiblepressure_color", txtMaxPressure);
                        setColor(dr, "ismaintained_color", chkIsMaintained);
                        loadstatus = 1;
                    }
                    else
                    {
                        dr = dtsource.NewRow();
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
            ComboBox box = (ComboBox)sender;
            if(box.Text.Contains("Motor"))
            {
                chkIsMaintained.IsChecked = false;
            }
        }


        //判断是否是电力启动
        private bool isMotor()
        {            
            if (dr["sourcetype"].ToString().Contains("Motor"))
                return true;
            else
                return false;

        }

        private void chkGeneralElectricalPowerFailure()
        {
            if (chkIsMaintained.IsChecked == true)
            {
                DataTable dtCase4 = dbR.getDataByVsdFile("frmcase_feed", vsdFile, "case_id=4 and streamname='" + dr["streamname"].ToString() + "'");
                if (dtCase4.Rows.Count > 0)
                {
                    DataRow r = dtCase4.Rows[0];
                    r["flowstop"] = false;
                    dbR.saveDataByRow(r, 1);
                }
            }
            else
            {
                DataTable dtCase4 = dbR.getDataByVsdFile("frmcase_feed", vsdFile, "case_id=4 and streamname='" + dr["streamname"].ToString() + "'");
                if (dtCase4.Rows.Count > 0)
                {
                    DataRow r = dtCase4.Rows[0];
                    if (isMotor())
                    {                       
                        r["flowstop"] = true;
                    }
                    else
                    {
                        r["flowstop"] = false;
                    }
                    dbR.saveDataByRow(r, 1);
                }
            }
        }
    }
}
