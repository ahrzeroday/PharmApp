using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
    public partial class Admission_page : Window
    {
        private List<string> dNamelist = new List<string>();
        private List<int> dCodelist = new List<int>();
        public Admission_page()
        {
            InitializeComponent();
            InitHeader();
            initDataTemp();

            DataTable dt = Helper.GetTable("داروی شرکت", "'کد یکتای دارو','نام دارو','نام شرکت'");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dCodelist.Add(int.Parse(dt.Rows[i]["کد یکتای دارو"].ToString()));
                dNamelist.Add(dt.Rows[i]["کد یکتای دارو"].ToString() + "-" + dt.Rows[i]["نام دارو"].ToString());

            }
            dt = Helper.GetTable("شرکت بیمه", "'نام شرکت'");
            List<string> temp = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                temp.Add(dt.Rows[i]["نام شرکت"].ToString());


            }
            dName.ItemsSource = dNamelist;
            cName.ItemsSource = temp;
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
                Close();
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
        private void autodetect(object sender, KeyEventArgs e)
        {
            ComboBox? cmb = (ComboBox)sender;
            cmb.IsDropDownOpen = true;

            string? textbox = (cmb.Template.FindName("PART_EditableTextBox", cmb) as TextBox).Text;
            cmb.ItemsSource = dNamelist.Where(p => string.IsNullOrEmpty(cmb.Text) || p.ToLower().Contains(textbox.ToLower())).ToList();

        }
        private void Bime_DropDownClosed(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Bime.Text.Contains("آزاد"))
                {
                    ID.IsEnabled = true;
                    Name.IsEnabled = true;
                    cName.IsEnabled = false;
                }
                else if (Bime.Text.Contains("بیمه درمان"))
                {
                    ID.IsEnabled = true;
                    Name.IsEnabled = true;
                    cName.IsEnabled = true;
                }
                else if (Bime.Text.Contains("OTC"))
                {
                    ID.IsEnabled = true;
                    Name.IsEnabled=false;
                    cName.IsEnabled=false;
                }
            }
            catch
            {

            }
        }
        private bool checkinput()
        {
            if (string.IsNullOrEmpty(dName.Text) && string.IsNullOrEmpty(cName.Text) && string.IsNullOrEmpty(dCode.Text))
            {
                MessageBox.Show("لطفا ورودی ها را بررسی کنید");
                return false;
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



                return true;
            }
        }

        #endregion

        #region Object
        public class Drag
        {
            public int ocode { get; set; }
            public int code { get; set; }
            public int listnum { get; set; }
            public string Name { get; set; }
            public int ID { get; set; }
            public DateTime bTime { get; set; }
            public string bime { get; set; }
            public string cName { get; set; }
            public string Description { get; set; }
            public string dName { get; set; }
            public string eTime { get; set; }
            public int count { get; set; }
        }
        #endregion
        #region DataTemp

        private void initDataTemp()
        {
            tempData.Columns.Add(new DataGridTextColumn() { Header = "کد یکتای دارو", Binding = new Binding("ocode"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "کد دارو", Binding = new Binding("code"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "شماره لیست", Binding = new Binding("listnum"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "نام بیمار", Binding = new Binding("Name"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "کد ملی", Binding = new Binding("ID"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "تاریخ تولد", Binding = new Binding("bTime"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "نوع بیمار", Binding = new Binding("bime"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "نوع بیمه", Binding = new Binding("cName"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "توضیحات", Binding = new Binding("Description"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "نام دارو", Binding = new Binding("dName"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "تاریخ انقضا", Binding = new Binding("eTime"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "مقدار", Binding = new Binding("count"), IsReadOnly = true });

        }

        #endregion

        private void DeleteTempdata(object sender, RoutedEventArgs e)
        {

            tempData.Items.Clear();

        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mr = MessageBox.Show("آیا میخواهید از صفحه خارج شوید؟", "اخطار", MessageBoxButton.YesNo);
            if (mr.HasFlag(MessageBoxResult.Yes))
            {
                this.Close();
            }
        }

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {

        }
    }
}
