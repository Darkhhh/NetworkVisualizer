using Petzold.Media2D;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using VisualizerLibrary.Core.Networks;
using VisualizerLibrary.Utilities;

namespace VisualizerLibrary.Drawing;

public class ConnectionLinesDrawer
{
    private readonly ObjectPool<ArrowLine> _linesPool = new();
    private readonly List<ArrowLine> _oneWayLines = new();
    private readonly List<ArrowLine> _twoWayLines = new();
    private readonly List<LineData> _oneWayIterationLines = new();
    private readonly List<LineData> _twoWayIterationLines = new();


    public void DrawConnectionLines(Canvas canvas, INetwork network, bool[,] matrix, int nodesCount)
    {
        FillIterationLines(network, matrix, nodesCount);
        NormalizeLinesCount(canvas, _oneWayLines, _oneWayIterationLines.Count, OneWayLine);
        NormalizeLinesCount(canvas, _twoWayLines, _twoWayIterationLines.Count, TwoWayLine);

#if DEBUG
        if (_oneWayLines.Count != _oneWayIterationLines.Count || _twoWayLines.Count != _twoWayIterationLines.Count)
            throw new Exception("Normalizing lines number went wrong");
#endif

        for (int i = 0; i < _oneWayIterationLines.Count; i++)
        {
            PlaceLine(_oneWayLines[i], _oneWayIterationLines[i].Start, _oneWayIterationLines[i].End);
        }
        for (int i = 0; i < _twoWayIterationLines.Count; i++)
        {
            PlaceLine(_twoWayLines[i], _twoWayIterationLines[i].Start, _twoWayIterationLines[i].End);
        }
        _oneWayIterationLines.Clear();
        _twoWayIterationLines.Clear();
    }

    public void Clear(Canvas canvas)
    {
        foreach (var line in _oneWayLines)
        {
            _linesPool.Return(line);
            canvas.Children.Remove(line);
        }
        foreach (var line in _twoWayLines)
        {
            _linesPool.Return(line);
            canvas.Children.Remove(line);
        }
        _oneWayLines.Clear();
        _twoWayLines.Clear();
    }


    private void FillIterationLines(INetwork network, bool[,] matrix, int nodesCount)
    {
        for (int i = 1; i < nodesCount; i++)
        {
            for (int j = 0; j < i; j++)
            {
                if (!matrix[i, j] && !matrix[j, i]) continue;

                var startNode = network.GetNode(i);
                var endNode = network.GetNode(j);

                if (matrix[i, j] && matrix[j, i])
                {
                    // TwoWay Line
                    _twoWayIterationLines.Add(new LineData
                    {
                        Start = new Point(startNode.X, startNode.Y),
                        End = new Point(endNode.X, endNode.Y)
                    });
                }
                else if (matrix[i, j] && !matrix[j, i])
                {
                    // OneWay Line From i To j
                    _oneWayIterationLines.Add(new LineData
                    {
                        Start = new Point(startNode.X, startNode.Y),
                        End = new Point(endNode.X, endNode.Y)
                    });
                }
                else if (!matrix[i, j] && matrix[j, i])
                {
                    // OneWay Line From j To i
                    _oneWayIterationLines.Add(new LineData
                    {
                        Start = new Point(endNode.X, endNode.Y),
                        End = new Point(startNode.X, startNode.Y)
                    });
                }
            }
        }
    }
    private void NormalizeLinesCount(Canvas canvas, List<ArrowLine> activeLines, int requiredLines, Action<ArrowLine> formerForNewLines)
    {
        var diff = Math.Abs(activeLines.Count - requiredLines);
        if (diff == 0) return;

        if (activeLines.Count < requiredLines)
        {
            for (int i = 0; i < diff; i++)
            {
                var line = _linesPool.Get();
                formerForNewLines.Invoke(line);
                activeLines.Add(line);
                canvas.Children.Add(activeLines[^1]);
            }
        }
        else
        {
            for (int i = 0; i < diff; i++)
            {
                _linesPool.Return(activeLines[^1]);
                canvas.Children.Remove(activeLines[^1]);
                activeLines.RemoveAt(activeLines.Count - 1);
            }
        }
    }


    private static void PlaceLine(ArrowLine line, Point start, Point end)
    {
        line.X1 = start.X;
        line.Y1 = start.Y;
        line.X2 = end.X;
        line.Y2 = end.Y;
        line.StrokeThickness = 1;
    }
    private static void TwoWayLine(ArrowLine line)
    {
        line.ArrowEnds = ArrowEnds.Both;
        line.Stroke = Brushes.Green;
    }
    private static void OneWayLine(ArrowLine line)
    {
        line.ArrowEnds = ArrowEnds.End;
        line.Stroke = Brushes.Orange;
    }

    private struct LineData
    {
        public Point Start, End;
    }
}
