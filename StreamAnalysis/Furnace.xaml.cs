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
using ReliefAnalysis;
using System.Data;
using System.Data.OleDb;

namespace ReliefAnalysis
{
    /// <summary>
    /// Furnace.xaml 的交互逻辑
    /// </summary>
    public partial class Furnace : Window
    {
        public Furnace()
        {
            InitializeComponent();
        }
        public string dbFile;
        public string nameU;
        public  List<ShapeInfo> list = new List<ShapeInfo>();
        public ShapeInfo startV;
        public ShapeInfo endV;
        public string eqType;
        public bool isExit;
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (startCbox.SelectedIndex != -1)
            {
                ComboBoxItem item= (ComboBoxItem)startCbox.SelectedItem;
                ShapeInfo si =(ShapeInfo) item.Tag;
                startV = si;
            }
            if (endCbox.SelectedIndex != -1)
            {
                ComboBoxItem item = (ComboBoxItem)endCbox.SelectedItem;
                ShapeInfo si = (ShapeInfo)item.Tag;
                endV = si;
            }

            string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbFile + ";Persist Security Info=False;";
            OleDbConnection conn = new OleDbConnection(connectString);
            conn.Open();
            if (isExit)
            {
                StringBuilder sqlSB = new StringBuilder("update vstream set ");
                sqlSB.Append("streamname").Append("='").Append(this.txtName.Text).Append("',");
                //sqlSB.Append("Temperature").Append("='").Append(this.txtTemp.Text).Append("',");
                //sqlSB.Append("Pressure").Append("='").Append(this.txtPres.Text).Append("',");
                //sqlSB.Append("WeightFlow").Append("='").Append(this.txtPres.Text).Append("',");
                //sqlSB.Append("Enthalpy").Append("='").Append(this.txtPres.Text).Append("',");
                //sqlSB.Append("SpEnthalpy").Append("='").Append(this.txtPres.Text).Append("',");
                //sqlSB.Append("VaporFraction").Append("='").Append(this.txtVabFrac.Text).Append("' ");

                sqlSB.Append(" where nameU='").Append(nameU).Append("'");
                OleDbCommand cmd = new OleDbCommand(sqlSB.ToString(), conn);
                cmd.ExecuteNonQuery();

            }
            else
            {
                StringBuilder sqlSB = new StringBuilder("insert into vstream(nameU,streamname,Temperature,Pressure,WeightFlow,Enthalpy,SpEnthalpyVaporFraction)values(");
               // sqlSB.Append("'").Append(nameU).Append("','").Append(this.txtName.Text).Append("','").Append(this.txtTemp.Text).Append("','").Append(this.txtPres.Text).Append("','").Append(this.txtWf.Text).Append("','").Append(this.txtH.Text).Append("','").Append(this.txtSph.Text).Append("','").Append(this.txtVabFrac.Text).Append("'");
                sqlSB.Append(")");
                OleDbCommand cmd = new OleDbCommand(sqlSB.ToString(), conn);
                cmd.ExecuteNonQuery();

            }
            conn.Close();
            this.DialogResult = true;




            this.DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            fuelTypeCbox.SelectedIndex = 0;
            try
            {
                foreach (ShapeInfo si in list)
                {
                    ComboBoxItem item = new ComboBoxItem();
                    item.Content = si.Text;
                    item.Tag = si;
                    startCbox.Items.Add(item);
                    
                }
                foreach (ShapeInfo si in list)
                {
                    ComboBoxItem item = new ComboBoxItem();
                    item.Content = si.Text;
                    item.Tag = si;
                    endCbox.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
            }

           
        }


        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            OptionEquipment frm= new OptionEquipment();
            frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frm.dbFile = dbFile;
            frm.eqType = eqType;
            if (frm.ShowDialog() == true)
            {
                DataSet eqInfo = (DataSet)Application.Current.Properties["eqInfo"];
                DataTable dt = eqInfo.Tables[0];
                DataRow dr = dt.Rows[0];
                txtName.Text = dr["eqname"].ToString();
                bindCombox(dr["feeddata"].ToString(), startCbox);
                bindCombox(dr["productdata"].ToString(), endCbox);
                Application.Current.Properties.Remove("eqInfo");
                
            }
        }
        private void bindCombox(string values, ComboBox cbx)
        {
            cbx.Items.Clear();
            string[] items = values.Split(',');
            foreach (string i in items)
            {
                if(i!=string.Empty)
                {
                    ShapeInfo si = new ShapeInfo();
                    si.Text = i;
                    si.NameU = i;
                    ComboBoxItem item = new ComboBoxItem();
                    item.Content = si.Text;
                    item.Tag = si;
                    cbx.Items.Add(item);
                }
            }


            if (cbx.Items.Count > 0)
                cbx.SelectedIndex = 0;
        }
    
    
    
    
    }
}
