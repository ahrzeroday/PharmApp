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
    public partial class Insurance_page : Window
    {
        private List<string> dNamelist = new List<string>();
        public Insurance_page()
        {
            InitializeComponent();
            InitHeader();

             DataTable dt = Helper.GetTable("داروی شرکت","'نام دارو','کد یکتای دارو'");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dNamelist.Add(dt.Rows[i]["کد یکتای دارو"].ToString() + "-" + dt.Rows[i]["نام دارو"].ToString());

            }
            dName.ItemsSource = dNamelist;

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
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mr = MessageBox.Show("آیا میخواهید از صفحه خارج شوید؟", "اخطار", MessageBoxButton.YesNo);
            if (mr.HasFlag(MessageBoxResult.Yes))
            {
                this.Close();
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
        private void add_insuranceC(object sender, RoutedEventArgs e)
        {
            Helper.Insert("شرکت بیمه", $"'{Insurance_Company.Text}'");
            Helper.Insert("داروی تحت بیمه", $"'{Insurance_Company.Text}',{dCode.Text},{Off_price.Text}");
            MessageBox.Show("اطلاعات ذخیره شد");
            Insurance_Company.Text = "";
            Off_price.Text = "0";
        }
    }
}
        