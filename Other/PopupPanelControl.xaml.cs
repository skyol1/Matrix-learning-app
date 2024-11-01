using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MaticeApp
{
    /// <summary>
    /// Interaction logic for PopupPanelControl.xaml
    /// </summary>
    public partial class PopupPanelControl : UserControl
    {
        public PopupPanelControl()
        {
            InitializeComponent();
        }

        // Popup trigger content
        public static readonly DependencyProperty PopupTriggerContentProperty =
            DependencyProperty.Register("PopupTriggerContent", typeof(object), typeof(PopupPanelControl), new PropertyMetadata(null));

        public object PopupTriggerContent
        {
            get { return GetValue(PopupTriggerContentProperty); }
            set { SetValue(PopupTriggerContentProperty, value); }
        }

        // Custom content for the Popup
        public static readonly DependencyProperty PopupContentProperty =
            DependencyProperty.Register("PopupContent", typeof(object), typeof(PopupPanelControl), new PropertyMetadata(null));

        public object PopupContent
        {
            get { return GetValue(PopupContentProperty); }
            set { SetValue(PopupContentProperty, value); }
        }

        private void OnTextBlockMouseEnter(object sender, MouseEventArgs e)
        {
            // Show the Popup when hovering the TextBlock
            popup.IsOpen = true;

            // Calculate available space and adjust Popup placement
            var triggerElement = sender as FrameworkElement;

            if (triggerElement != null)
            {
                var window = Window.GetWindow(triggerElement);
                if (window != null)
                {
                    
                    var triggerElementPosition = triggerElement.TransformToAncestor(window).Transform(new Point(0, 0));

                    // Decide the placement (above or below the trigger element)
                    // based on available space between it and the window borders
                    double availableSpaceBelow = window.ActualHeight - (triggerElementPosition.Y + triggerElement.ActualHeight);
                    double availableSpaceAbove = triggerElementPosition.Y;
                    if (availableSpaceBelow > availableSpaceAbove)
                    {
                        popup.VerticalOffset = 5;
                    }
                    else
                    {
                        popup.VerticalOffset = -(triggerElement.ActualHeight + borderContent.ActualHeight);
                    }

                    popup.HorizontalOffset = (triggerElement.ActualWidth - borderContent.ActualWidth) / 2;

                    Point popupCoordinates = popup.Child.PointToScreen(new Point(0, 0));

                    // Get DPI scaling
                    var (dpiX, dpiY) = GetDpi();
                    double scaleX = dpiX / 96.0;
                    double scaleY = dpiY / 96.0;
                    
                    popupCoordinates.X /= scaleX;
                    popupCoordinates.Y /= scaleY;

                    double offsetLeft = window.Left - popupCoordinates.X;
                    double offsetRight = popupCoordinates.X + borderContent.ActualWidth - (window.Left + window.ActualWidth);

                    if (offsetLeft > 0)
                    {
                        popup.HorizontalOffset += (offsetLeft + 15);
                    }
                    else if (offsetRight > 0)
                    {
                        popup.HorizontalOffset -= (offsetRight + 25);
                    }
                }
            }
        }

        private (double dpiX, double dpiY) GetDpi()
        {
            var hwndSource = PresentationSource.FromVisual(Application.Current.MainWindow) as HwndSource;
            if (hwndSource != null)
            {
                var dpi = hwndSource.CompositionTarget.TransformToDevice;
                return (dpi.M11 * 96, dpi.M22 * 96); // 96 is the default DPI
            }
            return (96, 96);
        }

        private void OnTextBlockMouseLeave(object sender, MouseEventArgs e)
        {
            // Hide the Popup when the mouse leaves the TextBlock
            popup.IsOpen = false;
        }
    }
}
