using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace PharmApp
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Add_medicine_page : Window
    {
        public Add_medicine_page()
        {
            InitializeComponent();
            InitHeader();
            initDataTemp();

        }
        #region InitHeader

        private void InitHeader()
        {
            bool restoreIfMove = false;

            Header.MouseLeftButtonDown += (s, e) =>
            {
                if (e.ClickCount == 2)
                {
                    if ((ResizeMode == ResizeMode.CanResize) ||
                        (ResizeMode == ResizeMode.CanResizeWithGrip))
                    {
                        SwitchState();
                    }
                }
                else
                {
                    if (WindowState == WindowState.Maximized)
                    {
                        restoreIfMove = true;
                    }

                    DragMove();
                }
            };
            Header.MouseLeftButtonUp += (s, e) =>
            {
                restoreIfMove = false;
            };
            Header.MouseMove += (s, e) =>
            {
                if (restoreIfMove)
                {
                    restoreIfMove = false;
                    double mouseX = e.GetPosition(this).X;
                    double width = RestoreBounds.Width;
                    double x = mouseX - width / 2;

                    if (x < 0)
                    {
                        x = 0;
                    }
                    else
                    if (x + width > System.Windows.SystemParameters.WorkArea.Width)
                    {
                        x = System.Windows.SystemParameters.WorkArea.Width - width;
                    }

                    WindowState = WindowState.Normal;
                    Left = x;
                    Top = 0;
                    DragMove();
                }
            };
            Close.MouseUp += (s, e) =>
            {
                this.Close();
            };
            Min.MouseUp += (s, e) =>
            {
                WindowState = WindowState.Minimized;
            };
        }

        private void SwitchState()
        {
            switch (WindowState)
            {
                case WindowState.Normal:
                    {
                        WindowState = WindowState.Maximized;
                        break;
                    }
                case WindowState.Maximized:
                    {
                        WindowState = WindowState.Normal;
                        break;
                    }
            }
        }

        #endregion

        #region Rules
        private List<int> allcode = new List<int>();
        private int tempnum = 1;
        private void OnlyNumAccept(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private int dragcode()
        {
            if (dCode.Text.Contains("0"))
            {
                
                while (true)
                {
                    if (!allcode.Contains(tempnum))
                    {
                        return tempnum;
                    }
                    else
                    {
                        tempnum++;
                    }
                }
            }
            else
            {
                return int.Parse(dCode.Text);
            }
        }
        private bool checkinput(bool edit)
        {
            if (string.IsNullOrEmpty(dName.Text) || string.IsNullOrEmpty(cName.Text) || string.IsNullOrEmpty(dCode.Text) || string.IsNullOrEmpty(price.Text) )
            {
                MessageBox.Show("لطفا ورودی ها را بررسی کنید");
                return false;
            }
            else
            {
                if (!edit)
                {
                    if (dCode.Text.Contains("0"))
                    {
                        return true;
                    }
                    else
                    {
                        foreach (Drag item in tempData.Items)
                        {
                            if (item.code == int.Parse(dCode.Text))
                            {
                                MessageBox.Show("لطفا کد دارو را مجدد بررسی کنید");
                                return false;
                            }
                        }
                    }
                }
               
                

                return true;
            }
        }

        #endregion

        #region DataTemp

        private void initDataTemp()
        {
            tempData.Columns.Add(new DataGridTextColumn() {Header="کد یکتا دارو",Binding= new Binding("code"), IsReadOnly=true});
            tempData.Columns.Add(new DataGridTextColumn() {Header="نام دارو", Binding = new Binding("Name"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() {Header="نام شرکت", Binding = new Binding("CompanyName"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() {Header="نرخ آزاد", Binding = new Binding("Amoney"), IsReadOnly = true });
        }

        #endregion

        public class Drag
        {
            public int code { get; set; }
            public string Name { get; set; }
            public string CompanyName { get; set; }
            
            public int Amoney { get; set; }
        }

        private void AddTempData(object sender, RoutedEventArgs e)
        {
            if (!checkinput(false))
            {
                return;
            }
            int newcode = dragcode();
            allcode.Add(newcode);
            tempData.Items.Add(new Drag() { Name=dName.Text,code= newcode, CompanyName= cName.Text, Amoney= int.Parse( price.Text) });
        }
        private void EditTempData(object sender, RoutedEventArgs e)
        {
           
            try
            {
                if (!checkinput(true))
                {
                    return;
                }
                int index = tempData.SelectedIndex;
                tempData.Items.RemoveAt(index);
                tempData.Items.Insert(index, new Drag() { Name = dName.Text, code = dragcode(), CompanyName = cName.Text, Amoney = int.Parse(price.Text) });
            }
            catch
            {
                MessageBox.Show("لطفا آیتم مورد نظر جهت تغییر را از جدول زیر انتخاب کنید");
            }
           
        }
        private void AddTempToReal(object sender, RoutedEventArgs e)
        {
            foreach(Drag item in tempData.Items)
            {
                Helper.Insert("داروی شرکت", $"{item.code},'{item.Name}','{item.CompanyName}',{item.Amoney}");
            }
            
           
        }   
        private void DeleteTempdata(object sender, RoutedEventArgs e)
        {

            tempData.Items.Clear();
           
        }

        private void tempData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Drag row = tempData.SelectedItem as Drag;
                if (row == null) return;
                dName.Text = row.Name;
                dCode.Text = row.code.ToString();
                cName.Text = row.CompanyName;
                price.Text = row.Amoney.ToString();
            }
            catch
            {
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
             MessageBoxResult mr = MessageBox.Show("آیا میخواهید از صفحه خارج شوید؟", "اخطار", MessageBoxButton.YesNo);
            if (mr.HasFlag(MessageBoxResult.Yes))
            {
                this.Close();
            }
        }
    }
}
