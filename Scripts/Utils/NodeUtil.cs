using System;
using System.Collections.Generic;
using Godot;

public static class NodeUtil
{
    private static Dictionary<Type, string> _typeUidMap = new()
    {
        {typeof(Cell),"uid://dtxiydwfie6rv"},
        {typeof(Junction),"uid://bstgki13l6x1n"},
        {typeof(Void),"uid://bnyowvibhqt6c"},
    };

    public static T Create<T>() where T : Node
    {
        if (!_typeUidMap.TryGetValue(typeof(T), out var uid)) return null;
        var instantiate = ResourceLoader.Load<PackedScene>(uid).Instantiate();
        return instantiate.Duplicate() as T;
    }
}