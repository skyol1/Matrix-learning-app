using System.Windows;
using System.Windows.Controls;

namespace MaticeApp
{
    public partial class ButtonMenu : UserControl
    {
        public static readonly DependencyProperty ButtonTextProperty =
            DependencyProperty.Register("ButtonText", typeof(string), typeof(ButtonMenu), new PropertyMetadata("Button"));

        public string ButtonText
        {
            get { return (string)GetValue(ButtonTextProperty); }
            set { SetValue(ButtonTextProperty, value); }
        }

        public string UserControlName { get; set; } // Property to hold the UserControl name

        public event RoutedEventHandler ButtonClicked; // Event declaration

        public ButtonMenu()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ButtonClicked?.Invoke(this, e);
        }
    }
}

