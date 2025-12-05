using Godot;

public partial class InventoryComp : Component
{
    #region props
    public bool IsTargetSelecting => _inventory.IsTargetSelecting;
    public Vector2I TargetCursor => _inventory.TargetCursor;
    #endregion

    #region private
    private PlayerInventory _inventory = new PlayerInventory();
    #endregion

    #region op
    public void SelectItem(int index) => _inventory.SelectItem(index);

    public void UseSelectedItem()
    {
        var movementComp = GetParent().GetNodeOrNull<MovementComp>("MovementComp");
        if (movementComp != null)
        {
            _inventory.UseSelectedItem(movementComp.GridPosition, movementComp.Facing);
        }
    }

    public void UseSelectedItemAlt()
    {
        var movementComp = GetParent().GetNodeOrNull<MovementComp>("MovementComp");
        if (movementComp != null)
        {
            _inventory.UseSelectedItemAlt(movementComp.GridPosition, movementComp.Facing);
        }
    }

    public void DropSelectedItem()
    {
        var movementComp = GetParent().GetNodeOrNull<MovementComp>("MovementComp");
        if (movementComp != null)
        {
            _inventory.DropSelectedItem(movementComp.GridPosition);
        }
    }

    public void CancelTargetSelection() => _inventory.CancelTargetSelection();

    public void UpdateTargetCursor(Vector2I delta) => _inventory.UpdateTargetCursor(delta);

    public void ConfirmTargetAndUseItem()
    {
        var movementComp = GetParent().GetNodeOrNull<MovementComp>("MovementComp");
        if (movementComp != null)
        {
            _inventory.ConfirmTargetAndUseItem(movementComp.GridPosition, movementComp.Facing);
        }
    }
    #endregion
}

