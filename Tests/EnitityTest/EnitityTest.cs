using Godot;
using System;

public partial class EnitityTest : Node
{
    [Export] private Sprite2D _sprite;
    
    private Entity _entity;

    public override void _Ready()
    {
        _entity = new(this);
        var comp = Component.Create<TestComp>();
        GD.Print(comp);
        comp.Texture = _sprite.Texture;
        _entity.AddComponent(comp,"test");
        var testComp = _entity.GetComponent<TestComp>("test");
        GD.Print(testComp.Texture.ResourcePath);
    }
}
