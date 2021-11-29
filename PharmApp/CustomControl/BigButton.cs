using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PharmApp.CustomControl
{
    public class BigButton : Button
    {
        static BigButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BigButton), new FrameworkPropertyMetadata(typeof(BigButton)));
        }

     

        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
            
        }
       
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageSource), typeof(BigButton), new PropertyMetadata(null));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }

        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(BigButton), new PropertyMetadata(null));



    }
}
