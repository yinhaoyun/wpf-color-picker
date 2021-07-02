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
        public static readonly DependencyProperty HueProperty
            = DependencyProperty.Register(nameof(Hue), typeof(double), typeof(SaturationPicker), new PropertyMetadata(0.0, OnHueChanged));

        public SaturationPicker()
        {
            InitializeComponent();
        }

        public double Saturation
        {
            get => (double)GetValue(SaturationProperty);
            set => SetValue(SaturationProperty, value);
        }
        public double Hue
        {
            get => (double)GetValue(HueProperty);
            set => SetValue(HueProperty, value);
        }

        private static void OnSaturationChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var saturationPicker = (SaturationPicker)o;
            saturationPicker.UpdateAdorner((double)e.NewValue);
        }
        private static void OnHueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var saturationPicker = (SaturationPicker)o;
            saturationPicker.saturationGradients.GradientStops[0].Color = ColorHelper.FromHSV((double)e.NewValue, 1, 1);
        }
        private void UpdateAdorner(double saturation)
        {
            double percent = 1.0 - saturation;

            // Make it so that the arrow doesn't jump back to the top when it goes to the bottom
            Point mousePos = Mouse.GetPosition(this);
            if (percent == 0 && ActualHeight - mousePos.Y < 1)
            {
                percent = 1;
            }

            AdornerVerticalPercent = percent;
            AdornerColor = ColorHelper.FromHSV(Hue, saturation, 1);
        }

        protected override void OnAdornerPositionChanged(double verticalPercent)
        {
            Color c = saturationGradients.GradientStops.GetColorAtOffset(verticalPercent);
            AdornerColor = c;
            Saturation = c.GetSaturation();
        }
    }
}
