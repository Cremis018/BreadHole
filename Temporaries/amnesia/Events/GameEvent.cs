using Godot;

/// <summary>
/// 游戏事件基类。
/// </summary>
public abstract class GameEvent
{
    /// <summary>事件名称。</summary>
    public string EventName { get; protected set; }

    protected GameEvent(string eventName)
    {
        EventName = eventName;
    }
}

/// <summary>
/// 玩家到达某个位置时触发的事件。
/// </summary>
public class PlayerLocationReachedEvent : GameEvent
{
    /// <summary>到达的网格位置。</summary>
    public Vector2I GridPosition { get; }

    /// <summary>到达的世界位置。</summary>
    public Vector2 WorldPosition { get; }

    public PlayerLocationReachedEvent(Vector2I gridPosition, Vector2 worldPosition)
        : base("PlayerLocationReached")
    {
        GridPosition = gridPosition;
        WorldPosition = worldPosition;
    }
}

/// <summary>
/// 玩家方向改变时触发的事件。
/// </summary>
public class PlayerDirectionChangedEvent : GameEvent
{
    /// <summary>新的方向。</summary>
    public Direction NewDirection { get; }

    public PlayerDirectionChangedEvent(Direction newDirection)
        : base("PlayerDirectionChanged")
    {
        NewDirection = newDirection;
    }
}

/// <summary>
/// 播放动画事件。
/// </summary>
public class PlayAnimationEvent : GameEvent
{
    /// <summary>动画名称。</summary>
    public string AnimationName { get; }

    /// <summary>是否循环播放。</summary>
    public bool Loop { get; }

    public PlayAnimationEvent(string animationName, bool loop = false)
        : base("PlayAnimation")
    {
        AnimationName = animationName;
        Loop = loop;
    }
}

/// <summary>
/// 游戏结束事件（体力耗尽时触发）。
/// </summary>
public class GameOverEvent : GameEvent
{
    /// <summary>游戏结束原因。</summary>
    public string Reason { get; }

    public GameOverEvent(string reason = "体力耗尽")
        : base("GameOver")
    {
        Reason = reason;
    }
}

/// <summary>
/// 移动失败事件（遇到墙壁或无法穿越的房间时触发）。
/// </summary>
public class MovementFailedEvent : GameEvent
{
    /// <summary>尝试移动的方向。</summary>
    public Direction Direction { get; }

    /// <summary>目标房间的网格坐标。</summary>
    public Vector2I TargetGridPosition { get; }

    /// <summary>失败原因。</summary>
    public string Reason { get; }

    public MovementFailedEvent(Direction direction, Vector2I targetGridPosition, string reason = "遇到墙壁")
        : base("MovementFailed")
    {
        Direction = direction;
        TargetGridPosition = targetGridPosition;
        Reason = reason;
    }
}

