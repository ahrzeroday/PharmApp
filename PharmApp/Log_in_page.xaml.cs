using System.Data;
using System.Data.SQLite;
using System.Windows;

namespace PharmApp
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Log_in_page : Window
    {
        public Log_in_page()
        {
            InitializeComponent();
            InitSQLite();
        }

        #region InitSQLite

        private void InitSQLite()
        {

            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=PharmApp.db;Version=3;UTF8Encoding=True;");
            m_dbConnection.Open();
            Helper.dbs = m_dbConnection;
            Helper.CreateTable("مشتری با بیمه",
                "'شماره قبض' int," +
                "'شماره لیست' int," +
                "'کد ملی' int," +
                "'نام و نام خانوادگی' varchar(256)," +
                "'نام شرکت' varchar(256)," +
                "'سن' int," +
                "primary key('شماره قبض','شماره لیست')" +
                "foreign key('شماره لیست') references 'صندوق'");
            Helper.CreateTable("مشتری با نسخه",
                "'شماره قبض' int," +
                "'شماره لیست' int," +
                "'کد ملی' int," +
                "'نام و نام خانوادگی' varchar(256)," +
                "'سن' int," +
               "primary key('شماره قبض','شماره لیست')" +
                "foreign key('شماره لیست') references 'صندوق'");
            Helper.CreateTable("مشتری OTC",
                "'شماره قبض' int," +
                "'شماره لیست' int," +
                "'کد ملی' int," +
                "'سن' int," +
                "primary key('شماره قبض','شماره لیست')" +
                 "foreign key('شماره لیست') references 'صندوق'");
            Helper.CreateTable("شرکت بیمه",
                "'نام شرکت' varchar(128)," +
                "primary key('نام شرکت')");
            Helper.CreateTable("تجمیع درخواست میشود بین مشتری و لیست دارو",
                "'شماره قبض' int," +
                "'شماره لیست' int," +
                "'کدپرسنلی' int," +
                //////////////////////////////////////////////////////////////////////////
                "foreign key('شماره قبض') references 'مشتری با بیمه'," +
                "foreign key('شماره قبض') references 'مشتری با نسخه'," +
                "foreign key('شماره قبض') references 'مشتری OTC'," +
                "foreign key('شماره لیست') references 'صندوق'," +
                "foreign key('کدپرسنلی') references 'پرسنل'");
            Helper.CreateTable("تاریخ انقضا داروی موجود در انبار",
                "'کد دارو' int," +
                "'تاریخ انقضا' date," +
                "foreign key('کد دارو') references 'دارو موجود در انبار'");
            Helper.CreateTable("دارو موجود در انبار",
                "'کد دارو' int," +
                "'نام دارو' varchar(256)," +
                "'مقدار' int," +
                "'حق فنی' int," +
                "'حق OTC' int," +
                "'قیمت فروش' int," +
                "'قیمت خرید' int," +
                "primary key('کد دارو')");
            Helper.CreateTable("پذیرش",
                "'شماره لیست' int," +
                "'توضیحات' varchar(512)," +
                "'نوع بیمار' varchar(128)," +
                "primary key('شماره لیست')");
            Helper.CreateTable("صندوق",
                "'شماره لیست' int," +
                "'شماره قبض' int," +
                "'مبلغ قابل پرداخت' int," +
                "'کد پرداخت' int," +
                "'کدپرسنلی' int," +
                "'تاریخ ثبت' date," +
                "foreign key('شماره لیست') references 'تجمیع  درخواست میشود  بین مشتری و لیست دارو'," +
                "foreign key('شماره قبض') references 'تجمیع  درخواست میشود  بین مشتری و لیست دارو'," +
                "foreign key('کدپرسنلی') references 'پرسنل'");
            Helper.CreateTable("انتخاب می شود",
                "'شماره لیست' int," +
                "'کد دارو' int," +
                "'مقدار' int," +
                "'کدپرسنلی' int," +
                "foreign key('شماره لیست') references 'پذیرش'," +
                "foreign key('کد دارو') references 'دارو موجود در انبار'," +
                "foreign key('کدپرسنلی') references 'پرسنل'");
            Helper.CreateTable("پرسنل",
                "'کدپرسنلی' int," +
                "'کدملی'int," +
                "'نام و نام خانوادگی' varchar(256)," +
                "'کد نظام پزشکی' varchar(32)," +
                "'رمزعبور' varchar(64)," +
                "primary key('کدپرسنلی')");
            Helper.CreateTable("خرید دارو از شرکت",
                "'کد یکتای دارو' int," +
                "'کد دارو' int," +
                "'شماره فاکتور' int," +
                "'زمان ثبت خرید' date," +
                "'تحویل دهنده' varchar(128)," +
                "'توضیحات' varchar(512)," +
                "foreign key('کد یکتای دارو') references 'داروی شرکت'," +
                "foreign key('کد دارو') references 'دارو موجود در انبار'");
            Helper.CreateTable("ارجاع دارو به شرکت",
                "'کد یکتای دارو' int," +
                "'کد دارو' int," +
                "'شماره ارجاع' int," +
                "'زمان ثبت درخواست خرید' date," +
                "'دلیل مرجوعی' varchar(512)," +
                "foreign key('کد یکتای دارو') references 'داروی شرکت'," +
                "foreign key('کد دارو') references 'دارو موجود در انبار'");

            Helper.CreateTable("تجمیع خرید دارو",
                "'کد یکتای دارو' int," +
                "'کد دارو' int," +
                "'کدپرسنلی' int," +
                "foreign key('کد یکتای دارو') references 'داروی شرکت'," +
                "foreign key('کد دارو') references 'دارو موجود در انبار'," +
                "foreign key('کدپرسنلی') references 'پرسنل'");
            Helper.CreateTable("تجمیع ارجاع دارو",
                "'کد یکتای دارو' int," +
                "'کد دارو' int," +
                "'کدپرسنلی' int," +
                "foreign key('کد یکتای دارو') references 'داروی شرکت'," +
                "foreign key('کد دارو') references 'دارو موجود در انبار'," +
                "foreign key('کدپرسنلی') references 'پرسنل'");


            Helper.CreateTable("داروی شرکت",
                "'کد یکتای دارو' int," +
                "'نام دارو' varchar(128)," +
                "'نام شرکت' varchar(128)," +
                "'قیمت خرید' int," +
                "primary key('کد یکتای دارو')");

            Helper.CreateTable("داروی تحت بیمه",
                "'نام شرکت' varchar(128)," +
                "'کد یکتای دارو' int," +
                "'تخفیف' int," +
                "foreign key('نام شرکت') references 'شرکت بیمه'," +
                "foreign key('کد یکتای دارو') references 'داروی شرکت'");


            Helper.Insert("پرسنل", "1,'1','admin','admin','admin'");
        }

        #endregion

        private void Close_it(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Close();
        }   
         private void Minimize(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void login(object sender, RoutedEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = Helper.Search("پرسنل", "'کدپرسنلی','نام و نام خانوادگی'", $"'رمزعبور'=='{pass.Text}' and 'کدپرسنلی'=={peronel_code.Text}");
          //  dt = Helper.GetTable("پرسنل", "'رمزعبور','کدپرسنلی'");
            if (dt.Rows.Count>0)
            {
                Helper.PersonalID = int.Parse(dt.Rows[0]["کدپرسنلی"].ToString());
                Helper.PersonalName = dt.Rows[0]["نام و نام خانوادگی"].ToString();
                MessageBox.Show("ورود موفق");
                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mr = MessageBox.Show("آیا میخواهید از برنامه شوید؟", "اخطار", MessageBoxButton.YesNo);
            if (mr.HasFlag(MessageBoxResult.Yes))
            {
                this.Close();
            }
        }
        private bool checkinput()
        {
            if (string.IsNullOrEmpty(peronel_code .Text) || string.IsNullOrEmpty(pass.Text))
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
    }
}
