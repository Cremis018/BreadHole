using System;
using Godot;

[GlobalClass]
public partial class Modifier : Node
{
    public bool IsLoaded { get; private set; }
    public bool IsLoadedInEntity { get; private set; }
    
    private bool _enable = true;
    [Export]
    public bool Enable
    {
        get => _enable;
        set
        {
            if (_enable == value) return;
            _enable = value;
            if (_enable)
            {
                _OnEnabled();
                BeEnabled?.Invoke();
            }
            else
            {
                _OnDisabled();
                BeDisabled?.Invoke();
            }
        }
    }

    public event Action BeEnabled;
    public event Action BeDisabled;
    public event Action Loaded;
    public event Action LoadedInEntity;

    public void Load()
    {
        if (IsLoaded) return;
        _OnLoaded();
        IsLoaded = true;
        Loaded?.Invoke();
    }

    public void LoadInEntity(IEntity entity)
    {
        if (IsLoadedInEntity) return;
        _OnLoadedInEntity(entity);
        IsLoadedInEntity = true;
        LoadedInEntity?.Invoke();
    }
    
    public virtual void _OnLoaded()
    {
        
    }

    public virtual void _OnLoadedInEntity(IEntity entity)
    {
        
    }

    public virtual void _OnEnabled()
    {
        
    }

    public virtual void _OnDisabled()
    {
        
    }
}