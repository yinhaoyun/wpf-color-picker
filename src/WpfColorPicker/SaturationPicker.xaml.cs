using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Dsafa.WpfColorPicker
{
    /// <summary>
    /// Interaction logic for SaturationPicker.xaml
    /// </summary>
    internal partial class SaturationPicker : SliderPicker
    {
        public static readonly DependencyProperty SaturationProperty
            = DependencyProperty.Register(nameof(Saturation), typeof(double), typeof(SaturationPicker), new PropertyMetadata(0.0, OnSaturationChanged));

        public SaturationPicker()
        {
            InitializeComponent();
        }

        public double Saturation
        {
            get => (double)GetValue(SaturationProperty);
            set => SetValue(SaturationProperty, value);
        }
   
        private static void OnSaturationChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var huePicker = (SaturationPicker)o;
            huePicker.UpdateAdorner((double)e.NewValue);
        }

        private void UpdateAdorner(double hue)
        {
            double percent = hue / 360;

            // Make it so that the arrow doesn't jump back to the top when it goes to the bottom
            Point mousePos = Mouse.GetPosition(this);
            if (percent == 0 && ActualHeight - mousePos.Y < 1)
            {
                percent = 1;
            }

            AdornerVerticalPercent = percent;
            AdornerColor = ColorHelper.FromHSV(hue, 1, 1);
        }

        protected override void OnAdornerPositionChanged(double verticalPercent)
        {
            Color c = saturationGradients.GradientStops.GetColorAtOffset(verticalPercent);
            AdornerColor = c;
            Saturation = c.GetSaturation();
        }
    }
}
