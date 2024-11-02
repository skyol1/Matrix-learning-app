using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;

namespace MaticeApp
{
    public class TextBoxInputMatrix : TextBox
    {
        public bool Valid { get; set; }

        private bool IsDefault0;

        private readonly Brush DefaultForeground;

        public TextBoxInputMatrix()
        {
            this.GotFocus += TextBoxInputMatrix_GotFocus;
            this.LostFocus += TextBoxInputMatrix_LostFocus;
            //this.TextChanged += TextBoxInputMatrix_TextChanged;

            DefaultForeground = this.Foreground;

            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.VerticalAlignment = VerticalAlignment.Center;
            this.TextAlignment = TextAlignment.Center;
            this.FontSize = 18;

            this.Text = "0";
            this.Foreground = Brushes.DarkGray;
            IsDefault0 = true;
        }

        private void TextBoxInputMatrix_GotFocus(object sender, RoutedEventArgs e)
        {
            if (IsDefault0)
            {
                this.Foreground = DefaultForeground;
                IsDefault0 = false;
                this.Text = string.Empty;
            }
        }

        private void TextBoxInputMatrix_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.Text))
            {
                MakeDefault0();
            }
        }

        public void MakeDefault0()
        {
            this.Text = "0";
            this.Foreground = Brushes.DarkGray;
            IsDefault0 = true;
        }

        public void MakeNormal()
        {
            if (IsDefault0)
            {
                this.Foreground = DefaultForeground;
                IsDefault0 = false;
            }
        }
    }
}
