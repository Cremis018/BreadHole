using Godot;
using System.Collections.Generic;

public partial class MovementComp : Component
{
    #region props
    [Export] public Vector2 RoomSize { get; set; } = new Vector2(18, 18);
    [Export] public Vector2 RoomOrigin { get; set; } = Vector2.Zero;
    [Export] public Vector2 MovementMin { get; set; } = Vector2.Zero;
    [Export] public Vector2 MovementMax { get; set; } = new Vector2(320, 240);
    [Export] public Vector2I GridMin { get; set; } = new Vector2I(0, 0);
    [Export] public Vector2I GridMax { get; set; } = new Vector2I(9, 9);
    
    public Vector2I GridPosition { get; private set; } = Vector2I.Zero;
    public Direction Facing { get; private set; } = Direction.Up;
    #endregion

    #region private
    private HashSet<Vector2I> _blockedRooms = new HashSet<Vector2I>();
    private Vector2 _previousWorldPosition;
    private Node2D _playerNode;
    #endregion

    #region life
    public override void _Ready()
    {
        InitNodes();
        if (_playerNode != null)
        {
            GridPosition = new Vector2I(
                (int)(_playerNode.GlobalPosition.X / RoomSize.X),
                (int)(_playerNode.GlobalPosition.Y / RoomSize.Y)
            );
            UpdateRotation();
            _previousWorldPosition = _playerNode.GlobalPosition;
        }
        EventSystem.Subscribe<MovementFailedEvent>(OnMovementFailed);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        EventSystem.Unsubscribe<MovementFailedEvent>(OnMovementFailed);
    }

    private void InitNodes()
    {
        _playerNode ??= GetParent<Node2D>();
    }
    #endregion

    #region op
    public void ChangeDirection(Direction newDirection)
    {
        if (Facing == newDirection) return;
        Facing = newDirection;
        UpdateRotation();
        EventSystem.Emit(new PlayerDirectionChangedEvent(Facing));
        GD.Print($"面向方向切换为：{Facing}");
    }

    public bool MoveToDirection(Direction direction)
    {
        var staminaComp = GetParent().GetNodeOrNull<StaminaComp>("StaminaComp");
        if (staminaComp != null && !staminaComp.HasEnoughStamina(1.0f))
        {
            GD.Print("体力不足，无法移动！");
            return false;
        }

        var nextRoom = GridPosition + PlayerMovement.DirectionToOffset(direction);
        nextRoom = PlayerMovement.ClampRoomToBounds(nextRoom, GridMin, GridMax);

        if (nextRoom == GridPosition)
        {
            GD.Print($"已经在 {direction} 方向的最边缘房间：{GridPosition}，无法继续移动。");
            HandleMovementFailure(direction, nextRoom, "地图边界", staminaComp);
            return false;
        }

        if (IsRoomBlocked(nextRoom))
        {
            GD.Print($"目标房间 {nextRoom} 是墙壁或无法穿越！");
            HandleMovementFailure(direction, nextRoom, "遇到墙壁", staminaComp);
            return false;
        }

        _previousWorldPosition = _playerNode.GlobalPosition;

        if (staminaComp != null)
        {
            staminaComp.ConsumeStamina(1.0f);
            GD.Print($"消耗1点体力，当前体力：{staminaComp.CurrentStamina}");
        }

        GridPosition = nextRoom;
        var worldCenter = PlayerMovement.CalculateRoomWorldCenter(GridPosition, RoomSize, RoomOrigin);
        var clamped = PlayerMovement.ClampWorldPosition(worldCenter, MovementMin, MovementMax);
        _playerNode.GlobalPosition = clamped;

        EventSystem.Emit(new PlayerLocationReachedEvent(GridPosition, clamped));
        GD.Print($"已移动到房间：{GridPosition}，世界坐标：{clamped}，面向：{Facing}");
        return true;
    }

    public bool IsRoomBlocked(Vector2I gridPosition) => _blockedRooms.Contains(gridPosition);
    public void AddBlockedRoom(Vector2I gridPosition) => _blockedRooms.Add(gridPosition);
    public void RemoveBlockedRoom(Vector2I gridPosition) => _blockedRooms.Remove(gridPosition);
    public void ClearBlockedRooms() => _blockedRooms.Clear();

    private void HandleMovementFailure(Direction direction, Vector2I targetGridPosition, string reason, StaminaComp staminaComp)
    {
        _previousWorldPosition = _playerNode.GlobalPosition;
        if (staminaComp != null)
        {
            staminaComp.ConsumeStamina(1.0f);
            GD.Print($"移动失败，扣除1点体力，当前体力：{staminaComp.CurrentStamina}");
        }
        EventSystem.Emit(new MovementFailedEvent(direction, targetGridPosition, reason));
        EventSystem.Emit(new PlayAnimationEvent("bounce_back", false));
    }

    public void BounceBack()
    {
        _playerNode.GlobalPosition = _previousWorldPosition;
        GD.Print($"弹回原点，位置：{_previousWorldPosition}");
    }

    private void UpdateRotation()
    {
        if (_playerNode != null)
        {
            _playerNode.Rotation = PlayerMovement.GetRotationForDirection(Facing);
        }
    }

    public void SetInitialPosition(Vector2 worldPosition)
    {
        _playerNode.GlobalPosition = worldPosition;
        GridPosition = new Vector2I(
            (int)(worldPosition.X / RoomSize.X),
            (int)(worldPosition.Y / RoomSize.Y)
        );
        UpdateRotation();
    }

    private void OnMovementFailed(MovementFailedEvent e)
    {
        GetTree().CreateTimer(0.3).Timeout += BounceBack;
    }
    #endregion
}

