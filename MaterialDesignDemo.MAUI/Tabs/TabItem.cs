namespace MaterialDesignDemo.Maui.Tabs;

public sealed class TabItem
{
    public TabItem(string title, string content)
    {
        Title = title;
        Content = content;
    }

    public string Title { get; }
    public string Content { get; }
}
