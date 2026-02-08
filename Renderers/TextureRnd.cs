using Godot;
using Godot.Collections;

[GlobalClass]
public partial class TextureRnd : Renderer
{
    private Texture2D _cacheTexture;
    [Export] public Array<CanvasItem> Targets = [];

    public override void _Register(IEntity entity)
    {
        var comp = entity.E.GetComponent<RenderableComp>();
        comp.TextureChanged += TextureChanged;
        RegisterRenderMethod(RenderTexture);
    }

    private void RenderTexture()
    {
        foreach (var target in Targets)
        {
            switch (target)
            {
                case Sprite2D sprite:
                    sprite.Texture = _cacheTexture;
                    break;
                case TextureRect rect:
                    rect.Texture = _cacheTexture;
                    break;
                case Polygon2D polygon:
                    polygon.Texture = _cacheTexture;
                    break;
            }
        }
    }

    private void TextureChanged(Texture2D texture)
    {
        _cacheTexture = texture;
        RenderTexture();
    }
}