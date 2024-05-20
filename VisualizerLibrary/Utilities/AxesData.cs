namespace VisualizerLibrary.Utilities;

public struct AxesData
{
    public double MinX, MinY;
    public double MaxX, MaxY;
    public double TickX, TickY;

    public double Width => MaxX - MinX;
    public double Height => MaxY - MinY;
}
