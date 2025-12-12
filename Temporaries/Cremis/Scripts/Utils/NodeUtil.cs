using System.Linq;
using Godot;

public static class NodeUtil
{
    #region op
    public static T AddNodeIfNone<T>(this Node node,string name = null) where T : Node, new()
    {
        name = string.IsNullOrWhiteSpace(name)
            ? typeof(T).ToString()
            : name;
        if (node.GetNodeOrNull<T>(name) is null) node.AddChild(new T()
        {
            Name = name
        });
        return node.GetNode<T>(name);
    }

    public static void ClearChildren(this Node node)
    {
        var children = node.GetChildren();
        if (children.Count == 0) return;
        children.ToList().ForEach(n =>
        {
            node.RemoveChild(n);
            n.QueueFree();
        });
    }
    #endregion
}