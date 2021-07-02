using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Dsafa.WpfColorPicker
{
    internal partial class ColorWheelPicker : UserControl
    {
        public static readonly DependencyProperty HueProperty 
            = DependencyProperty.Register(nameof(Hue), typeof(double), typeof(ColorWheelPicker), new PropertyMetadata(0.0, OnHueChanged));
        public static readonly DependencyProperty SaturationProperty 
            = DependencyProperty.Register(nameof(Saturation), typeof(double), typeof(ColorWheelPicker), new PropertyMetadata(0.0, OnSaturationChanged));
        public static readonly DependencyProperty BrightnessProperty 
            = DependencyProperty.Register(nameof(Brightness), typeof(double), typeof(ColorWheelPicker), new PropertyMetadata(0.0));
        private readonly ColorWheelPickerAdorner _adorner;
        private Point mPoint = new Point();
        private Point mCenterPoint = new Point();

        public ColorWheelPicker()
        {
            InitializeComponent();
            _adorner = new ColorWheelPickerAdorner(this);
            Loaded += ColorWheelPickerOnLoaded;
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

        public Point CurrentPoint
        {
            get
            {
                mCenterPoint.X = ActualWidth / 2;
                mCenterPoint.Y = ActualHeight / 2;

                double theta = -Hue / 57.5; // TODO: Somehow I don't know the formula
                double r = Saturation * mCenterPoint.X;

                mPoint.X = mCenterPoint.X + r * Math.Cos(theta);
                mPoint.Y = mCenterPoint.Y + r * Math.Sin(theta);

                return mPoint;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            Mouse.Capture(this);
            var pos = e.GetPosition(this).Clamp(this);
            Update(pos);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            Mouse.Capture(null);
            var pos = e.GetPosition(this).Clamp(this);
            Update(pos);
        }

        private static void OnSaturationChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var picker = (ColorWheelPicker)o;
            var sat = (double)e.NewValue;
            var pos = picker._adorner.Position;
            picker._adorner.Position = picker.CurrentPoint;
        }

        private static void OnHueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var picker = (ColorWheelPicker)o;
            var bright = (double)e.NewValue;
            var pos = picker._adorner.Position;
            picker._adorner.Position = picker.CurrentPoint;
        }

        private void ColorWheelPickerOnLoaded(object sender, RoutedEventArgs e)
        {
            AdornerLayer.GetAdornerLayer(this).Add(_adorner);
            _adorner.Position = new Point(Saturation * ActualWidth, (1 - Brightness) * ActualHeight);
        }

        private void Update(Point p)
        {
            //_adorner.Position = p;
            Point center = new Point(ActualWidth / 2, ActualHeight / 2);
            double R = ActualWidth / 2;
            double deltaX = p.X - center.X;
            double deltaY = p.Y - center.Y;
            double degree = RadianToDegree(Math.Atan2(deltaY, deltaX));
            double r = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));
            double s = r / R;
            Color color = ColorHelper.FromHSV(degree, s, 1);
            Console.WriteLine($"Update: Center={center}, p={p}, degree={degree}, s={s}, color={color}");
            Hue = color.GetHue();
            Saturation = color.GetSaturation();
        }

        private double RadianToDegree(double angle)
        {
            double degree = angle * (180.0 / Math.PI);
            if (degree < 0)
            {
                degree = 360 + degree;
            }
            degree = 360 - degree;
            return degree;
        }

        private double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }
    }
}
