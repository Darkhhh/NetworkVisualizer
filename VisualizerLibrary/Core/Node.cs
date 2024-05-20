namespace VisualizerLibrary.Core;

public class Node : IConnectableNode<Node>
{
    public double X { get; set; }
    public double Y { get; set; }
    public double R { get; set; }

    public Node(double x, double y, double r)
    {
        X = x;
        Y = y;
        R = r;
    }

    public Node()
    {
        X = 0;
        Y = 0;
        R = 0;
    }


    public bool ConnectedTo(Node node)
    {
        return Distance(this, node) <= R;
    }

    public static double Distance(Node node1, Node node2)
    {
        return Math.Sqrt((node1.X - node2.X) * (node1.X - node2.X) + (node1.Y - node2.Y) * (node1.Y - node2.Y));
    }
}
