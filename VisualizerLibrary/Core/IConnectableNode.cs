namespace VisualizerLibrary.Core;

public interface IConnectableNode<in T> where T : IConnectableNode<T>
{
    public bool ConnectedTo(T node);
}
