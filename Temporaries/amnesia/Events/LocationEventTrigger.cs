using Godot;
using System.Collections.Generic;

/// <summary>
/// 位置事件触发器，当玩家到达指定位置时触发事件和动画。
/// </summary>
public partial class LocationEventTrigger : Node2D
{
    /// <summary>
    /// 触发位置配置。
    /// </summary>
    public class TriggerLocation
    {
        /// <summary>触发位置的网格坐标。</summary>
        public Vector2I GridPosition;

        /// <summary>要播放的动画名称。</summary>
        public string AnimationName;

        /// <summary>是否只触发一次。</summary>
        public bool TriggerOnce;

        /// <summary>是否已触发过。</summary>
        public bool HasTriggered;

        public TriggerLocation(Vector2I gridPosition, string animationName, bool triggerOnce = true)
        {
            GridPosition = gridPosition;
            AnimationName = animationName;
            TriggerOnce = triggerOnce;
            HasTriggered = false;
        }
    }

    /// <summary>触发位置列表。</summary>
    public List<TriggerLocation> TriggerLocations { get; private set; } = new List<TriggerLocation>();

    /// <summary>是否启用触发器。</summary>
    [Export]
    public bool Enabled { get; set; } = true;

    public override void _Ready()
    {
        base._Ready();
        
        // 订阅玩家位置到达事件
        EventSystem.Subscribe<PlayerLocationReachedEvent>(OnPlayerLocationReached);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        EventSystem.Unsubscribe<PlayerLocationReachedEvent>(OnPlayerLocationReached);
    }

    /// <summary>
    /// 添加触发位置。
    /// </summary>
    public void AddTriggerLocation(Vector2I gridPosition, string animationName, bool triggerOnce = true)
    {
        TriggerLocations.Add(new TriggerLocation(gridPosition, animationName, triggerOnce));
    }

    /// <summary>
    /// 移除触发位置。
    /// </summary>
    public void RemoveTriggerLocation(Vector2I gridPosition)
    {
        TriggerLocations.RemoveAll(t => t.GridPosition == gridPosition);
    }

    private void OnPlayerLocationReached(PlayerLocationReachedEvent e)
    {
        if (!Enabled)
            return;

        // 检查是否有匹配的触发位置
        foreach (var trigger in TriggerLocations)
        {
            if (trigger.GridPosition == e.GridPosition)
            {
                // 如果只触发一次且已经触发过，跳过
                if (trigger.TriggerOnce && trigger.HasTriggered)
                    continue;

                // 触发动画事件
                EventSystem.Emit(new PlayAnimationEvent(trigger.AnimationName));
                
                trigger.HasTriggered = true;
                GD.Print($"位置事件触发: 位置 {e.GridPosition}, 动画 {trigger.AnimationName}");
                
                // 如果只需要触发一次，可以移除
                if (trigger.TriggerOnce)
                {
                    // 可以选择保留记录或移除
                }
            }
        }
    }
}

