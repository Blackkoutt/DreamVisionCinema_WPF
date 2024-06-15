public static class GlobalEventAggregator
{
    public static event EventHandler<ViewChangedEventArgs> ViewChanged;

    public static void RaiseViewChanged(object newView, string tabText)
    {
        ViewChanged?.Invoke(null, new ViewChangedEventArgs(newView, tabText));
    }
}

public class ViewChangedEventArgs : EventArgs
{
    public object NewView { get; }
    public string TabText { get; }

    public ViewChangedEventArgs(object newView, string tabText)
    {
        NewView = newView;
        TabText = tabText;
    }
}
