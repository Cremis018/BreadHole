using System;
using System.Collections.Generic;
using Godot;

public static class NodeUtil
{
    private static Dictionary<Type, string> _typeUidMap = new()
    {
        {typeof(Cell),"uid://dtxiydwfie6rv"},
        {typeof(Edge),"uid://dfnm1bam34vxg"},
        {typeof(Junction),"uid://bstgki13l6x1n"},
        {typeof(Placeholder),"uid://cl68yx036ljlt"},
        {typeof(Void),"uid://bnyowvibhqt6c"},
    };

    public static T Create<T>() where T : Node
    {
        if (!_typeUidMap.TryGetValue(typeof(T), out var uid)) return null;
        var instantiate = ResourceLoader.Load<PackedScene>(uid).Instantiate();
        var node = instantiate.Duplicate();
        if (node is IEntity) node._EnterTree();
        return node as T;
    }
}