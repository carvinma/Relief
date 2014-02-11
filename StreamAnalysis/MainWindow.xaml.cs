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
using AxMicrosoft.Office.Interop.VisOcx;
using Visio = Microsoft.Office.Interop.Visio;
using System.Collections.ObjectModel;
using ReliefAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Configuration;

using System.Diagnostics;
using Microsoft.Win32;


namespace ReliefAnalysis
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);

        public const int OF_READWRITE = 2;
        public const int OF_SHARE_DENY_NONE = 0x40;
        public readonly IntPtr HFILE_ERROR = new IntPtr(-1);

        string curRefFile = string.Empty;
        string curvisio = string.Empty;
        string curFolder = string.Empty;
       
        public MainWindow()
        {
            InitializeComponent();                      
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuItem item = (MenuItem)e.OriginalSource;
                switch (item.Header.ToString())
                {
                    case "Open Plant":
                        openPlant();
                        break;
                    case "Exit":
                        this.Close();
                        break;
                    case "New Plant":
                        newPlant();
                        break;
                    case "Close Plant":
                        closePlant();
                        break;
                    case "Save As":
                        saveasfile();
                        break;
                    case "Save":
                        savefile();
                        break;
                    case "Dictionary Setting":
                        DictionarySetting dicSetting = new DictionarySetting();
                        dicSetting.Owner = this;
                        dicSetting.ShowDialog();
                        break;
                    case "Glossary Setting":

                        break;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Action");
            }
        }
        private void ToolbarButton_Click(object sender, RoutedEventArgs e)
        {
            Button item = (Button)sender;
            switch (item.ToolTip.ToString())
            {
                case "Open Plant":
                    openPlant();
                    break;                
                case "New Plant":
                    newPlant();
                    break;
                case "Close Plant":
                    closePlant();
                    break;
                case "Save Project":
                    savefile();
                    break;
                case "Save All Project":
                    savefile();
                    break;
               
                case "Flash Calculation":
                    FlashCal();
                    break;
            }
        }
        private void openPlant()
        {          
            Microsoft.Win32.OpenFileDialog dlgOpenDiagram = new Microsoft.Win32.OpenFileDialog();
            dlgOpenDiagram.Filter = "Relief(*.ref) |*.ref";
            string outFolder = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"temp\";
            if (dlgOpenDiagram.ShowDialog() == true)
            {
                int length=dlgOpenDiagram.SafeFileName.Length;
                outFolder = outFolder + dlgOpenDiagram.SafeFileName.Substring(0, length - 4);
                string refFile = dlgOpenDiagram.FileName;
                if (Directory.Exists(outFolder))
                {
                    Directory.Delete(outFolder,true);
                }
                CSharpZip.ExtractZipFile(refFile, "1", outFolder); 
                TreeViewItem tvi = new TreeViewItem();
                TreeViewItem root=(TreeViewItem)navView.Items[0];
                DirectoryInfo di = new DirectoryInfo(outFolder);
                getAllfiles(di, ref root);
                root.ExpandSubtree();
                curFolder = outFolder;
                curRefFile = refFile;
                this.leftdockpanel.Visibility = Visibility.Visible;
            }
        }

        private void getAllfiles(DirectoryInfo di, ref TreeViewItem tvi)
        {
            try
            {
                string name = di.Name;
                TreeViewItem item = GetTreeView(name, "images/plant.ico");
                item.Tag = di.FullName;
                tvi.Items.Add(item);

                foreach (FileInfo f in di.GetFiles())
                {
                    if (f.Extension.Contains(".vsd"))
                    {
                        string fname = f.Name.Substring(0, f.Name.Length - 4);
                        TreeViewItem fitem = GetTreeView(fname, "images/project.ico");
                        fitem.Tag = f.FullName;
                        item.Items.Add(fitem);
                    }
                }
                foreach (DirectoryInfo dinfo in di.GetDirectories())
                {
                    getAllfiles(dinfo, ref item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("You can not access this directory");
            }



        }

        private void newPlant()
        {
            System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();

            saveFileDialog1.Filter = "ref files (*.ref)|*.ref";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.Title = "Create New Plant";
            saveFileDialog1.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + ConfigurationManager.AppSettings["version"] + @"\";
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TreeViewItem item = (TreeViewItem)navView.Items[0];
                //CreatePlant plant = new CreatePlant();
                //plant.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                //if (plant.ShowDialog() == true)
                //{
                string refFile = saveFileDialog1.FileName;
                int start_idx = refFile.LastIndexOf(@"\") + 1;
                int end_idx = refFile.LastIndexOf(".");
                string plantName = refFile.Substring(start_idx, end_idx - start_idx);
                string folder = Environment.CurrentDirectory + @"\temp\" + plantName;
                DirectoryInfo dirInfo = System.IO.Directory.CreateDirectory(folder);
                string templatedb = AppDomain.CurrentDomain.BaseDirectory.ToString() + "template.accdb";
                string dbFile = folder + @"\" + dirInfo.Name + ".accdb";
                System.IO.File.Copy(templatedb, dbFile, true);
                CSharpZip.CompressZipFile(folder, refFile);
                TreeViewItem subitem = GetTreeView(plantName, "images/plant.ico");
                subitem.Name = plantName;
                subitem.Tag = folder;
                item.Items.Add(subitem);
                item.ExpandSubtree();
                this.leftdockpanel.Visibility = Visibility.Visible;
                curFolder = folder;
                curRefFile = refFile;
                //}
            }

        }
        private void closePlant()
        {
            TreeViewItem item = (TreeViewItem)navView.Items[0];
            item.Items.Clear();

        }

        private void saveasfile()
        {
            if (Directory.Exists(curFolder + @"\temp"))
                Directory.Delete(curFolder + @"\temp", true);
            CSharpZip.CompressZipFile(curFolder, curRefFile);
            Microsoft.Win32.SaveFileDialog dlgSaveDiagram = new Microsoft.Win32.SaveFileDialog();
            dlgSaveDiagram.Filter = "Ref |*.ref;";
            dlgSaveDiagram.FileName = this.Title + ".ref";
            if (dlgSaveDiagram.ShowDialog() == true)
            {
                File.Copy(curRefFile, dlgSaveDiagram.FileName, true);
            }
        }
        private void savefile()
        {
            if (Directory.Exists(curFolder + @"\temp"))
                Directory.Delete(curFolder + @"\temp",true);
            CSharpZip.CompressZipFile(curFolder, curRefFile);
        }
        
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            //DBRelief dbContext = new DBRelief();
            //DataTable dtSerialNumber= dbContext.getDataByTable("serialnumber", "");
            //string defalutSerialNumber = dtSerialNumber.Rows[0]["serialnumber"].ToString();
            //string confirmSerialNumber = dtSerialNumber.Rows[1]["serialnumber"].ToString();
            //if (defalutSerialNumber != confirmSerialNumber)
            //{
            //    SerialNumber frmSerialNumber = new SerialNumber();
            //    frmSerialNumber.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //    frmSerialNumber.defaultSerialNumber = defalutSerialNumber;                
            //    this.Hide();
            //    if (frmSerialNumber.ShowDialog() == true)
            //    {
            //        this.Show();
            //    }
            //}
            

                this.leftdockpanel.Visibility = Visibility.Hidden;
                this.centerdockpanel.Visibility = Visibility.Hidden;
                string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + ConfigurationManager.AppSettings["version"];
                if (Directory.Exists(path) == false)
                {
                    Directory.CreateDirectory(path);
                }
                // RegistryKey rkTest = Registry.ClassesRoot.OpenSubKey(Environment.CurrentDirectory + @"\Reference\P2Wrap91.dll");
                // if (rkTest == null)
                // {
                //     Process p = new Process();
                //     p.StartInfo.FileName = "Regsvr32.exe";
                //     p.StartInfo.Arguments = "/s " + Environment.CurrentDirectory + @"\Reference\P2Wrap91.dll";//路径中不能有空格
                //     p.Start();
                // }
            
        }
        private TreeViewItem GetTreeView(string text,string imagepath)
        {
            TreeViewItem newTreeViewItem = new TreeViewItem();

            // create stack panel
            StackPanel stack = new StackPanel();
            stack.Orientation = Orientation.Horizontal;
            stack.Height = 20;
            // create Image
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(imagepath, UriKind.Relative));
            image.Width = 16;
            image.Height = 16;
            // Label
            TextBlock lbl = new TextBlock();
            lbl.Text = text;

            // Add into stack
            stack.Children.Add(image);
            stack.Children.Add(lbl);

            // assign stack to header
            newTreeViewItem.Header = stack;

            return newTreeViewItem;
        }
        
        public void addPlant(object sender, RoutedEventArgs e)
        {
            newPlant();
        }
        public void tabRightClick(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            if (item.Header.ToString() == "Close Current Protected System")
            {
                closeProject();
            }
        }

        private void closeProject()
        {            
            if (mainTab.SelectedIndex > -1)
            {
                TabItem ti = (TabItem)mainTab.SelectedItem;
                string name = ti.Header + "1";
                VisioDrawing v = FindName(name) as VisioDrawing;
                string filefullname = ti.Tag.ToString();
                frmMessage msg = new frmMessage();
                msg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                msg.tipcontent.Text = "Save file " + ti.Header.ToString() + "?";
                int status = -1;
                if (msg.ShowDialog() == true)
                {
                    status = msg.status;
                    if (status == 1)
                    {
                        v.visioControl.Document.SaveAs(filefullname);
                        //v.visioControl.Document.Close();
                        mainTab.Items.Remove(ti);
                        UnregisterName(name);
                    }
                    else if (status == 0)
                    {
                        mainTab.Items.Remove(ti);
                        v.visioControl.Src = null;
                        UnregisterName(name);
                    }
                    
                }
            }
        }
        private void closeAllProjects()
        {
            //MessageBox.Show("You can never close this tab");
            //e.Handled = true;
            
            if (mainTab.Items.Count>0)
            {
                frmMessage msg = new frmMessage();
                msg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                msg.tipcontent.Text = "Save all files ?";
                int status = -1;
                if (msg.ShowDialog() == true)
                {
                    status = msg.status;
                    if (status == 1)
                    {
                        for (int i = 0; i < mainTab.Items.Count;i++ )
                        {
                            TabItem ti = (TabItem)mainTab.Items[i];
                            string name = ti.Header + "1";
                            VisioDrawing v = FindName(name) as VisioDrawing;
                            string filefullname = ti.Tag.ToString();
                            v.visioControl.Document.SaveAs(filefullname);
                            //v.visioControl.Document.Close();

                            mainTab.Items.Remove(ti);
                            UnregisterName(name);
                        }
                    }
                    

                }
            }
        }
        public void addProject(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)navView.SelectedItem;
            CreateProject newp = new CreateProject();           
            newp.path = item.Tag.ToString();
            newp.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            newp.Owner = this;
            //DirectoryInfo dir = new DirectoryInfo(newp.path);
            
            if (newp.ShowDialog() == true)
            {
                try
                {
                    //VisioDrawing v = new VisioDrawing();
                    TreeViewItem subitem = GetTreeView(newp.projectName, "images/project.ico");
                    subitem.Name = newp.projectName;
                    subitem.Tag = newp.path;
                    item.Items.Add(subitem);
                    item.ExpandSubtree();

                    /*
                    TabItem ti = new TabItem();
                    ti.Header = subitem.Name;
                    DockPanel sp = new DockPanel();
                    sp.Margin = new Thickness(0, 0, 0, 0);
                    //sp.Orientation = Orientation.Vertical;
                    ti.Content = sp;


                    v.dbFile = dir.FullName + @"\" + dir.Name + ".accdb";
                    ((System.ComponentModel.ISupportInitialize)v).BeginInit();
                    sp.Children.Add(v);
                    ((System.ComponentModel.ISupportInitialize)v).EndInit();
                    curvisio = subitem.Name + "1";
                    if (FindName(curvisio) == null)
                        RegisterName(curvisio, v);
                    if (subitem.Tag != null)
                    {
                        string vname = subitem.Tag.ToString();
                        ti.Tag = vname;
                        try
                        {
                            closeSharedFile(vname);
                            v.vName = vname;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }

                    mainTab.Items.Add(ti);
                    mainTab.SelectedItem = ti;
                    if (this.centerdockpanel.Visibility != Visibility.Visible)
                    {
                        centerdockpanel.Visibility = Visibility.Visible;
                    }
                    */
                }
                catch (Exception ex)
                {
                    MessageBox.Show("The System is busy, please wait a moment");
                    return;
                }
                
            }
            

        }      
        private void navView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
           
            if (navView.SelectedItem == null)
                return;
            TreeViewItem tvi=(TreeViewItem)navView.SelectedItem;
            if (tvi.Tag == null || tvi.Tag.ToString().Contains(".vsd") == false)
            {
                return;
            }
            StackPanel panel=(StackPanel)tvi.Header;
            TextBlock tbk=(TextBlock)panel.Children[1];
            string header=tbk.Text;
            string name = header;
            bool isopen = false;
            foreach (object o in mainTab.Items)
            {

                TabItem item = o as TabItem;
                if (item != null)
                {
                    if (item.Header.ToString().CompareTo(header) == 0)
                    {
                        isopen = true;
                        mainTab.SelectedItem = item;
                        curvisio = name+"1";
                        break;
                    }
                }
            }
            
            if (!isopen)
            {
                TabItem ti = new TabItem();
                ti.Header = header;
                ti.Tag = tvi.Tag;
                DockPanel sp = new DockPanel();
                sp.Margin = new Thickness(0, 0, 0, 0);
                //sp.Orientation = Orientation.Vertical;
                ti.Content = sp;
                string vsdFile=ti.Tag.ToString();
                try
                {
                    
                    if (tvi.Tag != null)
                    {
                        string vname = tvi.Tag.ToString();
                        ti.Tag = vname;
                        try
                        {
                            VisioDrawing v = new VisioDrawing();

                            string dir = System.IO.Path.GetDirectoryName(vsdFile);
                            DirectoryInfo dirInfo = new DirectoryInfo(dir);

                            v.dbFile = dir + @"\" + dirInfo.Name + ".accdb";
                            ((System.ComponentModel.ISupportInitialize)v).BeginInit();
                            sp.Children.Add(v);
                            ((System.ComponentModel.ISupportInitialize)v).EndInit();
                            curvisio = name + "1";
                            if (FindName(curvisio) == null)
                                RegisterName(curvisio, v);
                            closeSharedFile(vname);
                            v.vName = vname;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                    mainTab.Items.Add(ti);
                    mainTab.SelectedItem = ti;
                }
                catch (Exception ex2)
                {
                    MessageBox.Show(ex2.ToString());
                }

            }
            if (this.centerdockpanel.Visibility != Visibility.Visible)
            {
                centerdockpanel.Visibility = Visibility.Visible;
            }
            


        }
              
        private List<ShapeInfo> getAllConnectors(VisioDrawing v)
        {
            List<ShapeInfo> list = new List<ShapeInfo>();
            AxDrawingControl dc = v.visioControl;
            Visio.Page currentPage = dc.Window.Application.ActivePage;
            foreach (Visio.Shape shp in currentPage.Shapes)
            {
                if (shp.NameU.Contains("Connector"))
                {
                    ShapeInfo si = new ShapeInfo();
                    si.NameU = shp.NameU;
                    si.Text = shp.Text;
                    list.Add(si);
                }
            }
            return list;
        }

        private static void ConnectShapes( Visio.Shape shape,int connectPoint,Visio.Shape connector,int direction)
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
            (short)connectPoint, 0 );

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
                connector.get_Cells("EndArrow").Formula = "5";
                endXCell.GlueTo(fromCell);
            }
            else
            {
                connector.get_Cells("EndArrow").Formula = "=5";
                beginXCell.GlueTo(fromCell);
            }
            //connector.get_Cells("LineColor").Formula = "3";

        }

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Marshal.FinalReleaseComObject(cp2Srv);
            //GC.ReRegisterForFinalize(cp2Srv);
            //closeAllProjects();
            if (curFolder != string.Empty)
            {
                if (Directory.Exists(curFolder + @"\temp"))
                    Directory.Delete(curFolder + @"\temp",true);
                CSharpZip.CompressZipFile(curFolder, curRefFile);
                Directory.Delete(curFolder, true);
            }
            Environment.Exit(0);
            
        }
             
        public void importData(object sender, RoutedEventArgs e)
        {
            if (navView.SelectedItem == null)
                return;
            TreeViewItem tvi = (TreeViewItem)navView.SelectedItem;
            ImportData imptdata = new ImportData();
            imptdata.dirInfo = tvi.Tag.ToString();
            imptdata.WindowStartupLocation = WindowStartupLocation.CenterScreen;
           
            imptdata.Owner = this;
            imptdata.ShowDialog();
            
        }

        
        private void closeSharedFile(string fileName)
        {
            IntPtr vHandle = _lopen(fileName, OF_READWRITE | OF_SHARE_DENY_NONE);
            if (vHandle == HFILE_ERROR)
            {
                MessageBox.Show("文件被占用！");
                return;
            }
            else
                CloseHandle(vHandle); 
        }      
       
        private void FlashCal()
        {
            FlashCalculation frm = new FlashCalculation();
            frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            if (mainTab.SelectedIndex > -1)
            {
                TabItem ti = (TabItem)mainTab.SelectedItem;
                string vsdFile = ti.Tag.ToString();
                frm.dbFile = vsdFile.Substring(0, vsdFile.Length - 3) + "accdb";
                if (frm.ShowDialog() == true)
                {

                }
            }
        }

        private void TreeViewItem_PreviewMouseRightButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            var treeViewItem = VisualUpwardSearch<TreeViewItem>(e.OriginalSource as DependencyObject) as TreeViewItem;
            if (treeViewItem != null)
            {
                treeViewItem.Focus();
                e.Handled = true;
                string selItem = @"c:\";
                if (treeViewItem.Tag != null)
                {
                    selItem = treeViewItem.Tag.ToString();
                }
                ContextMenu rmenu = (ContextMenu)this.Resources["RightContextMenu"];
                if (File.Exists(selItem))
                {                  
                    for (int i = 0; i < 3; i++)
                    {
                        MenuItem item = (MenuItem)rmenu.Items[i];
                        item.IsEnabled = false;
                    }
                }
                else if (Directory.Exists(selItem))
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(selItem);
                    DirectoryInfo[] subDir = dirInfo.GetDirectories();
                    if (subDir.Length > 0)
                    {
                        MenuItem item = (MenuItem)rmenu.Items[0];
                        item.IsEnabled = true;
                        for (int i = 1; i < 3; i++)
                        {
                            item = (MenuItem)rmenu.Items[i];
                            item.IsEnabled = false;
                        }
                    }
                    else
                    {
                        MenuItem item = (MenuItem)rmenu.Items[0];
                        item.IsEnabled = false;
                        for (int i = 1; i < 3; i++)
                        {
                            item = (MenuItem)rmenu.Items[i];
                            item.IsEnabled = true;
                        }
                    }

                }
                
            }
        }
        static DependencyObject VisualUpwardSearch<T>(DependencyObject source)
        {
            while (source != null && source.GetType() != typeof(T))
                source = VisualTreeHelper.GetParent(source);

            return source;
        }

        private void navView_Loaded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)navView.Items[0];
            string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + ConfigurationManager.AppSettings["version"] + @"\";
            item.Tag = path;
            item.Name = "Relief1";
            
        }
    }
    public class LVData
    {

        public string Name { get; set; }

        public string Pic { get; set; }      
    }
    public class ShapeInfo
    {
        public string NameID { get; set; }
        public string NameU { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }     
    }
   

}
