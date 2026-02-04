using Godot;

[GlobalClass]
public partial class Component : Resource
{
    #region props
    /// <summary>
    /// 是否被固定在实体中，被固定的组件将在删除操作中忽略掉
    /// </summary>
    [Export] public bool IsFixed { get; set; }
    #endregion

    #region op
    public virtual void _OnLoaded(IEntity entity)
    {
        
    }
    
    public virtual void _OnAdded(IEntity entity)
    {
        
    }

    public virtual void _OnRemoved(IEntity entity)
    {
        
    }
    #endregion
}