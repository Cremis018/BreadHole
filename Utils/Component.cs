using Godot;

[GlobalClass]
public partial class Component : Resource
{
    #region props
    [Export] public bool IsFixed { get; set; }
    #endregion

    #region oper
    public virtual void _OnAdded(IEntity entity)
    {
        
    }

    public virtual void _OnRemoved(IEntity entity)
    {
        
    }
    #endregion
}