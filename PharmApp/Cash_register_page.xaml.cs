using System;
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
    public partial class Cash_register_page : Window
    {
        private string Bime;
        private int listnum;
        private int customercode;
        public Cash_register_page()
        {
            InitializeComponent();
            InitHeader();

            DataTable dt = Helper.GetTable("صندوق", "'کد پرداخت'");
            Helper.alllistbuycode.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                Helper.alllistbuycode.Add(int.Parse(dr["کد پرداخت"].ToString()));
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

        #region DataTemp

        private void initDataTemp()
        {
            tempData.Items.Clear();
            tempData.Columns.Clear();
            if (Bime.Contains("آزاد"))
            {
                tempData.Columns.Add(new DataGridTextColumn() { Header = "شماره قبض ", Binding = new Binding("CustomerCode"), IsReadOnly = true });
                tempData.Columns.Add(new DataGridTextColumn() { Header = "شماره لیست ", Binding = new Binding("listCode"), IsReadOnly = true });
                tempData.Columns.Add(new DataGridTextColumn() { Header = "کد دارو", Binding = new Binding("code"), IsReadOnly = true });
                tempData.Columns.Add(new DataGridTextColumn() { Header = "نام دارو ", Binding = new Binding("Name"), IsReadOnly = true });
                tempData.Columns.Add(new DataGridTextColumn() { Header = "قیمت ", Binding = new Binding("value"), IsReadOnly = true });
                tempData.Columns.Add(new DataGridTextColumn() { Header = "مقدار ", Binding = new Binding("count"), IsReadOnly = true });
                tempData.Columns.Add(new DataGridTextColumn() { Header = "حق فنی", Binding = new Binding("hfani"), IsReadOnly = true });
                tempData.Columns.Add(new DataGridTextColumn() { Header = "زمان ثبت خرید", Binding = new Binding("fTime"), IsReadOnly = true });
            }
            else if (Bime.Contains("بیمه"))
            {
                tempData.Columns.Add(new DataGridTextColumn() { Header = "شماره قبض ", Binding = new Binding("CustomerCode"), IsReadOnly = true });
                tempData.Columns.Add(new DataGridTextColumn() { Header = "شماره لیست ", Binding = new Binding("listCode"), IsReadOnly = true });
                tempData.Columns.Add(new DataGridTextColumn() { Header = "کد دارو", Binding = new Binding("code"), IsReadOnly = true });
                tempData.Columns.Add(new DataGridTextColumn() { Header = "نام دارو ", Binding = new Binding("Name"), IsReadOnly = true });
                tempData.Columns.Add(new DataGridTextColumn() { Header = "قیمت ", Binding = new Binding("value"), IsReadOnly = true });
                tempData.Columns.Add(new DataGridTextColumn() { Header = "مقدار ", Binding = new Binding("count"), IsReadOnly = true });
                tempData.Columns.Add(new DataGridTextColumn() { Header = "حق فنی", Binding = new Binding("hfani"), IsReadOnly = true });
                tempData.Columns.Add(new DataGridTextColumn() { Header = "تخفیف بیمه", Binding = new Binding("off"), IsReadOnly = true });
                tempData.Columns.Add(new DataGridTextColumn() { Header = "زمان ثبت خرید", Binding = new Binding("fTime"), IsReadOnly = true });
            }
            else if (Bime.Contains("OTC"))
            {
                tempData.Columns.Add(new DataGridTextColumn() { Header = "شماره قبض ", Binding = new Binding("CustomerCode"), IsReadOnly = true });
                tempData.Columns.Add(new DataGridTextColumn() { Header = "شماره لیست ", Binding = new Binding("listCode"), IsReadOnly = true });
                tempData.Columns.Add(new DataGridTextColumn() { Header = "کد دارو", Binding = new Binding("code"), IsReadOnly = true });
                tempData.Columns.Add(new DataGridTextColumn() { Header = "نام دارو ", Binding = new Binding("Name"), IsReadOnly = true });
                tempData.Columns.Add(new DataGridTextColumn() { Header = "قیمت ", Binding = new Binding("value"), IsReadOnly = true });
                tempData.Columns.Add(new DataGridTextColumn() { Header = "مقدار ", Binding = new Binding("count"), IsReadOnly = true });
                tempData.Columns.Add(new DataGridTextColumn() { Header = "حق فنی", Binding = new Binding("hfani"), IsReadOnly = true });
                tempData.Columns.Add(new DataGridTextColumn() { Header = "حق OTC", Binding = new Binding("hOTC"), IsReadOnly = true });
                tempData.Columns.Add(new DataGridTextColumn() { Header = "زمان ثبت خرید", Binding = new Binding("fTime"), IsReadOnly = true });
            }
        }

        #endregion


        #region Rules

        private void OnlyNumAccept(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
         private bool checkinput()
        {
            if (string.IsNullOrEmpty(customerNum.Text) )
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
            public int code { get; set; }
            public int fCode { get; set; }
            public string Name { get; set; }
            public int hOTC { get; set; }
            public int hfani { get; set; }
            public int CustomerCode { get; set; }     
            public int listCode { get; set; }
            public int value { get; set; }  
            public int count { get; set; }
            public int off { get; set; }
            public string fTime { get; set; }
        }

        #endregion
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mr = MessageBox.Show("آیا میخواهید از صفحه خارج شوید؟", "اخطار", MessageBoxButton.YesNo);
            if (mr.HasFlag(MessageBoxResult.Yes))
            {
                Close();
            }
        }
        private void BuyComplete(object sender, RoutedEventArgs e)
        {
            try
            {
                int buycode = Helper.GetNewbuycode();
                Helper.Insert("صندوق", $"{listnum},{customercode},'{TotalSum.Content}',{buycode},{Helper.PersonalID},'{ DateTime.Now.ToString("yyyy-MM-dd")}'");

                MessageBox.Show($"کد پرداخت : {buycode}");

            }
            catch
            {
                MessageBox.Show("ارور!!");
            }
           




        }

        private void customerNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                nBime.Content = "--";
                ID.Content = "--";
                cName.Content = "--";
                Name.Content = "--";
                Sum.Content = "0";
                TotalSum.Content = "0";
                hFani.Content = "0";
                hOTC.Content = "0";
                OFF.Content = "0";
                listnum = int.Parse(customerNum.Text.Split("-")[0]);
                customercode = int.Parse(customerNum.Text.Split("-")[1]);
                DataTable dt = Helper.Search("پذیرش", "'نوع بیمار'", $"'شماره لیست'=={listnum}");
                Bime = dt.Rows[0]["نوع بیمار"].ToString();
                initDataTemp();
                if (Bime.Contains("آزاد"))
                {
                    dt = Helper.Search("مشتری با نسخه", "*", $"'شماره لیست'=={listnum} and 'شماره قبض'=={customercode}");
                    nBime.Content = Bime;
                    ID.Content = dt.Rows[0]["کد ملی"].ToString();
                    Name.Content = dt.Rows[0]["نام و نام خانوادگی"].ToString();
                }
                else if (Bime.Contains("بیمه درمان"))
                {
                    dt = Helper.Search("مشتری با بیمه", "*", $"'شماره لیست'=={listnum} and 'شماره قبض'=={customercode}");
                    nBime.Content = Bime;
                    ID.Content = dt.Rows[0]["کد ملی"].ToString();
                    Name.Content = dt.Rows[0]["نام و نام خانوادگی"].ToString();
                    cName.Content = dt.Rows[0]["نام شرکت"].ToString();
                }
                else if (Bime.Contains("OTC"))
                {
                    dt = Helper.Search("مشتری OTC", "*", $"'شماره لیست'=={listnum} and 'شماره قبض'=={customercode}");
                    nBime.Content = Bime;
                    ID.Content = dt.Rows[0]["کد ملی"].ToString();
                }

                dt = Helper.Triply("دارو موجود در انبار", "انتخاب می شود", "خرید دارو از شرکت", "b.'کد دارو','نام دارو','حق فنی','حق OTC',b.'مقدار','قیمت فروش',c.'کد یکتای دارو'", $"a.'کد دارو' == b.'کد دارو' and a.'کد دارو' == c.'کد دارو' and b.'شماره لیست'={listnum}");
                
                foreach(DataRow row in dt.Rows)
                {
                    if (Bime.Contains("آزاد"))
                    {
                        tempData.Items.Add(new Drag() { CustomerCode = customercode, listCode = listnum, Name = row["نام دارو"].ToString(), code = int.Parse(row["کد دارو"].ToString()), value = int.Parse(row["قیمت فروش"].ToString()), count = int.Parse(row["مقدار"].ToString()), hfani = int.Parse(row["حق فنی"].ToString()), fTime = DateTime.Now.ToString("yyyy-MM-dd") });
                    }
                    else if (Bime.Contains("بیمه درمان"))
                    {
                        DataTable bime = Helper.Search("داروی تحت بیمه", "'تخفیف'",$"'نام شرکت'=='{cName.Content}' and 'کد یکتای دارو'=={row["کد یکتای دارو"]}");
                        if(bime.Rows.Count > 0)
                        {
                            tempData.Items.Add(new Drag() { CustomerCode = customercode, listCode = listnum,Name= row["نام دارو"].ToString(), code = int.Parse(row["کد دارو"].ToString()), value = int.Parse(row["قیمت فروش"].ToString()), count = int.Parse(row["مقدار"].ToString()), hfani = int.Parse(row["حق فنی"].ToString()), off =int.Parse(bime.Rows[0]["تخفیف"].ToString()), fTime = DateTime.Now.ToString("yyyy-MM-dd") });

                        }
                        else
                        {
                            tempData.Items.Add(new Drag() { CustomerCode = customercode, listCode = listnum, Name = row["نام دارو"].ToString(), code = int.Parse(row["کد دارو"].ToString()), value = int.Parse(row["قیمت فروش"].ToString()), count = int.Parse(row["مقدار"].ToString()), hfani = int.Parse(row["حق فنی"].ToString()), off =0, fTime = DateTime.Now.ToString("yyyy-MM-dd") });

                        }
                    }
                    else if (Bime.Contains("OTC"))
                    {
                        tempData.Items.Add(new Drag() { CustomerCode = customercode, listCode = listnum, Name = row["نام دارو"].ToString(), code = int.Parse(row["کد دارو"].ToString()), value = int.Parse(row["قیمت فروش"].ToString()), count = int.Parse(row["مقدار"].ToString()), hfani = int.Parse(row["حق فنی"].ToString()),hOTC= int.Parse(row["حق OTC"].ToString()), fTime = DateTime.Now.ToString("yyyy-MM-dd") });
                    }
                }
                calculate_details();
            }
            catch
            {

            }
        }
        private void calculate_details()
        {
            try
            {
                int sum = 0;
                int off = 0;
                int hfani = 0;
                int hotc = 0;
                foreach (Drag item in tempData.Items)
                {
                    sum += item.value * item.count;
                    hfani += item.hfani * item.value / 100;
                    if (Bime.Contains("بیمه"))
                    {
                        off += item.off * item.value / 100;
                    }
                    else if (Bime.Contains("OTC"))
                    {
                        hotc += item.hOTC * item.value / 100;
                    }
                    
                }
                Sum.Content = sum.ToString();
                TotalSum.Content = (sum - hfani - hotc - off).ToString();
                hFani.Content = hfani.ToString();
                hOTC.Content = hotc.ToString();
                OFF.Content = off.ToString();
            }
            catch
            {

            }

        }
    }
}
