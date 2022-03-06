using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp5
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    /// 

    public partial class UserControl1 : UserControl
    {
        public bool isClickBack = false;
        public UserControl1()
        {
            
            InitializeComponent();
            DataGridInfo.Items.SortDescriptions.Clear();
            DataGridInfo.Items.SortDescriptions.Add(new SortDescription(DataGridInfo.Columns[2].SortMemberPath, ListSortDirection.Descending));

        }
        public class File
        {
            public int Icon { get; set; }
            public string Name { get; set; }
            public string Path { get; set; }
            public string Size { get; set; }
            public string Date { get; set; }
           
        }
        public  void SetData(int icon,string name,string path,string size, string date)
        {
            DataGridInfo.Items.Add(new File() { Icon = icon, Name = name, Path = path, Size = size, Date = date });

            
        }
        public  void DataClear()
        {
            DataGridInfo.Items.Clear();
            
        }
        MainWindow m;
        public void getMain( MainWindow main)
        {
            m = main;
        }
     
        public void Datagird_Doubleclick(object sender, MouseButtonEventArgs e)
        {
            
            try
            {
                
                 var row_list = (File)DataGridInfo.SelectedItem;
                if (row_list != null)
                {
                    if (row_list.Size == "Folder")
                    {
                            m.BackPath(row_list.Path);
                        
                    }
                    else
                    {
                        m.UseOpenManager(row_list.Path);
                    }
                }
                else
                {
                    return;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            
        }
    }
}
