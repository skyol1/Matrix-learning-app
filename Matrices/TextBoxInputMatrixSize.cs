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
    public class TextBoxInputMatrixSize : TextBox
    {
        public bool Valid { get; set; }

        public TextBoxInputMatrixSize()
        {
            

            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.TextAlignment = TextAlignment.Center;
            this.FontSize = 18;
            this.MaxLength = 1;
            this.Valid = false;

            this.PreviewTextInput += TextBoxInputMatrixSize_PreviewTextInput;
        }

        private void TextBoxInputMatrixSize_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (this.Text.Length == MaxLength)
            {
                this.Text = e.Text;
                e.Handled = true;
                this.CaretIndex = this.Text.Length;
            }
        }
    }
}
