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
using static MaticeApp.Static;

namespace MaticeApp
{
    /// <summary>
    /// Useful: SetTriggerElement(), SetPlacementTarget(), popupHorizontalAlignment
    /// Trigger element can also be set in the designer
    /// </summary>
    public partial class PopupControl : UserControl
    {

        private UIElement ?triggerElement;
        private UIElement ?placementTargetElement;

        public enum PopupHorizontalAlignment
        {
            Left, Right, Center
        }
        public PopupHorizontalAlignment popupHorizontalAlignment { get; set; } = PopupHorizontalAlignment.Center;

        public PopupControl()
        {
            InitializeComponent();
            Loaded += HoverPanel_Loaded;
        }


        // PanelContent property for hover panel content
        public static readonly DependencyProperty PanelContentProperty =
            DependencyProperty.Register("PanelContent", typeof(UIElement), typeof(PopupControl));

        public UIElement PanelContent
        {
            get => (UIElement)GetValue(PanelContentProperty);
            set => SetValue(PanelContentProperty, value);
        }


        // TriggerContent property for setting the trigger element in XAML
        public static readonly DependencyProperty TriggerContentProperty =
            DependencyProperty.Register("TriggerContent", typeof(UIElement), typeof(PopupControl));

        public UIElement TriggerContent
        {
            get => (UIElement)GetValue(TriggerContentProperty);
            set => SetValue(TriggerContentProperty, value);
        }


        private void HoverPanel_Loaded(object sender, RoutedEventArgs e)
        {
            // If TriggerContent is defined in XAML, use it as the trigger element
            if (TriggerContent != null)
            {
                SetTriggerElement(TriggerContent);
            }
        }

        // Method to set the trigger element programmatically
        public void SetTriggerElement(UIElement trigger)
        {
            if (triggerElement != null)
            {
                triggerElement.MouseEnter -= OnTriggerElementMouseEnter;
                triggerElement.MouseLeave -= OnTriggerElementMouseLeave;
            }

            triggerElement = trigger;

            if (triggerElement != null)
            {
                triggerElement.MouseEnter += OnTriggerElementMouseEnter;
                triggerElement.MouseLeave += OnTriggerElementMouseLeave;
                if (placementTargetElement == null)
                    HoverPopup.PlacementTarget = triggerElement;
            }
        }

        // Method to set a custom placement target
        public void SetPlacementTarget(UIElement placementTarget)
        {
            placementTargetElement = placementTarget;
            HoverPopup.PlacementTarget = placementTarget ?? triggerElement;
        }

        public void SetPlacementMode(PlacementMode placementMode)
        {
            HoverPopup.Placement = placementMode;
        }

        // Event handler to show the popup on mouse enter
        private void OnTriggerElementMouseEnter(object sender, MouseEventArgs e)
        {
            HoverPopup.IsOpen = true;

            var triggerElement = sender as FrameworkElement;
            if (triggerElement == null) return;

            if (HoverPopup.Placement == PlacementMode.Bottom || HoverPopup.Placement == PlacementMode.Top) 
            { 
                switch (popupHorizontalAlignment)
                {
                    case PopupHorizontalAlignment.Center:
                        HoverPopup.HorizontalOffset = (triggerElement.ActualWidth - BorderContent.ActualWidth) / 2;
                        break;
                    case PopupHorizontalAlignment.Left:
                        break;
                    case PopupHorizontalAlignment.Right:
                        HoverPopup.HorizontalOffset = triggerElement.ActualWidth - BorderContent.ActualWidth;
                        break;
                }
            }
            else
            {
                HoverPopup.HorizontalOffset = 0;
            }
        }

        // Event handler to hide the popup on mouse leave
        private void OnTriggerElementMouseLeave(object sender, MouseEventArgs e)
        {
            HoverPopup.IsOpen = false;
        }



        /*
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


        */
        
    }
}
