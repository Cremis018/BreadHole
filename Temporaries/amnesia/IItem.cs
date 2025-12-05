using Godot;

/// <summary>
/// 道具接口：示例中只定义最核心的几个字段与方法。
/// 你可以在实际项目中继承自 Resource 或 Node。
/// </summary>
public interface IItem
{
    string Name { get; }

    /// <summary>是否为方向性道具（比如子弹、射线）。</summary>
    bool IsDirectional { get; }

    /// <summary>是否需要选择一个目标格子（无方向性时用）。</summary>
    bool RequiresTarget { get; }

    /// <summary>
    /// 使用道具。
    /// origin：玩家当前格子；facing：当前面向；target：实际作用格子。
    /// </summary>
    void Use(Vector2I origin, Direction facing, Vector2I target);

    /// <summary>长按 J 触发的副功能。</summary>
    void UseAlt(Vector2I origin, Direction facing);
}

