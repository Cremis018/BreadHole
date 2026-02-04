using System;
using System.Collections.Generic;
using Godot;

public static class ItemTextureStorage
{
    private static readonly object _lock = new();
    private static readonly Dictionary<string, Texture2D> _nameTextureMap = new(StringComparer.OrdinalIgnoreCase);

    public static void UpdateData(Dictionary<string, Texture2D> data)
    {
        _nameTextureMap.Clear();
        foreach (var kvp in data)
        {
            _nameTextureMap[kvp.Key] = kvp.Value;
        }
    }

    public static Texture2D GetTexture(string name) => _nameTextureMap.GetValueOrDefault(name);

    public static Texture2D GetTexture(Type type) => GetTexture(type.ToString());
}