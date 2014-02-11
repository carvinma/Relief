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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;

namespace ReliefAnalysis
{
    /// <summary>
    /// UC_Fire.xaml 的交互逻辑
    /// </summary>
    public partial class UC_Fire : UserControl
    {
        public UC_Fire()
        {
            InitializeComponent();
            int columnsCount = fireDataGrid.Columns.Count;
            fireDataGrid.Columns[columnsCount - 3].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }
        public string dbFile;
        public string vsdFile;
        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            Fire frmFire = new Fire();
            frmFire.vsdFile = vsdFile;
            frmFire.dbFile = dbFile;
            Window parentWindow = Window.GetWindow(this);
            frmFire.Owner = parentWindow;
            if (frmFire.ShowDialog() == true)
            {
                bindDataGrid();
            }
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            bindDataGrid();
        }
        private void bindDataGrid()
        {
             DBRelief dbR = new DBRelief(dbFile);
            DataTable  dt = dbR.getDataByVsdFile("frmfire", vsdFile);
            if (dt != null)
            {
                if (dt.Rows.Count != 0)
                {
                    fireDataGrid.ItemsSource = dt.DefaultView;
                }
                else
                {
                    
                    DataTable dtCondenser = dbR.getDataByVsdFile("frmcondenser", vsdFile);
                    foreach (DataRow drCondenser in dtCondenser.Rows)
                    {
                        DataRow dr = dt.NewRow();
                        dr["equipment_no"]=drCondenser["heatername"].ToString();
                        dr["visiofile"] = vsdFile;
                        dt.Rows.Add(dr);

                    }
                    DataTable dtReboiler = dbR.getDataByVsdFile("frmreboiler", vsdFile);
                    foreach (DataRow drReboiler in dtReboiler.Rows)
                    {
                        DataRow dr = dt.NewRow();
                        dr["equipment_no"] = drReboiler["heatername"].ToString();
                        dr["visiofile"] = vsdFile;
                        dt.Rows.Add(dr);

                    }

                    DataTable dtAccumulator = dbR.getDataByVsdFile("frmAccumulator", vsdFile);
                    foreach (DataRow drAccumulator in dtAccumulator.Rows)
                    {
                        DataRow dr = dt.NewRow();
                        dr["equipment_no"] = drAccumulator["accumulatorname"].ToString();
                        dr["visiofile"] = vsdFile;
                        dt.Rows.Add(dr);
                    }
                    dbR.saveDataByTable(dt, vsdFile);
                    dt = dbR.getDataByVsdFile("frmfire", vsdFile);
                    fireDataGrid.ItemsSource = dt.DefaultView;
                     
                }
            }
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            object ID = ((Button)sender).CommandParameter;
            Fire frmFire = new Fire();
            Window parentWindow = Window.GetWindow(this);
            frmFire.Owner = parentWindow;
            frmFire.vsdFile = vsdFile;
            frmFire.dbFile = dbFile;
            frmFire.id = int.Parse(ID.ToString());
            if (frmFire.ShowDialog() == true)
            {
                bindDataGrid();
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            object ID = ((Button)sender).CommandParameter;

            
        }

        private void btnCalulation_Click(object sender, RoutedEventArgs e)
        {
            DataView dv = (DataView)fireDataGrid.ItemsSource;
            double total = 0;
            for(int i=0;i<dv.Table.Rows.Count;i++)
            {
                DataRow dr = dv.Table.Rows[i];
                double vessel_wetted_surface = 0;
                string type = dr["vessel_type"].ToString();
                int numberOfIdenticalVesssels = int.Parse(dr["number_of_identical_vessels"].ToString());
                double D = double.Parse(dr["diameter_identical_vessels"].ToString());
                double H = double.Parse(dr["normal_liquid_level"].ToString());
                double ffactor=double.Parse(dr["vessel_factor"].ToString());
                double L = double.Parse(dr["vessel_length"].ToString());
                double pai=3.14159;
                if (H > 7.62)
                {
                    H = 7.62;
                }
                if (type == "Horizontal")
                {
                    if (H >= D / 2)
                    {
                        vessel_wetted_surface = pai * D * L / 2 + 2 * pai * D * D * 1.66 / 8;
                    }
                    else if (H < D / 2)
                    {
                        vessel_wetted_surface = Math.Acos((D / 2 - H) / (D / 2)) * D * L + 2 * 1.66 * (Math.Acos((D / 2 - H) / (D / 2)) * D * D / 4 - (D / 2 - H) * Math.Sqrt(D * H - H * H));
                    }
                }
                else if (type == "Vertical")
                {                    
                   vessel_wetted_surface = pai * D * H + pai * D * D * 1.66 / 4;
                }
                else
                {
                    if (H >= D / 2 )
                    {
                        vessel_wetted_surface = 2 * pai * D * D;
                    }
                    else if (H < D / 2)
                    {
                        vessel_wetted_surface = 2 * pai * 2*H * D;
                    }
                }
                double wetted_surface=vessel_wetted_surface;
                double heat_absorbed = 155426 * ffactor *  Math.Pow(wetted_surface, 0.82);
                double relief = heat_absorbed / 120.8;
                total = total + relief;
            }
            this.txtReliefRate.Text = total.ToString();
        }
    }
}
