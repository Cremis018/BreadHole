using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public class TextureModi
{
    public List<CanvasItem> Targets;

    public TextureModi(IEntity entity,params CanvasItem[] targets)
    {
        Targets = targets.ToList();
        var comp = entity.E.GetComponent<RenderableComp>();
        comp.TextureChanged += OnTextureChanged;
        OnTextureChanged(comp.Texture);
    }

    private void OnTextureChanged(Texture2D texture)
    {
        foreach (var target in Targets)
        {
            switch (target)
            {
                case Sprite2D sprite:
                    sprite.Texture = texture;
                    break;
                case TextureRect rect:
                    rect.Texture = texture;
                    break;
                case Polygon2D polygon:
                    polygon.Texture = texture;
                    break;
            }
        }
    }
}