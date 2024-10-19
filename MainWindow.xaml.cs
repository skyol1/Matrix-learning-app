using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MaticeApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserControl _currentUserControl;

        public MainWindow()
        {
            InitializeComponent();
            _currentUserControl = UCMatice; // Set initial UserControl
        }
        
        
        private void PanelOpenMenu_MouseEnter(object sender, MouseEventArgs e)
        {
            PanelOpenMenu.Visibility = Visibility.Collapsed; // Hide the arrow panel
            PanelMenu.Visibility = Visibility.Visible;    // Show the stack panel
        }

        private void MenuList_MouseLeave(object sender, MouseEventArgs e)
        {
            PanelMenu.Visibility = Visibility.Collapsed;  // Hide the stack panel
            PanelOpenMenu.Visibility = Visibility.Visible;  // Show the arrow panel
        }

        private void ButtonMenu_ButtonClicked(object sender, RoutedEventArgs e)
        {
            var buttonMenu = sender as ButtonMenu;
            string selectedUserControlName = buttonMenu.UserControlName;

            if (_currentUserControl != null && _currentUserControl.Name == selectedUserControlName)
                return; // Do nothing if the same UserControl is selected

            // Hide the currently visible UserControl
            if (_currentUserControl != null)
            {
                _currentUserControl.Visibility = Visibility.Collapsed;
            }

            // Find and show the new UserControl
            _currentUserControl = (UserControl)FindName(selectedUserControlName);
            if (_currentUserControl != null)
            {
                _currentUserControl.Visibility = Visibility.Visible;
            }
        }
    }
}