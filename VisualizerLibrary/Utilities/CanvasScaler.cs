using System.Windows;
using System.Windows.Controls;

namespace VisualizerLibrary.Utilities;

public static class CanvasScaler
{
    public static Point FromAxesToCanvas(Point point, Canvas canvas, double minX, double minY, double xAxisWidth, double yAxisHeight, bool exceptionWhenOutisdeOfCanvas = false)
    {
        if (!canvas.IsLoaded) throw new Exception("Trying to convert point while canvas is not loaded");
        var canvasWidth = canvas.ActualWidth;
        var canvasHeight = canvas.ActualHeight;

        var deltaX = point.X - minX;
        var desiredX = deltaX * canvasWidth / xAxisWidth;

        var deltaY = point.Y - minY;
        var desiredY = deltaY * canvasHeight / yAxisHeight;

        if (exceptionWhenOutisdeOfCanvas)
        {
            if (desiredX < 0 || desiredY < 0 || desiredX > canvasWidth || desiredY > canvasHeight)
                throw new Exception($"Could not convert point {point}");
        }        
        return new Point(desiredX, canvasHeight - desiredY);
    }

    public static double FromAxesToCanvas(double length, Canvas canvas, double minX, double minY, double xAxisWidth, double yAxisHeight, bool byX = true)
    {
        if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length), "Value can not be below zero");
        return byX ?
            FromAxesToCanvas(new Point(minX + length, 0), canvas, minX, minY, xAxisWidth, yAxisHeight).X :
            FromAxesToCanvas(new Point(0, minY + length), canvas, minX, minY, xAxisWidth, yAxisHeight).Y;
    }
}
