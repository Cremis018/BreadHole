using Godot;
using System.Collections.Generic;

public partial class PlayerInventory : RefCounted
{
    private List<IItem> _items = new List<IItem>();
    private int _selectedIndex = 0;
    private int _baseSlotCount = 3;
    private int _additionalSlots = 0;
    
    // 目标选择模式
    private bool _isTargetSelecting = false;
    private Vector2I _targetCursor;

    public int SelectedIndex => _selectedIndex;
    public bool IsTargetSelecting => _isTargetSelecting;
    public Vector2I TargetCursor => _targetCursor;
    public int TotalSlots => _baseSlotCount + _additionalSlots;
    public int ItemCount => _items.Count;

    /// <summary>
    /// 添加背包扩展槽位
    /// </summary>
    public void AddBackpackSlots(int slots = 2)
    {
        _additionalSlots += slots;
        GD.Print($"添加了 {slots} 个物品栏，当前总槽位: {TotalSlots}");
    }

    /// <summary>
    /// 添加道具到第一个空槽位
    /// </summary>
    public bool AddItem(IItem item)
    {
        if (_items.Count >= TotalSlots)
        {
            GD.Print("物品栏已满，无法添加新道具");
            return false;
        }
        
        _items.Add(item);
        GD.Print($"添加道具: {item.Name}，当前道具数量: {_items.Count}/{TotalSlots}");
        return true;
    }

    /// <summary>
    /// 选择道具槽位
    /// </summary>
    public void SelectItem(int index)
    {
        if (index < 0 || index >= TotalSlots)
        {
            GD.Print($"无效的物品栏索引: {index}");
            return;
        }

        _selectedIndex = index;
        string itemName = index < _items.Count ? _items[index]?.Name ?? "空" : "空";
        GD.Print($"选中物品栏 {index + 1}: {itemName}");
    }

    /// <summary>
    /// 获取当前选中的道具
    /// </summary>
    public IItem GetSelectedItem()
    {
        if (_selectedIndex < _items.Count)
            return _items[_selectedIndex];
        return null;
    }

    /// <summary>
    /// 使用当前选中的道具
    /// </summary>
    public bool UseSelectedItem(Vector2I gridPosition, Direction facing)
    {
        var item = GetSelectedItem();
        if (item == null)
        {
            GD.Print("当前槽位没有道具");
            return false;
        }

        if (item.IsDirectional)
        {
            // 方向性道具
            Vector2I targetPos = gridPosition + PlayerMovement.DirectionToOffset(facing);
            GD.Print($"使用方向性道具: {item.Name}，目标: {targetPos}");
            item.Use(gridPosition, facing, targetPos);
            return true;
        }
        else
        {
            if (item.RequiresTarget)
            {
                // 进入目标选择模式
                _isTargetSelecting = true;
                _targetCursor = gridPosition;
                GD.Print($"选择目标格子使用: {item.Name}");
                return false;
            }
            else
            {
                // 直接在当前格子使用
                GD.Print($"在当前格子使用: {item.Name}");
                item.Use(gridPosition, facing, gridPosition);
                return true;
            }
        }
    }

    // 其他方法保持不变...
    public void UseSelectedItemAlt(Vector2I gridPosition, Direction facing)
    {
        var item = GetSelectedItem();
        if (item == null) return;
        GD.Print($"使用道具副功能: {item.Name}");
        item.UseAlt(gridPosition, facing);
    }

    public void DropSelectedItem(Vector2I gridPosition)
    {
        if (_selectedIndex < _items.Count)
        {
            var item = _items[_selectedIndex];
            GD.Print($"丢弃道具: {item.Name}");
            _items.RemoveAt(_selectedIndex);
            // 创建掉落物...
        }
    }

    public void ConfirmTargetAndUseItem(Vector2I gridPosition, Direction facing)
    {
        var item = GetSelectedItem();
        if (item == null)
        {
            _isTargetSelecting = false;
            return;
        }
        
        GD.Print($"确认目标 {_targetCursor} 使用道具: {item.Name}");
        item.Use(gridPosition, facing, _targetCursor);
        _isTargetSelecting = false;
    }

    public void CancelTargetSelection() => _isTargetSelecting = false;
    
    public void UpdateTargetCursor(Vector2I delta) => _targetCursor += delta;
}