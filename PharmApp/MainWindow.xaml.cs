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
            InitSQLite();

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

        #region InitSQLite

        private void InitSQLite()
        {

            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=PharmApp.db;Version=3;UTF8Encoding=True;");
            m_dbConnection.Open();
            Helper.dbs = m_dbConnection;
            Helper.CreateTable("مشتری با بیمه", "'شماره قبض' int,'شماره لیست' int,'کد ملی' int,'نام و نام خانوادگی' varchar(256),'تاریخ تولد' date,'نام شرکت' int,primary key('شماره قبض')");
            Helper.CreateTable("مشتری با نسخه", "'شماره قبض' int,'شماره لیست' int,'کد ملی' int,'نام و نام خانوادگی' varchar(256),primary key('شماره قبض')");
            Helper.CreateTable("مشتری OTC", "'شماره قبض' int,'شماره لیست' int,'کد ملی' int,primary key('شماره قبض')");
            Helper.CreateTable("شرکت بیمه", "'نام شرکت' varchar(128),primary key('نام شرکت')");
            Helper.CreateTable("تجمیع  درخواست میشود  بین مشتری و لیست دارو", "'شماره قبض' int,'شماره لیست' int,'کدپرسنلی' int,foreign key('شماره قبض') references 'مشتری با بیمه',foreign key('شماره قبض') references 'مشتری با نسخه',foreign key('شماره قبض') references 'مشتری OTC',foreign key('شماره لیست') references 'صندوق',foreign key('کدپرسنلی') references 'پرسنل'");
            Helper.CreateTable("تاریخ انقضا داروی موجود در انبار", "'کد دارو' int,'تاریخ انقضا' date,foreign key('کد دارو') references 'دارو موجود در انبار'");
            Helper.CreateTable("دارو موجود در انبار", "'کد دارو' int,'نام دارو' varchar(256),'مقدار' int,'حق فنی' int,'حق OTC' int,'قیمت فروش' int,'قیمت خرید' int,primary key('کد دارو')");
            Helper.CreateTable("پذیرش", "'کد یکتای دارو' int,'نام دارو' varchar(128),'نام شرکت' varchar(128),'قیمت خرید' int,primary key('کد یکتای دارو')");
            Helper.CreateTable("دارو موجود درهر لیست", "'شماره لیست' int,'کد دارو' int,'مقدار' int,foreign key('شماره لیست') references 'پذیرش',foreign key('کد دارو') references 'دارو موجود در انبار'");
            Helper.CreateTable("صندق", "'شماره لیست' int,'شماره قبض' int,'توضیحات' varchar(512),'مبلغ قابل پرداخت' int,'کد پرداخت' int,'کدپرسنلی' int,'تاریخ ثبت' date,foreign key('شماره لیست') references 'تجمیع  درخواست میشود  بین مشتری و لیست دارو',foreign key('کدپرسنلی') references 'پرسنل'");
            Helper.CreateTable("انتخاب میشود", "'شماره لیست' int,'کد دارو' int,'کدپرسنلی' int,foreign key('شماره لیست') references 'پذیرش',foreign key('کد دارو') references 'دارو موجود در انبار',foreign key('کدپرسنلی') references 'پرسنل'");
            Helper.CreateTable("پرسنل", "'کدپرسنلی' int,'نام و نام خانوادگی' varchar(256),'کد نظام پزشکی' varchar(32),'رمزعبور' varchar(64),primary key('کدپرسنلی')");
            Helper.CreateTable("خرید دارو از شرکت", "'کد یکتای دارو' int,'کد دارو' int,'شماره فاکتور' int,'زمان ثبت خرید' date,'تحویل دهنده' varchar(128),'توضیحات' varchar(512),foreign key('کد یکتای دارو') references 'داروی شرکت',foreign key('کد دارو') references 'دارو موجود در انبار'");
            Helper.CreateTable("ارجاع دارو به شرکت", "'کد یکتای دارو' int,'کد دارو' int,'شماره ارجاع' int,'زمان ثبت درخواست خرید' date,'تحویل دهنده' varchar(128),'دلیل مرجوعی' varchar(512),foreign key('کد یکتای دارو') references 'داروی شرکت',foreign key('کد دارو') references 'دارو موجود در انبار'");

            Helper.CreateTable("تجمیع 1", "'کد یکتای دارو' int,'کد دارو' int,'کدپرسنلی' int,foreign key('کد یکتای دارو') references 'داروی شرکت',foreign key('کد دارو') references 'دارو موجود در انبار',foreign key('کدپرسنلی') references 'پرسنل'");
            Helper.CreateTable("تجمیع 2", "'کد یکتای دارو' int,'کد دارو' int,'کدپرسنلی' int,foreign key('کد یکتای دارو') references 'داروی شرکت',foreign key('کد دارو') references 'دارو موجود در انبار',foreign key('کدپرسنلی') references 'پرسنل'");


            Helper.CreateTable("داروی شرکت", "'کد یکتای دارو' int,'نام دارو' varchar(128),'نام شرکت' varchar(128),'قیمت خرید' int,primary key('کد یکتای دارو')");
            Helper.CreateTable("تاریخ انقضای داروی شرکت", "'کد یکتای دارو' int,'نام دارو' varchar(128),'نام شرکت' varchar(128),'قیمت خرید' int,foreign key('کد یکتای دارو') references 'داروی شرکت'");
            Helper.CreateTable("داروی تحت بیمه", "'نام شرکت' varchar(128),'کد یکتای دارو' int,'تخفیف' int,foreign key('نام شرکت') references 'شرکت بیمه',foreign key('کد یکتای دارو') references 'داروی شرکت'");
            


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
            Log_in_page ObjWindow7 = new Log_in_page();
            Visibility = Visibility.Visible;
            ObjWindow7.Show();
        }

        private void Storage_Button_clicked(object sender, RoutedEventArgs e)
        {
            Storage_page ObjWindow7 = new Storage_page();
            Visibility = Visibility.Visible;
            ObjWindow7.Show();

        }
    }
}
