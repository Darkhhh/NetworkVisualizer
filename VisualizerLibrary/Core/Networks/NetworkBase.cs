using System.Collections;

namespace VisualizerLibrary.Core.Networks;

public abstract class NetworkBase : INetwork
{
    private Dictionary<Node, HashSet<Node>>? _adjacencyList;
    private bool[,]? _matrix;
    private int _currentIndex;
    protected Node[] Nodes = null!;

    /// <summary>
    /// Создание новой сети и инициализация всех данных
    /// </summary>
    public abstract void Init();
    /// <summary>
    /// Подготовка к симуляции, сброс значений до значений по-умолчанию. Нельзя заново иницицализировать массив Nodes.
    /// </summary>
    public abstract void PrepareForSimulation();
    /// <summary>
    /// Обновление состояние сети перед перерисовкой на канвасе.
    /// </summary>
    public abstract void UpdateState();
    /// <summary>
    /// После остановки симуляции
    /// </summary>
    public abstract void AfterSimulationStop();


    public Dictionary<Node, HashSet<Node>> GetAdjacencyList()
    {
        if (_adjacencyList is null)
        {
            _adjacencyList = new Dictionary<Node, HashSet<Node>>();
            foreach (var node in Nodes) _adjacencyList.Add(node, new HashSet<Node>());
        }
        INetwork.FormAdjacencyList(Nodes, in _adjacencyList);
        return _adjacencyList;
    }

    public bool[,] GetAdjacencyMatrix()
    {
        if (_matrix is null)
        {
            _matrix = new bool[Nodes.Length, Nodes.Length];
        }
        INetwork.FormAdjacencyMatrix(Nodes, in _matrix);
        return _matrix;
    }

    public Node GetNode(int index)
    {
        return Nodes[index];
    }

    public int NodesCount => Nodes.Length;

    private void AddNewNode(Node node)
    {
        Array.Resize(ref Nodes, Nodes.Length + 1);
        Nodes[Nodes.Length - 1] = node;
        _adjacencyList = null;
        _matrix = null;
    }

    private void DeleteNode(Node node)
    {
        var temp = Nodes.ToList();
        temp.Remove(node);
        Nodes = temp.ToArray();
        _adjacencyList = null;
        _matrix = null;
    }


    #region Enumeration

    public void Dispose() { }

    public IEnumerator<Node> GetEnumerator()
    {
        Reset();
        return this;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public Node Current => Nodes[_currentIndex];

    object IEnumerator.Current => Current;

    public bool MoveNext()
    {
        _currentIndex++;
        return _currentIndex < Nodes.Length;
    }

    public void Reset()
    {
        _currentIndex = -1;
    }

    #endregion
}
