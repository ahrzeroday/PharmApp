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
    public partial class Buy_medicine_page : Window
    {
        private List<string> dNamelist = new List<string>();
        private List<int> dCodelist = new List<int>();
        public Buy_medicine_page()
        {
            InitializeComponent();
            InitHeader();
            initDataTemp();
            Add.IsEnabled = false;
            DataTable dt = Helper.GetTable("داروی شرکت", "'کد یکتای دارو','نام دارو','نام شرکت'");
            List<string> temp = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                
                temp.Add(dt.Rows[i]["نام شرکت"].ToString());


            }
            cName.ItemsSource = temp.Distinct().ToList();
            bperson.Content = Helper.PersonalName;
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
            try{
  ComboBox? cmb = (ComboBox)sender;
            cmb.IsDropDownOpen = true;

            string? textbox = (cmb.Template.FindName("PART_EditableTextBox", cmb) as TextBox).Text;
            cmb.ItemsSource = dNamelist.Where(p => string.IsNullOrEmpty(cmb.Text) || p.ToLower().Contains(textbox.ToLower())).ToList();
           
            }
            catch{

            }
          
        }

        private void dName_DropDownClosed(object sender, EventArgs e)
        {
            try{
                dCode.Text = dName.Text.Split('-')[0];
                DataTable dt = Helper.Search("داروی شرکت", "'قیمت خرید'", $"'کد یکتای دارو'='{dCode.Text}'");
                bPrice.Content = dt.Rows[0]["قیمت خرید"].ToString();
            }
            catch{

            }
                
                
            }
            
        private void cName_DropDownClosed(object sender, EventArgs e)
        {
            //DataTable dt = Helper.commandDbs("Select 'نام دارو','کد یکتای دارو' from 'داروی شرکت' ".Replace("'","\"") +
            //    $"where 'نام شرکت' == '{cName.Text}'".Replace("'", "\""));
            try{
DataTable dt = Helper.Search("داروی شرکت", "'نام دارو','کد یکتای دارو'", $"'نام شرکت'=='{cName.Text}'");

            dCodelist.Clear();
            dNamelist.Clear();
            dName.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dCodelist.Add(int.Parse(dt.Rows[i]["کد یکتای دارو"].ToString()));
                dNamelist.Add(dt.Rows[i]["کد یکتای دارو"].ToString() + "-" + dt.Rows[i]["نام دارو"].ToString());
                dName.Items.Add(dt.Rows[i]["کد یکتای دارو"].ToString() + "-" + dt.Rows[i]["نام دارو"].ToString());

            }
            }
            catch{

            }
            
           // dName.ItemsSource = dNamelist;
        }

       

        private bool checkinput()
        {
            if (string.IsNullOrEmpty(fTime.Text) || string.IsNullOrEmpty(fCode.Text) || string.IsNullOrEmpty(cmName.Text) || string.IsNullOrEmpty( dCode.Text) || string.IsNullOrEmpty( dName.Text)|| string.IsNullOrEmpty( hFani.Text)|| string.IsNullOrEmpty( hOTC.Text)|| string.IsNullOrEmpty( sPrice.Text)|| string.IsNullOrEmpty( Count.Text)|| string.IsNullOrEmpty( eDate.Text))
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
            public int ocode { get; set; }
            public int code { get; set; }
            public string Name { get; set; }
            public int fCode { get; set; }
            public string fTime { get; set; }
            public string cmName { get; set; }
            public string Description { get; set; }
            public int count { get; set; }
            public string eTime { get; set; }
            public int hfani { get; set; }
            public int hotc { get; set; }
            public int bprice { get; set; }
            public int sprice { get; set; }
        }

        #endregion

        #region DataTemp

        private void initDataTemp()
        {
            tempData.Columns.Add(new DataGridTextColumn() { Header = "کد یکتای دارو", Binding = new Binding("ocode"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "کد دارو", Binding = new Binding("code"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "نام دارو", Binding = new Binding("Name"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "شماره فاکتور", Binding = new Binding("fCode"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "زمان ثبت خرید", Binding = new Binding("fTime"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "تحویل دهنده", Binding = new Binding("cmName"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "توضیحات", Binding = new Binding("Description"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "مقدار", Binding = new Binding("count"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "تاریخ انقضا", Binding = new Binding("eTime"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "حق فنی", Binding = new Binding("hfani"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "OTC حق", Binding = new Binding("hotc"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "قیمت خرید", Binding = new Binding("bprice"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "قیمت فروش", Binding = new Binding("sprice"), IsReadOnly = true });
        }

        #endregion


        private void AddTempData(object sender, RoutedEventArgs e)
        {
            if (!checkinput())
            {
                return;
            }
            if(DateTime.Parse(fTime.Text).Date>= DateTime.Parse(eDate.Text).Date)
            {
                MessageBox.Show("دارو قابل فروش نیست!");
                return;
            }
            
            
            for(int i = 0; i<tempData.Items.Count;i++)
            {
                Drag d = (Drag)tempData.Items[i];
                if (d.ocode == int.Parse(dCode.Text))
                {
                    MessageBox.Show("این دارو در لیست موجود است لطفا از ویرایش استفاده کنید");
                    return;
                }
            }
            cName.IsEnabled = false;
            fTime.IsEnabled = false;
            fCode.IsEnabled = false;
            cmName.IsEnabled = false;
            Add.IsEnabled = true;
            tempData.Items.Add(new Drag() { ocode = int.Parse(dCode.Text), code = Helper.GetNewCode(),Name=dName.Text.Split("-")[1], fCode = int.Parse(fCode.Text), fTime = DateTime.Parse(fTime.Text).Date.ToString("yyyy-MM-dd"), cmName = cmName.Text, Description = Description.Text, count = int.Parse(Count.Text), eTime = DateTime.Parse(eDate.Text).Date.ToString("yyyy-MM-dd"), hfani=int.Parse(hFani.Text), hotc = int.Parse(hOTC.Text),bprice=int.Parse(bPrice.Content.ToString()),sprice=int.Parse(sPrice.Text) });
            Total_sum();
        }
        private void EditTempData(object sender, RoutedEventArgs e)
        {

            try
            {
                if (!checkinput())
                {
                    return;
                }
                Drag row = tempData.SelectedItem as Drag;
                int index = tempData.SelectedIndex;
                tempData.Items.RemoveAt(index);
                tempData.Items.Insert(index, new Drag() { ocode = int.Parse(dCode.Text), code = int.Parse(row.code.ToString()), Name = dName.Text.Split("-")[1], fCode = int.Parse(fCode.Text), fTime = DateTime.Parse(fTime.Text).Date.ToString("yyyy-MM-dd"), cmName = cmName.Text, Description = Description.Text, count = int.Parse(Count.Text), eTime = DateTime.Parse(eDate.Text).Date.ToString("yyyy-MM-dd"), hfani = int.Parse(hFani.Text), hotc = int.Parse(hOTC.Text), bprice = int.Parse(bPrice.Content.ToString()), sprice = int.Parse(sPrice.Text) });
                Total_sum();
            }
            catch
            {
                MessageBox.Show("لطفا آیتم مورد نظر جهت تغییر را از جدول زیر انتخاب کنید");
            }

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

                dCode.Text = row.ocode.ToString();
                fCode.Text = row.fCode.ToString();
                fTime.Text = row.fTime.ToString();
                cmName.Text = row.cmName;
                Description.Text = row.Description;
                Count.Text = row.count.ToString();
                eDate.Text = row.eTime.ToString();
                hFani.Text = row.hfani.ToString();
                hOTC.Text = row.hotc.ToString();
            }
            catch
            {
            }
        }
        private void AddTempToReal(object sender, RoutedEventArgs e)
        {
            foreach (Drag item in tempData.Items)
            {
                Helper.Insert("دارو موجود در انبار", $"{item.code},'{item.Name}',{item.count},{item.hfani},{item.hotc},{item.sprice},{item.bprice}");
                Helper.Insert("تاریخ انقضا داروی موجود در انبار", $"{item.code},'{item.eTime}'");
                Helper.Insert("خرید دارو از شرکت", $"{item.ocode},{item.code},{item.fCode},'{item.fTime}','{item.cmName}','{item.Description}'");
                Helper.Insert("تجمیع خرید دارو", $"{item.ocode},{item.code},{Helper.PersonalID}");
            }

            MessageBox.Show("اطلاعات ذخیره شد");
            tempData.Items.Clear();
            fTime.Text="";
            fCode.Text="";
            cmName.Text="";
            Totalsum.Content="-";
            cName.IsEnabled = true;
            fTime.IsEnabled = true;
            fCode.IsEnabled = true;
            cmName.IsEnabled = true;
            Add.IsEnabled = false;
            Description.Text = "";
            dName.Text = "";
            dCode.Text = "";
            bPrice.Content = "";
            sPrice.Text = "";
            Count.Text = "";
            eDate.Text = "";
            hFani.Text = "";
            hOTC.Text = "";
        }
        private void DeleteTempdata(object sender, RoutedEventArgs e)
        {
            cName.IsEnabled = true;
            fTime.IsEnabled = true;
            fCode.IsEnabled = true;
            cmName.IsEnabled = true;
            Add.IsEnabled = false;
            tempData.Items.Clear();
            Totalsum.Content = "-";
            fTime.Text="";
            fCode.Text="";
            cmName.Text="";
            Total_sum();

        }
        private void DeleteOneTempdata(object sender, RoutedEventArgs e)
        {
           
            try
            {
                int index = tempData.SelectedIndex;
                tempData.Items.RemoveAt(index);
                Total_sum();
                if (tempData.Items.Count == 0)
                {
                    cName.IsEnabled = true;
                    fTime.IsEnabled = true;
                    fCode.IsEnabled = true;
                    cmName.IsEnabled = true;
                    Add.IsEnabled = false;

                }
            }
            catch
            {
                MessageBox.Show("لطفا یک دارو از لیست انتخاب کنید");
            }

        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mr = MessageBox.Show("آیا میخواهید از صفحه خارج شوید؟", "اخطار", MessageBoxButton.YesNo);
            if (mr.HasFlag(MessageBoxResult.Yes))
            {
                Close();
            }
        }
        private void Total_sum()
        {
            try
            {
                int sum = 0;
                foreach (Drag item in tempData.Items)
                {
                    sum += item.bprice * item.count;
                }
                Totalsum.Content = sum.ToString();
            }
            catch
            {

            }
         
        }   
       
    }
}
