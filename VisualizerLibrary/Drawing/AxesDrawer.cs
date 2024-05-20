using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using VisualizerLibrary.Extensions;
using VisualizerLibrary.Utilities;

namespace VisualizerLibrary.Drawing;

public class AxesDrawer
{
    /// <summary>
    /// Length of axes markers counts as Canvas.Height * MarkersLengthScaler
    /// </summary>
    public double MarkersLengthScaler { get; set; } = 0.02;

    private readonly List<UIElement> _canvasElements = new();

    private readonly List<(Line Marker, Label Text)> _cachedListForXMarkers = new(), _cachedListForYMarkers = new();

    public void Draw(AxesData axes, Canvas canvas)
    {
        var canvasWidth = canvas.ActualWidth;
        var canvasHeight = canvas.ActualHeight;

        var center = CanvasScaler.FromAxesToCanvas(new Point(0, 0), canvas, axes.MinX, axes.MinY, axes.Width, axes.Height);
        (var xAxis, var yAxis) = GetAxes(center, canvasWidth, canvasHeight);

        GetAxesMarkers(axes, canvas, axes.MinX, axes.MaxX, axes.TickX, new Point(1, 0), LineOrientation.Vertical, string.Empty, in _cachedListForXMarkers);
        GetAxesMarkers(axes, canvas, axes.MinY, axes.MaxY, axes.TickY, new Point(0, 1), LineOrientation.Horizontal, "y", in _cachedListForYMarkers);

        canvas.Children.Add(xAxis);
        canvas.Children.Add(yAxis);
        _canvasElements.Add(xAxis);
        _canvasElements.Add(yAxis);

        foreach (var (Marker, Text) in _cachedListForXMarkers)
        {
            canvas.Children.Add(Marker);
            canvas.Children.Add(Text);
            _canvasElements.Add(Marker);
            _canvasElements.Add(Text);
        }
        foreach (var (Marker, Text) in _cachedListForYMarkers)
        {
            canvas.Children.Add(Marker);
            canvas.Children.Add(Text);
            _canvasElements.Add(Marker);
            _canvasElements.Add(Text);
        }
    }

    public void Erase(Canvas canvas) 
    {
        foreach (var item in _canvasElements)
        {
            canvas.Children.Remove(item);
        }
        _canvasElements.Clear();
    }

    private (Line, Line) GetAxes(Point center, double canvasWidth, double canvasHeight)
    {
        var xAxis = new Line
        {
            X1 = 0,
            Y1 = center.Y,
            X2 = canvasWidth,
            Y2 = center.Y,
            Stroke = Brushes.Black,
            StrokeThickness = 2
        };
        var yAxis = new Line
        {
            X1 = center.X,
            Y1 = 0,
            X2 = center.X,
            Y2 = canvasHeight,
            Stroke = Brushes.Black,
            StrokeThickness = 2
        };

        return (xAxis, yAxis);
    }

    private void GetAxesMarkers(AxesData axes, Canvas canvas, 
        double min, double max, double tick, Point axisMultiplier, LineOrientation orientation, string labelPrefix, in List<(Line Marker, Label Text)> list)
    {
        list.Clear();
        for (var axisValue = min; axisValue < max; axisValue += tick)
        {
            if (axisValue.IsZero()) continue;
            // y: axisMultiplier = (0,1)     x: axisMultiplier = (1,0)
            var axesPoint = new Point(axisMultiplier.X * axisValue, axisMultiplier.Y * axisValue);
            var length = canvas.ActualHeight * MarkersLengthScaler;

            var point = CanvasScaler.FromAxesToCanvas(axesPoint, canvas, axes.MinX, axes.MinY, axes.Width, axes.Height);
            var line = CommonShapes.CreateLineInPoint(point, length, orientation);

            // y: "y"    x: string.Empty
            var label = CreateLabel($"{labelPrefix}{axisValue:0.##}");

            Canvas.SetLeft(label, point.X - (length * axisMultiplier.Y));
            Canvas.SetTop(label, point.Y + (length / 2 * axisMultiplier.X));

            label.Loaded += LabelLoaded;
            list.Add((line, label));
        }
    }


    private static Label CreateLabel(string content)
    {
        return new Label
        {
            FontSize = 10,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Foreground = Brushes.Black,
            Content = content
        };
    }

    private void LabelLoaded(object sender, RoutedEventArgs e)
    {
        if (sender is not Label label) return;
        if (label.Content is not string text) return;

        var width = label.ActualWidth;
        var height = label.ActualHeight;
        var left = Canvas.GetLeft(label);
        var top = Canvas.GetTop(label);
        label.Loaded -= LabelLoaded;

        if (text.Contains('y'))
        {
            label.Content = text.Replace("y", string.Empty);
            Canvas.SetTop(label, Canvas.GetTop(label) - height / 2);
            Canvas.SetLeft(label, Canvas.GetLeft(label) - width / 2);
        }
        else
        {
            Canvas.SetLeft(label, Canvas.GetLeft(label) - width / 2);
        }
    }   
}
