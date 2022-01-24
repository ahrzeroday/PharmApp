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
    public partial class Storage_page : Window
    {

        public Storage_page()
        {
            InitializeComponent();
            InitHeader();
            initDataTemp();
            DataTable dt = Helper.Multiply("دارو موجود در انبار", "تاریخ انقضا داروی موجود در انبار", "*", "a.'کد دارو'==b.'کد دارو'");
            for(int i=0;i<dt.Rows.Count;i++)
            {
                tempData.Items.Add(new Drag() { code=int.Parse(dt.Rows[i]["کد دارو"].ToString()),
                Name=dt.Rows[i]["نام دارو"].ToString(),
                Count=int.Parse(dt.Rows[i]["مقدار"].ToString()),
                hFani=int.Parse(dt.Rows[i]["حق فنی"].ToString()),
                hOTC=int.Parse(dt.Rows[i]["حق OTC"].ToString()),
                bPrice=int.Parse(dt.Rows[i]["قیمت فروش"].ToString()),
                sPrice=int.Parse(dt.Rows[i]["قیمت خرید"].ToString()),
                eTime=DateTime.Parse(dt.Rows[i]["تاریخ انقضا"].ToString().Split(" ")[0]) });
            }
            foreach(DataColumn item in dt.Columns)
            {
                SearchBy.Items.Add(item.ColumnName);
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

        #region Rules

        private void OnlyNumAccept(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        #endregion

        #region Object

        public class Drag
        {
            public int code { get; set; }
            public string Name { get; set; }
            public int Count { get; set; }
            public int hFani { get; set; }
            public int hOTC { get; set; }
            public int bPrice { get; set; }
            public int sPrice { get; set; }
            public DateTime eTime { get; set; }
        }

        #endregion
        #region DataTemp

        private void initDataTemp()
        {
            tempData.Columns.Add(new DataGridTextColumn() { Header = "کد دارو", Binding = new Binding("code"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "نام دارو", Binding = new Binding("Name"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "مقدار", Binding = new Binding("Count"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "حق فنی", Binding = new Binding("hFani"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "حق OTC", Binding = new Binding("hOTC"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "قیمت فروش", Binding = new Binding("bPrice"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "قیمت خرید", Binding = new Binding("sPrice"), IsReadOnly = true });
            tempData.Columns.Add(new DataGridTextColumn() { Header = "تاریخ انقضا", Binding = new Binding("eTime"), IsReadOnly = true });
        }

        #endregion

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                
                DataTable dt = Helper.Multiply("دارو موجود در انبار", "تاریخ انقضا داروی موجود در انبار", "*",$"a.'کد دارو'== b.'کد دارو'  and '{SearchBy.Text}' like '{Search.Text}'");
                tempData.Items.Clear();
                for(int i=0;i<dt.Rows.Count;i++) 
            {
                tempData.Items.Add(new Drag() { code=int.Parse(dt.Rows[i]["کد دارو"].ToString()),
                Name=dt.Rows[i]["نام دارو"].ToString(),
                Count=int.Parse(dt.Rows[i]["مقدار"].ToString()),
                hFani=int.Parse(dt.Rows[i]["حق فنی"].ToString()),
                hOTC=int.Parse(dt.Rows[i]["حق OTC"].ToString()),
                bPrice=int.Parse(dt.Rows[i]["قیمت فروش"].ToString()),
                sPrice=int.Parse(dt.Rows[i]["قیمت خرید"].ToString()),
                eTime=DateTime.Parse(dt.Rows[i]["تاریخ انقضا"].ToString().Split(" ")[0]) });
            }
            }
            catch
            {

            }
        }
    }
}
