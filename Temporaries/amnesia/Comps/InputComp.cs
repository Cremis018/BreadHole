using Godot;

public partial class InputComp : Component
{
    #region private
    private bool _jPressed = false;
    private double _jPressStartTime = 0.0;
    private const double LongPressThreshold = 0.5;
    private bool _wasUpPressed = false;
    private bool _wasDownPressed = false;
    private bool _wasLeftPressed = false;
    private bool _wasRightPressed = false;
    private MovementComp _movementComp;
    private InventoryComp _inventoryComp;
    #endregion

    #region life
    public override void _Ready()
    {
        InitNodes();
    }

    public override void _Process(double delta)
    {
        if (_movementComp == null || _inventoryComp == null) return;
        HandleMoveInput();
        HandleSelectItemInput();
        HandleJKey(delta);
        HandleKKey();
        if (_inventoryComp.IsTargetSelecting) HandleTargetCursorMove();
    }

    private void InitNodes()
    {
        var parent = GetParent();
        _movementComp = parent.GetNodeOrNull<MovementComp>("MovementComp");
        _inventoryComp = parent.GetNodeOrNull<InventoryComp>("InventoryComp");
    }
    #endregion

    #region op
    private void HandleMoveInput()
    {
        if (_inventoryComp.IsTargetSelecting) return;

        bool up = Input.IsKeyPressed(Key.W);
        bool down = Input.IsKeyPressed(Key.S);
        bool left = Input.IsKeyPressed(Key.A);
        bool right = Input.IsKeyPressed(Key.D);

        bool upJustPressed = up && !_wasUpPressed;
        bool downJustPressed = down && !_wasDownPressed;
        bool leftJustPressed = left && !_wasLeftPressed;
        bool rightJustPressed = right && !_wasRightPressed;

        _wasUpPressed = up;
        _wasDownPressed = down;
        _wasLeftPressed = left;
        _wasRightPressed = right;

        int count = (upJustPressed ? 1 : 0) + (downJustPressed ? 1 : 0) + (leftJustPressed ? 1 : 0) + (rightJustPressed ? 1 : 0);
        if (count != 1) return;

        Direction inputDir = _movementComp.Facing;
        if (upJustPressed) inputDir = Direction.Up;
        if (downJustPressed) inputDir = Direction.Down;
        if (leftJustPressed) inputDir = Direction.Left;
        if (rightJustPressed) inputDir = Direction.Right;

        if (inputDir != _movementComp.Facing)
        {
            _movementComp.ChangeDirection(inputDir);
            return;
        }

        _movementComp.MoveToDirection(inputDir);
    }

    private void HandleSelectItemInput()
    {
        if (Input.IsKeyPressed(Key.Key1)) _inventoryComp.SelectItem(0);
        else if (Input.IsKeyPressed(Key.Key2)) _inventoryComp.SelectItem(1);
        else if (Input.IsKeyPressed(Key.Key3)) _inventoryComp.SelectItem(2);
    }

    private void HandleJKey(double delta)
    {
        if (Input.IsKeyPressed(Key.J))
        {
            if (!_jPressed)
            {
                _jPressed = true;
                _jPressStartTime = Time.GetTicksMsec() / 1000.0;
            }
            return;
        }

        if (_jPressed && !Input.IsKeyPressed(Key.J))
        {
            _jPressed = false;
            double duration = Time.GetTicksMsec() / 1000.0 - _jPressStartTime;
            bool isLongPress = duration >= LongPressThreshold;
            if (isLongPress) _inventoryComp.UseSelectedItemAlt();
            else _inventoryComp.UseSelectedItem();
        }
    }

    private void HandleKKey()
    {
        if (!Input.IsKeyPressed(Key.K)) return;
        if (_inventoryComp.IsTargetSelecting)
        {
            _inventoryComp.CancelTargetSelection();
            return;
        }
        _inventoryComp.DropSelectedItem();
    }

    private void HandleTargetCursorMove()
    {
        bool up = Input.IsKeyPressed(Key.W);
        bool down = Input.IsKeyPressed(Key.S);
        bool left = Input.IsKeyPressed(Key.A);
        bool right = Input.IsKeyPressed(Key.D);

        Vector2I delta = Vector2I.Zero;
        if (up) delta = new Vector2I(0, -1);
        else if (down) delta = new Vector2I(0, 1);
        else if (left) delta = new Vector2I(-1, 0);
        else if (right) delta = new Vector2I(1, 0);

        if (delta != Vector2I.Zero) _inventoryComp.UpdateTargetCursor(delta);
        if (Input.IsKeyPressed(Key.J)) _inventoryComp.ConfirmTargetAndUseItem();
    }
    #endregion
}

