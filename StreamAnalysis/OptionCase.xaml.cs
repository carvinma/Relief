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

namespace ReliefAnalysis
{
    /// <summary>
    /// OptionDuty.xaml 的交互逻辑
    /// </summary>
    public partial class OptionCase : Window
    {
        public OptionCase()
        {
            InitializeComponent();
        }

        public List<int> items = new List<int>();
        public string dbFile = string.Empty;
        public string eqName = string.Empty;
        public double Prelief = 0;
        public string przFile;
        public string vsdFile;

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                getChekedItems();
                CaseStudy frm = new CaseStudy();
                frm.Owner = this.Owner;
                frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                frm.dbFile = dbFile;
                frm.eqName = eqName;
                frm.dispCase = items;
                frm.Prelief = Prelief;
                frm.przFile = przFile;
                frm.vsdFile = vsdFile;
                this.Hide();
                frm.ShowDialog();
            
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Action");
            }
            
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void getChekedItems()
        {
            DBRelief dbr = new DBRelief(dbFile);
            DataTable dt = dbr.getStructure("frmoptionduty");
            IEnumerable<CheckBox> collection = gridDuty.FindChildren<CheckBox>();
            foreach (CheckBox chk in collection)
            {
                if (chk.IsChecked == true)
                {
                    string strIdx = chk.Name.Substring(3);
                    items.Add(int.Parse(strIdx));
                    DataRow dr = dt.NewRow();
                    dr["case_id"] = strIdx;
                    dr["visiofile"] = vsdFile;
                    dt.Rows.Add(dr);

                    if (strIdx == "3")
                    {
                        for (int i = 14; i <= 18; i++)
                        {
                             dr = dt.NewRow();
                            dr["case_id"] = i;
                            dr["visiofile"] = vsdFile;
                            dt.Rows.Add(dr);
                            items.Add(i);
                        }
                    }
                }
            }
            dbr.saveDataByTable(dt,vsdFile);
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            int[] partiStop={14,15,16,17,18};
            DBRelief dbr = new DBRelief(dbFile);
            IEnumerable<CheckBox> collection = gridDuty.FindChildren<CheckBox>();
            DataTable dt = dbr.getDataByVsdFile("frmoptionduty", vsdFile);
            for(int i=0;i<dt.Rows.Count;i++)
            {
                DataRow dr=dt.Rows[i];
                int idx=int.Parse(dr["case_id"].ToString());
                if (!partiStop.Contains(idx))
                {
                    CheckBox chk = collection.ElementAt(idx - 1);
                    chk.IsChecked = true;
                }
                
            }
        }
       
    }
    public static class ControlHelper
    {
         /// <summary>
        /// Analyzes both visual and logical tree in order to find all elements
        /// of a given type that are descendants of the <paramref name="source"/>
        /// item.
        /// </summary>
        /// <typeparam name="T">The type of the queried items.</typeparam>
        /// <param name="source">The root element that marks the source of the
        /// search. If the source is already of the requested type, it will not
        /// be included in the result.</param>
        /// <returns>All descendants of <paramref name="source"/> that match the
        /// requested type.</returns>
        public static  IEnumerable<T> FindChildren<T>(this DependencyObject source)
                                                     where T : DependencyObject
        {
            if (source != null)
            {
                var childs = GetChildObjects(source);
                foreach (DependencyObject child in childs)
                {
                    //analyze if children match the requested type
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    //recurse tree
                    foreach (T descendant in FindChildren<T>(child))
                    {
                        yield return descendant;
                    }
                }
            }
        }


        /// <summary>
        /// This method is an alternative to WPF's
        /// <see cref="VisualTreeHelper.GetChild"/> method, which also
        /// supports content elements. Do note, that for content elements,
        /// this method falls back to the logical tree of the element.
        /// </summary>
        /// <param name="parent">The item to be processed.</param>
        /// <returns>The submitted item's child elements, if available.</returns>
        public static IEnumerable<DependencyObject> GetChildObjects(
                                                    this DependencyObject parent)
        {
            if (parent == null) yield break;


            if (parent is ContentElement || parent is FrameworkElement)
            {
                //use the logical tree for content / framework elements
                foreach (object obj in LogicalTreeHelper.GetChildren(parent))
                {
                    var depObj = obj as DependencyObject;
                    if (depObj != null) yield return (DependencyObject)obj;
                }
            }
            else
            {
                //use the visual tree per default
                int count = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < count; i++)
                {
                    yield return VisualTreeHelper.GetChild(parent, i);
                }
            }
        }
    }
}
