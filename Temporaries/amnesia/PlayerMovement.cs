using Godot;

/// <summary>
/// 玩家移动和方向管理系统，负责房间移动、方向切换和旋转。
/// </summary>
public static class PlayerMovement
{
    /// <summary>
    /// 将方向转换为网格偏移量。
    /// </summary>
    public static Vector2I DirectionToOffset(Direction dir)
    {
        return dir switch
        {
            Direction.Up => new Vector2I(0, -1),
            Direction.Down => new Vector2I(0, 1),
            Direction.Left => new Vector2I(-1, 0),
            Direction.Right => new Vector2I(1, 0),
            _ => Vector2I.Zero
        };
    }

    /// <summary>
    /// 根据方向获取旋转角度（弧度）。
    /// </summary>
    public static float GetRotationForDirection(Direction facing)
    {
        return facing switch
        {
            Direction.Up => 0f,                       
            Direction.Right => Mathf.Pi / 2f,         
            Direction.Down => Mathf.Pi,               
            Direction.Left => -Mathf.Pi / 2f,         
            _ => 0f
        };
    }

    /// <summary>
    /// 把房间坐标限制在指定范围内。
    /// </summary>
    public static Vector2I ClampRoomToBounds(Vector2I room, Vector2I gridMin, Vector2I gridMax)
    {
        var x = Mathf.Clamp(room.X, gridMin.X, gridMax.X);
        var y = Mathf.Clamp(room.Y, gridMin.Y, gridMax.Y);
        return new Vector2I(x, y);
    }

    /// <summary>
    /// 计算房间中心的世界坐标。
    /// </summary>
    public static Vector2 CalculateRoomWorldCenter(Vector2I gridPosition, Vector2 roomSize, Vector2 roomOrigin)
    {
        return roomOrigin + new Vector2(
            gridPosition.X * roomSize.X + roomSize.X / 2f,
            gridPosition.Y * roomSize.Y + roomSize.Y / 2f
        );
    }

    /// <summary>
    /// 将世界坐标限制在移动范围内。
    /// </summary>
    public static Vector2 ClampWorldPosition(Vector2 worldPos, Vector2 movementMin, Vector2 movementMax)
    {
        return new Vector2(
            Mathf.Clamp(worldPos.X, movementMin.X, movementMax.X),
            Mathf.Clamp(worldPos.Y, movementMin.Y, movementMax.Y)
        );
    }
}

