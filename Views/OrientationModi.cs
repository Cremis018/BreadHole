using System;
using Godot;

[GlobalClass]
public partial class OrientationModi : Modifier
{
    [Export] private Node2D[] _rotationTargets;
    private DirectionComp _directionComp;
    
    public override void _OnLoadedInEntity(IEntity entity)
    {
        if (!entity.E.HasComponent<DirectionComp>(out var comp)) return;
        _directionComp = comp;
        _directionComp.DirectionChanged += OnDirectionChanged;
        UpdateView();
    }

    public override void _OnEnabled()
    {
        UpdateView();
    }

    private void OnDirectionChanged(Direction direction)
    {
        if (!Enable) return;
        UpdateView();
    }
    
    private void UpdateView()
    {
        var tween = CreateTween().SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Quad).SetParallel();
        foreach (var target in _rotationTargets)
        {
            var from = target.Rotation;
            var to = MathUtil.ShortestRotateTarget(from,
                CoordUtil.DirectionToRotation(_directionComp.Direction));
            tween.TweenMethod(Callable.From<float>(v =>
            {
                target.Rotation = v;
            }), from, to, 0.5);
        }
    }
}