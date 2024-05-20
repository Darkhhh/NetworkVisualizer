namespace VisualizerLibrary.Extensions;

public static class NumericExtensions
{
    public static bool IsZero(this double value, double precision = double.Epsilon) => Math.Abs(value) < precision;
}
