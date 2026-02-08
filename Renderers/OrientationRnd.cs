using Godot;
using Godot.Collections;

[GlobalClass]
public partial class OrientationRnd : Renderer
{
    private Direction _cacheDirection;
    [Export] public Array<Node2D> Targets = [];
    
    public override void _Register(IEntity entity)
    {
        var comp = entity.E.GetComponent<DirectionComp>();
        comp.DirectionChanged += DirectionChanged;
        RegisterRenderMethod(RenderDirection);
    }

    private void DirectionChanged(Direction direction)
    {
        _cacheDirection = direction;
        RenderDirection();
    }

    private void RenderDirection()
    {
        var tween = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quad).SetParallel();
        foreach (var target in Targets)
        {
            var from = target.Rotation; 
            var to = MathUtil.ShortestRotateTarget(from,
                CoordUtil.DirectionToRotation(_cacheDirection)); 
            tween.TweenMethod(Callable.From<float>(v => 
            { 
                target.Rotation = v;
            }), from, to, 0.5);
        }
    }
}