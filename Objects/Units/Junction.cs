using Godot;
using Godot.Collections;

public partial class Junction : Node2D, IJunction
{
    [Export] public Array<Component> Components { get; private set; } = [];
    public ComponentQuery E { get; private set; }
    
    private CoordModi _coordModi;
    private TextureModi _textureModi;
    private JunctionVHModi _junctionVHModi;
    
    [Export] private Sprite2D _sprite;

    public override void _EnterTree()
    {
        E ??= new(this);
        _coordModi = new(this);
        _textureModi = new(this,_sprite);
        _junctionVHModi = new(this);
    }
}
