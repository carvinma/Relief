using System;
using System.Collections.Generic;
using System.Data;
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

namespace ReliefAnalysis
{
    /// <summary>
    /// UC_CaseStudy.xaml 的交互逻辑
    /// </summary>
    public partial class UC_CaseStudy : UserControl
    {
        public decimal latestH = 1;
        public decimal defaultOverHeadH = 0;
        public string dbFile = string.Empty;
        public string vsdFile = string.Empty;
        public string case_id = string.Empty;
        public DataTable dtfrmcase_feed;
        public DataTable dtfrmcase_product;
        public DataTable dtfrmcase_reboiler;
        public DataTable dtfrmcase_condenser;
        public DataTable dtfrmcase;
        public bool isFirst;
        public string strReliefMW;
        public UC_CaseStudy()
        {
            InitializeComponent();
            int columnsCount = this.gridStreamIn.Columns.Count;
            gridStreamIn.Columns[columnsCount - 5].Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            columnsCount = this.gridStreamOut.Columns.Count;
            gridStreamOut.Columns[columnsCount - 5].Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            columnsCount = this.gridHeatIn.Columns.Count;
            gridHeatIn.Columns[columnsCount - 4].Width = new DataGridLength(1, DataGridLengthUnitType.Star);

            columnsCount = this.gridHeatOut.Columns.Count;
            gridHeatOut.Columns[columnsCount - 4].Width = new DataGridLength(1, DataGridLengthUnitType.Star);


            Binding.AddSourceUpdatedHandler(gridStreamIn, OnDataGridSourceUpdated);
        }
        private void chkRunreboiler_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.IsChecked == true)
            {
                btnRunreboiler.IsEnabled = true;
            }
            else
                btnRunreboiler.IsEnabled = false;
        }

        private void btnRunreboiler_Click(object sender, RoutedEventArgs e)
        {

        }


        private void chkHeatExchange_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.IsChecked == true)
            {
                btnHeatExchange.IsEnabled = true;
            }
            else
                btnHeatExchange.IsEnabled = false;
        }

        private void chkAjustment_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.IsChecked == true)
            {
                btnAjustment.IsEnabled = true;
            }
            else
                btnAjustment.IsEnabled = false;
        }

        private void btnAjustment_Click(object sender, RoutedEventArgs e)
        {

        }




        void OnDataGridSourceUpdated(object sender, DataTransferEventArgs e)
        {
            DataGridCell cell = FindVisualParent<DataGridCell>(e.TargetObject as UIElement);
            // Now you have the cell you are dealing with, so you can do what ever you want from here
            if (cell.Column.Header == "Dutylost")
            {

            }
        }

        static T FindVisualParent<T>(UIElement element) where T : UIElement
        {
            UIElement parent = element;
            while (parent != null)
            {
                T correctlyTyped = parent as T;
                if (correctlyTyped != null)
                {
                    return correctlyTyped;
                }

                parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }

            return null;
        }



        private void gridStreamIn_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

            //var editedTextbox = e.EditingElement as TextBox;
            //int r = e.Row.GetIndex();
            //DataView dv = (DataView)gridStreamIn.ItemsSource;
            //DataGridTextColumn col = e.Column as DataGridTextColumn;
            //Binding bind = col.Binding as Binding;
            //string c = bind.Path.Path;
            //string oldvalue = dv.Table.Rows[r][c].ToString();
            //string newvalue=editedTextbox.Text;

            //DataView dvout = (DataView)gridStreamOut.ItemsSource;
            //for(int i=0;i<dvout.Table.Rows.Count;i++)
            //{
            //    DataRow dr=dvout.Table.Rows[i];
            //    if (dr["FlowRate"].ToString() != "0")
            //    {
            //        decimal FlowRate =decimal.Parse(dr["FlowRate"].ToString())+decimal.Parse(newvalue)-decimal.Parse(oldvalue);
            //        dr["FlowRate"]=FlowRate;

            //        if (FlowRate > 0)
            //            break;
            //    }
            //}



        }
        public DataTable defalutStreamIn = new DataTable();
        private void btnCheckData_Click(object sender, RoutedEventArgs e)
        {
            txtReliefMW.Text=strReliefMW  ;
            decimal defalutH = 0;
            foreach (DataRow dr in defalutStreamIn.Rows)
            {
                //if (bool.Parse(dr["FlowStop"].ToString()) == false)
                //{
                    defalutH += decimal.Parse(dr["FlowCalcFactor"].ToString()) * decimal.Parse(dr["FlowRate"].ToString());
                //}
            }
            decimal editH = 0;
            DataView editStreamIn = (DataView)gridStreamIn.ItemsSource;
            foreach (DataRow dr in editStreamIn.ToTable().Rows)
            {
                if (bool.Parse(dr["FlowStop"].ToString()) == false)
                {
                    editH += decimal.Parse(dr["FlowCalcFactor"].ToString()) * decimal.Parse(dr["FlowRate"].ToString());
                }
            }
            DataView dv = (DataView)gridStreamOut.ItemsSource;
            DataTable editTable = dv.ToTable();
            if(editH == defalutH)
            {
                for(int i=0;i<editTable.Rows.Count;i++)
                {
                    DataRow dr = editTable.Rows[i];
                    dr["FlowCalcFactor"]=1;
                }
            }
            else
            {
                decimal diffH = defalutH - editH;               
                DataRow[] drs = editTable.Select("ProdType not in (3,4,6)", "SpecificEnthalpy desc");
                for (int i = 0; i < drs.Length; i++)
                {
                    DataRow dr = drs[i];
                    decimal factor = 1;//decimal.Parse(dr["FlowCalcFactor"].ToString());
                    decimal flowrate = decimal.Parse(dr["FlowRate"].ToString());
                    decimal tempH = factor * flowrate;
                    if (tempH >= diffH)
                    {
                        decimal tempfactor = (tempH - diffH) / tempH;
                        dr["FlowCalcFactor"] = tempfactor;
                        diffH = 0;
                        break;
                    }
                    else
                    {
                        dr["FlowCalcFactor"] = 0;
                        diffH = diffH - tempH;
                    }
                }
                if (diffH > 0)
                {
                    drs = editTable.Select("ProdType  in (3,4,6)", "SpecificEnthalpy desc");
                    for (int i = 0; i < drs.Length; i++)
                    {
                        DataRow dr = drs[i];
                        decimal factor = 1;// decimal.Parse(dr["FlowCalcFactor"].ToString());
                        decimal flowrate = decimal.Parse(dr["FlowRate"].ToString());
                        decimal tempH = factor * flowrate;
                        if (tempH >= diffH)
                        {
                            decimal tempfactor = (tempH - diffH) / tempH;
                            dr["FlowCalcFactor"] = tempfactor;
                            diffH = 0;
                            break;
                        }
                        else
                        {
                            diffH = diffH - tempH;
                            dr["FlowCalcFactor"] = 0;
                        }
                    }
                }
                


            }
                gridStreamOut.ItemsSource = editTable.DefaultView;


        }

        private void btnRunCalculation_Click(object sender, RoutedEventArgs e)
        {
            //不被冷凝的算法
            decimal StreamInH = 0;
            decimal StreamOutH = 0;
            decimal HeatInH = 0;
            decimal HeatOutH = 0;
            decimal overheadFlowRate = 0;
            decimal waterFlowRate = 0;
            DataView dvStreamIn = (DataView)gridStreamIn.ItemsSource;
            foreach (DataRow dr in dvStreamIn.ToTable().Rows)
            {
                if (bool.Parse(dr["FlowStop"].ToString()) == false)
                {
                    StreamInH += decimal.Parse(dr["SpecificEnthalpy"].ToString()) * decimal.Parse(dr["FlowCalcFactor"].ToString()) * decimal.Parse(dr["FlowRate"].ToString());
                }
            }

            DataView dvStreamOut = (DataView)gridStreamOut.ItemsSource;
            foreach (DataRow dr in dvStreamOut.ToTable().Rows)
            {
                StreamOutH += decimal.Parse(dr["SpecificEnthalpy"].ToString()) * decimal.Parse(dr["FlowCalcFactor"].ToString()) * decimal.Parse(dr["FlowRate"].ToString());
                if (dr["ProdType"].ToString() == "2" || dr["ProdType"].ToString() == "4")
                {
                    overheadFlowRate = decimal.Parse(dr["FlowRate"].ToString());
                }
                else if (dr["ProdType"].ToString() == "6")
                {
                    waterFlowRate = decimal.Parse(dr["FlowRate"].ToString());
                }
            }

            DataView dvHeatIn = (DataView)gridHeatIn.ItemsSource;
            foreach (DataRow dr in dvHeatIn.ToTable().Rows)
            {
                if (bool.Parse(dr["DutyLost"].ToString()) == false)
                {
                    HeatInH += decimal.Parse(dr["DutyCalcFactor"].ToString()) * decimal.Parse(dr["HeaterDuty"].ToString()) * 3600000;
                }
            }

            DataView dvHeatOut = (DataView)gridHeatOut.ItemsSource;
            foreach (DataRow dr in dvHeatOut.ToTable().Rows)
            {
                if (bool.Parse(dr["DutyLost"].ToString()) == false)
                {
                    HeatOutH += decimal.Parse(dr["DutyCalcFactor"].ToString()) * decimal.Parse(dr["HeaterDuty"].ToString()) * 3600000;
                }
            }
            decimal totalH = StreamInH - StreamOutH + HeatInH + HeatOutH;

            decimal wAccumulation = totalH / latestH + overheadFlowRate;
            decimal wRelief = wAccumulation + waterFlowRate;
            decimal tempmw = decimal.Parse(txtReliefMW.Text);
            decimal MWRelief = wRelief / (wAccumulation / tempmw + waterFlowRate / 18);
            if (wRelief < 0)
            {
                wRelief = 0;
            }
            txtReliefRate.Text = decimal.Round(wRelief,4).ToString();

            txtReliefMW.Text = decimal.Round(MWRelief, 4).ToString();
        }

        private void btnRunCalculation2_Click(object sender, RoutedEventArgs e)
        {
            //被冷凝
            decimal StreamInH = 0;
            decimal StreamOutH = 0;
            decimal HeatInH = 0;
            decimal HeatOutH = 0;
            decimal overheadH = 0;
            decimal overheadFlowRate = 0;
            DataView dvStreamIn = (DataView)gridStreamIn.ItemsSource;
            foreach (DataRow dr in dvStreamIn.ToTable().Rows)
            {
                StreamInH += decimal.Parse(dr["SpecificEnthalpy"].ToString()) * decimal.Parse(dr["FlowCalcFactor"].ToString()) * decimal.Parse(dr["FlowRate"].ToString());
            }

            DataView dvStreamOut = (DataView)gridStreamOut.ItemsSource;
            foreach (DataRow dr in dvStreamOut.ToTable().Rows)
            {
                StreamOutH += decimal.Parse(dr["SpecificEnthalpy"].ToString()) * decimal.Parse(dr["FlowCalcFactor"].ToString()) * decimal.Parse(dr["FlowRate"].ToString());
                if (dr["ProdType"].ToString() == "3")
                {
                    overheadFlowRate = decimal.Parse(dr["FlowRate"].ToString());
                    overheadH = decimal.Parse(dr["SpecificEnthalpy"].ToString()) * decimal.Parse(dr["FlowCalcFactor"].ToString());
                }
            }

            DataView dvHeatIn = (DataView)gridHeatIn.ItemsSource;
            foreach (DataRow dr in dvHeatIn.ToTable().Rows)
            {
                if (bool.Parse(dr["DutyLost"].ToString()) == false)
                {
                    HeatInH += decimal.Parse(dr["DutyCalcFactor"].ToString()) * decimal.Parse(dr["HeaterDuty"].ToString()) * 3600;
                }
            }

            DataView dvHeatOut = (DataView)gridHeatOut.ItemsSource;
            foreach (DataRow dr in dvHeatOut.ToTable().Rows)
            {
                if (bool.Parse(dr["DutyLost"].ToString()) == false)
                {
                    HeatOutH += decimal.Parse(dr["DutyCalcFactor"].ToString()) * decimal.Parse(dr["HeaterDuty"].ToString()) * 3600;
                }
            }
            decimal totalH = StreamInH - StreamOutH + HeatInH + HeatOutH;
            totalH = totalH + overheadH - defaultOverHeadH;
            decimal wAccumulation = totalH / latestH + overheadFlowRate;
            decimal wRelief = wAccumulation;
            if (wRelief < 0)
            {
                wRelief = 0;
            }
            txtReliefRate.Text = decimal.Round(wRelief,4).ToString();

        }



        private void tiStreamIn_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void TabControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            DataView dv = (DataView)gridStreamIn.ItemsSource;
            if (dv != null && dv.Table!=null)
            {
                defalutStreamIn = dv.ToTable().Copy();
            }
            strReliefMW = txtReliefMW.Text;
        }

        private void saveData()
        {
            DBRelief dbWrite = new DBRelief(dbFile);



            if (gridStreamIn.ItemsSource != null)
            {
                DataView dvFeed = (DataView)gridStreamIn.ItemsSource;
                dtfrmcase_feed.Merge(dvFeed.Table);

                DataView dvProduct = (DataView)gridStreamOut.ItemsSource;
                dtfrmcase_product.Merge(dvProduct.Table);

                DataView dvReboiler = (DataView)gridHeatIn.ItemsSource;
                dtfrmcase_reboiler.Merge(dvReboiler.Table);

                DataView dvCondenser = (DataView)gridHeatOut.ItemsSource;
                dtfrmcase_condenser.Merge(dvCondenser.Table);

                DataRow dr = dtfrmcase.NewRow();
                dr["case_id"] = case_id;
                dr["visiofile"] = vsdFile;
                dr["relieftemp"] = txtReliefTemp.Text;
                dr["reliefpress"] = txtReliefPress.Text;
                dr["reliefrate"] = txtReliefRate.Text;
                dr["reliefmw"] = txtReliefMW.Text;
                dtfrmcase.Rows.Add(dr);
            }

            dbWrite.saveDataByTable(dtfrmcase, vsdFile);
            dbWrite.saveDataByTable(dtfrmcase_feed, vsdFile);
            dbWrite.saveDataByTable(dtfrmcase_product, vsdFile);
            dbWrite.saveDataByTable(dtfrmcase_reboiler, vsdFile);
            dbWrite.saveDataByTable(dtfrmcase_condenser, vsdFile);
        }

        private void DataGrid_CellGotFocus(object sender, RoutedEventArgs e)
        {
            // Lookup for the source to be DataGridCell
            if (e.OriginalSource.GetType() == typeof(DataGridCell))
            {
                // Starts the Edit on the row;
                DataGrid grd = (DataGrid)sender;
                grd.BeginEdit(e);

                Control control = GetFirstChildByType<Control>(e.OriginalSource as DataGridCell);
                if (control != null)
                {
                    control.Focus();
                }
            }
        }

        private T GetFirstChildByType<T>(DependencyObject prop) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(prop); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild((prop), i) as DependencyObject;
                if (child == null)
                    continue;

                T castedProp = child as T;
                if (castedProp != null)
                    return castedProp;

                castedProp = GetFirstChildByType<T>(child);

                if (castedProp != null)
                    return castedProp;
            }
            return null;
        }
    }

}
