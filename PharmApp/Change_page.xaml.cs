using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PharmApp
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Change_page : Window
    {
        private List<string> dNamelist = new List<string>();
        public Change_page()
        {
            InitializeComponent();
            InitHeader();
            DataTable dt = Helper.Multiply("خرید دارو از شرکت", "دارو موجود در انبار","'کد یکتای دارو','نام دارو'", $" a.'کد دارو' == b.'کد دارو'") ;
            for(int i =0;i<dt.Rows.Count;i++)
            {
                dName.Items.Add(dt.Rows[i]["کد یکتای دارو"]+"-"+dt.Rows[i]["نام دارو"]);
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
        private void dName_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = Helper.Multiply("خرید دارو از شرکت", "دارو موجود در انبار", "'کد یکتای دارو','حق OTC','حق فنی'", $" a.'کد دارو' == b.'کد دارو' and 'کد یکتای دارو'=={dName.Text.Split("-")[0]}") ;
                code.Content = dt.Rows[0]["کد یکتای دارو"].ToString();
                hfani.Text = dt.Rows[0]["حق فنی"].ToString();
                hOTC.Text = dt.Rows[0]["حق OTC"].ToString();
                
            }
            catch
            {

            }


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
            if (string.IsNullOrEmpty(dName .Text) || string.IsNullOrEmpty(hfani.Text) || string.IsNullOrEmpty(hOTC .Text))
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
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mr = MessageBox.Show("آیا میخواهید از صفحه خارج شوید؟", "اخطار", MessageBoxButton.YesNo);
            if (mr.HasFlag(MessageBoxResult.Yes))
            {
                this.Close();
            }
        }
        private void Hupdate(object sender, RoutedEventArgs e)
        {
            //DataTable dt1 = Helper.Multiply("خرید دارو از شرکت","دارو موجود در انبار","a.'کد دارو'","a.'کد یکتای دارو'== b.'کد یکتای دارو'");
            Helper.Update("دارو موجود در انبار", $"'حق فنی'={hfani.Text} ,'حق OTC'={hOTC.Text}", $"'کد دارو' in (select a.\"کد دارو\" from \"خرید دارو از شرکت\"as a,\"دارو موجود در انبار\"as b where a.\"کد یکتای دارو\"== {code.Content.ToString()})");
            MessageBox.Show("تغییرات اعمال شد");
        }
    }
}
