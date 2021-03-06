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
        private DataTable dt = new DataTable();
        public Admission_page()
        {
            InitializeComponent();
            InitHeader();
            initDataTemp();

            dt = Helper.Multiply_exc("دارو موجود در انبار", "خرید دارو از شرکت", "دارو موجود در انبار", "ارجاع دارو به شرکت", "a.'کد دارو','قیمت خرید','کد یکتای دارو','مقدار','نام دارو'", "a.'کد دارو','قیمت خرید','کد یکتای دارو','مقدار','نام دارو'", "a.'کد دارو' == b.'کد دارو'", "a.'کد دارو' == b.'کد دارو'");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dCodelist.Add(int.Parse(dt.Rows[i]["کد یکتای دارو"].ToString()));
                dNamelist.Add(dt.Rows[i]["کد یکتای دارو"].ToString() + "-" + dt.Rows[i]["نام دارو"].ToString());

            }
            dt = Helper.GetTable("شرکت بیمه", "'نام شرکت'");
            if (dt.Rows.Count < 0)
            {
                cName.IsEnabled = false;
            }
            List<string> temp = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                temp.Add(dt.Rows[i]["نام شرکت"].ToString());


            }
            dNamelist = dNamelist.Distinct().ToList();
            dName.ItemsSource = dNamelist;
            cName.ItemsSource = temp;


            DataTable tempdt = Helper.GetTable("پذیرش", "'شماره لیست'");
            Helper.alllistCode.Clear();
            foreach (DataRow dr in tempdt.Rows)
            {
                Helper.alllistCode.Add(int.Parse(dr["شماره لیست"].ToString()));
            }
            tempdt = Helper.GetTable("تجمیع درخواست میشود بین مشتری و لیست دارو", "'شماره قبض'");
            Helper.alllistCustomer.Clear();
            foreach (DataRow dr in tempdt.Rows)
            {
                Helper.alllistCustomer.Add(int.Parse(dr["شماره لیست"].ToString()));
            }

        }
        #region InitHeader

        private void InitHeader()
        {
            Add.IsEnabled = false;
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
        private void dName_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                dCode.Text = dName.Text.Split('-')[0];
            }
            catch
            {

            }


        }
        private void Bime_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                if (Bime.Text.Contains("آزاد"))
                {
                    ID.IsEnabled = true;
                    Name.IsEnabled = true;
                    cName.IsEnabled = false;
                    initDataTempnoskhe();
                }
                else if (Bime.Text.Contains("بیمه درمان"))
                {
                    ID.IsEnabled = true;
                    Name.IsEnabled = true;
                    cName.IsEnabled = true;
                    initDataTemp();
                }
                else if (Bime.Text.Contains("OTC"))
                {
                    ID.IsEnabled = true;
                    Name.IsEnabled = false;
                    cName.IsEnabled = false;
                    initDataTempOTC();
                }
            }
            catch
            {

            }
        }
        private bool checkinput(bool edit)
        {
            if (Bime.Text.Contains("آزاد"))
            {
                if (string.IsNullOrEmpty(dName.Text) || string.IsNullOrEmpty(Name.Text) || string.IsNullOrEmpty(ID.Text) || string.IsNullOrEmpty(Age.Text) || string.IsNullOrEmpty(dCode.Text) || string.IsNullOrEmpty(dName.Text) || string.IsNullOrEmpty(Count.Text))
                {
                    MessageBox.Show("لطفا ورودی ها را بررسی کنید");
                    return false;
                }
                else
                {
                    if (!edit)
                    {
                        foreach (Drag item in tempData.Items)
                        {
                            if (item.dCode == int.Parse(dCode.Text))
                            {
                                MessageBox.Show("لطفا کد دارو را مجدد بررسی کنید");
                                return false;
                            }
                        }
                    }




                    return true;
                }

            }
            else if (Bime.Text.Contains("بیمه درمان"))
            {
                if (string.IsNullOrEmpty(dName.Text) || string.IsNullOrEmpty(Name.Text) || string.IsNullOrEmpty(cName.Text) || string.IsNullOrEmpty(ID.Text) || string.IsNullOrEmpty(Age.Text) || string.IsNullOrEmpty(dCode.Text) || string.IsNullOrEmpty(dName.Text) || string.IsNullOrEmpty(Count.Text))
                {
                    MessageBox.Show("لطفا ورودی ها را بررسی کنید");
                    return false;
                }
                else
                {
                    if (!edit)
                    {
                        foreach (Drag item in tempData.Items)
                        {
                            if (item.dCode == int.Parse(dCode.Text))
                            {
                                MessageBox.Show("لطفا کد دارو را مجدد بررسی کنید");
                                return false;
                            }
                        }
                    }



                    return true;
                }
            }
            else if (Bime.Text.Contains("OTC"))
            {
                if (string.IsNullOrEmpty(dName.Text) || string.IsNullOrEmpty(ID.Text) || string.IsNullOrEmpty(Age.Text) || string.IsNullOrEmpty(dCode.Text) || string.IsNullOrEmpty(dName.Text) || string.IsNullOrEmpty(Count.Text))
                {
                    MessageBox.Show("لطفا ورودی ها را بررسی کنید");
                    return false;
                }
                else
                {

                    if (!edit)
                    {
                        foreach (Drag item in tempData.Items)
                        {
                            if (item.dCode == int.Parse(dCode.Text))
                            {
                                MessageBox.Show("لطفا کد دارو را مجدد بررسی کنید");
                                return false;
                            }
                        }
                    }



                    return true;
                }
            }
            else
            {
                return false;
            }


        }

        #endregion

        #region Object
        public class Drag
        {
            public int dCode { get; set; }
            public int listnum { get; set; }
            public string Name { get; set; }
            public long ID { get; set; }
            public int Age { get; set; }
            public string bime { get; set; }
            public string cName { get; set; }
            public string Description { get; set; }
            public string dName { get; set; }
            public DateTime eTime { get; set; }
            public int Count { get; set; }


        }

        #endregion
        #region DataTemp 

        private void initDataTemp()
        {
            tempData.Columns.Clear();
            tempData.Columns.Add(new DataGridTextColumn() { Header = "کد یکتای دارو", Binding = new Binding("dCode"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "نام بیمار", Binding = new Binding("Name"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "کد ملی", Binding = new Binding("ID"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "سن", Binding = new Binding("Age"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "نام دارو", Binding = new Binding("dName"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "تاریخ انقضا", Binding = new Binding("eTime"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "مقدار", Binding = new Binding("Count"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "نام شرکت بیمه", Binding = new Binding("cName"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "توضیحات", Binding = new Binding("Description"), IsReadOnly = true });

        }
        private void initDataTempnoskhe()
        {
            tempData.Columns.Clear();
            tempData.Columns.Add(new DataGridTextColumn() { Header = "کد یکتای دارو", Binding = new Binding("dCode"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "نام بیمار", Binding = new Binding("Name"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "کد ملی", Binding = new Binding("ID"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "سن", Binding = new Binding("Age"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "نام دارو", Binding = new Binding("dName"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "تاریخ انقضا", Binding = new Binding("eTime"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "مقدار", Binding = new Binding("Count"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "توضیحات", Binding = new Binding("Description"), IsReadOnly = true });

        }
        private void initDataTempOTC()
        {
            tempData.Columns.Clear();
            tempData.Columns.Add(new DataGridTextColumn() { Header = "کد یکتای دارو", Binding = new Binding("dCode"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "کد ملی", Binding = new Binding("ID"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "سن", Binding = new Binding("Age"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "نام دارو", Binding = new Binding("dName"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "تاریخ انقضا", Binding = new Binding("eTime"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "مقدار", Binding = new Binding("Count"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "توضیحات", Binding = new Binding("Description"), IsReadOnly = true });

        }
        #endregion

        private void DeleteTempdata(object sender, RoutedEventArgs e)
        {
            Name.IsEnabled = true;
            cName.IsEnabled = true;
            ID.IsEnabled = true;
            Bime.IsEnabled = true;
            Age.IsEnabled = true;
            Add.IsEnabled = false;
            tempData.Items.Clear();

        }
        private void AddTempData(object sender, RoutedEventArgs e)
        {
            DataTable Tempnoskhe = Helper.Triply("تاریخ انقضا داروی موجود در انبار", "خرید دارو از شرکت", "دارو موجود در انبار", " a.'کد دارو','مقدار','تاریخ انقضا'", $"a.'کد دارو' == b.'کد دارو' and b.'کد دارو' == c.'کد دارو' and 'کد یکتای دارو' == {dCode.Text} ");

            //Tempnoskhe = Helper.Search("تاریخ انقضا داروی موجود در انبار", "'تاریخ انقضا'", $"'کد یکتای دارو'=={dName.Text.Split("-")[0]}");

            if (Tempnoskhe.Rows.Count <= 0)
            {
                MessageBox.Show("مقدار دارو کافی نیست");
                return;
            }
            else
            {
                SortedDictionary<DateTime, Dictionary<int, int>> elist = new SortedDictionary<DateTime, Dictionary<int, int>>();
                for (int i = 0; i < Tempnoskhe.Rows.Count; i++)
                {
                    Dictionary<int, int> tp = new Dictionary<int, int>
                    {
                        { int.Parse(Tempnoskhe.Rows[i]["کد دارو"].ToString()), int.Parse(Tempnoskhe.Rows[i]["مقدار"].ToString()) }
                    };
                    elist.Add(DateTime.Parse(Tempnoskhe.Rows[i]["تاریخ انقضا"].ToString()), tp);

                }
                int total = elist.Sum(x => x.Value.ElementAt(0).Value);
                int need = int.Parse(Count.Text);
                if (need > total)
                {
                    MessageBox.Show("مقدار داده شده بیش از مقدار موجودی است");
                    return;
                }
                Add.IsEnabled = true;

            }

            if (!checkinput(false))
            {
                return;
            }
            try
            {
                Name.IsEnabled = false;
                cName.IsEnabled = false;
                ID.IsEnabled = false;
                Bime.IsEnabled = false;
                Age.IsEnabled = false;
                if (Bime.Text.Contains("آزاد"))
                {

                    tempData.Items.Add(new Drag() { dCode = int.Parse(dCode.Text), Name = Name.Text, ID = long.Parse(ID.Text), Age = int.Parse(Age.Text), dName = dName.Text.Split("-")[1], Count = int.Parse(Count.Text), Description = Description.Text });
                }
                else if (Bime.Text.Contains("بیمه درمان"))
                {

                    tempData.Items.Add(new Drag() { dCode = int.Parse(dCode.Text), Name = Name.Text, ID = long.Parse(ID.Text), Age = int.Parse(Age.Text), dName = dName.Text.Split("-")[1], Count = int.Parse(Count.Text), cName = cName.Text, Description = Description.Text });
                }
                else if (Bime.Text.Contains("OTC"))
                {

                    tempData.Items.Add(new Drag() { dCode = int.Parse(dCode.Text), ID = long.Parse(ID.Text), Age = int.Parse(Age.Text), dName = dName.Text.Split("-")[1], Count = int.Parse(Count.Text), Description = Description.Text });
                }
            }
            catch
            {

            }

//        tempData.Items.Add(new Drag() { dCode = int.Parse(dCode.Text), Code = Helper.GetNewCode(), Name = dName.Text.Split("-")[1], ID = int.Parse(ID.Text), fTime = DateTime.Parse(fTime.Text).Date, cmName = cmName.Text, Description = Description.Text, count = int.Parse(Count.Text), eTime = DateTime.Parse(eDate.Text), hfani = int.Parse(hFani.Text), hotc = int.Parse(hOTC.Text), bprice = int.Parse(bPrice.Content.ToString()), sprice = int.Parse(sPrice.Text) });
;
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mr = MessageBox.Show("آیا میخواهید از صفحه خارج شوید؟", "اخطار", MessageBoxButton.YesNo);
            if (mr.HasFlag(MessageBoxResult.Yes))
            {
                Close();
            }
        }
        private void EditTempData(object sender, RoutedEventArgs e)
        {
            DataTable Tempnoskhe = Helper.Triply("تاریخ انقضا داروی موجود در انبار", "خرید دارو از شرکت", "دارو موجود در انبار", " a.'کد دارو','مقدار','تاریخ انقضا'", $"a.'کد دارو' == b.'کد دارو' and b.'کد دارو' == c.'کد دارو' and 'کد یکتای دارو' == {dCode.Text} ");

            //Tempnoskhe = Helper.Search("تاریخ انقضا داروی موجود در انبار", "'تاریخ انقضا'", $"'کد یکتای دارو'=={dName.Text.Split("-")[0]}");

            if (Tempnoskhe.Rows.Count <= 0)
            {
                MessageBox.Show("مقدار دارو کافی نیست");
                return;
            }
            else
            {
                SortedDictionary<DateTime, Dictionary<int, int>> elist = new SortedDictionary<DateTime, Dictionary<int, int>>();
                for (int i = 0; i < Tempnoskhe.Rows.Count; i++)
                {
                    Dictionary<int, int> tp = new Dictionary<int, int>
                    {
                        { int.Parse(Tempnoskhe.Rows[i]["کد دارو"].ToString()), int.Parse(Tempnoskhe.Rows[i]["مقدار"].ToString()) }
                    };
                    elist.Add(DateTime.Parse(Tempnoskhe.Rows[i]["تاریخ انقضا"].ToString()), tp);

                }
                int total = elist.Sum(x => x.Value.ElementAt(0).Value);
                int need = int.Parse(Count.Text);
                if (need > total)
                {
                    MessageBox.Show("مقدار داده شده بیش از مقدار موجودی است");
                    return;
                }


            }
            if (Bime.Text.Contains("آزاد"))
            {
                try
                {
                    if (!checkinput(true))
                    {
                        return;
                    }
                    Drag row = tempData.SelectedItem as Drag;
                    int index = tempData.SelectedIndex;
                    tempData.Items.RemoveAt(index);
                    tempData.Items.Insert(index, new Drag() { dCode = int.Parse(dCode.Text), Name = Name.Text, ID = int.Parse(ID.Text), Age = int.Parse(Age.Text), dName = dName.Text.Split("-")[1], Count = int.Parse(Count.Text), Description = Description.Text });

                }
                catch
                {
                    MessageBox.Show("لطفا آیتم مورد نظر جهت تغییر را از جدول زیر انتخاب کنید");
                }
            }
            else if (Bime.Text.Contains("بیمه درمان"))
            {
                try
                {
                    if (!checkinput(true))
                    {
                        return;
                    }
                    Drag row = tempData.SelectedItem as Drag;
                    int index = tempData.SelectedIndex;
                    tempData.Items.RemoveAt(index);
                    tempData.Items.Insert(index, new Drag() { dCode = int.Parse(dCode.Text), Name = Name.Text, ID = int.Parse(ID.Text), Age = int.Parse(Age.Text), dName = dName.Text.Split("-")[1], Count = int.Parse(Count.Text), cName = cName.Text, Description = Description.Text });

                }
                catch
                {
                    MessageBox.Show("لطفا آیتم مورد نظر جهت تغییر را از جدول زیر انتخاب کنید");
                }
            }
            else if (Bime.Text.Contains("OTC"))
            {
                try
                {
                    if (!checkinput(true))
                    {
                        return;
                    }
                    Drag row = tempData.SelectedItem as Drag;
                    int index = tempData.SelectedIndex;
                    tempData.Items.RemoveAt(index);
                    tempData.Items.Insert(index, new Drag() { dCode = int.Parse(dCode.Text), ID = int.Parse(ID.Text), Age = int.Parse(Age.Text), dName = dName.Text.Split("-")[1], Count = int.Parse(Count.Text), Description = Description.Text });

                }
                catch
                {
                    MessageBox.Show("لطفا آیتم مورد نظر جهت تغییر را از جدول زیر انتخاب کنید");
                }
            }



        }
        private void addtemptoreal(object sender, RoutedEventArgs e)
        {
            try
            {
                Dictionary<int, int> allcode = new Dictionary<int, int>();
                foreach (Drag d in tempData.Items)
                {
                    DataTable Tempnoskhe = Helper.Triply("تاریخ انقضا داروی موجود در انبار", "خرید دارو از شرکت", "دارو موجود در انبار", " a.'کد دارو','مقدار','تاریخ انقضا'", $"a.'کد دارو' == b.'کد دارو' and b.'کد دارو' == c.'کد دارو' and 'کد یکتای دارو' == {d.dCode} ");

                    SortedDictionary<DateTime, Dictionary<int, int>> elist = new SortedDictionary<DateTime, Dictionary<int, int>>();
                    for (int i = 0; i < Tempnoskhe.Rows.Count; i++)
                    {
                        Dictionary<int, int> tp = new Dictionary<int, int>
                    {
                        { int.Parse(Tempnoskhe.Rows[i]["کد دارو"].ToString()), int.Parse(Tempnoskhe.Rows[i]["مقدار"].ToString()) }
                    };
                        elist.Add(DateTime.Parse(Tempnoskhe.Rows[i]["تاریخ انقضا"].ToString()), tp);

                    }
                    int need = d.Count;

                    for (int i = 0; i < elist.Count; i++)
                    {
                        if (elist.ElementAt(i).Value.ElementAt(0).Value >= need)
                        {

                            Helper.Update("دارو موجود در انبار", $"'مقدار'=={elist.ElementAt(i).Value.ElementAt(0).Value - need}", $"'کد دارو'=={elist.ElementAt(i).Value.ElementAt(0).Key}");

                            allcode[elist.ElementAt(i).Value.ElementAt(0).Key] = need;
                            break;
                        }
                        else
                        {

                            allcode[elist.ElementAt(i).Value.ElementAt(0).Key] = elist.ElementAt(i).Value.ElementAt(0).Value;
                            Helper.Update("دارو موجود در انبار", $"'مقدار'==0", $"'کد دارو'=={elist.ElementAt(i).Value.ElementAt(0).Key}");
                            need = need - elist.ElementAt(i).Value.ElementAt(0).Value;
                          
                        }
                    }
                }

                int listcode = Helper.GetNewlistNum();
                int customerCode = Helper.GetNewsCustomer();
                foreach (KeyValuePair<int, int> item in allcode)
                {
                    Helper.Insert("انتخاب می شود", $"{listcode},'{item.Key}',{item.Value},{Helper.PersonalID}");
                }
                if (Bime.Text.Contains("آزاد"))
                {

                    foreach (Drag item in tempData.Items)
                    {
                        Helper.Insert("پذیرش", $"{listcode},'{item.Description}','{Bime.Text}'");


                        Helper.Insert("مشتری با نسخه", $"{customerCode},{listcode},{item.ID},'{item.Name}',{item.Age}");
                        Helper.Insert("تجمیع درخواست میشود بین مشتری و لیست دارو", $"{customerCode},{listcode},{Helper.PersonalID}");

                    }
                }
                else if (Bime.Text.Contains("بیمه درمان"))
                {

                    foreach (Drag item in tempData.Items)
                    {
                        Helper.Insert("پذیرش", $"{listcode},'{item.Description}','{Bime.Text}'");
                        Helper.Insert("مشتری با بیمه", $"{customerCode},{listcode},{item.ID},'{item.Name}','{item.cName}',{item.Age}");
                        Helper.Insert("تجمیع درخواست میشود بین مشتری و لیست دارو", $"{customerCode},{listcode},{Helper.PersonalID}");
                    }
                }
                else if (Bime.Text.Contains("OTC"))
                {

                    foreach (Drag item in tempData.Items)
                    {
                        Helper.Insert("پذیرش", $"{listcode},'{item.Description}','{Bime.Text}'");
                        Helper.Insert("مشتری OTC", $"{customerCode},{listcode},{item.ID},{item.Age}");
                        Helper.Insert("تجمیع درخواست میشود بین مشتری و لیست دارو", $"{customerCode},{listcode},{Helper.PersonalID}");
                    }
                }
                MessageBox.Show($"اطلاعات ذخیره شد شماره قبض :{listcode + "-" + customerCode}");
            }
            catch
            {

            }
            Add.IsEnabled = false;
            Age.Text = "";
            ID.Text = "";
            Name.Text = "";
            Description.Text = "";
            dCode.Text = "";
            dName.Text = "";
            Count.Text = "";
            tempData.Items.Clear();

        }

        private void tempData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Drag row = tempData.SelectedItem as Drag;
                if (row == null)
                {
                    return;
                }

                Name.Text = row.Name;
                ID.Text = row.ID.ToString();
                Age.Text = row.Age.ToString();
                Count.Text = row.Count.ToString();
                Description.Text = row.Description;
                dCode.Text = row.dCode.ToString();
                dName.Text = row.dCode.ToString() + "-" + row.dName.ToString();
            }
            catch
            {
            }
        }

        private void DeleteOneTempdata(object sender, RoutedEventArgs e)
        {
            try
            {

                int index = tempData.SelectedIndex;
                tempData.Items.RemoveAt(index);
                if (tempData.Items.Count == 0)
                {
                    Name.IsEnabled = true;
                    cName.IsEnabled = true;
                    ID.IsEnabled = true;
                    Bime.IsEnabled = true;
                    Age.IsEnabled = true;
                    Add.IsEnabled = false;
                }
            }
            catch
            {

            }
        }
    }
}
