using Godot;

public partial class Component : Node
{
    public static T Create<T>() where T : Component
    {
        var script = GD.Load<CSharpScript>(ComponentPath.Get<T>());
        var node = (T)script.New();
        return (T)node;
    }
}