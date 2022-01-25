using System.Data;
using System.Data.SQLite;
using System.Windows;

namespace PharmApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitHeader();
            

            /////////
            DataTable dt = Helper.GetTable("دارو موجود در انبار","'کد دارو'");
            foreach(DataRow item in dt.Rows)
            {
                Helper.allCode.Add(int.Parse(item[0].ToString()));
            }
            /////////
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
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
                Helper.dbs.Close();

                Application.Current.Shutdown();
            };
            Restore.MouseUp += (s, e) =>
            {
                SwitchState();
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

        private void Buy_medicine_button_Clicked(object sender, RoutedEventArgs e)
        {
            Buy_medicine_page ObjWindow1 = new Buy_medicine_page();
            Visibility = Visibility.Visible;
            ObjWindow1.Show();

        }


        private void Cash_register_button_Clicked(object sender, RoutedEventArgs e)
        {
            Cash_register_page ObjWindow2 = new Cash_register_page();
            Visibility = Visibility.Visible;
            ObjWindow2.Show();
        }
        private void Admission_button_Clicked(object sender, RoutedEventArgs e)
        {
            Admission_page ObjWindow3 = new Admission_page();
            Visibility = Visibility.Visible;
            ObjWindow3.Show();
        }
        private void Refer_to_the_seller_button_Clicked(object sender, RoutedEventArgs e)
        {
            Refer_to_the_seller_page ObjWindow4 = new Refer_to_the_seller_page();
            Visibility = Visibility.Visible;
            ObjWindow4.Show();
        }

        private void Clicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            WindowState = System.Windows.WindowState.Minimized;
        }

        private void Add_medicine_Clicked(object sender, RoutedEventArgs e)
        {
            Add_medicine_page ObjWindow5 = new Add_medicine_page();
            Visibility = Visibility.Visible;
            ObjWindow5.Show();
        }

        private void Change(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Change_page ObjWindow6 = new Change_page();
            Visibility = Visibility.Visible;
            ObjWindow6.Show();
        }

        private void Log_in_clicked(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Storage_Button_clicked(object sender, RoutedEventArgs e)
        {
            Storage_page ObjWindow8 = new Storage_page();
            Visibility = Visibility.Visible;
            ObjWindow8.Show();

        }
        private void Go_to_personel(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Add_personel_page ObjWindow9 = new Add_personel_page();
            Visibility = Visibility.Visible;
            ObjWindow9.Show();
        }

        private void Go_to_insurance(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Insurance_page ObjWindow10 = new Insurance_page();
            Visibility = Visibility.Visible;
            ObjWindow10.Show();
        }
    }
}
