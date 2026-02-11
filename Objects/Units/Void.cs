using Godot;
using System;
using Godot.Collections;

public partial class Void : Node2D, ICell
{
    [Export] public Array<Component> Components { get; private set; } = [];
    public ComponentQuery E { get; private set; }
    
    private CoordModi _coordModi;
    private TextureModi _textureModi;
    
    [Export] private Sprite2D _sprite;
    
    public override void _EnterTree()
    {
        E ??= new(this);
        _coordModi = new(this);
        _textureModi = new(this,_sprite);
    }
}
