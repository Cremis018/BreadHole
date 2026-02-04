using Godot;

public static class ViewInitializer
{
    public static void LoadingView(Node node)
    {
        var isEntity = node is IEntity;
        var entity = node as IEntity;
        var children = node.GetChildren();
        foreach (var child in children)
        {
            if (child is not Modifier modifier) continue;
            modifier.Load();
            if (isEntity) modifier.LoadInEntity(entity);
        }
    }
}