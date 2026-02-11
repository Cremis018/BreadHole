using Godot;

public class DirectionModi
{
    private readonly Node2D _node;

    public DirectionModi(IEntity entity)
    {
        _node = entity as Node2D;
        var comp = entity.E.GetComponent<DirectionComp>();
        comp.DirectionChanged += OnDirectionChanged;
        OnDirectionChanged(comp.Direction);
    }

    private void OnDirectionChanged(Direction direction)
    {
        var tween = _node.CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quad).SetParallel();
        var from = _node.Rotation;
        var to = MathUtil.ShortestRotateTarget(from,
            CoordUtil.DirectionToRotation(direction));
        tween.TweenMethod(Callable.From<float>(v => { _node.Rotation = v; }), from, to, 0.5);
    }
}