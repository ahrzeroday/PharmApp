using System.Windows;

namespace PharmApp
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Change_page : Window
    {
        public Change_page()
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

       
    }
}
