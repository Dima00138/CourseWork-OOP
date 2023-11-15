using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CourseWork
{
    public class MenuItem : RadioButton
    {

        public static readonly DependencyProperty ImageProperty =
        DependencyProperty.Register("Image", typeof(ImageSource), typeof(MenuItem));

        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        static MenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MenuItem), new FrameworkPropertyMetadata(typeof(MenuItem)));
        }
    }
}
