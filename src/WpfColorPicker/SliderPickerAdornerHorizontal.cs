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
        private static readonly Pen Pen = new Pen(Brushes.Black, 1);
        private Brush _brush = Brushes.Red;

        public SliderPickerAdornerHorizontal(UIElement adornedElement)
            : base(adornedElement)
        {
            IsHitTestVisible = false;
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
            var height = 13;
            var x = ElementSize.Width * VerticalPercent;
            var y = 6;

            var triangleGeometry = new StreamGeometry();
            using (var context = triangleGeometry.Open())
            {
                context.BeginFigure(new Point(x, y), true, true);
                context.LineTo(new Point(x - 6, y + height), true, false);
                context.LineTo(new Point(x + 6, y + height), true, false);
            }

            var transformGroup = new TransformGroup();
            transformGroup.Children.Add(new ScaleTransform(-1, 1));
            transformGroup.Children.Add(new TranslateTransform(ElementSize.Width, 0));
            drawingContext.DrawGeometry(_brush, Pen, triangleGeometry);
        }
    }
}
