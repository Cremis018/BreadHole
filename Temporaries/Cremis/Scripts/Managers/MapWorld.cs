using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;

public class MapWorld
{
    #region life
    public MapWorld(Node2D mapNode)
    {
        _mapNode = mapNode;
    }
    #endregion
    
    #region members
    private Node2D _mapNode;
    private readonly Dictionary<Vector2I, Floor> _floorMap = [];
    private readonly Dictionary<Vector2I, Junction> _junctionMap = [];
    private readonly List<int> _itemWhitelist = [];
    public Player Player { get; private set; }
    #endregion

    #region gen
    public void GenerateMap(string mapFilePath)
    {
        _floorMap.Clear();
        _junctionMap.Clear();
        _itemWhitelist.Clear();
        if (string.IsNullOrWhiteSpace(mapFilePath)
            || !File.Exists(mapFilePath))
        {
            GD.PrintErr("地图文件不存在");
            return;
        }
        
        string[] lines = File.ReadAllLines(mapFilePath);
        ParseMapData(lines);
    }

    #region floor
    private void ParseFloorData(string[] lines)
    {
        const int start = 1;
        var end = lines.Length - 1;
        for (var y = start; y < end; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] is 'O' or 'o')
                    CreateFloor(new Vector2I(x-1,y-1));
                else if (lines[y][x] is 'X' or 'x')
                    CreateFloor(new Vector2I(x-1,y-1),true);
            }
        }
    }

    private void CreateFloor(Vector2I rawPos, bool wasMarked = false)
    {
        if (ResourceLoader.Load<PackedScene>(Consts.Floor)
                .Instantiate().Duplicate() is not Floor floor) return;
        var pos = rawPos * 2 + Vector2I.One;
        floor.Name = $"Floor({pos.X},{pos.Y})";
        
        _floorMap[pos] = floor;
        _mapNode.AddChild(floor);

        floor.E.GetComponent<MapCompositionComp>()
            .Coordinate = pos;

        if (wasMarked)
            floor.E.GetComponent<MarkableComp>()
                .WasMarked = true;
    }
    #endregion

    #region junction
    private void ParseJunctionData()
    {
        var floors = _floorMap.Keys.ToHashSet();
        foreach (var floor in floors)
        {
            var lPos = floor + Vector2I.Left;
            CreateJunction(lPos);
            var rPos = floor + Vector2I.Right;
            CreateJunction(rPos);
            var uPos = floor + Vector2I.Up;
            CreateJunction(uPos);
            var dPos = floor + Vector2I.Down;
            CreateJunction(dPos);
        }
    }
    
    private void CreateJunction(Vector2I pos)
    {
        if (_junctionMap.ContainsKey(pos)) return;
        if (ResourceLoader.Load<PackedScene>(Consts.Junction)
                .Instantiate().Duplicate() is not Junction junction) return;
        junction.Name = $"Junction({pos.X},{pos.Y})";
        
        _junctionMap[pos] = junction;
        _mapNode.AddChild(junction);

        var neighbors = FindJunctionHasNeighbor(pos);
        
        junction.E.GetComponent<MapCompositionComp>()
            .Coordinate = pos;
        //判断是不是墙
        junction.E.GetComponent<JunctionComp>()
            .JunctionType = neighbors.Count != 2
            ? JunctionType.Wall 
            : JunctionType.Unknown;
        //判断是否被标记
        if (neighbors.Count <= 0) return;
        foreach (var _ in neighbors.Select(neighbor => _floorMap[neighbor])
                     .Where(floor => floor.E.GetComponent<MarkableComp>().WasMarked))
        {
            junction.E.GetComponent<MarkableComp>()
                .WasMarked = true;
        }
            
    }

    private List<Vector2I> FindJunctionHasNeighbor(Vector2I junctionPos)
    {
        List<Vector2I> neighbors = [];
        var isH = junctionPos.X % 2 != 0 && junctionPos.Y % 2 == 0;
        var isV = junctionPos.X % 2 == 0 && junctionPos.Y % 2 != 0;
        if (isH)
        {
            var dN = junctionPos + Vector2I.Down;
            if (_floorMap.ContainsKey(dN))
                neighbors.Add(dN);
            var uN = junctionPos + Vector2I.Up;
            if (_floorMap.ContainsKey(uN))
                neighbors.Add(uN);
        }
        else if (isV)
        {
            var lN = junctionPos + Vector2I.Left;
            if (_floorMap.ContainsKey(lN))
                neighbors.Add(lN);
            var rN = junctionPos + Vector2I.Right;
            if (_floorMap.ContainsKey(rN))
                neighbors.Add(rN);
        }
        return neighbors;
    }
    #endregion

    #region items
    private void ParseItemData(string[] lines)
    {
        _itemWhitelist.AddRange(GetItemWhitelist(lines));
    }
    
    private int[] GetItemWhitelist(string[] lines)
    {
        foreach (var line in lines)
        {
            if (!line.StartsWith("items[") || !line.EndsWith(']')) continue;
            var rawStr = line.Substring(6, line.Length - 7);
            var whitelist = rawStr.Split(',');
            return whitelist.Select(int.Parse).ToArray();
        }
        return [];
    }
    #endregion
    
    #region player
    private void ParsePlayerData(string[] lines)
    {
        var pos = GetPlayerPos(lines);
        CreatePlayer(pos);
    }
    
    private Vector2I GetPlayerPos(string[] lines)
    {
        foreach (var line in lines)
        {
            if (!line.StartsWith("player(") || !line.EndsWith(')')) continue;
            var rawStr = line.Substring(7, line.Length - 8);
            var pos = rawStr.Split(',');
            return new Vector2I(int.Parse(pos[0]), int.Parse(pos[1]));
        }
        return Vector2I.One;
    }
    
    private void CreatePlayer(Vector2I pos)
    {
        if (ResourceLoader.Load<PackedScene>(Consts.Player)
                .Instantiate().Duplicate() is not Player player) return;
        Player ??= player;
        
        _mapNode.AddChild(Player);
        
        Player.E.GetComponent<MapCompositionComp>()
            .Coordinate = pos;
    }
    #endregion
    
    private void ParseMapData(string[] lines)
    {
        var mapLines = GetMapLines(lines);
        GD.Print("正在读取地图……");
        GD.Print("地图轮廓如下：");
        PrintLines(mapLines);
        GD.Print("正在生成地板……");
        ParseFloorData(mapLines);
        GD.Print("正在生成衔接处……");
        ParseJunctionData();
        GD.Print("正在查看物品白名单……");
        ParseItemData(lines);
        GD.Print("正在查看物品白名单如下……");
        PrintItemWhitelist();
        GD.Print("正在放置玩家……");
        ParsePlayerData(lines);
    }

    private string[] GetMapLines(string[] lines)
    {
        List<string> mapLines = [];
        var ends = 0;
        foreach (var line in lines)
        {
            if (!IdentifyMapHEnds(line) && !IdentifyMapYEnds(line)) continue;
            if (IdentifyMapYEnds(line)) ends++;
            mapLines.Add(line);
            if (ends == 2) return mapLines.ToArray();
        }
        return mapLines.ToArray();
    }
    
    private bool IdentifyMapYEnds(string line) => !string.IsNullOrEmpty(line) && line.StartsWith('#') && line.Distinct().Count() == 1;
    private bool IdentifyMapHEnds(string line) => line.StartsWith('#') && line.EndsWith('#');

    private void PrintLines(string[] lines)
    {
        foreach (var line in lines) GD.Print(line);
    }
    
    private void PrintItemWhitelist()
    { 
        GD.Print(_itemWhitelist.ToArray().Join(","));
    }
    #endregion
}