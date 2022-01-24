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
    public partial class Refer_to_the_seller_page : Window
    {
        private List<string> dNamelist = new List<string>();
        private List<int> dCodelist = new List<int>();
        DataTable dt = new DataTable();
        public Refer_to_the_seller_page()
        {
            InitializeComponent();
            InitHeader();
            initDataTemp();
            dt = Helper.GetTable("ارجاع دارو به شرکت", "'شماره ارجاع'");
            Helper.alllistsCode.Clear();
            foreach(DataRow dr in dt.Rows)
            {
                Helper.alllistsCode.Add(int.Parse(dr["شماره ارجاع"].ToString()));
            }

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

        #region DataTemp

        private void initDataTemp()
        {
            tempData.Columns.Add(new DataGridTextColumn() { Header = "کد یکتای دارو", Binding = new Binding("dCode"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "کد دارو", Binding = new Binding("code"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "شماره ارجاع", Binding = new Binding("sCode"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "زمان ثبت خرید", Binding = new Binding("date_sendback"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "توضیحات", Binding = new Binding("Description"), IsReadOnly = true });
        }

        #endregion


        #region Rules

        private void AddTempData(object sender, RoutedEventArgs e)
        {
            if (!checkinput())
            {
                return;
            }
            dt = Helper.Multiply("دارو موجود در انبار", "خرید دارو از شرکت","'کد یکتای دارو',a.'کد دارو'", $"'شماره فاکتور' == {fCode.Text} and a.'کد دارو' == b.'کد دارو' and b.'کد یکتای دارو' == {dName.Text.Split("-")[0]}");
            tempData.Items.Add(new Drag() { sCode = Helper.GetNewsCode(), dCode =int.Parse( dt.Rows[0]["کد دارو"].ToString()) ,code =int.Parse( dt.Rows[0]["کد دارو"].ToString()), date_sendback = DateTime.Parse(date_sendback.Text).Date, Description = Description.Text});
        }
        private void AddTempToReal(object sender, RoutedEventArgs e)
        {
            foreach (Drag item in tempData.Items)
            {
                Helper.Insert("ارجاع دارو به شرکت", $"{item.dCode},{item.code},{item.sCode},'{item.date_sendback}','{item.Description}'");
                Helper.DeleteT("دارو موجود در انبار", $"'کد دارو'=={item.code}");
                Helper.DeleteT("تاریخ انقضا داروی موجود در انبار", $"'کد دارو'=={item.code}"); 

            }
            MessageBox.Show("اطلاعات ذخیره شد");

        }
        private void OnlyNumAccept(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void autodetect(object sender, KeyEventArgs e)
        {
            try
            {
                ComboBox? cmb = (ComboBox)sender;
                cmb.IsDropDownOpen = true;

                string? textbox = (cmb.Template.FindName("PART_EditableTextBox", cmb) as TextBox).Text;
                cmb.ItemsSource = dNamelist.Where(p => string.IsNullOrEmpty(cmb.Text) || p.ToLower().Contains(textbox.ToLower())).ToList();

            }
            catch
            {

            }
        

        }
        private bool checkinput()
        {
            if (string.IsNullOrEmpty(dName.Text) &&  string.IsNullOrEmpty(fCode.Text) )
            {
                MessageBox.Show("لطفا ورودی ها را بررسی کنید");
                return false;
            }
            else
            {

                //foreach (Drag item in tempData.Items)
                //{
                //    if (item.code == int.Parse(dCode.Text))
                //    {
                //        MessageBox.Show("لطفا کد دارو را مجدد بررسی کنید");
                //        return false;
                //    }
                //}



                return true;
            }
        }
        #endregion

        #region Object

        public class Drag
        {
            public int dCode { get; set; }
            public int code { get; set; }
            public int sCode { get; set; }
            public DateTime date_sendback { get; set; }
            public string Description { get; set; }
        }

        #endregion

        private void DeleteTempdata(object sender, RoutedEventArgs e)
        {

            tempData.Items.Clear();

        }
        private void Delete_onetemp(object sender, RoutedEventArgs e)
        {

            try
            {
                int index = tempData.SelectedIndex;
                tempData.Items.RemoveAt(index);
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
        private void fCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                DataTable fdt = Helper.Multiply("داروی شرکت", "خرید دارو از شرکت", "'شماره فاکتور','نام شرکت'",$"'شماره فاکتور' == {fCode.Text} " );
                sendback.Content=fdt.Rows[0]["نام شرکت"].ToString();
                DataTable dt = Helper.Multiply_dis("خرید دارو از شرکت", "دارو موجود در انبار", "'نام دارو','کد یکتای دارو'", $"'شماره فاکتور'=={fCode.Text}");
                dNamelist.Clear();
                dName.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    dNamelist.Add(dt.Rows[i]["کد یکتای دارو"].ToString() + "-"+dt.Rows[i]["نام دارو"].ToString());
                    dName.Items.Add(dt.Rows[i]["کد یکتای دارو"].ToString() + "-" + dt.Rows[i]["نام دارو"].ToString());

                }
            }
            catch
            {

            }
        }
        
    }
}
