using System.Data;
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
        }

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
            dt = Helper.GetTable("پرسنل", "'رمزعبور','کد پرسنلی'");
            if (dt.Rows[1]["رمزعبور"].ToString() == pass.Text)
            {
                /////////////////
                MessageBox.Show("ورود موفق");

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
    }
}
