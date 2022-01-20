using System.Windows;

namespace PharmApp
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Buy_medicine_page : Window
    {
        public Buy_medicine_page()
        {
            InitializeComponent();
        }

        private void Close_it(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Close();
        }
         private void Minimized(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
            this.WindowState = System.Windows.WindowState.Minimized;
        }

       
    }
}
