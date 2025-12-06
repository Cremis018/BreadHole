using System;
using System.Collections.Generic;
using Godot;

public static class JunctionApplier
{
    #region fields
    private static readonly Dictionary<string, Action<Junction, string[]>> _handlers = new()
    {
        {"wall",HandleWallFeat},
        
    };
    #endregion
    
    #region op
    public static void Apply(Junction junction)
    { 
        if (!junction.E.HasComponent<FeatureComp>()) return;
        var feats = junction.E.GetComponent<FeatureComp>().Features;
        foreach (var feat in feats) ApplySingle(junction, feat);
    }

    private static void ApplySingle(Junction junction, string feature)
    {
        var (feat,args) = ParseFeature(feature);
    }
    
    private static (string,string[]) ParseFeature(string feature)
    {
        var args = feature.SubstringWithBracket("{", "}").Split(",");
        var feat = feature[..feature.IndexOf('{')];
        return (feat,args);
    }
    #endregion

    #region handle
    private static void HandleWallFeat(Junction junction, string[] args)
    {
        var comp = junction.E.AddComponentIfNone<JunctionComp>();
        comp.CanPass = true;
        comp.Texture = TileTextureFactory.Get("wall");
    }
    
    private static void HandleAirFeat(Junction junction, string[] args)
    {
        var comp = junction.E.AddComponentIfNone<JunctionComp>();
        comp.CanPass = true;
        comp.Texture = null;
    }
    
    private static void HandleDoorFeat(Junction junction, string[] args)
    {
        var comp = junction.E.AddComponentIfNone<JunctionComp>();
        comp.CanPass = true;
        comp.Texture = TileTextureFactory.Get("door");
    }

    private static void HandleGateFeat(Junction junction, string[] args)
    {
        var comp = junction.E.AddComponentIfNone<JunctionComp>();
        comp.CanPass = false;
        comp.Texture = TileTextureFactory.Get("gate");
    }

    private static void HandleEyeFeat(Junction junction, string[] args)
    {
        var sprite = junction.AddNodeIfNone<Sprite2D>("Peephole");
        sprite.Texture = TileTextureFactory.Get("eye");
    }

    private static void HandleLockedFeat(Junction junction, string[] args)
    {
        
    }
    #endregion
}

