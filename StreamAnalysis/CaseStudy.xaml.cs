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
using System.Threading;
using System.IO;
using Microsoft.Office.Interop;
using Microsoft.CSharp;
using System.ComponentModel;

namespace ReliefAnalysis
{
    /// <summary>
    /// CaseStudy.xaml 的交互逻辑
    /// </summary>
    public partial class CaseStudy : Window
    {
        public CaseStudy()
        {
            InitializeComponent();
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
        }
        public string dbFile = string.Empty;
        public string eqName = string.Empty;
        public List<int> dispCase = new List<int>();
        public DataTable dtStreamIn = new DataTable();
        public DataTable dtStreamOut = new DataTable();
        public DataTable dtHeatIn = new DataTable();
        public DataTable dtHeatOut = new DataTable();

        public DataTable dtfrmbasecase_feed = new DataTable();
        public DataTable dtfrmbasecase_product = new DataTable();
        public DataTable dtfrmbasecase_reboiler = new DataTable();
        public DataTable dtfrmbasecase_condenser = new DataTable();
        public DataTable dtfrmbasecase = new DataTable();

        public DataTable dtfrmcase = new DataTable();
        public DataTable dtfrmcase_feed = new DataTable();
        public DataTable dtfrmcase_product = new DataTable();
        public DataTable dtfrmcase_reboiler = new DataTable();
        public DataTable dtfrmcase_condenser = new DataTable();

        public double Prelief = 0;
        public string przFile;
        public string vsdFile;
        Dictionary<int, UC_CaseStudy> dicCase = new Dictionary<int, UC_CaseStudy>();
        Dictionary<int, int> dicCaseIndex = new Dictionary<int, int>();
        BackgroundWorker backgroundWorker = new BackgroundWorker();
        private void initDicCaseIndex()
        {
            dicCaseIndex.Add(1, 1);
            dicCaseIndex.Add(2, 2);
            dicCaseIndex.Add(3, 3);
            dicCaseIndex.Add(4, 9);
            dicCaseIndex.Add(5, 10);
            dicCaseIndex.Add(6, 11);
            dicCaseIndex.Add(7, 12);
            dicCaseIndex.Add(8, 13);
            dicCaseIndex.Add(9, 14);
            dicCaseIndex.Add(10, 15);
            dicCaseIndex.Add(11, 16);
            dicCaseIndex.Add(12, 17);
            dicCaseIndex.Add(13, 18);
            dicCaseIndex.Add(14, 4);
            dicCaseIndex.Add(15, 5);
            dicCaseIndex.Add(16, 6);
            dicCaseIndex.Add(17, 7);
            dicCaseIndex.Add(18, 8);
        }
        private void initDicCase()
        {
            dicCase.Add(1, uc1);
            dicCase.Add(2, uc2);
            dicCase.Add(3, uc3);
            dicCase.Add(4, uc4);
            dicCase.Add(5, uc5);
            dicCase.Add(6, uc6);
            dicCase.Add(7, uc7);
            dicCase.Add(8, uc8);
            dicCase.Add(9, uc9);
            dicCase.Add(10, uc10);
            dicCase.Add(11, uc11);
            dicCase.Add(12, uc12);
            dicCase.Add(14, uc14);
            dicCase.Add(15, uc15);
            dicCase.Add(16, uc16);
            dicCase.Add(17, uc17);
            dicCase.Add(18, uc18);
        }
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            dbF = new DBRelief(dbFile);
            initDicCaseIndex();
            initDicCase();

            InitTabBaseCase();



            for (int i = 1; i < mainTab.Items.Count; i++)
            {
                TabItem ti = (TabItem)mainTab.Items[i];
                ti.Visibility = Visibility.Collapsed;
            }

            foreach (int i in dispCase)
            {
                TabItem ti = (TabItem)FindName("ti" + i.ToString());
                ti.Visibility = Visibility.Visible;
                if (i == 13)
                {
                    uc13.dbFile = dbFile;
                    uc13.vsdFile = vsdFile;
                }
            }
            

        }

        private void clearData()
        {
            dtfrmbasecase_feed.Rows.Clear();
            dtfrmbasecase_product.Rows.Clear();
            dtfrmbasecase_reboiler.Rows.Clear();
            dtfrmbasecase_condenser.Rows.Clear();
            dtfrmbasecase.Rows.Clear();

            dtfrmcase.Rows.Clear();
            dtfrmcase_feed.Rows.Clear();
            dtfrmcase_product.Rows.Clear();
            dtfrmcase_reboiler.Rows.Clear();
            dtfrmcase_condenser.Rows.Clear();

        }
        private void InitTabBaseCase()
        {
            clearData();
            DataSet dsStreamIn = new DataSet();
            DataSet dsStreamOut = new DataSet();
            DataSet dsHeat = new DataSet();
            DBRelief dbReader = new DBRelief(dbFile);
            string[] arrColumns = { "id", "visiofile", "case_id" };
            dtfrmbasecase_feed = dbReader.getDataByVsdFile("frmfeed", vsdFile);
            dtfrmbasecase_product = dbReader.getDataByVsdFile("frmproduct", vsdFile);
            dtfrmbasecase_reboiler = dbReader.getDataByVsdFile("frmreboiler", vsdFile);
            dtfrmbasecase_condenser = dbReader.getDataByVsdFile("frmcondenser", vsdFile);
            dtfrmbasecase = dbReader.getDataByVsdFile("frmbasecase", vsdFile);
            dtFlashResult = dbReader.getDataByVsdFile("flashresult", vsdFile);

            DataTable dtfrmtower = dbReader.getDataByVsdFile("frmtower", vsdFile);
            if (dtfrmtower.Rows.Count > 0)
            {
                string path = System.IO.Path.GetDirectoryName(dbFile);
                przFile = path + @"\" + dtfrmtower.Rows[0]["przfile"].ToString();
            }

            DataTable dtfrmpsv = dbReader.getDataByVsdFile("frmpsv", vsdFile);
            if (dtfrmpsv.Rows.Count > 0)
            {
                DataRow dr = dtfrmpsv.Rows[0];
                Prelief = double.Parse(dr["pressure"].ToString()) * double.Parse(dr["reliefmultiple"].ToString());
            }
            if (dtfrmbasecase.Rows.Count > 0)
            {
                DataRow dr = dtfrmbasecase.Rows[0];
                txtDescription.Text = dr["Description"].ToString();
                txtRunResult.Text = dr["latentheat"].ToString();
                strTray1Pressure = dr["Tray1Pressure"].ToString();
                vapor = dr["vapor"].ToString();
                liquid = dr["liquid"].ToString();
                tempdir = dr["dir"].ToString();
                tempTemperature = dr["Temperature"].ToString();
            }


            gridStreamIn.ItemsSource = dtfrmbasecase_feed.DefaultView;
            gridStreamOut.ItemsSource = dtfrmbasecase_product.DefaultView;
            gridHeatIn.ItemsSource = dtfrmbasecase_reboiler.DefaultView;
            gridHeatOut.ItemsSource = dtfrmbasecase_condenser.DefaultView;

            DataTable dtcases = getCases();
            checkDictionarySource(dtcases);
            checkDictionaryCondenser(dtcases);
            checkDictionaryReboiler(dtcases);

            dtfrmcase = dbReader.getDataByVsdFile("frmcase", vsdFile);
            dtfrmcase_feed = dbReader.getDataByVsdFile("frmcase_feed", vsdFile);
            dtfrmcase_product = dbReader.getDataByVsdFile("frmcase_product", vsdFile);
            dtfrmcase_reboiler = dbReader.getDataByVsdFile("frmcase_reboiler", vsdFile);
            dtfrmcase_condenser = dbReader.getDataByVsdFile("frmcase_condenser", vsdFile);

            foreach (DataRow drcase in dtfrmcase.Rows)
            {
                int caseid = int.Parse(drcase["case_id"].ToString());
                int i = dicCaseIndex[caseid];
                TabItem ti = (TabItem)mainTab.Items[i];
                string ucName = "uc" + ti.Name.Substring(2);
                object uc = ti.FindName(ucName);
                if (uc is UC_CaseStudy)
                {
                    UC_CaseStudy uccase = (UC_CaseStudy)uc;
                    DataView dv = new DataView(dtfrmcase_feed);
                    dv.RowFilter = "case_id=" + caseid.ToString() + " and visiofile='" + vsdFile + "'";
                    uccase.gridStreamIn.ItemsSource = dv;

                    dv = new DataView(dtfrmcase_product);
                    dv.RowFilter = "case_id=" + caseid.ToString() + " and visiofile='" + vsdFile + "'";
                    uccase.gridStreamOut.ItemsSource = dv;

                    dv = new DataView(dtfrmcase_reboiler);
                    dv.RowFilter = "case_id=" + caseid.ToString() + " and visiofile='" + vsdFile + "'";
                    uccase.gridHeatIn.ItemsSource = dv;

                    dv = new DataView(dtfrmcase_condenser);
                    dv.RowFilter = "case_id=" + caseid.ToString() + " and visiofile='" + vsdFile + "'";
                    uccase.gridHeatOut.ItemsSource = dv;


                    DataRow[] drs = dtfrmcase.Select("case_id=" + caseid.ToString() + " and visiofile='" + vsdFile + "'");
                    uccase.txtDescription.Text = drs[0]["Description"].ToString();
                    uccase.txtReliefMW.Text = drs[0]["ReliefMW"].ToString();
                    uccase.txtReliefPress.Text = drs[0]["ReliefPress"].ToString();
                    uccase.txtReliefRate.Text = drs[0]["reliefrate"].ToString();
                    uccase.txtReliefTemp.Text = drs[0]["ReliefTemp"].ToString();
                    ti.Visibility = Visibility.Visible;
                    uccase.latestH = decimal.Round(decimal.Parse(txtRunResult.Text), 4);
                }
            }
            //chkGeneralElectricalPowerFailure();


        }

        string tray1_s = string.Empty;
        string tray1_f;
        DataTable dtFlashResult = new DataTable();
        string ReliefPress = string.Empty;
        string ReliefTemp = string.Empty;
        string ReliefMW = string.Empty;
        string Compressibility = string.Empty;
        string CpCv = string.Empty;

        int op = 1;
        private void btnRunCalculation_Click(object sender, RoutedEventArgs e)
        {
            op = 2;
            progressBar.Visibility = Visibility.Visible;
            backgroundWorker.RunWorkerAsync();
        }


        string strTray1Pressure = string.Empty;

        string vapor = string.Empty;
        string liquid = string.Empty;
        string tempdir = string.Empty;
        string tempTemperature = string.Empty;
        private void btnCheckData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Prelief > 0)
                {
                    progressBar.Visibility = Visibility.Visible;
                    btnCheckData.IsEnabled = false;
                    op = 1;
                    backgroundWorker.RunWorkerAsync();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            int idx = dbFile.LastIndexOf(@"\");
             Microsoft.Win32.SaveFileDialog dlgSaveDiagram = new Microsoft.Win32.SaveFileDialog();
            dlgSaveDiagram.Filter = "Excel xlsx |*.xlsx;";
            dlgSaveDiagram.Title = "Export Report";
            if (dlgSaveDiagram.ShowDialog() == true)
            {
                string filePath = dlgSaveDiagram.FileName;
                string vsd = AppDomain.CurrentDomain.BaseDirectory.ToString() + "SimTech-PRV_DataSheet_Model.xlsx";
                System.IO.File.Copy(vsd, filePath);
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook xlWorkBook = xlApp.Workbooks.Open(filePath, Type.Missing, false, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Sheets.get_Item(1);
                xlWorkSheet.Cells[5][15] = "test";
                int count = 0;
                int row1 = 14;
                int note1 = 42;
                int row2 = 74;
                int note2 = 102;
                int row3 = 134;
                int note3 = 162;

                for (int i = 1; i < mainTab.Items.Count; i++)
                {
                    TabItem ti = (TabItem)mainTab.Items[i];
                    if (ti.Visibility == Visibility.Visible)
                    {
                        string num = ti.Name.Remove(0, 2);
                        UC_CaseStudy uc = (UC_CaseStudy)ti.FindName("uc" + num);
                        int col = count % 5;
                        if (count <= 4)
                        {
                            xlWorkSheet.Cells[5 + col * 2][row1] = ti.Header.ToString();
                            xlWorkSheet.Cells[5 + col * 2][row1 + 1] = 16;
                            if (ti.Header.ToString().Contains("Fire"))
                            {
                                xlWorkSheet.Cells[5 + col * 2][row1 + 1] = 21;
                            }
                            xlWorkSheet.Cells[5 + col * 2][row1 + 2] = uc.txtReliefPress.Text;
                            xlWorkSheet.Cells[5 + col * 2][row1 + 3] = uc.txtReliefTemp.Text;
                            xlWorkSheet.Cells[5 + col * 2][row1 + 4] = uc.txtReliefRate.Text;
                            xlWorkSheet.Cells[5 + col * 2][row1 + 5] = uc.txtReliefMW.Text;
                            xlWorkSheet.Cells[5 + col * 2][row1 + 6] = Compressibility;
                            xlWorkSheet.Cells[5 + col * 2][row1 + 7] = CpCv;
                            xlWorkSheet.Cells[3][note1 + count] = uc.txtDescription.Text;
                            xlWorkSheet.Cells[3][note2 + count] = uc.txtDescription.Text;
                            xlWorkSheet.Cells[3][note3 + count] = uc.txtDescription.Text;
                        }
                        else if (count >= 5 && count <= 9)
                        {
                            xlWorkSheet.Cells[5 + col * 2][row2] = ti.Header.ToString();
                            xlWorkSheet.Cells[5 + col * 2][row2 + 1] = 16;
                            if (ti.Header.ToString().Contains("Fire"))
                            {
                                xlWorkSheet.Cells[5 + col * 2][row2 + 1] = 21;
                            }
                            xlWorkSheet.Cells[5 + col * 2][row2 + 2] = uc.txtReliefPress.Text;
                            xlWorkSheet.Cells[5 + col * 2][row2 + 3] = uc.txtReliefTemp.Text;
                            xlWorkSheet.Cells[5 + col * 2][row2 + 4] = uc.txtReliefRate.Text;
                            xlWorkSheet.Cells[5 + col * 2][row2 + 5] = uc.txtReliefMW.Text;
                            xlWorkSheet.Cells[5 + col * 2][row2 + 6] = Compressibility;
                            xlWorkSheet.Cells[5 + col * 2][row2 + 7] = CpCv;
                            xlWorkSheet.Cells[3][note1 + count] = uc.txtDescription.Text;
                            xlWorkSheet.Cells[3][note2 + count] = uc.txtDescription.Text;
                            xlWorkSheet.Cells[3][note3 + count] = uc.txtDescription.Text;
                        }
                        else
                        {
                            xlWorkSheet.Cells[5 + col * 2][row3] = ti.Header.ToString();
                            xlWorkSheet.Cells[5 + col * 2][row3 + 1] = 16;
                            if (ti.Header.ToString().Contains("Fire"))
                            {
                                xlWorkSheet.Cells[5 + col * 2][row3 + 1] = 21;
                            }
                            xlWorkSheet.Cells[5 + col * 2][row3 + 2] = uc.txtReliefPress.Text;
                            xlWorkSheet.Cells[5 + col * 2][row3 + 3] = uc.txtReliefTemp.Text;
                            xlWorkSheet.Cells[5 + col * 2][row3 + 4] = uc.txtReliefRate.Text;
                            xlWorkSheet.Cells[5 + col * 2][row3 + 5] = uc.txtReliefMW.Text;
                            xlWorkSheet.Cells[5 + col * 2][row3 + 6] = Compressibility;
                            xlWorkSheet.Cells[5 + col * 2][row3 + 7] = CpCv;
                            xlWorkSheet.Cells[3][note1 + count] = uc.txtDescription.Text;
                            xlWorkSheet.Cells[3][note2 + count] = uc.txtDescription.Text;
                            xlWorkSheet.Cells[3][note3 + count] = uc.txtDescription.Text;
                        }

                        count++;
                    }
                }
                if (count <= 5)
                {
                    Microsoft.Office.Interop.Excel.Range r = xlWorkSheet.Range[xlWorkSheet.Cells[2][122], xlWorkSheet.Cells[3][181]];
                    r.UnMerge();
                    r = xlWorkSheet.Range[xlWorkSheet.Cells[2][122], xlWorkSheet.Cells[14][181]];
                    r.Clear();

                    Microsoft.Office.Interop.Excel.Shape pic = xlWorkSheet.Shapes.Item(3) as Microsoft.Office.Interop.Excel.Shape;
                    pic.Delete();

                    r = xlWorkSheet.Range[xlWorkSheet.Cells[2][62], xlWorkSheet.Cells[3][121]];
                    r.UnMerge();
                    r = xlWorkSheet.Range[xlWorkSheet.Cells[2][62], xlWorkSheet.Cells[14][121]];
                    r.Clear();

                    pic = xlWorkSheet.Shapes.Item(2) as Microsoft.Office.Interop.Excel.Shape;
                    pic.Delete();


                }
                else if (count <= 10)
                {
                    Microsoft.Office.Interop.Excel.Range r = xlWorkSheet.Range[xlWorkSheet.Cells[2][122], xlWorkSheet.Cells[3][181]];
                    r.UnMerge();
                    r = xlWorkSheet.Range[xlWorkSheet.Cells[2][122], xlWorkSheet.Cells[14][181]];
                    r.Clear();

                    Microsoft.Office.Interop.Excel.Shape pic = xlWorkSheet.Shapes.Item(3) as Microsoft.Office.Interop.Excel.Shape;
                    pic.Delete();
                }

                xlWorkBook.Save();
                xlWorkBook.Close(true, Type.Missing, Type.Missing);
                xlApp.Quit();


                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);
            }
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Unable to release the Object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }


        private void btnLoadData_Click(object sender, RoutedEventArgs e)
        {
            op = 3;
            progressBar.Visibility = Visibility.Visible;
            backgroundWorker.RunWorkerAsync();

        }


        private void MetroWindow_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                e.Cancel = true;
                MessageBox.Show("Data is loading...", "Tip");
                return;
            }
            saveDataToDB();
            
        }


        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (op == 1)
            {
                checkData();
            }
            else if (op == 2)
            {
                runCalculation();
            }
            else if (op == 3)
            {
                loadData();
            }

            backgroundWorker.ReportProgress(100);

        }
        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }
        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (op == 1)
            {
                this.txtRunResult.Text = decimal.Round(decimal.Parse((double.Parse(vaporH[1]) - double.Parse(liqidH[1])).ToString()), 4).ToString();
                ReliefPress = decimal.Round(decimal.Parse(vaporH[4]), 4).ToString();
                ReliefTemp = decimal.Round(decimal.Parse(vaporH[3]), 4).ToString();
                ReliefMW = decimal.Round(decimal.Parse(vaporH[6]), 4).ToString();
                progressBar.Visibility = Visibility.Hidden;
                progressBar.Value = 0;
                btnCheckData.IsEnabled = true;
            }
            else if (op == 2)
            {
                progressBar.Visibility = Visibility.Hidden;
                progressBar.Value = 0;
            }
            else if (op == 3)
            {
                progressBar.Visibility = Visibility.Hidden;
                progressBar.Value = 0;
                bindUCData();
            }
        }

        private void checkData()
        {
            tempdir =  System.IO.Path.GetDirectoryName(dbFile)+ @"\temp\" + Guid.NewGuid().ToString();
            if (!Directory.Exists(tempdir))
                Directory.CreateDirectory(tempdir);
            backgroundWorker.ReportProgress(5);
            PRZWriter w = new PRZWriter();
            DataTable dt = w.copyStream(przFile, eqName);
            backgroundWorker.ReportProgress(15);
            FlashCompute flashc = new FlashCompute();
            strTray1Pressure = dt.Rows[0]["pressure"].ToString();
            if (strTray1Pressure == "")
            {
                strTray1Pressure = "0";
            }
            strTray1Pressure = UnitConverter.unitConv(strTray1Pressure, "KPA", "MPAG", "{0:0.0000}");
            backgroundWorker.ReportProgress(25);
            string strTray1Temperature = dt.Rows[0]["Temperature"].ToString();
            if (strTray1Temperature == "")
            {
                strTray1Temperature = "0";
            }
            strTray1Temperature = UnitConverter.unitConv(strTray1Temperature, "K", "C", "{0:0.0000}");


            tray1_s = dt.Rows[0]["streamname"].ToString().ToUpper();
            string gd = Guid.NewGuid().ToString();
            vapor = "S_" + gd.Substring(0, 5).ToUpper();
            liquid = "S_" + gd.Substring(gd.Length - 5, 5).ToUpper();
            backgroundWorker.ReportProgress(30);
            string content = InpReader.getUsableContent(przFile, dt.Rows[0]["streamname"].ToString(),tempdir);
            backgroundWorker.ReportProgress(40);
            tray1_f = flashc.compute(content, 1, Prelief.ToString(), 3, "", dt.Rows[0], vapor, liquid, tempdir);
            backgroundWorker.ReportProgress(60);
            PRZReader reader = new PRZReader(tray1_f);
            DataSet ds = reader.getDataFromFile();
            while (ds.Tables[0].Rows[0]["temperature"].ToString() == "")
            {
                ds = reader.getDataFromFile();
            }
            backgroundWorker.ReportProgress(80);
            DBRelief dbreader = new DBRelief();
            feedH = dbreader.computeH(ds.Tables[0], tray1_s.ToUpper());
            vaporH = dbreader.computeH(ds.Tables[0], vapor);
            liqidH = dbreader.computeH(ds.Tables[0], liquid);
            tempTemperature = vaporH[3];

            backgroundWorker.ReportProgress(100);
        }
        string[] feedH = new string[7];
        string[] vaporH = new string[7];
        string[] liqidH = new string[7];

        DataTable dtfrmcase_feed_single = new DataTable();
        DataTable dtfrmcase_product_single = new DataTable();
        private void loadData()
        {
            DataSet ds = new DataSet();
            DBRelief dbreader = new DBRelief();

            dtfrmcase_feed_single = dtfrmcase_feed.Clone();
            dtfrmcase_product_single = dtfrmcase_product.Clone();
            foreach (DataRow dr in dtfrmbasecase_feed.Rows)
            {
                DataRow drIn = dtfrmcase_feed_single.NewRow();
                drIn["SpecificEnthalpy"] = dr["SpEnthalpy"];
                drIn["FlowRate"] = dr["weightflow"];
                drIn["StreamName"] = dr["StreamName"];
                drIn["visiofile"] = vsdFile;
                dtfrmcase_feed_single.Rows.Add(drIn);
            }

            int count = dtFlashResult.Rows.Count;
            for (int i = 1; i <= count; i++)
            {
                DataRow dr = dtFlashResult.Rows[i - 1];
                string prodtype = dr["prodtype"].ToString();
                string tray = dr["tray"].ToString();
                if (dr["przfile"].ToString() != "")
                {
                    PRZReader przReader = new PRZReader(dr["przfile"].ToString());
                    ds = przReader.getDataFromFile();
                    if (ds != null)
                    {
                        DataRow drout = dtfrmcase_product_single.NewRow();
                        while (ds.Tables[0].Rows[0]["temperature"].ToString() == "")
                        {
                            Thread.Sleep(10000);
                            ds = przReader.getDataFromFile();
                        }
                        dbreader = new DBRelief();
                        if (prodtype == "4" || (prodtype == "2" && tray == "1") || prodtype == "3" || prodtype == "6")
                        {
                            string[] liquidH2 = dbreader.computeH(ds.Tables[0], dr["vapor"].ToString().ToUpper());
                            drout["SpecificEnthalpy"] = liquidH2[1];
                        }
                        else
                        {
                            string[] liquidH2 = dbreader.computeH(ds.Tables[0], dr["liquid"].ToString().ToUpper());
                            drout["SpecificEnthalpy"] = liquidH2[1];
                        }


                        drout["StreamName"] = dr["stream"].ToString().ToUpper();
                        drout["FlowRate"] = 0;
                        foreach (DataRow dro in dtStreamOut.Rows)
                        {
                            if (drout["streamname"].ToString() == dro["streamname"].ToString())
                            {
                                drout["FlowRate"] = dro["weightflow"].ToString();
                                drout["ProdType"] = dro["ProdType"].ToString();
                            }
                        }
                        drout["tray"] = tray;
                        drout["visiofile"] = vsdFile;
                        dtfrmcase_product_single.Rows.Add(drout);
                        int percents = (i * 100) / count;
                        backgroundWorker.ReportProgress(percents, i);
                    }
                }
            }
            

        }

        private void bindUCData()
        {
            
            for (int i = 1; i < mainTab.Items.Count; i++)
            {
                TabItem ti = (TabItem)mainTab.Items[i];
                if (ti.Visibility == Visibility.Visible)
                {
                    string caseid = ti.Name.ToString().Substring(2);
                    if (caseid != "13")
                    {
                        UC_CaseStudy uc = (UC_CaseStudy)ti.FindName("uc" + caseid.ToString());
                        DataTable feed = dtfrmcase_feed_single.Copy();
                        foreach (DataRow dr in feed.Rows)
                        {
                            dr["case_id"] = caseid;
                            dr["flowstop"] = false;
                            dr["flowcalcfactor"] = 1;

                        }
                        uc.gridStreamIn.ItemsSource = feed.DefaultView;

                        DataTable product = dtfrmcase_product_single.Copy();
                        foreach (DataRow dr in product.Rows)
                        {
                            dr["case_id"] = caseid;
                            dr["flowstop"] = false;
                            dr["flowcalcfactor"] = 1;
                        }
                        uc.gridStreamOut.ItemsSource = product.DefaultView;

                        DataTable reboiler = dtfrmcase_reboiler.Clone();
                        foreach (DataRow dr in dtfrmbasecase_reboiler.Rows)
                        {
                            DataRow r = reboiler.NewRow();
                            r["case_id"] = caseid;
                            r["visiofile"] = vsdFile;
                            r["heatername"] = dr["heatername"].ToString();
                            r["heaterduty"] = dr["heaterduty"].ToString();
                            r["dutylost"] = false;
                            r["dutycalcfactor"] = 1;
                            reboiler.Rows.Add(r);
                        }
                        uc.gridHeatIn.ItemsSource = reboiler.DefaultView;

                        DataTable condenser = dtfrmcase_condenser.Clone();
                        foreach (DataRow dr in dtfrmbasecase_condenser.Rows)
                        {
                            DataRow r = condenser.NewRow();
                            r["case_id"] = caseid;
                            r["visiofile"] = vsdFile;
                            r["heatername"] = dr["heatername"].ToString();
                            r["heaterduty"] = dr["heaterduty"].ToString();
                            r["dutylost"] = false;
                            r["dutycalcfactor"] = 1;
                            condenser.Rows.Add(r);
                        }
                        uc.gridHeatOut.ItemsSource = condenser.DefaultView;

                        uc.latestH = decimal.Round(decimal.Parse(txtRunResult.Text), 4);
                        DataRow[] drs = dtStreamOut.Select("ProdType='3' or ProdType='4'");
                        uc.defaultOverHeadH = decimal.Round(decimal.Parse(drs[0]["enthalpy"].ToString()) * 4180000, 4);
                        uc.txtReliefPress.Text = ReliefPress;
                        uc.txtReliefTemp.Text = ReliefTemp;
                        uc.txtReliefMW.Text = ReliefMW;
                    }
                    
                }
                if (ti13.Visibility == Visibility.Visible)
                {
                    uc4.txtReliefPress.Text = ReliefPress;
                    uc4.txtReliefTemp.Text = ReliefTemp;
                    uc4.txtReliefMW.Text = ReliefMW;
                }
            }
            saveDataToDB();
            checkDictionary();
            for (int i = 1; i < mainTab.Items.Count; i++)
            {
                TabItem ti = (TabItem)mainTab.Items[i];
                if (ti.Visibility == Visibility.Visible)
                {
                    string caseid = ti.Name.ToString().Substring(2);
                    if (caseid != "13")
                    {
                        UC_CaseStudy uc = (UC_CaseStudy)ti.FindName("uc" + caseid.ToString());
                        uc.gridStreamIn.ItemsSource = dbF.getDataByTable("frmcase_feed", "visiofile='" + vsdFile + "' and case_id=" + caseid).DefaultView;
                        uc.gridStreamOut.ItemsSource = dbF.getDataByTable("frmcase_product", "visiofile='" + vsdFile + "' and case_id=" + caseid).DefaultView;
                        uc.gridHeatIn.ItemsSource = dbF.getDataByTable("frmcase_reboiler", "visiofile='" + vsdFile + "' and case_id=" + caseid).DefaultView;
                        uc.gridHeatOut.ItemsSource = dbF.getDataByTable("frmcase_condenser", "visiofile='" + vsdFile + "' and case_id=" + caseid).DefaultView;
                    }
                }
            }




        }
        private void runCalculation()
        {
            if (strTray1Pressure != string.Empty)
            {
                DataView dvStreamOut = (DataView)gridStreamOut.ItemsSource;
                dtStreamOut = dvStreamOut.Table;
                dtFlashResult.Rows.Clear();
                int count = dtStreamOut.Rows.Count;
                for (int i = 1; i <= count; i++)
                {
                    DataRow dr = dtStreamOut.Rows[i - 1];
                    if (dr["TotalMolarRate"].ToString() != "0")
                    {
                        FlashCompute fc = new FlashCompute();
                        string l = string.Empty;
                        string v = string.Empty;
                        string prodtype = dr["prodtype"].ToString();
                        string tray = dr["tray"].ToString();
                        string streamname = dr["streamname"].ToString();
                        string strPressure = dr["pressure"].ToString();
                        string f = string.Empty;

                        double pressure = 0;
                        if (strPressure != "")
                        {
                            pressure = double.Parse(strPressure);
                        }
                        string content = InpReader.getUsableContent(przFile, dr["streamname"].ToString(),tempdir);
                        int percents = (int)((i - 0.4) * 100) / count;
                        backgroundWorker.ReportProgress(percents, i);
                        if (prodtype == "4" || (prodtype == "2" && tray == "1"))
                        {
                            f = fc.compute(content, 1, Prelief.ToString(), 3, "", dr, vapor, liquid, tempdir);
                        }

                        else if (prodtype == "6" || prodtype == "3")
                        {
                            f = fc.compute(content, 2, tempTemperature, 3, "", dr, vapor, liquid, tempdir);
                        }
                        else
                        {
                            double p = Prelief + (double.Parse(dr["pressure"].ToString()) - double.Parse(strTray1Pressure));
                            f = fc.compute(content, 1, p.ToString(), 4, "", dr, vapor, liquid, tempdir);
                        }
                        DataRow drFlash = dtFlashResult.NewRow();
                        drFlash["przfile"] = f;
                        drFlash["liquid"] = liquid;
                        drFlash["vapor"] = vapor;
                        drFlash["stream"] = streamname;
                        drFlash["prodtype"] = prodtype;
                        drFlash["tray"] = tray;
                        drFlash["visiofile"] = vsdFile;
                        dtFlashResult.Rows.Add(drFlash);

                        percents = (i * 100) / count;
                        backgroundWorker.ReportProgress(percents, i);
                    }
                }
            }
        }

        //判断是否是电力启动
        private bool isMotor(string streamname)
        {
            DBRelief dbReader = new DBRelief(dbFile); ;
            DataTable dt = dbReader.getDataByVsdFile("frmSource", vsdFile, "streamname='" + streamname + "' and sourcetype like '%%Motor%%'");
            if (dt.Rows.Count == 0)
                return false;
            else
                return true;

        }

        private void chkGeneralElectricalPowerFailure()
        {
            try
            {
                DataView dvFeed = (DataView)uc4.gridStreamIn.ItemsSource;
                if (dvFeed != null)
                {
                    DataTable dtFeed = dvFeed.ToTable();
                    int count = dtFeed.Rows.Count;
                    for (int i = 0; i < count; i++)
                    {
                        DataRow dr = dtFeed.Rows[i];
                        if (isMotor(dr["streamname"].ToString()))
                        {
                            dr["flowstop"] = true;
                        }
                        else
                        {
                            dr["flowstop"] = false;
                        }
                    }
                    uc4.gridStreamIn.ItemsSource = dtFeed.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void chkPumparoundFailure()
        {
            try
            {
                DataView dvFeed = (DataView)uc4.gridStreamIn.ItemsSource;
                if (dvFeed != null)
                {
                    DataTable dtFeed = dvFeed.ToTable();
                    int count = dtFeed.Rows.Count;
                    for (int i = 0; i < count; i++)
                    {
                        DataRow dr = dtFeed.Rows[i];
                        if (isMotor(dr["streamname"].ToString()))
                        {
                            dr["flowstop"] = true;
                        }
                    }
                    uc4.gridStreamIn.ItemsSource = dtFeed.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        DBRelief dbF = new DBRelief();
        DBRelief db=new DBRelief();
        private void checkDictionarySource(DataTable dtCase)
        {
            
            DataTable dtSource= dbF.getDataByTable("frmsource","visiofile='"+vsdFile+"'");

            foreach (DataRow drSource in dtSource.Rows)
            {
                checkSource(drSource, dtCase);
            }

            


        }

        private void checkSource(DataRow drSource,DataTable dtCase)
        {           
                string streamName = drSource["streamname"].ToString();
                string sourceType = drSource["sourcetype"].ToString();
                DataTable dtDic_Source = db.getDataByTable("dictionarysource", "categoryvalue='" + sourceType + "'");
                DataRow dr = dtDic_Source.Rows[0];

                foreach (DataRow drCase in dtCase.Rows)
                {
                    string caseid = drCase["case_id"].ToString();
                    string caseName = drCase["casename"].ToString();
                    string sql = "update frmcase_feed set flowstop=0 where streamname='" + streamName + "' and case_id=" + caseid;
                    if (dr[caseName].ToString() == "0")
                    {
                        sql = "update frmcase_feed set flowstop=1 where streamname='" + streamName + "' and case_id=" + caseid;
                    }
                    dbF.saveDataBySQL(sql);
                }
        }

        private void checkCondenser(DataRow drCondenser,DataTable dtDic_Condenser ,DataTable dtCase)
        {
            string heaterName = drCondenser["heatername"].ToString();
            string heaterduty = drCondenser["heaterduty"].ToString();
            string WaterFactor = drCondenser["waterfactor"].ToString();
            string AirFactor = drCondenser["airfactor"].ToString();
            string WetAirFactor = drCondenser["wetairfactor"].ToString();
            string PumpAboundFactor = drCondenser["pumpaboundfactor"].ToString();
   
            foreach (DataRow drCase in dtCase.Rows)
            {
                string caseid = drCase["case_id"].ToString();
                string caseName = drCase["casename"].ToString();
                DataRow[] drs=dtDic_Condenser.Select("categoryvalue='water'");
                string water = drs[0][caseName].ToString();
                drs=dtDic_Condenser.Select("categoryvalue='air'");
                string air = dtDic_Condenser.Rows[1][caseName].ToString();
                drs=dtDic_Condenser.Select("categoryvalue='wetair'");
                string wetair =dtDic_Condenser.Rows[2][caseName].ToString();
                drs=dtDic_Condenser.Select("categoryvalue='pumpabound'");
                string pumpabound = dtDic_Condenser.Rows[3][caseName].ToString();
                decimal dutycalfactor = decimal.Parse(WaterFactor) * decimal.Parse(water) + decimal.Parse(AirFactor) * decimal.Parse(air) + decimal.Parse(WetAirFactor) * decimal.Parse(wetair) + decimal.Parse(PumpAboundFactor) * decimal.Parse(pumpabound);
                string sql = "update frmcase_condenser set heaterduty='"+heaterduty+"', dutycalcfactor='"+dutycalfactor.ToString()+"' where heatername='" + heaterName + "' and case_id=" + caseid;
                
                dbF.saveDataBySQL(sql);
            }
        }

        private void checkDictionaryCondenser(DataTable dtCase)
        {
            DataTable dtCondenser= dbF.getDataByTable("frmCondenser","visiofile='"+vsdFile+"'");
            DataTable dtDic_Condenser = db.getDataByTable("dictionarycondenser", "");
            foreach (DataRow drCondenser in dtCondenser.Rows)
            {
                checkCondenser(drCondenser, dtDic_Condenser, dtCase);
            }
        }

        private void checkReboiler(DataRow drReboiler, DataTable dtDic_Reboiler, DataTable dtCase)
        {
            string heaterName = drReboiler["heatername"].ToString();
            string heaterduty = drReboiler["heaterduty"].ToString();
            string steamfactor = drReboiler["steamfactor"].ToString();
            string hotstreamfactor = drReboiler["hotstreamfactor"].ToString();
            string hotoilfactor = drReboiler["hotoilfactor"].ToString();
            string furnacefactor = drReboiler["furnacefactor"].ToString();

            foreach (DataRow drCase in dtCase.Rows)
            {
                string caseid = drCase["case_id"].ToString();
                string caseName = drCase["casename"].ToString();
                DataRow[] drs = dtDic_Reboiler.Select("categoryvalue='steam'");
                string steam = drs[0][caseName].ToString();
                drs = dtDic_Reboiler.Select("categoryvalue='hotstream'");
                string hotstream = dtDic_Reboiler.Rows[1][caseName].ToString();
                drs = dtDic_Reboiler.Select("categoryvalue='hotoil'");
                string hotoil = dtDic_Reboiler.Rows[2][caseName].ToString();
                drs = dtDic_Reboiler.Select("categoryvalue='furnace'");
                string furnace = dtDic_Reboiler.Rows[3][caseName].ToString();
                decimal dutycalfactor = decimal.Parse(steamfactor) * decimal.Parse(steam) + decimal.Parse(hotstreamfactor) * decimal.Parse(hotstream) + decimal.Parse(hotoilfactor) * decimal.Parse(hotoil) + decimal.Parse(furnacefactor) * decimal.Parse(furnace);
                string sql = "update frmcase_reboiler set heaterduty='" + heaterduty + "', dutycalcfactor='" + dutycalfactor.ToString() + "' where heatername='" + heaterName + "' and case_id=" + caseid;

                dbF.saveDataBySQL(sql);
            }
        }

        private void checkDictionaryReboiler( DataTable dtCase)
        {
            DataTable dtReboiler = dbF.getDataByTable("frmReboiler", "visiofile='" + vsdFile + "'");
            DataTable dtDic_Reboiler = db.getDataByTable("dictionaryreboiler", "");
            foreach (DataRow drReboiler in dtReboiler.Rows)
            {
                checkReboiler(drReboiler, dtDic_Reboiler, dtCase);
            }
        }

        private DataTable getCases()
        {
            DataTable dt=dbF.getDataBySQL("select case_id,casename from frmcase inner join casetype on frmcase.case_id=casetype.id");
            return dt;
        }

        private void saveDataToDB()
        {
            DBRelief dbr = new DBRelief(dbFile);
            dtfrmbasecase = dtfrmbasecase.Clone();
            DataRow dr = dtfrmbasecase.NewRow();

            dr["description"] = this.txtDescription.Text;
            dr["latentheat"] = this.txtRunResult.Text;
            dr["visiofile"] = vsdFile;
            dr["Tray1Pressure"] = strTray1Pressure;
            dr["vapor"] = vapor;
            dr["liquid"] = liquid;
            dr["dir"] = tempdir;
            dr["Temperature"] = tempTemperature;
            dtfrmbasecase.Rows.Add(dr);

            DataView dvFeed = (DataView)gridStreamIn.ItemsSource;
            DataView dvProduct = (DataView)gridStreamOut.ItemsSource;
            DataView dvReboiler = (DataView)gridHeatIn.ItemsSource;
            DataView dvCondenser = (DataView)gridHeatOut.ItemsSource;

            DataTable dtFeed = dvFeed.Table.Copy();
            DataTable dtProduct = dvProduct.Table;

            dbr.saveDataByTable(dtfrmbasecase, vsdFile);
            dbr.saveDataByTable(dtFeed, vsdFile);
            dbr.saveDataByTable(dtProduct, vsdFile);
            dbr.saveDataByTable(dvReboiler.Table, vsdFile);
            dbr.saveDataByTable(dvCondenser.Table, vsdFile);
            dbr.saveDataByTable(dtFlashResult, vsdFile);
            dtfrmcase_feed = dtfrmcase_feed.Clone();
            dtfrmcase_product = dtfrmcase_product.Clone();
            dtfrmcase_reboiler = dtfrmcase_reboiler.Clone();
            dtfrmcase_condenser = dtfrmcase_condenser.Clone();
            dtfrmcase = dtfrmcase.Clone();
            foreach (KeyValuePair<int, UC_CaseStudy> uc in dicCase)
            {
                if (uc.Value.gridStreamIn.ItemsSource != null)
                {
                    dvFeed = (DataView)uc.Value.gridStreamIn.ItemsSource;
                    dtfrmcase_feed.Merge(dvFeed.ToTable());

                    dvProduct = (DataView)uc.Value.gridStreamOut.ItemsSource;
                    dtfrmcase_product.Merge(dvProduct.ToTable());

                    dvReboiler = (DataView)uc.Value.gridHeatIn.ItemsSource;
                    dtfrmcase_reboiler.Merge(dvReboiler.ToTable());

                    dvCondenser = (DataView)uc.Value.gridHeatOut.ItemsSource;
                    dtfrmcase_condenser.Merge(dvCondenser.ToTable());

                    dr = dtfrmcase.NewRow();
                    dr["case_id"] = uc.Key;
                    dr["visiofile"] = vsdFile;
                    dr["relieftemp"] = uc.Value.txtReliefTemp.Text;
                    dr["reliefpress"] = uc.Value.txtReliefPress.Text;
                    dr["reliefrate"] = uc.Value.txtReliefRate.Text;
                    dr["reliefmw"] = uc.Value.txtReliefMW.Text;
                    dtfrmcase.Rows.Add(dr);
                }
            }
            dbr.saveDataByTable(dtfrmcase, vsdFile);
            dbr.saveDataByTable(dtfrmcase_feed, vsdFile);
            dbr.saveDataByTable(dtfrmcase_product, vsdFile);
            dbr.saveDataByTable(dtfrmcase_reboiler, vsdFile);
            dbr.saveDataByTable(dtfrmcase_condenser, vsdFile);
        }

        private void checkDictionary()
        {
            DataTable dtcases = getCases();
            checkDictionarySource(dtcases);
            checkDictionaryCondenser(dtcases);
            checkDictionaryReboiler(dtcases);
        }

    }
}
