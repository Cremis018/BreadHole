using System.Collections.Generic;
using System.Linq;
using Godot;

public class CoordUnitStorage
{
    private Dictionary<Vector2I,ICell> _coordCellMap = [];
    private Dictionary<Vector2I,IJunction> _coordJunctionMap = [];
    private Dictionary<Vector2I, IUnit> _coordOtherMap = [];

    public void SetCellStorage(Dictionary<Vector2I, ICell> storage)
    {
        _coordCellMap = storage;
    }
    
    public void SetJunctionStorage(Dictionary<Vector2I, IJunction> storage)
    {
        _coordJunctionMap = storage;
    }
    
    public bool SetUnit(Vector2I coord, IUnit unit)
    {
        switch (unit)
        {
            case ICell cell when CoordUtil.IsCellCoord(coord):
                _coordCellMap[coord] = cell;
                break;
            case IJunction junction when CoordUtil.IsJunctionCoord(coord,out _):
                _coordJunctionMap[coord] = junction;
                break;
            default:
                _coordOtherMap[coord] = unit;
                return false;
        }
        return true;
    }

    public IUnit GetUnit(Vector2I coord)
    {
        if (CoordUtil.IsCellCoord(coord)) return _coordCellMap.GetValueOrDefault(coord);
        if (CoordUtil.IsJunctionCoord(coord,out _)) return _coordJunctionMap.GetValueOrDefault(coord);
        return _coordOtherMap.GetValueOrDefault(coord);
    }

    public T[] GetUnits<T>() where T : IUnit
    {
        if (typeof(T) == typeof(ICell)) return _coordCellMap.Values.OfType<T>().ToArray();
        if (typeof(T) == typeof(IJunction)) return _coordJunctionMap.Values.OfType<T>().ToArray();
        return _coordOtherMap.Values.OfType<T>().ToArray();
    }

    public Vector2I[] GetCoords<T>(bool sort = false,bool xy = true) where T : IUnit
    {
        IEnumerable<Vector2I> coords;
        if (typeof(T) == typeof(IUnit))
            coords = _coordCellMap.Keys
                .Concat(_coordJunctionMap.Keys).Concat(_coordOtherMap.Keys);
        else if (typeof(T) == typeof(ICell)) coords = _coordCellMap.Keys;
        else if (typeof(T) == typeof(Cell)) coords = _coordCellMap.Where(p => p.Value is Cell).Select(p => p.Key);
        else if (typeof(T) == typeof(Void)) coords = _coordCellMap.Where(p => p.Value is Void).Select(p => p.Key);
        else if (typeof(T) == typeof(IJunction)) coords = _coordJunctionMap.Keys;
        else if (typeof(T) == typeof(Junction)) coords = _coordJunctionMap.Where(p => p.Value is Junction).Select(p => p.Key);
        else if (typeof(T) == typeof(Edge)) coords = _coordJunctionMap.Where(p => p.Value is Edge).Select(p => p.Key);
        else coords = _coordOtherMap.Where(p => p.Value is T).Select(p => p.Key);
        if (!sort) return coords.ToArray();
        return xy 
            ? coords.OrderBy(c => c.X).ThenBy(c => c.Y).ToArray() 
            : coords.OrderBy(c => c.Y).ThenBy(c => c.X).ToArray();
    }

    public Vector2I Size => GetSize();

    private Vector2I GetSize()
    {
        return _coordOtherMap.LastOrDefault().Key + Vector2I.One;
    }
}