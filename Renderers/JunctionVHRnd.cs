using System.Collections.Generic;
using Godot;
using Godot.Collections;

[GlobalClass]
public  partial class JunctionVHRnd : Renderer
{
    [Export] public Array<Junction> Targets = [];
    private List<Modifier<Junction, Vector2I>> _coordModifiers = [];
    
    public override void _Register()
    {
        foreach (var target in Targets)
        {
            var modifier = new Modifier<Junction,Vector2I>(target,CoordChanged);
            var comp = target.E.GetComponent<CoordinateComp>();
            comp.CoordinateChanged += modifier.Modify;
            _coordModifiers.Add(modifier);
        }
        RegisterRenderMethod(RenderVH);
    }

    private void CoordChanged(Junction junction,Vector2I coord)
    {
        junction.Rotation = (coord.X % 2) switch
        {
            0 when coord.Y % 2 == 1 => float.Pi / 2,
            1 when coord.Y % 2 == 0 => 0,
            _ => junction.Rotation
        };
    }

    private void RenderVH()
    {
        foreach (var target in Targets)
        {
            var coord = target.E.GetComponent<CoordinateComp>().Coordinate;
            CoordChanged(target,coord);
        }
    }
}