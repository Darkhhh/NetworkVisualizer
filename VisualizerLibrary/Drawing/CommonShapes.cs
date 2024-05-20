using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace VisualizerLibrary.Drawing;

public enum LineOrientation
{
    Vertical, Horizontal
}

public static class CommonShapes
{
    public static Line CreateLineInPoint(Point center, double length, LineOrientation orientation, int thickness = 2)
    {
        double x1, y1, x2, y2;

        switch (orientation)
        {
            case LineOrientation.Horizontal:
                {
                    x1 = center.X - length / 2;
                    x2 = center.X + length / 2;
                    y1 = center.Y;
                    y2 = center.Y;
                    break;
                }
            case LineOrientation.Vertical:
                {
                    x1 = center.X;
                    x2 = center.X;
                    y1 = center.Y - length / 2;
                    y2 = center.Y + length / 2;
                    break;
                }
            default:
                {
                    throw new ArgumentException("Incorrect value for orientation parameter", nameof(orientation));
                }
        }

        return new Line { X1 = x1, Y1 = y1, X2 = x2, Y2 = y2, Stroke = Brushes.Black, StrokeThickness = thickness };
    }

    public static Ellipse DrawCircle(Canvas canvas, Point center, double radius, int thickness = 1, bool fill = false)
    {
        var circle = new Ellipse
        {
            Width = radius * 2,
            Height = radius * 2,
            Stroke = Brushes.Black,
            StrokeThickness = thickness
        };
        if (fill) circle.Fill = Brushes.Black;

        canvas.Children.Add(circle);
        Canvas.SetLeft(circle, center.X - radius);
        Canvas.SetTop(circle, center.Y - radius);

        return circle;
    }

    public static void DrawCircle(ref Ellipse circle, Canvas canvas, Point center, double radius, int thickness = 1, bool fill = false)
    {
        circle.Width = radius * 2;
        circle.Height = radius * 2;
        circle.Stroke = Brushes.Black;
        circle.StrokeThickness = thickness;
        if (fill) circle.Fill = Brushes.Black;

        canvas.Children.Add(circle);
        Canvas.SetLeft(circle, center.X - radius);
        Canvas.SetTop(circle, center.Y - radius);
    }

    public static Line DrawLine(Canvas canvas, Point start, Point end, Color color, int thickness = 1)
    {
        var line = new Line
        {
            X1 = start.X,
            Y1 = start.Y,
            X2 = end.X,
            Y2 = end.Y,
            Stroke = new SolidColorBrush(color),
            StrokeThickness = thickness
        };
        canvas.Children.Add(line);
        return line;
    }
}
