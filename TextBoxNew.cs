using System.Windows;
using System.Windows.Controls;

namespace CourseWork
{
    /// <summary>
    /// Extension of TextBox
    /// </summary>
    public class TextBoxNew : TextBox
    {
        static TextBoxNew()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBox), new FrameworkPropertyMetadata(typeof(TextBox)));
        }
        public TextBoxNew() : base() 
        {
            KeyDown += TextBox_TextChanged;
            LostFocus += TextBoxNew_LostFocus;
        }

        private void TextBoxNew_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBoxNew box = (TextBoxNew)sender;
            if (box.Text == "")
            {
                box.Text = "Username";
            }
        }
        private void TextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            TextBoxNew box = (TextBoxNew)sender;
            if (box.Text == "Username")
            {
                box.Text = "";
            }
        }
    }
}
