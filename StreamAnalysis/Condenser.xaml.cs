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
    /// Condenser.xaml 的交互逻辑
    /// </summary>
    public partial class Condenser : Window
    {
        public string vsdFile;
        public string dbFile;
        private int op = 0;
        private DataTable dtCondenser = new DataTable();
        private DataRow dr;
        private int loadstatus = 0;
        DBRelief dbR = new DBRelief();
        public Condenser()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Save Conendser data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            decimal water = decimal.Parse(txtWater.Text.Trim());
            decimal air = decimal.Parse(txtAir.Text.Trim());
            decimal wetair = decimal.Parse(txtWetAir.Text.Trim());
            decimal pumpabound = decimal.Parse(txtPumpAbound.Text.Trim());
            decimal duty = decimal.Parse(txtDuty.Text.Trim());
            if(duty!=(water+air+wetair+pumpabound))
            {
                //txtWaterFactor.Focus();
                txtWaterFactor.BorderBrush = Brushes.Red;
                txtWaterFactor.BorderThickness = new Thickness(2, 2, 2, 2);
                return;
            }

            dr["heatername"] = txtName.Text.Trim();
            dr["description"] = txtDescription.Text.Trim();
            dr["heaterduty"] = txtDuty.Text.Trim();
            dr["water"] = txtWater.Text.Trim();
            dr["waterfactor"] = txtWaterFactor.Text.Trim();
            dr["air"] = txtAir.Text.Trim();
            dr["airfactor"] = txtAirFactor.Text.Trim();
            dr["wetair"] = txtWetAir.Text.Trim();
            dr["wetairfactor"] = txtWetAirFactor.Text.Trim();
            dr["pumpabound"] = txtPumpAbound.Text.Trim();
            dr["pumpaboundfactor"] = txtPumpAboundFactor.Text.Trim();
            dr["visiofile"] = vsdFile;

            getColor(ref dr, "heatername_color", txtName);
            getColor(ref dr, "heaterduty_color", txtDuty);
            getColor(ref dr, "water_color", txtWater);
            getColor(ref dr, "waterFactor_color", txtWaterFactor);
            getColor(ref dr, "air_color", txtAir);
            getColor(ref dr, "airFactor_color", txtAirFactor);
            getColor(ref dr, "wetair_color", txtWetAir);
            getColor(ref dr, "wetairfactor_color", txtWetAirFactor);
            getColor(ref dr, "pumpabound_color", txtPumpAbound);
            getColor(ref dr, "pumpaboundFactor_color", txtPumpAboundFactor);
            dbR.saveDataByRow(dr, op);
            chkGeneralElectricalPowerFailure();
            this.DialogResult = true;
        }

        /// <summary>
        /// Cancel Action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void MetroWindow_Loaded_1(object sender, RoutedEventArgs e)
        {
            dbR = new DBRelief(dbFile);
            string condenserName = txtName.Text;
            dtCondenser = dbR.getDataByVsdFile("frmcondenser", vsdFile, "heatername='" + condenserName + "'");
            if (dtCondenser != null)
            {
                if (dtCondenser.Rows.Count > 0)
                {
                    op = 1;
                    dr = dtCondenser.Rows[0];

                    txtName.Text = condenserName;
                    txtDescription.Text = dr["description"].ToString();
                    txtDuty.Text = dr["heaterduty"].ToString();
                    txtWater.Text = dr["water"].ToString();
                    txtWaterFactor.Text = dr["waterfactor"].ToString();
                    txtAir.Text = dr["air"].ToString();
                    txtAirFactor.Text = dr["airfactor"].ToString();
                    txtWetAir.Text = dr["wetair"].ToString();
                    txtWetAirFactor.Text = dr["wetairfactor"].ToString();
                    txtPumpAbound.Text = dr["pumpabound"].ToString();
                    txtPumpAboundFactor.Text = dr["pumpaboundfactor"].ToString();

                    setColor(dr, "heatername_color", txtName);
                    setColor(dr, "heaterduty_color", txtDuty);
                    setColor(dr, "water_color", txtWater);
                    setColor(dr, "waterFactor_color", txtWaterFactor);
                    setColor(dr, "air_color", txtAir);
                    setColor(dr, "airFactor_color", txtAirFactor);
                    setColor(dr, "wetair_color", txtWetAir);
                    setColor(dr, "wetairfactor_color", txtWetAirFactor);
                    setColor(dr, "pumpabound_color", txtPumpAbound);
                    setColor(dr, "pumpaboundFactor_color", txtPumpAboundFactor);
                    
                    loadstatus = 1;
                }
                
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
                decimal totalDuty=decimal.Parse(txtDuty.Text);
                if (txtBox.Name == "txtWaterFactor")
                {
                    decimal f = 0;
                    if (decimal.TryParse(txtWaterFactor.Text, out f) == false)
                        f = 0;
                    txtWater.Text = (totalDuty * f).ToString();
                }
                else if (txtBox.Name == "txtAirFactor")
                {
                    decimal f = 0;
                    if (decimal.TryParse(txtAirFactor.Text, out f) == false)
                        f = 0;
                    txtAir.Text = (totalDuty * f).ToString();
                }
                if (txtBox.Name == "txtWetAirFactor")
                {
                    decimal f = 0;
                    if (decimal.TryParse(txtWetAirFactor.Text, out f) == false)
                        f = 0;
                    txtWetAir.Text = (totalDuty * f).ToString();
                }
                if (txtBox.Name == "txtPumpAboundFactor")
                {
                    decimal f = 0;
                    if (decimal.TryParse(txtPumpAboundFactor.Text, out f) == false)
                        f = 0;
                    txtPumpAbound.Text = (totalDuty * f).ToString();
                }

            }
        }

        private void chkGeneralElectricalPowerFailure()
        {
            DataTable dtCase4 = dbR.getDataByVsdFile("frmcase_condenser", vsdFile, "case_id=4 and heatername='" + dr["heatername"].ToString() + "'");
            if (dtCase4.Rows.Count > 0)
            {
                DataRow r = dtCase4.Rows[0];
                decimal airfactor = decimal.Parse(txtAirFactor.Text.Trim())*0.2m;
                decimal wetairfactor = decimal.Parse(txtWetAirFactor.Text.Trim()) * 0.3m;
                decimal waterfactor = decimal.Parse(txtWaterFactor.Text.Trim());
                
                r["dutycalcfactor"] = airfactor + wetairfactor + waterfactor ;
                dbR.saveDataByRow(r, 1);
            }

        }

        
    }
}
