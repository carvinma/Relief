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
using ReliefAnalysis;



namespace ReliefAnalysis
{
    /// <summary>
    /// Tower.xaml 的交互逻辑
    /// </summary>
    public partial class Tower : Window
    {
        public Tower()
        {
            InitializeComponent();
        }
        public string dbFile;
        public string eqType;
        public string towerName;
        public string przFile;
        public string vsdFile;
        public int op=0;
        public DataTable dtFeed=new DataTable();
        public DataTable dtProd=new DataTable();
        public DataTable dtCondenser = new DataTable();
        public DataTable dtHxCondenser = new DataTable();
        public DataTable dtReboiler = new DataTable();
        public DataTable dtHxReboiler = new DataTable();
        public DataTable dtTower = new DataTable();
        public DataSet dsTower=new DataSet();
        public DataTable dtSource = new DataTable();
        public DataTable dtSink = new DataTable();

        public DataTable dtFeed_init = new DataTable();
        public DataTable dtProd_init = new DataTable();
        public DataTable dtCondenser_init = new DataTable();
        public DataTable dtHxCondenser_init = new DataTable();
        public DataTable dtReboiler_init = new DataTable();
        public DataTable dtHxReboiler_init = new DataTable();

        private int loadstatus = 0;
        private void btnImport_Click(object sender, RoutedEventArgs e)
        {           
            OptionEquipment frm = new OptionEquipment();
            frm.Owner = this;
            
            frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frm.dbFile = dbFile;
            frm.eqType = eqType;
            if (frm.ShowDialog() == true)
            {
                op = 0;
                dtFeed.Rows.Clear();
                dtProd.Rows.Clear();
                dtSource.Rows.Clear();
                dtSink.Rows.Clear();
                dtCondenser.Rows.Clear();
                dtHxCondenser.Rows.Clear();
                dtReboiler.Clear();
                dtHxReboiler.Rows.Clear();
                dtTower.Rows.Clear();
                przFile = frm.przFile;
                dsTower = (DataSet)Application.Current.Properties["eqInfo"];
                Application.Current.Properties.Remove("eqInfo");
                bindTowerInfo();
                bindCombox();
                txtName.BorderBrush = Brushes.Green;
                txtName.BorderThickness = new Thickness(2, 2, 2, 2);
                txtStageNumber.BorderBrush = Brushes.Green;
                txtStageNumber.BorderThickness = new Thickness(2, 2, 2, 2);  
            }
        }
        private void bindCombox()
        {
            lvFeed.ItemsSource = dtFeed.DefaultView;
            lvProd.ItemsSource = dtProd.DefaultView;
            lvCondenser.ItemsSource = dtCondenser.DefaultView;
            lvHxCondenser.ItemsSource = dtHxCondenser.DefaultView;
            lvReboiler.ItemsSource = dtReboiler.DefaultView;
            lvHxReboiler.ItemsSource = dtHxReboiler.DefaultView;
        }
        private void bindTowerInfo()
        {
            if (dsTower.Tables.Count > 0 && dsTower.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsTower.Tables[0].Rows[0];
                txtName.Text = dr["eqname"].ToString();
                //txtDescription.Text = dr["description"].ToString();
                txtStageNumber.Text = dr["numberoftrays"].ToString();

                DBRelief dbReader = new DBRelief(dbFile);
                Dictionary<string, string> dicFeeds = new Dictionary<string, string>();
                Dictionary<string, string> dicProducts = new Dictionary<string, string>();
                dbReader.getMaincolumnRealFeedProduct(dr["eqname"].ToString(), ref dicFeeds, ref dicProducts);

                DBRelief dbR=new DBRelief(dbFile);
                foreach (KeyValuePair<string, string> feed in dicFeeds)
                {
                    DataRow r = dtFeed.NewRow();
                    r["streamname"] = feed.Key;
                    r["tray"] = feed.Value;
                    r["visiofile"] = vsdFile;
                    dbR.getAndConvertStreamInfo(feed.Key, ref r);
                    dtFeed.Rows.Add(r);

                    DataRow rsource = dtSource.NewRow();
                    rsource["streamname"] = feed.Key;
                    rsource["sourcename"] = feed.Key+"_Source";
                    rsource["visiofile"] = vsdFile;
                    rsource["ismaintained"] = false;
                    rsource["sourcetype"] = "Pump（Motor）";
                    rsource["maxpossiblepressure"] = r["pressure"].ToString();
                    rsource["maxpossiblepressure_color"] = "green";
                    dtSource.Rows.Add(rsource);

                }

                foreach (KeyValuePair<string, string> prodcut in dicProducts)
                {
                    DataRow r = dtProd.NewRow();
                    r["streamname"] = prodcut.Key;
                    r["tray"] = prodcut.Value;
                    r["visiofile"] = vsdFile;
                    dbR.getAndConvertStreamInfo(prodcut.Key, ref r);
                    dtProd.Rows.Add(r);

                    DataRow rsink = dtSink.NewRow();
                    rsink["streamname"] = prodcut.Key;
                    rsink["sinkname"] = prodcut.Key + "_Sink";
                    rsink["visiofile"] = vsdFile;
                    rsink["ismaintained"] = false;
                    rsink["sinktype"] = "Pump（Motor）";
                    rsink["maxpossiblepressure"] = r["pressure"].ToString();
                    rsink["maxpossiblepressure_color"] = "green";

                    dtSink.Rows.Add(rsink);

                }

                string heaterNames = dr["HeaterNames"].ToString();
                string heaterDuties = dr["HeaterDuties"].ToString();
                string heaterTrayLoc = dr["HeaterTrayLoc"].ToString();
                string[] arrHeaterNames = heaterNames.Split(',');
                string[] arrHeaterDuties = heaterDuties.Split(',');
                string[] arrHeaterTrayLoc = heaterTrayLoc.Split(',');
                for (int i = 0; i < arrHeaterNames.Length; i++)
                {
                    decimal duty=decimal.Parse(arrHeaterDuties[i])/1000;
                    duty=decimal.Round(duty, 4); 
                    if (arrHeaterNames[i] == "CONDENSER")
                    {
                        
                        DataRow r = dtCondenser.NewRow();
                        r["heatername"] = arrHeaterNames[i];
                        r["heaterduty"] = duty;
                        r["visiofile"] = vsdFile;
                        r["water"] = duty; 
                        r["waterfactor"] = 1;                       
                        r["ishx"] = false;

                        r["air"] = 0;
                        r["airfactor"] = 0;
                        r["wetair"] = 0;
                        r["wetairfactor"] = 0;
                        r["pumpabound"] = 0;
                        r["pumpaboundfactor"] = 0;

                        r["heatername_color"] = "green";
                        r["heaterduty_color"] = "green";
                        r["water_color"] = "green";
                        r["waterfactor_color"] = "green";
                        dtCondenser.Rows.Add(r);

                    }
                    else if (arrHeaterNames[i] == "REBOILER")
                    {
                        
                        DataRow r = dtReboiler.NewRow();
                        r["heatername"] = arrHeaterNames[i];
                        r["heaterduty"] = duty; 
                        r["visiofile"] = vsdFile;
                        r["steam"] = duty; 
                        r["steamfactor"] = 1;

                        r["hotstream"] = 0;
                        r["hotstreamfactor"] = 0;
                        r["hotoil"] = 0;
                        r["hotoilfactor"] = 0;
                        r["furnace"] = 0;
                        r["furnacefactor"] = 0;

                        r["ishx"] = false;
                        r["iscontinued"] = false;

                        
                        dtReboiler.Rows.Add(r);

                    }
                    else if (double.Parse(arrHeaterDuties[i]) <= 0 && arrHeaterNames[i] != "CONDENSER")
                    {
                        if (arrHeaterTrayLoc[i] == "1")
                        {
                            DataRow r = dtCondenser.NewRow();
                            r["heatername"] = arrHeaterNames[i];
                            r["heaterduty"] = duty; 
                            r["visiofile"] = vsdFile;
                            r["water"] = duty; 
                            r["waterfactor"] = 1;
                            r["ishx"] = false;
                            r["air"] = 0;
                            r["airfactor"] = 0;
                            r["wetair"] = 0;
                            r["wetairfactor"] = 0;
                            r["pumpabound"] = 0;
                            r["pumpaboundfactor"] = 0;                          
                            dtCondenser.Rows.Add(r);
                        }
                        else
                        {
                            DataRow r = dtHxCondenser.NewRow();
                            r["heatername"] = arrHeaterNames[i];
                            r["heaterduty"] = duty; 
                            r["visiofile"] = vsdFile;
                            r["water"] = duty; 
                            r["waterfactor"] = 1;
                            r["ishx"] = true;

                            r["air"] = 0;
                            r["airfactor"] = 0;
                            r["wetair"] = 0;
                            r["wetairfactor"] = 0;
                            r["pumpabound"] = 0;
                            r["pumpaboundfactor"] = 0;                          
                            dtHxCondenser.Rows.Add(r);
                        }
                    }
                    else if (double.Parse(arrHeaterDuties[i]) > 0 && arrHeaterNames[i] != "REBOILER")
                    {
                        if (arrHeaterTrayLoc[i] == dr["numberoftrays"].ToString())
                        {
                            DataRow r = dtReboiler.NewRow();
                            r["heatername"] = arrHeaterNames[i];
                            r["heaterduty"] = duty; 
                            r["visiofile"] = vsdFile;
                            r["steam"] = duty; 
                            r["steamfactor"] = 1;
                            r["ishx"] = false;
                            r["iscontinued"] = false;

                            r["hotstream"] = 0;
                            r["hotstreamfactor"] = 0;
                            r["hotoil"] = 0;
                            r["hotoilfactor"] = 0;
                            r["furnace"] = 0;
                            r["furnacefactor"] = 0;                         
                            dtReboiler.Rows.Add(r);
                        }
                        else
                        {
                            DataRow r = dtHxReboiler.NewRow();
                            r["heatername"] = arrHeaterNames[i];
                            r["heaterduty"] = duty; 
                            r["visiofile"] = vsdFile;
                            r["steam"] = duty; 
                            r["steamfactor"] = 1;
                            r["ishx"] = true;
                            r["iscontinued"] = false;

                            r["hotstream"] = 0;
                            r["hotstreamfactor"] = 0;
                            r["hotoil"] = 0;
                            r["hotoilfactor"] = 0;
                            r["furnace"] = 0;
                            r["furnacefactor"] = 0;

                          

                            dtHxReboiler.Rows.Add(r);
                        }
                    }
                }


            }
            dtFeed_init = dtFeed.Copy();
            dtProd_init = dtProd.Copy();
            dtCondenser_init = dtCondenser.Copy();
            dtHxCondenser_init = dtHxCondenser.Copy();
            dtReboiler_init = dtReboiler.Copy();
            dtHxReboiler_init = dtHxReboiler.Copy();
        }
        private void btnDeleteFeed_Click(object sender, RoutedEventArgs e)
        {
            int idx = lvFeed.SelectedIndex;
            if ( idx> -1)
            {
                DataView dv = (DataView)lvFeed.ItemsSource;
                DataTable dt = dv.Table;
                dt.Rows.RemoveAt(idx);
                lvFeed.ItemsSource = dt.DefaultView;
                
            }
        }

        private void btnNewFeed_Click(object sender, RoutedEventArgs e)
        {
            TowerFeed feed = new TowerFeed();
            feed.Owner = this;
            feed.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            feed.Title = "Feed Data";
            if (feed.ShowDialog() == true)
            {
                DataView dv = (DataView)lvFeed.ItemsSource;
                DataTable dt = dv.Table;
                DataRow dr = dt.NewRow();
                dr["streamname"] = feed.txtStreamName.Text;
                dr["stage"] = feed.txtStage.Text;
                dr["type"] = feed.cbxType.Text.ToString();
                dr["visiofile"] = vsdFile;
                dt.Rows.Add(dr);
                lvFeed.ItemsSource = dt.DefaultView;
                
            }
        }

        private void lvFeed_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(lvFeed.SelectedIndex>-1)
            {
             DataView dv = (DataView)lvFeed.ItemsSource;
                DataTable dt = dv.Table;
                DataRow dr=dt.Rows[lvFeed.SelectedIndex];
            TowerFeed feed = new TowerFeed();
            feed.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            feed.txtStreamName.Text = dr[0].ToString();
            feed.txtStage.Text = dr[1].ToString();
            feed.cbxType.Text = dr[2].ToString();
            feed.Owner = this;

            if (feed.ShowDialog() == true)
            {             
                dr[0] = feed.txtStreamName.Text;
                dr[1] = feed.txtStage.Text;
                dr[2] = feed.cbxType.Text.ToString();
                lvFeed.ItemsSource = dt.DefaultView;
            }
            }
        }

        private void btnNewCondenser_Click(object sender, RoutedEventArgs e)
        {
            TowerCondenser frm = new TowerCondenser();
            frm.WindowStartupLocation=WindowStartupLocation.CenterScreen;
            frm.Title = "Condenser";
            frm.categoryTag = 1;
            frm.Owner = this;
            if (frm.ShowDialog() == true)
            {
                DataView dv =(DataView)lvCondenser.ItemsSource;
                DataTable dt = dv.Table;
                DataRow dr = dt.NewRow();
                dr["heatername"] = frm.txtName.Text;
                dr["heaterduty"] = frm.txtDuty.Text;
                dr["Type"] = frm.cbxType.Text;
                dr["Driven"]=frm.cbxDriven.Text;
                dr["visiofile"] = vsdFile;
                dt.Rows.Add(dr);
                lvCondenser.ItemsSource = dt.DefaultView;
                
            }
        }

        private void btnDeleteCondenser_Click(object sender, RoutedEventArgs e)
        {
            int idx = lvCondenser.SelectedIndex;
            if (idx > -1)
            {
                DataView dv = (DataView)lvCondenser.ItemsSource;
                DataTable dt = dv.Table;
                dt.Rows.RemoveAt(idx);
                lvCondenser.ItemsSource = dt.DefaultView;
                
            }
        }

        private void btnNewHxCondenser_Click(object sender, RoutedEventArgs e)
        {
            TowerCondenser frm = new TowerCondenser();
            frm.Owner = this;
            
            frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frm.Title = "Hx Condenser";
            frm.categoryTag = 2;
            if (frm.ShowDialog() == true)
            {
                DataView dv = (DataView)lvHxCondenser.ItemsSource;
                DataTable dt = dv.Table;
                DataRow dr = dt.NewRow();
                dr["heatername"] = frm.txtName.Text;
                dr["heaterduty"] = frm.txtDuty.Text;
                dr["Driven"] = frm.cbxDriven.Text;
                dr["Type"] = frm.cbxType.Text;
                dr["visiofile"] = vsdFile;
                dt.Rows.Add(dr);
                lvHxCondenser.ItemsSource = dt.DefaultView;
                
            }
        }

        private void btnDeleteHxCondenser_Click(object sender, RoutedEventArgs e)
        {
            int idx = lvHxCondenser.SelectedIndex;
            if (idx > -1)
            {
                DataView dv = (DataView)lvHxCondenser.ItemsSource;
                DataTable dt = dv.Table;
                dt.Rows.RemoveAt(idx);
                lvHxCondenser.ItemsSource = dt.DefaultView;
                
            }
        }

        private void btnNewReboiler_Click(object sender, RoutedEventArgs e)
        {
            TowerCondenser frm = new TowerCondenser();
            frm.Owner = this;
            frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frm.Title = "Reboiler";
            frm.categoryTag = 3;
            if (frm.ShowDialog() == true)
            {
                DataView dv = (DataView)lvReboiler.ItemsSource;
                DataTable dt = dv.Table;
                DataRow dr = dt.NewRow();
                dr["heatername"] = frm.txtName.Text;
                dr["heaterduty"] = frm.txtDuty.Text;
                dr["Type"] = frm.cbxType.Text;
                dr["Driven"] = frm.cbxDriven.Text;
                dr["visiofile"] = vsdFile;
                dt.Rows.Add(dr);
                lvReboiler.ItemsSource = dt.DefaultView;
                
            }
        }

        private void btnDeleteReboiler_Click(object sender, RoutedEventArgs e)
        {
            int idx = lvReboiler.SelectedIndex;
            if (idx > -1)
            {
                DataView dv = (DataView)lvReboiler.ItemsSource;
                DataTable dt = dv.Table;
                dt.Rows.RemoveAt(idx);
                lvReboiler.ItemsSource = dt.DefaultView;
                
            }
        }

        private void btnNewHxReboiler_Click(object sender, RoutedEventArgs e)
        {
            TowerCondenser frm = new TowerCondenser();
            frm.Owner = this;
            frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frm.Title = "Hx Reboiler";
            frm.categoryTag = 4;
            if (frm.ShowDialog() == true)
            {
                DataView dv = (DataView)lvHxReboiler.ItemsSource;
                DataTable dt = dv.Table;
                DataRow dr = dt.NewRow();
                dr["heatername"] = frm.txtName.Text;
                dr["heaterduty"] = frm.txtDuty.Text;
                dr["Type"] = frm.cbxType.Text;
                dr["Driven"] = frm.cbxDriven.Text;
                dr["visiofile"] = vsdFile;
                dt.Rows.Add(dr);
                lvHxReboiler.ItemsSource = dt.DefaultView;
                
            }
        }

        private void btnDeleteHxReboiler_Click(object sender, RoutedEventArgs e)
        {
            int idx = lvHxReboiler.SelectedIndex;
            if (idx > -1)
            {
                DataView dv = (DataView)lvHxReboiler.ItemsSource;
                DataTable dt = dv.Table;
                dt.Rows.RemoveAt(idx);
                lvHxReboiler.ItemsSource = dt.DefaultView;
               
            }
        }

        private void btnNewProd_Click(object sender, RoutedEventArgs e)
        {
            TowerFeed feed = new TowerFeed();
            feed.Owner = this;
            feed.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            feed.Title = "Product Data";
            if (feed.ShowDialog() == true)
            {
                DataView dv = (DataView)lvProd.ItemsSource;
                DataTable dt = dv.Table;
                DataRow dr = dt.NewRow();
                dr["streamname"] = feed.txtStreamName.Text;
                dr["stage"] = feed.txtStage.Text;
                dr["type"] = feed.cbxType.Text.ToString();
                dr["visiofile"] = vsdFile;
                dt.Rows.Add(dr);
                lvProd.ItemsSource = dt.DefaultView;
                
            }
        }

        private void btnDeleteProd_Click(object sender, RoutedEventArgs e)
        {
            int idx = lvProd.SelectedIndex;
            if (idx > -1)
            {
                DataView dv = (DataView)lvProd.ItemsSource;
                DataTable dt = dv.Table;
                dt.Rows.RemoveAt(idx);
                lvProd.ItemsSource = dt.DefaultView;
                
            }
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            InitDataTable();            
            getTowerInfo();
            bindTowerInfo();
            loadstatus = 1;
        }
        public void getTowerInfo()
        {
            try
            {
                DBRelief dbR = new DBRelief(dbFile);
                dtTower = dbR.getDataByVsdFile("frmtower", vsdFile);
                if (dtTower.Rows.Count != 0)
                {
                    op = 1;
                    DataRow dr = dtTower.Rows[0];
                    txtName.Text = dr["towername"].ToString();
                    this.txtStageNumber.Text = dr["StageNumber"].ToString();
                    this.txtDescription.Text = dr["Description"].ToString();

                    string towercolor = dr["towername_color"].ToString();
                    if (towercolor=="blue")
                    {
                        txtName.BorderBrush = Brushes.Blue;
                        txtName.BorderThickness = new Thickness(2, 2, 2, 2);
                    }
                    else
                    {
                        txtName.BorderBrush = Brushes.Green;
                        txtName.BorderThickness = new Thickness(2, 2, 2, 2);
                    }

                    if (dr["StageNumber_color"].ToString() == "blue")
                    {
                        txtStageNumber.BorderBrush = Brushes.Blue;
                        txtStageNumber.BorderThickness = new Thickness(2, 2, 2, 2);
                    }
                    else
                    {
                        txtStageNumber.BorderBrush = Brushes.Green;
                        txtStageNumber.BorderThickness = new Thickness(2, 2, 2, 2);
                    }


                }
                DataTable dt = new DataTable();
                dt = dbR.getDataByVsdFile("frmfeed", vsdFile);
                if (dt.Rows.Count != 0)
                {
                    dtFeed = dt;
                }
                dt = dbR.getDataByVsdFile("frmproduct", vsdFile);
                if (dt.Rows.Count != 0)
                {
                    dtProd = dt;
                }
                dt = dbR.getDataByVsdFile("frmcondenser", vsdFile, "ishx=false");
                if (dt.Rows.Count != 0)
                {
                    this.dtCondenser = dt;
                }
                dt = dbR.getDataByVsdFile("frmcondenser", vsdFile, "ishx=true");
                if (dt.Rows.Count != 0)
                {
                    this.dtHxCondenser = dt;
                }
                dt = dbR.getDataByVsdFile("frmreboiler", vsdFile,"ishx=false");
                if (dt.Rows.Count != 0)
                {
                    this.dtReboiler = dt;
                }
                dt = dbR.getDataByVsdFile("frmreboiler", vsdFile, "ishx=true");
                if (dt.Rows.Count != 0)
                {
                    this.dtHxReboiler = dt;
                }

                bindCombox();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }

        public void InitDataTable()
        {
            DBRelief dbr = new DBRelief(dbFile);
            dtFeed = dbr.getStructure("frmfeed");
            dtProd = dbr.getStructure("frmproduct");
            dtCondenser = dbr.getStructure("frmcondenser");
            dtReboiler = dbr.getStructure("frmreboiler");
            dtHxReboiler = dtReboiler.Clone();
            dtHxCondenser = dtCondenser.Clone();

            dtSource = dbr.getStructure("frmsource");
            dtSink = dbr.getStructure("frmsink");

        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DBRelief dbR = new DBRelief(dbFile);
            DataRow dr = dtTower.NewRow();
            if (op == 0)
            {
                Application.Current.Properties.Add("FeedData", dtFeed);
                Application.Current.Properties.Add("Condenser", dtCondenser);
                Application.Current.Properties.Add("HxCondenser", dtHxCondenser);
                Application.Current.Properties.Add("Reboiler", dtReboiler);
                Application.Current.Properties.Add("HxReboiler", dtHxReboiler);
                Application.Current.Properties.Add("ProdData", dtProd);

                
                dtTower.Rows.Add(dr);
                dr["towername"] = txtName.Text;
                dr["stagenumber"] = this.txtStageNumber.Text;
                dr["description"] = this.txtDescription.Text;
                dr["visiofile"] = vsdFile;

                if (!string.IsNullOrEmpty(przFile))
                {
                    dr["przfile"] = przFile;
                }
                DataTable dtCondenserTemp = dtCondenser.Clone();
                dtCondenserTemp.Merge(dtCondenser);
                dtCondenserTemp.Merge(dtHxCondenser);
                DataTable dtReboilerTemp = dtReboiler.Clone();
                dtReboilerTemp.Merge(dtReboiler);
                dtReboilerTemp.Merge(dtHxReboiler);

                
                dbR.saveDataByTable(dtFeed, vsdFile);
                dbR.saveDataByTable(dtProd, vsdFile);
                dbR.saveDataByTable(dtSource, vsdFile);
                dbR.saveDataByTable(dtSink, vsdFile);
                dbR.saveDataByTable(dtCondenserTemp, vsdFile);
                dbR.saveDataByTable(dtReboilerTemp, vsdFile);
                dbR.saveDataByTable(dtTower, vsdFile);
            }
            else
            {
                dr = dtTower.Rows[0];
                dr["towername"] = txtName.Text;
                dr["stagenumber"] = this.txtStageNumber.Text;
                dr["description"] = this.txtDescription.Text;
                dr["visiofile"] = vsdFile;

                if (!string.IsNullOrEmpty(przFile))
                {
                    dr["przfile"] = przFile;
                }
                if (txtName.BorderBrush == Brushes.Green)
                {
                    dr["towername_color"] = "green";
                }
                else
                {
                    dr["towername_color"] = "blue";
                }
                if (txtStageNumber.BorderBrush == Brushes.Green)
                {
                    dr["stagenumber_color"] = "green";
                }
                else
                {
                    dr["stagenumber_color"] = "blue";
                }
                dbR.saveDataByRow(dr, 1);
            }
            this.DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private bool compareInfo()
        {
            bool result = false;

            if (dtFeed.Rows.Count == dtFeed_init.Rows.Count)
            {
                foreach (DataRow dr in dtFeed.Rows)
                {
                    string name = dr["streamname"].ToString();
                    if (dtFeed_init.Select("streamname='" + name + "'").Length == 0)
                    {
                        result = true;
                        break;
                    }
                }
                if (result)
                    return true;
            }
            else
            {
                return true;
            }

            if (this.dtProd.Rows.Count == dtProd_init.Rows.Count)
            {
                foreach (DataRow dr in dtProd.Rows)
                {
                    string name = dr["streamname"].ToString();
                    if (dtProd_init.Select("streamname='" + name + "'").Length == 0)
                    {
                        result = true;
                        break;
                    }
                }
                if (result)
                    return true;
            }
            else
            {
                return true;
            }

            if (this.dtHxCondenser.Rows.Count == dtHxCondenser_init.Rows.Count)
            {
                foreach (DataRow dr in dtHxCondenser.Rows)
                {
                    string name = dr["hxcondensername"].ToString();
                    if (dtHxCondenser_init.Select("hxcondensername='" + name + "'").Length == 0)
                    {
                        result = true;
                        break;
                    }
                }
                if (result)
                    return true;
            }
            else
            {
                return true;
            }

            if (this.dtCondenser.Rows.Count == dtCondenser_init.Rows.Count)
            {
                foreach (DataRow dr in dtCondenser.Rows)
                {
                    string name = dr["condensername"].ToString();
                    if (dtCondenser_init.Select("condensername='" + name + "'").Length == 0)
                    {
                        result = true;
                        break;
                    }
                }
                if (result)
                    return true;
            }
            else
            {
                return true;
            }


            if (this.dtReboiler.Rows.Count == dtReboiler_init.Rows.Count)
            {
                foreach (DataRow dr in dtReboiler.Rows)
                {
                    string name = dr["reboilername"].ToString();
                    if (dtReboiler_init.Select("reboilername='" + name + "'").Length == 0)
                    {
                        result = true;
                        break;
                    }
                }
                if (result)
                    return true;
            }
            else
            {
                return true;
            }

            if (this.dtHxReboiler.Rows.Count == dtHxReboiler_init.Rows.Count)
            {
                foreach (DataRow dr in dtHxReboiler.Rows)
                {
                    string name = dr["hxreboilername"].ToString();
                    if (dtHxReboiler_init.Select("hxreboilername='" + name + "'").Length == 0)
                    {
                        result = true;
                        break;
                    }
                }
                if (result)
                    return true;
            }
            else
            {
                return true;
            }

            return result;
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (loadstatus == 1)
            {
                txtName.BorderBrush = Brushes.Blue;
                txtName.BorderThickness = new Thickness(2,2,2,2);
            }
        }

        private void txtStageNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (loadstatus == 1)
            {
                txtStageNumber.BorderBrush = Brushes.Blue;
                txtStageNumber.BorderThickness = new Thickness(2,2,2,2);
            }
        }
    }
}
