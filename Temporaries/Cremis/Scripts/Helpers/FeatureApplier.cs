using System;
using System.Collections.Generic;
using Godot;

public static class FeatureApplier
{
    #region fields
    private static readonly Dictionary<string, Action<IEntity, string[]>> _handlers = new()
    {
        {"wall",HandleWallFeat},
        {"air",HandleAirFeat},
        {"door",HandleDoorFeat},
        {"gate",HandleGateFeat},
        {"eye",HandleEyeFeat},
        {"locked",HandleLockedFeat},
        {"carpet",HandleCarpetFeat},
        {"heal",HandleHealFeat},
        {"text",HandleTextFeat},
        {string.Empty,HandleAirFeat},
    };
    #endregion
    
    #region op
    public static void Apply(IEntity entity)
    { 
        Reset(entity);
        var feats = entity.E.AddComponentIfNone<FeatureComp>().Features;
        foreach (var feat in feats) ApplySingle(entity, feat);
    }

    private static void Reset(IEntity entity)
    {
        if (entity is not Node node) return;
        var junctionComp = entity.E.AddComponentIfNone<JunctionComp>();
        var mapCompositionComp = entity.E.AddComponentIfNone<MapCompositionComp>();
        var markableComp = entity.E.AddComponentIfNone<MarkableComp>();
        var featureComp = entity.E.AddComponentIfNone<FeatureComp>();
        foreach (var child in node.GetChildren())
            if (child is Sprite2D sprite)
                sprite.Texture = null;
        entity.E.ClearComponentWithBlacklist([junctionComp,mapCompositionComp,markableComp,featureComp]);
    }

    private static void ApplySingle(IEntity entity, string feature)
    {
        var (feat,args) = ParseFeature(feature);
        _handlers.TryGetValue(feat, out var handler);
        handler?.Invoke(entity, args);
    }
    
    private static (string,string[]) ParseFeature(string feature)
    {
        var found = feature.IndexOf('{');
        if (found < 0) return (feature,[]);
        var args = feature.SubstringWithBracket("{", "}").Split('|');
        var feat = feature[..found];
        return (feat,args);
    }
    #endregion

    #region handle
    private static void HandleWallFeat(IEntity entity, string[] args)
    {
        var comp = entity.E.AddComponentIfNone<JunctionComp>();
        comp.CanPass = true;
        comp.Texture = TileTextureProvider.Get("wall");
    }
    
    private static void HandleAirFeat(IEntity entity, string[] args)
    {
        var comp = entity.E.AddComponentIfNone<JunctionComp>();
        comp.CanPass = true;
        comp.Texture = TileTextureProvider.Get("air");
    }
    
    private static void HandleDoorFeat(IEntity entity, string[] args)
    {
        var comp = entity.E.AddComponentIfNone<JunctionComp>();
        comp.CanPass = true;
        comp.Texture = TileTextureProvider.Get("door");
    }

    private static void HandleGateFeat(IEntity entity, string[] args)
    {
        var comp = entity.E.AddComponentIfNone<JunctionComp>();
        comp.CanPass = false;
        comp.Texture = TileTextureProvider.Get("gate");
    }

    private static void HandleEyeFeat(IEntity entity, string[] args)
    {
        if (entity is not Node node) return;
        if (args.Length < 1) return;
        var direction = args[0] switch
        {
            "+" or "positive" or "+1" or "1" => PeepholePassDirection.Positive,
            "-" or "negative" or "-1" or "0" => PeepholePassDirection.Negative,
            _ => PeepholePassDirection.Unknown
        };
        if (direction == PeepholePassDirection.Unknown) return;
        var comp = entity.E.AddComponentIfNone<PeepholeComp>();
        comp.PassDirection = direction;
        var sprite = node.AddNodeIfNone<Sprite2D>("PeepholeIcon");
        sprite.Texture = TileTextureProvider.Get("eye");
    }

    private static void HandleLockedFeat(IEntity entity, string[] args)
    {
        if (entity is not Node node) return;
        if (args.Length < 1) return;
        if  (!bool.TryParse(args[0],out var isLock))  return;
        var comp = entity.E.AddComponentIfNone<JunctionComp>();
        comp.CanPass = !isLock;
        var sprite = node.AddNodeIfNone<Sprite2D>("LockIcon");
        sprite.Texture = TileTextureProvider.Get(isLock ? "lock" : "unlock");
    }
    
    private static void HandleCarpetFeat(IEntity entity, string[] args)
    {
        if (entity is not Node node) return;
        if (args.Length < 1) return;
        var texture = TileTextureProvider.Get(args[0]);
        var sprite = node.AddNodeIfNone<Sprite2D>("Carpet");
        sprite.Texture = texture;
    }
    
    private static void HandleHealFeat(IEntity entity, string[] args)
    { 
        if (args.Length < 1) return;
        if (!int.TryParse(args[0],out var heal)) return;
        var comp = entity.E.AddComponentIfNone<HealComp>();
        comp.HealAmount = heal;
    }

    private static void HandleTextFeat(IEntity entity, string[] args)
    {
        if (args.Length < 1) return;
        var comp = entity.E.AddComponentIfNone<InfoBoardComp>();
        comp.InfoText = args;
    }
    #endregion
}