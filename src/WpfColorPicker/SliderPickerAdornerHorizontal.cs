using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Dsafa.WpfColorPicker
{
    internal class SliderPickerAdornerHorizontal : Adorner
    {
        private static readonly DependencyProperty VerticalPercentProperty 
            = DependencyProperty.Register(nameof(VerticalPercent), typeof(double), typeof(SliderPickerAdornerHorizontal), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));
        private static readonly DependencyProperty ColorProperty
            = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(SliderPickerAdornerHorizontal), new FrameworkPropertyMetadata(Colors.Red, FrameworkPropertyMetadataOptions.AffectsRender));
        private Pen Pen;
        private Brush _brush = Brushes.Red;

        public SliderPickerAdornerHorizontal(UIElement adornedElement)
            : base(adornedElement)
        {
            IsHitTestVisible = false;
            Brush borderColor = new SolidColorBrush(Color.FromRgb(90, 90, 90));
            Pen = new Pen(borderColor, 0.75);
        }

        public double VerticalPercent
        {
            get => (double)GetValue(VerticalPercentProperty);
            set => SetValue(VerticalPercentProperty, value);
        }

        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set
            {
                SetValue(ColorProperty, value);
                _brush = new SolidColorBrush(value);
            }
        }

        public Rect ElementSize { get; set; }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            var height = 16;
            var triangleWidth = 8;
            var x = triangleWidth + (ElementSize.Width - triangleWidth * 2) * VerticalPercent;
            var y = 13;

            var triangleGeometry = new StreamGeometry();
            using (var context = triangleGeometry.Open())
            {
                context.BeginFigure(new Point(x, y), true, true);
                context.LineTo(new Point(x - triangleWidth, y + height), true, false);
                context.LineTo(new Point(x + triangleWidth, y + height), true, false);
            }

            var transformGroup = new TransformGroup();
            transformGroup.Children.Add(new ScaleTransform(-1, 1));
            transformGroup.Children.Add(new TranslateTransform(ElementSize.Width, 0));
            drawingContext.DrawGeometry(_brush, Pen, triangleGeometry);
        }
    }
}
