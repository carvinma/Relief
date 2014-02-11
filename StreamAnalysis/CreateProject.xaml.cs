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
using System.IO;



namespace ReliefAnalysis
{
    /// <summary>
    /// NewProject.xaml 的交互逻辑
    /// </summary>
    public partial class CreateProject : Window
    {
        public CreateProject()
        {
            InitializeComponent();
        }
        public string projectName = "";
        public string path = "";
        
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                projectName = txtName.Text.Trim();
                path = path + "\\" + projectName + ".vsd";
                if (projectName == string.Empty)
                {
                    MessageBox.Show("Project Name can not be empty", "操作提示");
                }
                else
                {
                    if (System.IO.File.Exists(path))
                    {
                        MessageBox.Show("This Project is exist", "操作提示");
                    }
                    else
                    {
                        //using (System.IO.FileStream fs = System.IO.File.Create(path))
                        //{
                        //    fs.Close();
                        //    fs.Dispose();
                        //}
                        string vsd = AppDomain.CurrentDomain.BaseDirectory.ToString() + "/template.vsd";
                        System.IO.File.Copy(vsd, path);



                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Action");
            }

            this.DialogResult = true;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            txtName.Focusable=true;
            Keyboard.Focus(txtName);
        }
    }
}
