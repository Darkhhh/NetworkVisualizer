namespace VisualizerLibrary.Core.Networks;

public interface INetwork : IEnumerator<Node>, IEnumerable<Node>
{
    public void Init();

    public void PrepareForSimulation();

    public void AfterSimulationStop();

    public Dictionary<Node, HashSet<Node>> GetAdjacencyList();

    public bool[,] GetAdjacencyMatrix();

    public void UpdateState();

    public Node GetNode(int index);

    public int NodesCount { get; }

    public static void FormAdjacencyList<T>(T[] items, in Dictionary<T, HashSet<T>> list) where T : IConnectableNode<T>
    {
        foreach (var item in list) item.Value.Clear();
        for (var i = 0; i < items.Length; i++)
        {
            if (!list.ContainsKey(items[i])) list.Add(items[i], new HashSet<T>());

            for (var j = 0; j < items.Length; j++)
            {
                if (ReferenceEquals(items[i], items[j])) continue;
                var connected = items[i].ConnectedTo(items[j]);

                if (connected) list[items[i]].Add(items[j]);
            }
        }
    }

    public static void FormAdjacencyMatrix<T>(T[] items, in bool[,] matrix) where T : IConnectableNode<T>
    {
        for (var i = 0; i < items.Length; i++)
        {
            for (var j = 0; j < items.Length; j++)
            {
                if (i == j)
                {
                    matrix[i, j] = false;
                    continue;
                }
                matrix[i, j] = items[i].ConnectedTo(items[j]);
            }
        }
    }

    public static bool IsConnected<T>(Dictionary<T, HashSet<T>> adjacencyList, T start, Stack<T> cachedStack, HashSet<T> cachedSet) where T : notnull
    {
        cachedSet.Clear();

        if (!adjacencyList.ContainsKey(start)) return false;

        cachedStack.Clear();
        cachedStack.Push(start);

        while (cachedStack.Count > 0)
        {
            var vertex = cachedStack.Pop();
            if (cachedSet.Contains(vertex)) continue;
            cachedSet.Add(vertex);
            foreach (var neighbor in adjacencyList[vertex]) if (!cachedSet.Contains(neighbor)) cachedStack.Push(neighbor);
        }

        return cachedSet.Count == adjacencyList.Count;
    }
}
