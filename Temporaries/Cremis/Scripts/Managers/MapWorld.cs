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
    
    #region fields
    private Node2D _mapNode;
    private readonly Dictionary<Vector2I, Floor> _floorMap = new();
    private readonly Dictionary<Vector2I, Junction> _junctionMap = new();
    #endregion

    #region op
    public void GenerateMap(string mapFilePath)
    {
        // 检查文件是否存在
        if (string.IsNullOrWhiteSpace(mapFilePath)
            || !File.Exists(mapFilePath))
        {
            GD.PrintErr("地图文件不存在");
            return;
        }

        // 读取文件内容
        string[] lines = File.ReadAllLines(mapFilePath);
        
        // 解析地图数据
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
                {
                    CreateFloor(new Vector2I(x-1,y-1));
                }
            }
        }
    }

    private void CreateFloor(Vector2I rawPos)
    {
        if (ResourceLoader.Load<PackedScene>(Consts.Floor)
                .Instantiate().Duplicate() is not Floor floor) return;
        var pos = rawPos * 2 + Vector2I.One;
        floor.Name = $"Floor({pos.X},{pos.Y})";
        
        _floorMap[pos] = floor;
        _mapNode.AddChild(floor);

        floor.E.GetComponent<MapCompositionComp>()
            .Coordinate = pos;
    }
    #endregion

    #region junction
    private void ParseJunctionData()
    {
        var floors = _floorMap.Keys.ToList();
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
        
        junction.E.GetComponent<MapCompositionComp>()
            .Coordinate = pos;
    }
    #endregion
    
    private void ParseMapData(string[] lines)
    {
        var mapLines = ExtractionMap(lines);
        GD.Print("正在读取地图……");
        GD.Print("地图轮廓如下：");
        PrintLines(mapLines);
        GD.Print("正在生成地板……");
        ParseFloorData(mapLines);
        GD.Print("正在生成衔接处……");
        ParseJunctionData();
    }

    private string[] ExtractionMap(string[] lines)
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
    #endregion
}