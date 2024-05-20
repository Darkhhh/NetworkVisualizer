using System.Windows;
using Wpf.Ui;

namespace NetworkVisualizer.Code.Core;

public class PageService : IPageService
{
    private readonly Dictionary<Type, FrameworkElement> _pages = new();

    public PageService Register<T>(T instance) where T : FrameworkElement
    {
        _pages.TryAdd(typeof(T), instance);
        return this;
    }

    public T? GetPage<T>() where T : class
    {
        return GetPage(typeof(T)) as T;
    }

    public FrameworkElement? GetPage(Type pageType)
    {
        return _pages.TryGetValue(pageType, out var result) ? result : null;
    }
}
