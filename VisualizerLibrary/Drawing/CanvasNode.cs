using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace VisualizerLibrary.Drawing;

public class CanvasNode
{
    private Ellipse _radius = new(), _dot= new ();

    public Ellipse RadiusCircle { get => _radius; private set => _radius = value; }

    public Ellipse CenterCircle { get => _dot; private set => _dot = value; }

    public void Draw(Canvas canvas)
    {
        if (canvas.Children.Contains(RadiusCircle) || canvas.Children.Contains(CenterCircle)) return;
        canvas.Children.Add(RadiusCircle);
        canvas.Children.Add(CenterCircle);
    }

    public void Erase(Canvas canvas)
    {
        canvas.Children.Remove(RadiusCircle);
        canvas.Children.Remove(CenterCircle);
    }

    public void MoveTo(Point center, double radius)
    {
        MoveCircle(ref _radius, center, radius);
        MoveCircle(ref _dot, center, radius * 0.05, fill: true);
    }

    private static void MoveCircle(ref Ellipse circle, Point center, double radius, int thickness = 1, bool fill = false)
    {
        circle.Width = radius * 2;
        circle.Height = radius * 2;
        circle.Stroke = Brushes.Black;
        circle.StrokeThickness = thickness;
        if (fill) circle.Fill = Brushes.Black;

        Canvas.SetLeft(circle, center.X - radius);
        Canvas.SetTop(circle, center.Y - radius);
    }
}
