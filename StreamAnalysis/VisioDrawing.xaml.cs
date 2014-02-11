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
using System.Collections.ObjectModel;
using AxMicrosoft.Office.Interop.VisOcx;
using Visio = Microsoft.Office.Interop.Visio;
using ReliefAnalysis;
using System.Data;
using System.IO;
using System.Collections;

namespace ReliefAnalysis
{
    /// <summary>
    /// visio.xaml 的交互逻辑
    /// </summary>
    public partial class VisioDrawing : UserControl
    {
        public AxDrawingControl visioControl= new AxDrawingControl();
        public string przFile;
        public string dbFile;
        public string vName;
        public VisioDrawing()
        {
            InitializeComponent();
           
            
            this.host.Child = this.visioControl;
                
                
           
        }

        private void host_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void visio_DragDrop(object data, System.Windows.Forms.DragEventArgs e)
        {
            //if (e.Data.GetDataPresent(DataFormats.StringFormat))
            //{
            //    string str = (string)e.Data.GetData(DataFormats.StringFormat);                
            //}

        }

        private void visio_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
        {
            //e.Effect = System.Windows.Forms.DragDropEffects.All;

        }

        private void host_Drop(object sender, DragEventArgs e)
        {
            //if (e.Data.GetDataPresent(DataFormats.StringFormat))
            //{
            //    string str = (string)e.Data.GetData(DataFormats.StringFormat);
            //    Visio.Document currentStencil = visioControl.Document.Application.Documents.OpenEx("Basic_U.vss", (short)Visio.VisOpenSaveArgs.visOpenDocked);
            //    Visio.Page currentPage = visioControl.Document.Pages[1]; 
            //    Visio.Shape shape1 = currentPage.Drop(currentStencil.Masters["三角形"], 1.50, 1.50); 
            //}

        }
        double Prelief = 0;

        private void ToolbarButton_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            Button item = (Button)sender;
            string visioFile = this.visioControl.Src.ToString();
            string dir = System.IO.Path.GetDirectoryName(visioFile);
            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            string dbFilepath = dir + @"\" + dirInfo.Name + ".accdb";
            if (string.IsNullOrEmpty(przFile))
                przFile = findPrzFile(dirInfo);
            string przFilepath = System.IO.Path.GetDirectoryName(visioFile) + @"\" + przFile;
            string vsdFile = System.IO.Path.GetFileName(visioFile);
            switch (item.ToolTip.ToString())
            {
                case "Case Study":
                    OptionCase frm = new OptionCase();
                    frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                   
                   frm.Owner = parentWindow;
                    frm.dbFile = dbFilepath;
                    frm.przFile = przFilepath;
                    frm.vsdFile = vsdFile;
                    foreach (Visio.Shape shape in visioControl.Window.Selection)
                    {
                        frm.eqName = shape.Text;
                        frm.Prelief = Prelief;
                        frm.Show();
                        
                    }

                    break;
                case "PSV":
                    PSV frmPSV = new PSV();
                    frmPSV.Owner = parentWindow;
                    frmPSV.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    frmPSV.vsdFile = vsdFile;
                    frmPSV.dbFile = dbFilepath;
                    if (frmPSV.ShowDialog() == true)
                    {
                        double Pset = double.Parse(frmPSV.txtPress.Text);
                        Prelief = double.Parse(frmPSV.txtPrelief.Text) * Pset;
                    }

                    break;
                case "Property":
                    openProperty();
                    break;
            }
        }

        private void openProperty()
        {
            string visioFile = this.visioControl.Src.ToString();
            string vsdFile = System.IO.Path.GetFileName(visioFile);
            if (visioControl.Window.Selection.Count == 1)
            {
                foreach (Visio.Shape shape in visioControl.Window.Selection)
                {
                    if (shape.NameU.Contains("connector"))
                    {
                        CustomStream frm = new CustomStream();
                        Window parentWindow = Window.GetWindow(this);
                        frm.Owner = parentWindow;
                        frm.txtName.Text = shape.Text;
                        frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        frm.dbFile = dbFile;
                        frm.vsdFile = vsdFile;
                        frm.streamName = shape.Text;
                        if (frm.ShowDialog() == true)
                        {
                            shape.Text = frm.txtName.Text;
                        }

                    }
                    else if (shape.NameU.Contains("Fired heater"))
                    {
                        Furnace frm = new Furnace();
                        frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        Window parentWindow = Window.GetWindow(this);
                        frm.Owner = parentWindow;
                        frm.dbFile = dbFile;
                        frm.nameU = shape.NameU;
                        if (frm.ShowDialog() == true)
                        {
                            if (frm.startV != null || frm.endV != null)
                            {
                                if (frm.startV != null)
                                {

                                    Visio.Shape connector = visioControl.Window.Application.ActivePage.Shapes[frm.startV];
                                    ConnectShapes(shape, 1, connector, 0);
                                }

                                if (frm.endV != null)
                                {
                                    Visio.Shape connector = visioControl.Window.Application.ActivePage.Shapes[frm.endV];
                                    ConnectShapes(shape, 1, connector, 1);
                                }
                            }

                            shape.Text = frm.txtName.Text;
                        }

                    }
                    else if (shape.NameU.Contains("Vessel2"))
                    {
                        Furnace frm = new Furnace();
                        frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        Window parentWindow = Window.GetWindow(this);
                        frm.Owner = parentWindow;
                        frm.dbFile = dbFile;
                        frm.eqType = "Flash";
                        if (frm.ShowDialog() == true)
                        {
                            if (frm.startV != null || frm.endV != null)
                            {
                                if (frm.startV != null && !string.IsNullOrEmpty(frm.startV.NameID))
                                {
                                    Visio.Shape connector = visioControl.Window.Application.ActivePage.Shapes[frm.startV];
                                    connector.Text = frm.startV.Text;
                                    ConnectShapes(shape, 1, connector, 0);
                                }
                                else
                                {
                                    Visio.Document currentStencil = visioControl.Document.Application.Documents.OpenEx("PEPIPE_M.vss", (short)Visio.VisOpenSaveArgs.visAddHidden);
                                    Visio.Master visioRectMaster = currentStencil.Masters.get_ItemU(@"Major PipelineR");
                                    Visio.Shape connector = visioControl.Window.Application.ActivePage.Drop(visioRectMaster, 4, 4);
                                    connector.Text = frm.startV.Text;
                                    ConnectShapes(shape, 1, connector, 0);

                                }

                                if (frm.endV != null && !string.IsNullOrEmpty(frm.endV.NameID))
                                {
                                    Visio.Shape connector = visioControl.Window.Application.ActivePage.Shapes[frm.endV];
                                    connector.Text = frm.endV.Text;
                                    ConnectShapes(shape, 1, connector, 1);
                                }
                                else
                                {
                                    Visio.Document currentStencil = visioControl.Document.Application.Documents.OpenEx("PEPIPE_M.vss", (short)Visio.VisOpenSaveArgs.visAddHidden);
                                    Visio.Master visioRectMaster = currentStencil.Masters.get_ItemU(@"Major PipelineR");
                                    Visio.Shape connector = visioControl.Window.Application.ActivePage.Drop(visioRectMaster, 4, 4);
                                    connector.Text = frm.endV.Text;
                                    ConnectShapes(shape, 1, connector, 1);
                                }
                            }

                            shape.Text = frm.txtName.Text;
                        }

                    }
                    else if (shape.NameU.Contains("Dis"))
                    {
                        DrawDistillation(shape);
                    }
                    else if (shape.NameU.Contains("Carrying vessel"))
                    {
                        Source frmSource = new Source();
                        Window parentWindow = Window.GetWindow(this);
                        frmSource.Owner = parentWindow;
                        frmSource.dbFile = dbFile;
                        frmSource.vsdFile = vsdFile;
                        frmSource.txtName.Text = shape.Text;
                        frmSource.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        frmSource.ShowDialog();
                    }
                    else if (shape.NameU.Contains("Clarifier"))
                    {
                        Sink frmSink = new Sink();
                        Window parentWindow = Window.GetWindow(this);
                        frmSink.Owner = parentWindow;
                        frmSink.dbFile = dbFile;
                        frmSink.vsdFile = vsdFile;
                        frmSink.txtName.Text = shape.Text;
                        frmSink.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        frmSink.ShowDialog();
                    }
                    else if (shape.NameU.Contains("Kettle reboiler"))
                    {
                        Reboiler frmReboiler = new Reboiler();
                        Window parentWindow = Window.GetWindow(this);
                        frmReboiler.Owner = parentWindow;
                        frmReboiler.dbFile = dbFile;
                        frmReboiler.vsdFile = vsdFile;
                        frmReboiler.txtName.Text = shape.Text;
                        frmReboiler.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        frmReboiler.ShowDialog();
                    }
                    else if (shape.NameU.Contains("Heat exchanger1"))
                    {
                        Condenser frmCondenser = new Condenser();
                        Window parentWindow = Window.GetWindow(this);
                        frmCondenser.Owner = parentWindow;
                        frmCondenser.dbFile = dbFile;
                        frmCondenser.vsdFile = vsdFile;
                        frmCondenser.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        frmCondenser.txtName.Text = shape.Text;
                        frmCondenser.ShowDialog();
                    }
                    else if (shape.NameU.Contains("Vessel"))
                    {
                        Accumulator frmAccumulator = new Accumulator();
                        frmAccumulator.dbFile = dbFile;
                        frmAccumulator.vsdFile = vsdFile;
                        frmAccumulator.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        frmAccumulator.txtName.Text = shape.Text;
                        Window parentWindow = Window.GetWindow(this);
                        frmAccumulator.Owner = parentWindow;
                        if (frmAccumulator.ShowDialog() == true)
                        {
                            shape.Text = frmAccumulator.AccumulationName;
                        }
                        
                    }
                    
                }
            }
        }

        private void ConnectShapes(Visio.Shape shape, int connectPoint, Visio.Shape connector, int direction)
        {
            // get the cell from the source side of the connector
            Visio.Cell beginXCell = connector.get_CellsSRC(
            (short)Visio.VisSectionIndices.visSectionObject,
            (short)Visio.VisRowIndices.visRowXForm1D,
            (short)Visio.VisCellIndices.vis1DBeginX);
            // glue the source side of the connector to the first shape

            //shape1.AutoConnect(shape2, Visio.VisAutoConnectDir.visAutoConnectDirRight,connector);

            Visio.Cell fromCell = shape.get_CellsSRC(
            (short)Visio.VisSectionIndices.visSectionConnectionPts,
            (short)connectPoint, 0);

            //beginXCell.GlueTo(fromCell);
            //shape1.get_Cells("FillForegnd").Formula = "3";


            //get the cell from the destination side of the connector
            Visio.Cell endXCell = connector.get_CellsSRC(
            (short)Visio.VisSectionIndices.visSectionObject,
            (short)Visio.VisRowIndices.visRowXForm1D,
            (short)Visio.VisCellIndices.vis1DEndX);

            //// glue the destination side of the connector to the second shape

            //Visio.Cell toXCell = shape2.get_CellsSRC(
            //(short)Visio.VisSectionIndices.visSectionObject,
            //(short)Visio.VisRowIndices.visRowXFormOut,
            //(short)Visio.VisCellIndices.visXFormPinX);
            //endXCell.GlueTo(toXCell);

            //Visio.Cell arrowCell = connector.get_CellsSRC((short)Visio.VisSectionIndices.visSectionObject, (short)Visio.VisRowIndices.visRowLine, (short)Visio.VisCellIndices.visLineEndArrow);
            if (direction == 0)
            {
                //connector.get_Cells("BeginArrow").Formula = "=5";
                connector.get_Cells("EndArrow").Formula = "=5";
                endXCell.GlueTo(fromCell);
            }
            else
            {
                connector.get_Cells("EndArrow").Formula = "=5";
                beginXCell.GlueTo(fromCell);
            }
            //connector.get_Cells("LineColor").Formula = "3";

        }

        private void DrawDistillation(Visio.Shape shape)
        {
            Tower frm = new Tower();
            frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Window parentWindow = Window.GetWindow(this);
            frm.Owner = parentWindow;
            frm.dbFile = dbFile;
            //frm.txtName.Text = shape.Text;
            frm.eqType = "Column";
            frm.vsdFile = System.IO.Path.GetFileName(visioControl.Src);
            shape.get_Cells("Height").ResultIU = 2;

            double width = shape.get_Cells("Width").ResultIU;
            double height = shape.get_Cells("Height").ResultIU;
            double pinX = shape.get_Cells("PinX").ResultIU;
            double pinY = shape.get_Cells("PinY").ResultIU;
            if (frm.ShowDialog() == true)
            {
                if (frm.op == 1)
                {                  
                    shape.Text = frm.txtName.Text;
                    return;
                }
                deleteShapesExcept(shape);
                przFile = frm.przFile;
                Visio.Document currentStencil_1 = visioControl.Document.Application.Documents.OpenEx("PEHEAT_M.vss", (short)Visio.VisOpenSaveArgs.visAddHidden);
                Visio.Document currentStencil_2 = visioControl.Document.Application.Documents.OpenEx("CONNEC_M.vss", (short)Visio.VisOpenSaveArgs.visAddHidden);
                Visio.Document currentStencil_3 = visioControl.Document.Application.Documents.OpenEx("PEVESS_M.vss", (short)Visio.VisOpenSaveArgs.visAddHidden);
                Visio.Master condenserMaster = currentStencil_1.Masters.get_ItemU(@"Heat exchanger1");
                Visio.Master reboilerMaster = currentStencil_1.Masters.get_ItemU(@"Kettle reboiler");
                Visio.Master streamMaster = currentStencil_2.Masters.get_ItemU(@"Dynamic connector");
                Visio.Master condenserVesselMaster = currentStencil_3.Masters.get_ItemU(@"Vessel");

                Visio.Document startStencil = visioControl.Document.Application.Documents.OpenEx("PEVESS_M.vss", (short)Visio.VisOpenSaveArgs.visAddHidden);
                Visio.Master startMaster = startStencil.Masters.get_ItemU(@"Carrying vessel");
                Visio.Master endMaster = startStencil.Masters.get_ItemU(@"Clarifier");


                int stagenumber = int.Parse(frm.txtStageNumber.Text);
                DataTable dtFeed = (DataTable)Application.Current.Properties["FeedData"];
                int start = 16;
                double multiple = 0.125;
                //double leftmultiple = 1.5;
                int center = 13;
                foreach (DataRow dr in dtFeed.Rows)
                {
                    Visio.Shape connector = visioControl.Window.Application.ActivePage.Drop(streamMaster, 5, 5);
                    ConnectShapes(shape, start, connector, 0);
                    connector.Text = dr["streamname"].ToString();

                    Visio.Shape startShp = visioControl.Window.Application.ActivePage.Drop(startMaster, pinX - 2, pinY + (start - center) * multiple * height);
                    startShp.get_Cells("Height").ResultIU = 0.1;
                    startShp.get_Cells("Width").ResultIU = 0.2;
                    startShp.Text = connector.Text + "_Source";
                    ConnectShapes(startShp, 2, connector, 1);
                    start--;
                    if (start < 11)
                    {
                        start = 16;
                    }

                }

                DataTable dtProd = (DataTable)Application.Current.Properties["ProdData"];
                DataTable dtReboiler = (DataTable)Application.Current.Properties["Reboiler"];
                DataTable dtCondenser = (DataTable)Application.Current.Properties["Condenser"];
                DataTable dtHxCondenser = (DataTable)Application.Current.Properties["HxCondenser"];
               
                Visio.Shape condenser;
                Visio.Shape condenserVessel = null;
                double twidth = 0;
                double theight = 0;
                double tpinX = 0;
                double tpinY = 0;
                if (dtCondenser.Rows.Count > 0)
                {
                    condenser = visioControl.Window.Application.ActivePage.Drop(condenserMaster, pinX + 1, pinY + height / 2 + 0.2);
                    condenserVessel = visioControl.Window.Application.ActivePage.Drop(condenserVesselMaster, pinX + 1.5, pinY + height / 2 + 0.1);
                    condenserVessel.get_Cells("Height").ResultIU = 0.2;
                    condenser.get_Cells("Height").ResultIU = 0.2;
                    condenser.get_Cells("Width").ResultIU = 0.2;
                    Visio.Shape connector1 = visioControl.Window.Application.ActivePage.Drop(streamMaster, 5, 5);
                    Visio.Shape connector2 = visioControl.Window.Application.ActivePage.Drop(streamMaster, 5, 5);
                    Visio.Shape connector3 = visioControl.Window.Application.ActivePage.Drop(streamMaster, 5, 5);
                    ConnectShapes(shape, 1, connector1, 1);//从塔到冷凝器
                    ConnectShapes(condenser, 2, connector1, 0);

                    ConnectShapes(condenser, 1, connector2, 1);
                    ConnectShapes(condenserVessel, 1, connector2, 0);

                    ConnectShapes(condenserVessel, 8, connector3, 1);
                    ConnectShapes(shape, 2, connector3, 0);
                    twidth = condenserVessel.get_Cells("Width").ResultIU;
                    theight = condenserVessel.get_Cells("Height").ResultIU;
                    tpinX = condenserVessel.get_Cells("PinX").ResultIU;
                    tpinY = condenserVessel.get_Cells("PinY").ResultIU;

                    condenser.Text = dtCondenser.Rows[0]["heatername"].ToString();
                }

                for(int i=1;i<=dtHxCondenser.Rows.Count;i++)
                {
                    DataRow dr= dtHxCondenser.Rows[i-1];
                    condenser = visioControl.Window.Application.ActivePage.Drop(condenserMaster, pinX, pinY + height / 2 -i*0.4);
                    condenserVessel = visioControl.Window.Application.ActivePage.Drop(condenserVesselMaster, pinX + 1.5, pinY + height / 2 + 0.1);
                    condenserVessel.get_Cells("Height").ResultIU = 0.2;
                    condenser.get_Cells("Height").ResultIU = 0.2;
                    condenser.get_Cells("Width").ResultIU = 0.2;
                    condenser.Text = dr["heatername"].ToString();
                }




                Visio.Shape reboiler = null;
                double bwidth = 0;
                double bheight = 0;
                double bpinX = 0;
                double bpinY = 0;
                if (dtReboiler.Rows.Count > 0)
                {
                    reboiler = visioControl.Window.Application.ActivePage.Drop(reboilerMaster, pinX + 1, pinY - height / 2 - 0.2);
                    Visio.Shape connector1 = visioControl.Window.Application.ActivePage.Drop(streamMaster, 5, 5);
                    Visio.Shape connector2 = visioControl.Window.Application.ActivePage.Drop(streamMaster, 5, 5);
                    ConnectShapes(shape, 9, connector1, 1);//从塔到加热器
                    ConnectShapes(reboiler, 4, connector1, 0);

                    ConnectShapes(shape, 8, connector2, 0);//从加热器到塔
                    ConnectShapes(reboiler, 3, connector2, 1);
                    reboiler.Text = dtReboiler.Rows[0]["heatername"].ToString();
                }


                start = 3;
                center = 5;
                int topcount = 1;
                int bottomcount = 1;
                foreach (DataRow dr in dtProd.Rows)
                {
                    int tray = -1;
                    if(dr["tray"].ToString()!=string.Empty)
                    {
                        tray = int.Parse(dr["tray"].ToString());
                    }
                    if (tray == 1)
                    {
                        if (dtCondenser.Rows.Count == 0)
                        {
                            Visio.Shape connector = visioControl.Window.Application.ActivePage.Drop(streamMaster, 5, 5);
                            ConnectShapes(shape, 1, connector, 1);
                            connector.Text = dr["streamname"].ToString();
                            Visio.Shape endShp = visioControl.Window.Application.ActivePage.Drop(endMaster, pinX + 2, pinY - 2 - height / 2);
                            endShp.get_Cells("Height").ResultIU = 0.1;
                            endShp.get_Cells("Width").ResultIU = 0.2;
                            endShp.Text = connector.Text+"_Sink";
                            ConnectShapes(endShp, 7, connector, 0);
                        }
                        else
                        {
                            if (topcount == 1) //开放3，6，7
                            {
                                Visio.Shape connector = visioControl.Window.Application.ActivePage.Drop(streamMaster, 5, 5);
                                ConnectShapes(condenserVessel, 3, connector, 1);
                                connector.Text = dr["streamname"].ToString();

                                Visio.Shape endShp = visioControl.Window.Application.ActivePage.Drop(endMaster, tpinX + 2, tpinY - 0.2);
                                endShp.get_Cells("Height").ResultIU = 0.1;
                                endShp.get_Cells("Width").ResultIU = 0.2;
                                endShp.Text = connector.Text + "_Sink";
                                ConnectShapes(endShp, 7, connector, 0);
                                topcount++;
                            }
                            else if (topcount == 2)
                            {
                                Visio.Shape connector = visioControl.Window.Application.ActivePage.Drop(streamMaster, 5, 5);
                                ConnectShapes(condenserVessel, 6, connector, 1);
                                connector.Text = dr["streamname"].ToString();

                                Visio.Shape endShp = visioControl.Window.Application.ActivePage.Drop(endMaster, tpinX + 2, tpinY + 0.4);
                                endShp.get_Cells("Height").ResultIU = 0.1;
                                endShp.get_Cells("Width").ResultIU = 0.2;
                                endShp.Text = connector.Text + "_Sink";
                                ConnectShapes(endShp, 7, connector, 0);
                                topcount++;
                            }
                            else
                            {
                                Visio.Shape connector = visioControl.Window.Application.ActivePage.Drop(streamMaster, 5, 5);
                                ConnectShapes(condenserVessel, 7, connector, 1);
                                connector.Text = dr["streamname"].ToString();


                                Visio.Shape endShp = visioControl.Window.Application.ActivePage.Drop(endMaster, tpinX + 2, tpinY - 0.1);
                                endShp.get_Cells("Height").ResultIU = 0.1;
                                endShp.get_Cells("Width").ResultIU = 0.2;
                                endShp.Text = connector.Text + "_Sink";
                                ConnectShapes(endShp, 7, connector, 0);
                                topcount++;
                            }
                        }

                    }
                    else if (tray == stagenumber)
                    {
                        if (dtReboiler.Rows.Count == 0)
                        {
                            Visio.Shape connector = visioControl.Window.Application.ActivePage.Drop(streamMaster, 5, 5);
                            ConnectShapes(shape, 9, connector, 1);
                            connector.Text = dr["streamname"].ToString();
                            Visio.Shape endShp = visioControl.Window.Application.ActivePage.Drop(endMaster, pinX + 2, pinY + 0.5 - height / 2);
                            endShp.get_Cells("Height").ResultIU = 0.1;
                            endShp.get_Cells("Width").ResultIU = 0.2;
                            endShp.Text = connector.Text + "_Sink";
                            ConnectShapes(endShp, 7, connector, 0);
                        }
                        else
                        {
                            Visio.Shape connector = visioControl.Window.Application.ActivePage.Drop(streamMaster, 5, 5);
                            ConnectShapes(reboiler, 1, connector, 1);
                            connector.Text = dr["streamname"].ToString();
                            Visio.Shape endShp = visioControl.Window.Application.ActivePage.Drop(endMaster, pinX + 2, pinY - 0.5 - height / 2);
                            endShp.get_Cells("Height").ResultIU = 0.1;
                            endShp.get_Cells("Width").ResultIU = 0.2;
                            endShp.Text = connector.Text + "_Sink";
                            ConnectShapes(endShp, 7, connector, 0);
                        }
                    }
                    else
                    {
                        Visio.Shape connector = visioControl.Window.Application.ActivePage.Drop(streamMaster, 5, 5);
                        ConnectShapes(shape, start, connector, 1);
                        connector.Text = dr["streamname"].ToString();

                        Visio.Shape endShp = visioControl.Window.Application.ActivePage.Drop(endMaster, pinX + 2, pinY + (center - start) * multiple * height);
                        endShp.get_Cells("Height").ResultIU = 0.1;
                        endShp.get_Cells("Width").ResultIU = 0.2;
                        endShp.Text = connector.Text + "_Sink";
                        ConnectShapes(endShp, 7, connector, 0);
                        start++;
                        if (start > 8)
                        {
                            start = 3;
                        }
                    }
                }



                currentStencil_1.Close();
                currentStencil_2.Close();
                currentStencil_3.Close();

                shape.Text = frm.txtName.Text;
                Application.Current.Properties.Remove("Condenser");
                Application.Current.Properties.Remove("HxCondenser");
                Application.Current.Properties.Remove("Reboiler");
                Application.Current.Properties.Remove("HxReboiler");
                Application.Current.Properties.Remove("FeedData");
                Application.Current.Properties.Remove("ProdData");
                visioControl.Document.SaveAs(visioControl.Src);
            }
        }


        private void initGeneral()
        {
            ObservableCollection<LVData> LVDatas = new ObservableCollection<LVData>();
            LVDatas.Add(new LVData { Name = "Stream", Pic = "images/Stream.ico" });
            LVDatas.Add(new LVData { Name = "Steam", Pic = "images/Steam.ico" });
            LVDatas.Add(new LVData { Name = "Valve", Pic = "images/Valve.ico" });
            LVDatas.Add(new LVData { Name = "Mixer", Pic = "images/Mixer.ico" });
            LVDatas.Add(new LVData { Name = "Splitter", Pic = "images/Splitter.ico" });
            LVDatas.Add(new LVData { Name = "Flash", Pic = "images/Flash.ico" });
            LVDatas.Add(new LVData { Name = "Pump", Pic = "images/Pump.ico" });
            LVDatas.Add(new LVData { Name = "Source", Pic = "images/Source.ico" });
            LVDatas.Add(new LVData { Name = "Sink", Pic = "images/Source.ico" });
            lvGeneral.ItemsSource = LVDatas;
        }

        private void initHeatExchanger()
        {
            ObservableCollection<LVData> LVDatas = new ObservableCollection<LVData>();
            LVDatas.Add(new LVData { Name = "Stream", Pic = "images/Stream.ico" });
            LVDatas.Add(new LVData { Name = "Steam", Pic = "images/Steam.ico" });
            LVDatas.Add(new LVData { Name = "Utility Heater", Pic = "images/Heater.ico" });
            LVDatas.Add(new LVData { Name = "Utility Cooler", Pic = "images/Cooler.ico" });
            LVDatas.Add(new LVData { Name = "Process HX", Pic = "images/ProcessHX.ico" });
            LVDatas.Add(new LVData { Name = "Furnace", Pic = "images/Furnace.ico" });

            this.lvHeatExchanger.ItemsSource = LVDatas;
        }

        private void initTower()
        {
            ObservableCollection<LVData> LVDatas = new ObservableCollection<LVData>();
            LVDatas.Add(new LVData { Name = "Stream", Pic = "images/Stream.ico" });
            LVDatas.Add(new LVData { Name = "Steam", Pic = "images/Steam.ico" });
            LVDatas.Add(new LVData { Name = "Source", Pic = "images/Source.ico" });
            LVDatas.Add(new LVData { Name = "Sink", Pic = "images/Sink.ico" });
            LVDatas.Add(new LVData { Name = "Absorber", Pic = "images/Absorber.ico" });
            LVDatas.Add(new LVData { Name = "Regenerator", Pic = "images/Tower.ico" });
            LVDatas.Add(new LVData { Name = "Distillation", Pic = "images/Distillation.ico" });
            LVDatas.Add(new LVData { Name = "Accumulator", Pic = "images/Tower.ico" });
            LVDatas.Add(new LVData { Name = "Reboiler", Pic = "images/Reboiler.ico" });
            LVDatas.Add(new LVData { Name = "Condenser(Water)", Pic = "images/Condenser.ico" });
            LVDatas.Add(new LVData { Name = "Condenser(Air)", Pic = "images/Condenser.ico" });
            LVDatas.Add(new LVData { Name = "Condenser(Wet Air)", Pic = "images/Condenser.ico" });
            this.lvTower.ItemsSource = LVDatas;
        }

        private void initCompressor()
        {
            ObservableCollection<LVData> LVDatas = new ObservableCollection<LVData>();
            LVDatas.Add(new LVData { Name = "Stream", Pic = "images/Stream.ico" });
            LVDatas.Add(new LVData { Name = "Steam", Pic = "images/Steam.ico" });
            LVDatas.Add(new LVData { Name = "Centrifugal Compressor", Pic = "images/CentrifugalCompressor.ico" });
            LVDatas.Add(new LVData { Name = "Recipe Compressor", Pic = "images/RecipeCompressor.ico" });
            this.lvCompressor.ItemsSource = LVDatas;
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            
            visioControl.Src = vName;    
            Visio.Page currentPage = visioControl.Document.Pages[1];
            visioControl.AllowDrop = true;
            visioControl.Name = "vc";
            initGeneral();
            initHeatExchanger();
            initTower();
            initCompressor();
            
            visioControl.Window.Zoom = 1;
            visioControl.Window.ShowGrid = 0;
            visioControl.Window.ShowRulers = 0;
            visioControl.Window.ShowConnectPoints = -1;
        }
        private void lvGeneral_MouseMove(object sender, MouseEventArgs e)
        {
            Image lvi = (Image)sender;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Visio.Page currentPage = visioControl.Document.Pages[1];
                if (lvi.Source.ToString().Contains("Stream"))
                {
                    Visio.Document currentStencil = visioControl.Document.Application.Documents.OpenEx("CONNEC_M.vss", (short)Visio.VisOpenSaveArgs.visAddHidden);
                    Visio.Master visioRectMaster = currentStencil.Masters.get_ItemU(@"Dynamic connector");
                    DragDropEffects dde1 = DragDrop.DoDragDrop(lvi, visioRectMaster, DragDropEffects.All);
                    currentStencil.Close();
                    openProperty();


                }
                else if (lvi.Source.ToString().Contains("Steam"))
                {
                    Visio.Document currentStencil = visioControl.Document.Application.Documents.OpenEx("CONNEC_M.vss", (short)Visio.VisOpenSaveArgs.visAddHidden);
                    Visio.Master visioRectMaster = currentStencil.Masters.get_ItemU(@"Dynamic connector");
                    DragDropEffects dde1 = DragDrop.DoDragDrop(lvi, visioRectMaster, DragDropEffects.All);
                    foreach (Visio.Shape shape in visioControl.Window.Selection)
                    {
                        shape.get_Cells("LineColor").Formula = "=THEMEGUARD(RGB(255,0,0))";
                    }
                    currentStencil.Close();
                    openProperty();


                }
                else if (lvi.Source.ToString().Contains("Valve"))
                {
                    Visio.Document currentStencil = visioControl.Document.Application.Documents.OpenEx("PEVALV_M.vss", (short)Visio.VisOpenSaveArgs.visAddHidden);
                    Visio.Master visioRectMaster = currentStencil.Masters.get_ItemU(@"Gate valve");
                    DragDropEffects dde1 = DragDrop.DoDragDrop(lvi, visioRectMaster, DragDropEffects.All);
                    currentStencil.Close();
                    openProperty();

                }
                else if (lvi.Source.ToString().Contains("Mixer"))
                {
                    Visio.Document currentStencil = visioControl.Document.Application.Documents.OpenEx("PEVESS_M.vss", (short)Visio.VisOpenSaveArgs.visAddHidden);
                    Visio.Master visioRectMaster = currentStencil.Masters.get_ItemU(@"Access point");
                    DragDropEffects dde1 = DragDrop.DoDragDrop(lvi, visioRectMaster, DragDropEffects.All);
                    currentStencil.Close();
                    openProperty();
                }

                else if (lvi.Source.ToString().Contains("Splitter"))
                {
                    Visio.Document currentStencil = visioControl.Document.Application.Documents.OpenEx("PEVESS_M.vss", (short)Visio.VisOpenSaveArgs.visAddHidden);
                    Visio.Master visioRectMaster = currentStencil.Masters.get_ItemU(@"Branch fitting");
                    DragDropEffects dde1 = DragDrop.DoDragDrop(lvi, visioRectMaster, DragDropEffects.All);
                    currentStencil.Close();
                    openProperty();
                }
                else if (lvi.Source.ToString().Contains("Flash"))
                {
                    Visio.Document currentStencil = visioControl.Document.Application.Documents.OpenEx("PEVESS_M.vss", (short)Visio.VisOpenSaveArgs.visAddHidden);
                    Visio.Master visioRectMaster = currentStencil.Masters.get_ItemU(@"Vessel");
                    DragDropEffects dde1 = DragDrop.DoDragDrop(lvi, visioRectMaster, DragDropEffects.All);
                    foreach (Visio.Shape shape in visioControl.Window.Selection)
                    {
                        //short count = shape.get_RowCount((short)Visio.VisSectionIndices.visSectionConnectionPts);
                        deleteConnector(shape, 0, 1);
                        deleteConnector(shape, 4, 4);

                    }
                    currentStencil.Close();
                    openProperty();
                }
                else if (lvi.Source.ToString().Contains("Pump"))
                {
                    Visio.Document currentStencil = visioControl.Document.Application.Documents.OpenEx("PEPUMP_M.vss", (short)Visio.VisOpenSaveArgs.visAddHidden);
                    Visio.Master visioRectMaster = currentStencil.Masters.get_ItemU(@"Centrifugal pump");
                    DragDropEffects dde1 = DragDrop.DoDragDrop(lvi, visioRectMaster, DragDropEffects.All);
                    foreach (Visio.Shape shape in visioControl.Window.Selection)
                    {
                        shape.Text = shape.Name;
                        deleteConnector(shape, 0, 1);
                        deleteConnector(shape, 2, 2);
                    }
                    currentStencil.Close();
                    openProperty();
                }
                else if (lvi.Source.ToString().Contains("Source"))
                {
                    Visio.Document currentStencil = visioControl.Document.Application.Documents.OpenEx("PEVESS_M.vss", (short)Visio.VisOpenSaveArgs.visAddHidden);
                    Visio.Master visioRectMaster = currentStencil.Masters.get_ItemU(@"Carring vessel");
                    DragDropEffects dde1 = DragDrop.DoDragDrop(lvi, visioRectMaster, DragDropEffects.All);
                    
                    currentStencil.Close();
                    openProperty();
                }
                else if (lvi.Source.ToString().Contains("Sink"))
                {
                    Visio.Document currentStencil = visioControl.Document.Application.Documents.OpenEx("PEVESS_M.vss", (short)Visio.VisOpenSaveArgs.visAddHidden);
                    Visio.Master visioRectMaster = currentStencil.Masters.get_ItemU(@"Open tank");
                    DragDropEffects dde1 = DragDrop.DoDragDrop(lvi, visioRectMaster, DragDropEffects.All);
                    
                    currentStencil.Close();
                    openProperty();
                }


                else if (lvi.Source.ToString().Contains("Reboiler"))
                {
                    Visio.Document currentStencil = visioControl.Document.Application.Documents.OpenEx("PEHEAT_M.vss", (short)Visio.VisOpenSaveArgs.visAddHidden);
                    Visio.Master visioRectMaster = currentStencil.Masters.get_ItemU(@"Kettle reboiler");
                    DragDropEffects dde1 = DragDrop.DoDragDrop(lvi, visioRectMaster, DragDropEffects.All);
                    foreach (Visio.Shape shape in visioControl.Window.Selection)
                    {
                        shape.get_Cells("LineColor").Formula = "=THEMEGUARD(RGB(255,0,0))";
                        deleteConnector(shape, 0, 1);
                        deleteConnector(shape, 2, 2);
                    }
                    currentStencil.Close();
                    openProperty();
                }
                else if (lvi.Source.ToString().Contains("Condenser"))
                {
                    Visio.Document currentStencil = visioControl.Document.Application.Documents.OpenEx("PEHEAT_M.vss", (short)Visio.VisOpenSaveArgs.visAddHidden);
                    Visio.Master visioRectMaster = currentStencil.Masters.get_ItemU(@"Heat exchanger1");
                    DragDropEffects dde1 = DragDrop.DoDragDrop(lvi, visioRectMaster, DragDropEffects.All);
                    foreach (Visio.Shape shape in visioControl.Window.Selection)
                    {
                        deleteConnector(shape, 0, 1);
                        deleteConnector(shape, 2, 2);
                    }
                    currentStencil.Close();
                    openProperty();
                }
                else if (lvi.Source.ToString().Contains("ProcessHX"))
                {
                    Visio.Document currentStencil = visioControl.Document.Application.Documents.OpenEx("PEHEAT_M.vss", (short)Visio.VisOpenSaveArgs.visAddHidden);
                    Visio.Master visioRectMaster = currentStencil.Masters.get_ItemU(@"Heat exchanger1");
                    DragDropEffects dde1 = DragDrop.DoDragDrop(lvi, visioRectMaster, DragDropEffects.All);
                    foreach (Visio.Shape shape in visioControl.Window.Selection)
                    {
                        deleteConnector(shape, 0, 1);
                    }
                    currentStencil.Close();
                    openProperty();
                }
                else if (lvi.Source.ToString().Contains("Furnace"))
                {
                    Visio.Document currentStencil = visioControl.Document.Application.Documents.OpenEx("PEHEAT_M.vss", (short)Visio.VisOpenSaveArgs.visAddHidden);
                    Visio.Master visioRectMaster = currentStencil.Masters.get_ItemU(@"Fired heater");
                    DragDropEffects dde1 = DragDrop.DoDragDrop(lvi, visioRectMaster, DragDropEffects.All);
                    currentStencil.Close();
                    openProperty();
                }


                else if (lvi.Source.ToString().Contains("Distillation"))
                {
                    Visio.Document myCurrentStencil = visioControl.Document.Application.Documents.OpenEx(System.Environment.CurrentDirectory + @"/Tower.vss", (short)Visio.VisOpenSaveArgs.visAddHidden);
                    Visio.Master visioRectMaster = myCurrentStencil.Masters.get_ItemU(@"Dis");
                    DragDropEffects dde1 = DragDrop.DoDragDrop(lvi, visioRectMaster, DragDropEffects.All);
                    myCurrentStencil.Close();
                    openProperty();
                }


                else if (lvi.Source.ToString().Contains("CentrifugalCompressor"))
                {
                    Visio.Document currentStencil = visioControl.Document.Application.Documents.OpenEx("PEPUMP_M.vss", (short)Visio.VisOpenSaveArgs.visAddHidden);
                    Visio.Master visioRectMaster = currentStencil.Masters.get_ItemU(@"Compressor / turbine");
                    DragDropEffects dde1 = DragDrop.DoDragDrop(lvi, visioRectMaster, DragDropEffects.All);
                    currentStencil.Close();
                    openProperty();
                }
                else if (lvi.Source.ToString().Contains("RecipeCompressor"))
                {
                    Visio.Document currentStencil = visioControl.Document.Application.Documents.OpenEx("PEPUMP_M.vss", (short)Visio.VisOpenSaveArgs.visAddHidden);
                    Visio.Master visioRectMaster = currentStencil.Masters.get_ItemU(@"Reciprocating pump/compr.");
                    DragDropEffects dde1 = DragDrop.DoDragDrop(lvi, visioRectMaster, DragDropEffects.All);
                    currentStencil.Close();
                    openProperty();
                }


            }
        }

        private void deleteConnector(Visio.Shape shape, int start, int number)
        {
            int n = 0;
            while (n < number)
            {
                shape.DeleteRow((short)Visio.VisSectionIndices.visSectionConnectionPts, short.Parse(start.ToString()));
                n++;
            }
        }

        private void deleteShapesExcept(Visio.Shape shape)
        {
            ArrayList arr = new ArrayList();

            int count = visioControl.Window.Document.Pages[1].Shapes.Count;
            for (int i = 1; i <= count; i++)
            {
                Visio.Shape shp = visioControl.Window.Document.Pages[1].Shapes[i];
                if (shp.NameID != shape.NameID)
                {
                    arr.Add(shp);
                }
            }
            foreach (Visio.Shape shp in arr)
            {
                shp.Delete();
            }

        }
        private string findPrzFile(DirectoryInfo dirInfo)
        {
            FileInfo[] fInfos = dirInfo.GetFiles();
            foreach (FileInfo f in fInfos)
            {
                if (f.Name.Contains("prz"))
                {
                    return f.Name;
                }
            }
            return "";
        }

    }


    
}
