using Godot;

[GlobalClass]
public abstract partial class Component : Resource
{
    #region props
    [ExportGroup("Base")]
    /// <summary>
    /// 是否被固定在实体中，被固定的组件将在删除操作中忽略掉
    /// </summary>
    [Export]
    public bool IsFixed { get; set; } = true;
    /// <summary>
    /// 独特名称，对运行时加载的动态组件而言，它们都需要独特名称避免用于区分同种组件
    /// </summary>
    [Export] public string UniqueName { get; set; }
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