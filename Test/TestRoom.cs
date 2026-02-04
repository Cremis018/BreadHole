using Godot;
using System;

public partial class TestRoom : Node2D
{
    [Export] public VBoxContainer _co;
    
    public override void _Ready()
    {
        var tc = new TextureConverter();
        var map = tc.FolderToTextureMap(@"Assets");
        GD.Print(map.Count);
        foreach (var (name, texture)  in map)
        {
            var label = new Label();
            label.Text = name;
            var rect = new TextureRect();
            rect.Texture = texture;
            var h = new HBoxContainer();
            h.AddChild(label);
            h.AddChild(rect);
            _co.AddChild(h);
        }
        // var texture2D = ResourceLoader.Load<Texture2D>(@"C:\MyProjects\GameProjects\BreadHole\Assets\99CF745E7D527EB9842CC1D9F50FBE3D.png");
        // var textureRect = new TextureRect();
        // textureRect.Texture = texture2D;
        // AddChild(textureRect);
    }
}
