namespace VisualizerLibrary.Extensions;

public static class RandomExtensions
{
    public static double Sign(this Random random)
    {
        return random.NextDouble() < 0.5 ? -1 : 1;
    }
}
