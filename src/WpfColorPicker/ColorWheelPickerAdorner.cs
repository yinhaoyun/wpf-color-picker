using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Dsafa.WpfColorPicker
{
    internal class ColorWheelPickerAdorner : Adorner
    {
        private static readonly DependencyProperty PositionProperty
            = DependencyProperty.Register(nameof(Position), typeof(Point), typeof(ColorWheelPickerAdorner), new FrameworkPropertyMetadata(new Point(), FrameworkPropertyMetadataOptions.AffectsRender));
        private static readonly Brush FillBrush = Brushes.Transparent;
        private static readonly Pen InnerRingPen = new Pen(Brushes.White, 2.6);
        private static readonly Pen OuterRingPen = new Pen(Brushes.Black, 0.6);

        internal ColorWheelPickerAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            IsHitTestVisible = false;
        }

        internal Point Position
        {
            get => (Point)GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            drawingContext.DrawEllipse(FillBrush, InnerRingPen, Position, 10, 10);
            drawingContext.DrawEllipse(FillBrush, OuterRingPen, Position, 12, 12);
        }
    }
}
