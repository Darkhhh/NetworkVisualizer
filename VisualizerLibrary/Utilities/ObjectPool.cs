namespace VisualizerLibrary.Utilities;

public class ObjectPool<T> where T : new()
{
    private readonly Queue<T> _queue;
    private readonly Func<T>? _generator;

    public ObjectPool(int capacity = 8, Func<T>? generator = null, bool fill = false)
    {
        _queue = new Queue<T>(capacity);
        _generator = generator;
        if (fill) FillQueue(capacity);
    }


    public T Get()
    {
        return _queue.TryDequeue(out var result) ? result : CreateNew();
    }

    public void Return(T value)
    {
        _queue.Enqueue(value);
    }


    private void FillQueue(int items)
    {
        for (int i = 0; i < items; i++)
        {
            _queue.Enqueue(CreateNew());
        }
    }
    private T CreateNew()
    {
        return _generator is null ? new T() : _generator.Invoke();
    }
}
