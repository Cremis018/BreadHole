using System;
using System.Collections.Generic;
using Godot;

[GlobalClass]
public abstract partial class Renderer : Node
{
    private bool _enable = true;
    [Export] public bool Enable
    {
        get => _enable;
        set
        {
            if (_enable == value) return;
            _enable = value;
            if (value)
            {
                OnEnable?.Invoke();
                _OnEnable();
                Render();
            }
            else
            {
                OnDisable?.Invoke();
                _OnDisable();
            }
        }
    }
    
    [Export] public Node HostNode { get; set; }
    
    public event Action OnEnable;
    public event Action OnDisable;

    private readonly List<Action> _onRenderList = [];

    protected void RegisterRenderMethod(Action method) => _onRenderList.Add(method);

    public void Render()
    {
        if (!Enable) return;
        _PreRender();
        foreach (var action in _onRenderList) action();
        _PostRender();
    }

    public override void _Ready()
    {
        HostNode ??= GetParent();
        if (HostNode is IEntity entity) _Register(entity);
        _Register();
        Render();
    }

    public virtual void _Register(IEntity entity){}
    public virtual void _Register(){}
    public virtual void _OnEnable(){}
    public virtual void _OnDisable(){}
    protected virtual void _PreRender(){}
    protected virtual void _PostRender(){}
}