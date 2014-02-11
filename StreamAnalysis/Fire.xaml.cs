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
    /// Fire.xaml 的交互逻辑
    /// </summary>
    public partial class Fire : Window
    {
        public Fire()
        {
            InitializeComponent();
        }
        public string vsdFile;
        public string dbFile;
        private int op = 0;
        public int id=0;
        private DataTable dt = new DataTable();
        private DataRow dr;
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

            DBRelief dbR = new DBRelief(dbFile);
            dt = dbR.getDataByVsdFile("frmfire", vsdFile, "id=" + id + "");
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    op = 1;
                    dr = dt.Rows[0];

                    this.txtEquipmentNO.Text = dr["equipment_no"].ToString();
                    txtService.Text = dr["equipment_service"].ToString();
                    txtLocation.Text = dr["equipment_location"].ToString();
                    cbxStream.Text = dr["stream"].ToString();

                    cbxVesselType.Text = dr["vessel_type"].ToString();
                    txtNumberOfIdentcalVessels.Text = dr["number_of_identical_vessels"].ToString();
                    txtIVDiameter.Text = dr["diameter_identical_vessels"].ToString();
                    txtIVFactor.Text = dr["vessel_factor"].ToString();
                    txtIVLength.Text = dr["vessel_length"].ToString();
                    txtNumberOfWettedHeads.Text = dr["number_of_wetted_heads"].ToString();
                    txtNormalLiquidLevel.Text = dr["normal_liquid_level"].ToString();
                    txtFireExposedLiquidLevel.Text = dr["fire_exposed_liquid_level"].ToString();
                    txtElevationtoBottom.Text = dr["elevation_to_bottom"].ToString();
                    txtWettedDiameter.Text = dr["diameter_wetted_heads"].ToString();
                    txtTT.Text = dr["tt"].ToString();

                    txtHeatInput.Text = dr["heat_input"].ToString();
                    txtC1.Text = dr["c1"].ToString();
                    txtC2.Text = dr["c2"].ToString();
                    txtC3.Text = dr["c3"].ToString();
                    txtC4.Text = dr["c4"].ToString();
                    txtC5.Text = dr["c5"].ToString();
                    txtInlet.Text = dr["piping_inlet"].ToString();
                    txtInletLength.Text = dr["inlet_length"].ToString();
                    this.txtOutLet.Text = dr["piping_outlet"].ToString();
                    this.txtOutLength.Text = dr["outlet_length"].ToString();
                    this.txtPipingDiameter.Text = dr["piping_diameter"].ToString();
                    txtWettedArea.Text = dr["Wetted_Area"].ToString();
                }
                else
                {
                    dr = dt.NewRow();
                }
            }



        }
        
        

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnImg_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dr["equipment_no"] = this.txtEquipmentNO.Text.Trim();
                dr["equipment_service"] = txtService.Text.Trim();
                dr["equipment_location"] = txtLocation.Text.Trim();
                dr["stream"] = cbxStream.Text.Trim();

                dr["vessel_type"] = cbxVesselType.Text.Trim();
                dr["number_of_identical_vessels"] = txtNumberOfIdentcalVessels.Text.Trim();
                dr["diameter_identical_vessels"] = txtIVDiameter.Text.Trim();
                dr["vessel_factor"] = txtIVFactor.Text.Trim();
                dr["vessel_length"] = txtIVLength.Text.Trim();
                dr["number_of_wetted_heads"] = txtNumberOfWettedHeads.Text.Trim();
                dr["normal_liquid_level"] = txtNormalLiquidLevel.Text.Trim();
                dr["fire_exposed_liquid_level"] = txtFireExposedLiquidLevel.Text.Trim();
                dr["elevation_to_bottom"] = txtElevationtoBottom.Text.Trim();
                dr["diameter_wetted_heads"] = txtWettedDiameter.Text.Trim();
                dr["tt"] = txtTT.Text.Trim();

                dr["heat_input"] = txtHeatInput.Text.Trim();
                dr["c1"] = txtC1.Text.Trim();
                dr["c2"] = txtC2.Text.Trim();
                dr["c3"] = txtC3.Text.Trim();
                dr["c4"] = txtC4.Text.Trim();
                dr["c5"] = txtC5.Text.Trim();
                dr["piping_inlet"] = txtInlet.Text.Trim();
                dr["inlet_length"] = txtInletLength.Text.Trim();
                dr["piping_outlet"] = this.txtOutLet.Text.Trim();
                dr["outlet_length"] = this.txtOutLength.Text.Trim();
                dr["piping_diameter"] = this.txtPipingDiameter.Text.Trim();
                dr["wetted_area"] = this.txtWettedArea.Text.Trim();
                dr["visiofile"] = vsdFile;
                DBRelief dbR = new DBRelief(dbFile);
                dbR.saveDataByRow(dr, op);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Action");
            }
            this.DialogResult = true;
        }
    }
}
