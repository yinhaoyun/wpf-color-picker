using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Dsafa.WpfColorPicker
{
    /// <summary>
    /// Interaction logic for BrightnessPicker.xaml
    /// </summary>
    internal partial class BrightnessPicker : SliderPicker
    {
        public static readonly DependencyProperty HueProperty
            = DependencyProperty.Register(nameof(Hue), typeof(double), typeof(BrightnessPicker), new PropertyMetadata(0.0, OnHueChanged));
        public static readonly DependencyProperty SaturationProperty
            = DependencyProperty.Register(nameof(Saturation), typeof(double), typeof(BrightnessPicker), new PropertyMetadata(0.0, OnSaturationChanged));
        public static readonly DependencyProperty BrightnessProperty
            = DependencyProperty.Register(nameof(Brightness), typeof(double), typeof(BrightnessPicker), new PropertyMetadata(0.0, OnBrightnessChanged));

        public BrightnessPicker()
        {
            InitializeComponent();
        }

        public double Hue
        {
            get => (double)GetValue(HueProperty);
            set => SetValue(HueProperty, value);
        }

        public double Saturation
        {
            get => (double)GetValue(SaturationProperty);
            set => SetValue(SaturationProperty, value);
        }

        public double Brightness
        {
            get => (double)GetValue(BrightnessProperty);
            set => SetValue(BrightnessProperty, value);
        }

        private static void OnHueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            BrightnessPicker brightnessPicker = (BrightnessPicker)o;
            double hue = (double)e.NewValue;
            brightnessPicker.brightnessGradients.GradientStops[0].Color = 
                ColorHelper.FromHSV(hue, brightnessPicker.Saturation, 1);
            brightnessPicker.UpdateAdorner(brightnessPicker.Brightness);
        }

        private static void OnSaturationChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            BrightnessPicker brightnessPicker = (BrightnessPicker)o;
            double saturation = (double)e.NewValue;
            brightnessPicker.brightnessGradients.GradientStops[0].Color = 
                ColorHelper.FromHSV(brightnessPicker.Hue, saturation, 1);
            brightnessPicker.UpdateAdorner(brightnessPicker.Brightness);
        }

        private static void OnBrightnessChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            BrightnessPicker brightnessPicker = (BrightnessPicker)o;
            brightnessPicker.UpdateAdorner((double)e.NewValue);
        }

        private void UpdateAdorner(double brightness)
        {
            double percent = 1.0 - brightness;

            // Make it so that the arrow doesn't jump back to the top when it goes to the bottom
            //Point mousePos = Mouse.GetPosition(this);
            //if (percent == 0 && ActualHeight - mousePos.Y < 1)
            //{
            //    percent = 1;
            //}

            AdornerVerticalPercent = percent;
            AdornerColor = ColorHelper.FromHSV(Hue, Saturation, brightness);
        }

        protected override void OnAdornerPositionChanged(double verticalPercent)
        {
            Color c = brightnessGradients.GradientStops.GetColorAtOffset(verticalPercent);
            AdornerColor = c;
            Brightness = c.GetBrightness();
        }
    }
}
