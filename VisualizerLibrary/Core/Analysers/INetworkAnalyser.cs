using System.Collections.ObjectModel;
using VisualizerLibrary.Core.Networks;

namespace VisualizerLibrary.Core.Analysers;

public interface INetworkAnalyser
{
    public void UpdateState(INetwork network, Dictionary<Node, HashSet<Node>> adjacencyList, bool connected, Action<string, string> onValueChanged);

    public bool HasChartVisualization();

    public ObservableCollection<double>? GetChartCollection();

    public string GetParameterName();
}
