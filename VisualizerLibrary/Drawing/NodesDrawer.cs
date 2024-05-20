using System.Windows.Controls;
using VisualizerLibrary.Core.Networks;
using VisualizerLibrary.Utilities;
using Point = System.Windows.Point;

namespace VisualizerLibrary.Drawing;

public class NodesDrawer
{
    private readonly ObjectPool<CanvasNode> _nodes = new();
    private readonly List<CanvasNode> _nodesOnCanvas = new();

    public void DrawNodes(Canvas canvas, INetwork network)
    {
        PrepareCanvasNodes(network.NodesCount);
        Draw(canvas, network);
    }

    private void PrepareCanvasNodes(int networkNodes)
    {
        var diff = networkNodes - _nodesOnCanvas.Count;
        if (diff == 0) return;
        if (diff > 0)
        {
            for (int i = 0; i < diff; i++)
            {
                _nodesOnCanvas.Add(_nodes.Get());
            }
        }
        else
        {
            for (int i = 0; i < -diff; i++)
            {
                _nodes.Return(_nodesOnCanvas[^1]);
                _nodesOnCanvas.RemoveAt(_nodesOnCanvas.Count - 1);
            }
        }
    }
    private void Draw(Canvas canvas, INetwork network)
    {
        var index = 0;
        foreach (var node in network)
        {
            _nodesOnCanvas[index].Draw(canvas);
            _nodesOnCanvas[index].MoveTo(new Point(node.X, node.Y), node.R);
            index++;
        }
    }

    public void Clear(Canvas canvas)
    {
        foreach (var node in _nodesOnCanvas)
        {
            node.Erase(canvas);
            _nodes.Return(node);
        }
        _nodesOnCanvas.Clear();
    }
}
