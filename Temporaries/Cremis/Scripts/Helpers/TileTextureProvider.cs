using System.Collections.Generic;
using Godot;

public static class TileTextureProvider
{
    #region fields
    private static string _sceneSetName = "debug";

    private const string _rootPath = "res://Assets/Canvas/Sprites/";
    #endregion
    
    #region op
    public static Texture2D Get(string name,string suffix = "tres")
    {
        var file = $"spr_{_sceneSetName}_{name}.{suffix}";
        var path = $"{_rootPath}{file}";
        return ResourceLoader.Exists(path)
            ? ResourceLoader.Load<Texture2D>(path)
            : null;
    }
    
    public static void SetSceneSetName(string name)
    {
        _sceneSetName = name;
    }
    #endregion
}