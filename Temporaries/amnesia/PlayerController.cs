using Godot;

/// <summary>
/// 玩家控制器，使用组件化架构。
/// 负责组合各个组件：移动、道具、输入、动画等。
/// </summary>
public partial class PlayerController : Node2D
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    // 组件引用
    private MovementComp _movementComp;
    private InventoryComp _inventoryComp;
    private InputComp _inputComp;
    private StaminaComp _staminaComp;

    public override void _Ready()
    {
        base._Ready();

        // 初始化组件
        InitializeComps();

        // 设置初始位置
        var initialPosition = new Vector2(12, 12);
        if (_movementComp != null)
        {
            _movementComp.SetInitialPosition(initialPosition);
        }
        else
        {
            GlobalPosition = initialPosition;
        }
    }

    /// <summary>
    /// 初始化所有组件。
    /// </summary>
    private void InitializeComps()
    {
        // 创建并添加组件节点
        _movementComp = GetNodeOrNull<MovementComp>("MovementComp");
        if (_movementComp == null)
        {
            _movementComp = new MovementComp();
            _movementComp.Name = "MovementComp";
            AddChild(_movementComp);
        }
        // 移除这一行：_movementComp.Initialize(this);

        _inventoryComp = GetNodeOrNull<InventoryComp>("InventoryComp");
        if (_inventoryComp == null)
        {
            _inventoryComp = new InventoryComp();
            _inventoryComp.Name = "InventoryComp";
            AddChild(_inventoryComp);
        }
        // 移除这一行：_inventoryComp.Initialize(this);

        _inputComp = GetNodeOrNull<InputComp>("InputComp");
        if (_inputComp == null)
        {
            _inputComp = new InputComp();
            _inputComp.Name = "InputComp";
            AddChild(_inputComp);
        }
        // 移除这一行：_inputComp.Initialize(this);

        _staminaComp = GetNodeOrNull<StaminaComp>("StaminaComp");
        if (_staminaComp == null)
        {
            _staminaComp = new StaminaComp();
            _staminaComp.Name = "StaminaComp";
            AddChild(_staminaComp);
        }
        // 移除这一行：_staminaComp.Initialize(this);
    }

    /// <summary>
    /// 获取移动组件。
    /// </summary>
    public MovementComp GetMovementComp() => _movementComp;

    /// <summary>
    /// 获取道具组件。
    /// </summary>
    public InventoryComp GetInventoryComp() => _inventoryComp;

    /// <summary>
    /// 获取输入组件。
    /// </summary>
    public InputComp GetInputComp() => _inputComp;

    /// <summary>
    /// 获取体力组件。
    /// </summary>
    public StaminaComp GetStaminaComp() => _staminaComp;
}

