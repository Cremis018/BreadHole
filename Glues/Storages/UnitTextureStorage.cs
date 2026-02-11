using System;
using System.Collections.Generic;
using Godot;

internal static class UnitTextureStorage
{
    public const string InnerPath = "res://Assets/Sprites/Units/";
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
    public static Texture2D GetTexture(string prefix, string name) => _nameTextureMap.GetValueOrDefault($"{prefix}_{name}");
}