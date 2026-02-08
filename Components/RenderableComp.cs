using Godot;
using GodotSimpleTools;

[GlobalClass]
public partial class RenderableComp : Component
{
    [Export,Notify] public Texture2D Texture { get => GetTexture(); set => SetTexture(value); }
}