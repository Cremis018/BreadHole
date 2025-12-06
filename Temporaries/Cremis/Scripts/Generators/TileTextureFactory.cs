using System.Collections.Generic;
using Godot;

public static class TileTextureFactory
{
    #region fields
    private static string _sceneSetName = "";
    
    private static readonly Dictionary<string, string> _textures = new()
    {
        {"spr_home_floor",""},
    };
    #endregion
    
    #region op
    public static Texture2D Get(string name)
    {
        var key = $"spr_{_sceneSetName}_{name}";
        return _textures.TryGetValue(key, out var path) 
            ? ResourceLoader.Load<Texture2D>(path) 
            : null;
    }
    
    public static void SetSceneSetName(string name)
    {
        _sceneSetName = name;
    }
    #endregion
}