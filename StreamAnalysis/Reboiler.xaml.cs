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
    /// Reboiler.xaml 的交互逻辑
    /// </summary>
    public partial class Reboiler : Window
    {
        public string vsdFile;
        public string dbFile;
        private int op = 0;
        private DataTable dt = new DataTable();
        private DataRow dr;
        private int loadstatus = 0;
        public Reboiler()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded_1(object sender, RoutedEventArgs e)
        {
            DBRelief dbR = new DBRelief(dbFile);
            string Name = txtName.Text;
            dt = dbR.getDataByVsdFile("frmreboiler", vsdFile, "heatername='" + Name + "'");
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    op = 1;
                    dr = dt.Rows[0];

                    txtName.Text = Name;
                    txtDescription.Text = dr["description"].ToString();
                    txtDuty.Text = dr["heaterduty"].ToString();
                    txtSteam.Text = dr["Steam"].ToString();
                    txtSteamFactor.Text = dr["SteamFactor"].ToString();
                    txtHotOil.Text = dr["HotOil"].ToString();
                    txtHotOilFactor.Text = dr["HotOilFactor"].ToString();
                    txtHotStream.Text = dr["HotStream"].ToString();
                    txtHotStreamFactor.Text = dr["HotStreamfactor"].ToString();
                    txtFurnace.Text = dr["Furnace"].ToString();
                    txtFurnaceFactor.Text = dr["FurnaceFactor"].ToString();
                    chkIsContinued.IsChecked = bool.Parse(dr["iscontinued"].ToString());
                    setColor(dr, "heatername_color", txtName);
                    setColor(dr, "heaterduty_color", txtDuty);
                    setColor(dr, "Steam_color", txtSteam);
                    setColor(dr, "SteamFactor_color", txtSteamFactor);
                    setColor(dr, "HotOil_color", txtHotOil);
                    setColor(dr, "HotOilFactor_color", txtHotOilFactor);
                    setColor(dr, "HotStream_color", txtHotStream);
                    setColor(dr, "HotStreamfactor_color", txtHotStreamFactor);
                    setColor(dr, "Furnace_color", txtFurnace);
                    setColor(dr, "FurnaceFactor_color", txtFurnaceFactor);
                    setColor(dr, "iscontinued_color", chkIsContinued);
                    loadstatus = 1;
                }
                else
                {
                    dr = dt.NewRow();
                    
                }
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            decimal water = decimal.Parse(txtSteam.Text.Trim());
            decimal air = decimal.Parse(txtHotOil.Text.Trim());
            decimal wetair = decimal.Parse(txtHotStream.Text.Trim());
            decimal pumpabound = decimal.Parse(txtFurnace.Text.Trim());
            decimal duty = decimal.Parse(txtDuty.Text.Trim());
            if (duty != (water + air + wetair + pumpabound))
            {
                //txtSteamFactor.Focus();
                txtSteamFactor.BorderBrush = Brushes.Red;
                txtSteamFactor.BorderThickness = new Thickness(2, 2, 2, 2);
                return;
            }


            dr["heatername"] = txtName.Text.Trim();
            dr["description"] = txtDescription.Text.Trim();
            dr["heaterduty"] = txtDuty.Text.Trim();
            dr["Steam"]=txtSteam.Text.Trim();
            dr["SteamFactor"]=txtSteamFactor.Text.Trim();
            dr["HotOil"]=txtHotOil.Text.Trim();
            dr["HotOilFactor"]=txtHotOilFactor.Text.Trim();
            dr["HotStream"]=txtHotStream.Text.Trim();
            dr["HotStreamfactor"]=txtHotStreamFactor.Text.Trim();
            dr["Furnace"]=txtFurnace.Text.Trim();
            dr["FurnaceFactor"]=txtFurnaceFactor.Text.Trim();
            dr["iscontinued"]=chkIsContinued.IsChecked;
            dr["visiofile"] = vsdFile;
            
            getColor(ref dr, "heatername_color", txtName);
            getColor(ref dr, "heaterduty_color", txtDuty);
            getColor(ref dr, "Steam_color", txtSteam);
            getColor(ref dr, "SteamFactor_color", txtSteamFactor);
            getColor(ref dr, "HotOil_color", txtHotOil);
            getColor(ref dr, "HotOilFactor_color", txtHotOilFactor);
            getColor(ref dr, "HotStream_color", txtHotStream);
            getColor(ref dr, "HotStreamfactor_color", txtHotStreamFactor);
            getColor(ref dr, "Furnace_color", txtFurnace);
            getColor(ref dr, "FurnaceFactor_color", txtFurnaceFactor);
            getColor(ref dr, "iscontinued_color", chkIsContinued);
            
            DBRelief dbR = new DBRelief(dbFile);
            dbR.saveDataByRow(dr, op);
            this.DialogResult = true;
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
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

        private void setColor( DataRow dr, string dcName, TextBox txtBox)
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

        private void setColor( DataRow dr, string dcName, CheckBox chkBox)
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
      
        private void txtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (loadstatus == 1)
            {
                TextBox txtBox = (TextBox)sender;
                if (txtBox.Text.Trim() == string.Empty)
                {
                    txtBox.BorderBrush = Brushes.Red;                  
                }
                else
                {
                    txtBox.BorderBrush = Brushes.Blue;                    
                }
                txtBox.BorderThickness = new Thickness(2, 2, 2, 2);

                decimal totalDuty = decimal.Parse(txtDuty.Text);
                if (txtBox.Name == "txtSteamFactor")
                {
                    decimal f = 0;
                    if (decimal.TryParse(txtSteamFactor.Text, out f) == false)
                        f = 0;
                    txtSteam.Text = (totalDuty * f).ToString();
                }
                else if (txtBox.Name == "txtHotOilFactor")
                {
                    decimal f = 0;
                    if (decimal.TryParse(txtHotOilFactor.Text, out f) == false)
                        f = 0;
                    txtHotOil.Text = (totalDuty * f).ToString();
                }
                if (txtBox.Name == "txtHotStreamFactor")
                {
                    decimal f = 0;
                    if (decimal.TryParse(txtHotStreamFactor.Text, out f) == false)
                        f = 0;
                    txtHotStream.Text = (totalDuty * f).ToString();
                }
                if (txtBox.Name == "txtFurnaceFactor")
                {
                    decimal f = 0;
                    if (decimal.TryParse(txtFurnaceFactor.Text, out f) == false)
                        f = 0;
                    txtFurnace.Text = (totalDuty * f).ToString();
                }

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
    }
}
