using System.Collections.Generic;
using System.Linq;
using Godot;

public class CoordUnitStorage
{
    #region cache
    private bool _isDirty = false;
    private Rect2I _cacheRect;
    private Rect2I _cacheRowColRect;
    #endregion
    
    private Dictionary<Vector2I,IUnit> _coordUnitMap = [];

    public void SetStorage(Dictionary<Vector2I, IUnit> storage)
    {
        _coordUnitMap = storage;
        _isDirty = true;
    }
    
    public void SetUnit(Vector2I coord, IUnit unit)
    {
        _coordUnitMap[coord] = unit;
        _isDirty = true;
    }

    public IUnit GetUnit(Vector2I coord) => _coordUnitMap[coord];

    public T[] GetUnits<T>() where T : Node,IUnit
    {
        if (typeof(T) != typeof(Cell)) return _coordUnitMap.Values.OfType<T>().ToArray();
        var pairs = _coordUnitMap.Where(p => p.Key.X % 2 == 1 && p.Key.Y % 2 == 1);
        return pairs.Select(p => p.Value as T).ToArray();
    }

    public Vector2I[] GetCoords(bool sort = false)
    {
        var keys = _coordUnitMap.Keys;
        return sort ? keys.OrderBy(k => k.X).ThenBy(k => k.Y).ToArray() : keys.ToArray();
    }

    public Vector2I[][] GetVec2Coords(bool vertical = false)
    {
        var coords = GetCoords(true);
        return vertical
            ? coords.GroupBy(v => v.X).OrderBy(g => g.Key)
                .Select(g => g.OrderBy(v => v.Y).ToArray()).ToArray()
            : coords.GroupBy(v => v.Y).OrderBy(g => g.Key)
                .Select(g => g.OrderBy(v => v.X).ToArray()).ToArray();
    }

    public Vector2I[] GetCellCoords(bool sort = false)
    {
        var keys = _coordUnitMap.Where(p => p.Key.X % 2 == 1 && p.Key.Y % 2 == 1).Select(p => p.Key);
        return sort ? keys.OrderBy(k => k.X).ThenBy(k => k.Y).ToArray() : keys.ToArray();
    }

    public Vector2I[][] GetCellVec2Coords(bool vertical = false)
    {
        var coords = GetCellCoords(true);
        return vertical
            ? coords.GroupBy(v => v.X).OrderBy(g => g.Key)
                .Select(g => g.OrderBy(v => v.Y).ToArray()).ToArray()
            : coords.GroupBy(v => v.Y).OrderBy(g => g.Key)
                .Select(g => g.OrderBy(v => v.X).ToArray()).ToArray();
    }

    public Rect2I GetRect(bool toRowCol = true)
    {
        if (!_isDirty) return _cacheRect;
        var coords = GetCellCoords();
        var minX = coords.Min(v => v.X);
        var minY = coords.Min(v => v.Y);
        var maxX = coords.Max(v => v.X);
        var maxY = coords.Max(v => v.Y);
        var width = maxX - minX + 1;
        var height = maxY - minY + 1;
        var rect = toRowCol 
            ? new Rect2I((minX-1)/2, (minY-1)/2, (width+1)/2, (height+1)/2) 
            : new Rect2I(minX, minY, width, height);
        if (toRowCol) _cacheRowColRect = rect;
        else _cacheRect = rect;
        _isDirty = false;
        return rect;
    }
}