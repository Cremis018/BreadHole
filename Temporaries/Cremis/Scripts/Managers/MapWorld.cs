using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Godot;

//HACK:已经研究目标读图和地图格式，该类已经过于臃肿，需要新建一些类重构
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

    #region load
    public void Load(string mapFilePath)
    {
        _floorMap.Clear();
        _junctionMap.Clear();
        _itemWhitelist.Clear();
        if (!TextFileUtil.Exist(mapFilePath))
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
                switch (lines[y][x])
                {
                    case 'O' or 'o':
                        CreateFloor(new Vector2I(x-1,y-1));
                        break;
                    case 'X' or 'x':
                        CreateFloor(new Vector2I(x-1,y-1),true);
                        break;
                }
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
            .Features = neighbors.Count != 2
            ? [0] : [];
        //判断是否被标记
        if (neighbors.Count <= 0) return;
        foreach (var _ in neighbors.Select(neighbor => _floorMap[neighbor])
                     .Where(floor => floor.E.GetComponent<MarkableComp>().WasMarked))
        {
            junction.E.GetComponent<MarkableComp>()
                .WasMarked = true;
        }
        
        //TODO:对于被标记的衔接处是什么类型，还没有合适的读取方法和写入方法，需要日后设计
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

    private void ParseJunctionFeatures(string[] lines)
    {
        foreach (var line in lines)
        {
            var junctionPos = GetFeaturedJunctionPos(line);
            if (!_junctionMap.ContainsKey(junctionPos)) continue;
            var junctionFeatures = GetJunctionFeatures(line);
            if (junctionFeatures.Length <= 0) continue;
            SetJunctionFeatures(junctionPos, junctionFeatures);
        }
    }
    
    private Vector2I GetFeaturedJunctionPos(string line)
    {
        if (string.IsNullOrWhiteSpace(line) || !IdentifyFeaturesLine(line)) return Vector2I.Zero;
        
        const int coordStart = 2;
        var coordEnd = line.IndexOf(')');
        var coords = line.Substring(coordStart, coordEnd - coordStart).Split(',').Select(int.Parse).ToArray();
        return new Vector2I(coords[0], coords[1]);
    }
    
    private int[] GetJunctionFeatures(string line)
    {
        if (!IdentifyFeaturesLine(line)) return [];
        var featStart = line.IndexOf('[');
        var featEnd = line.IndexOf(']');
        if (featStart == -1 || featEnd == -1) return [];
        var features = line.Substring(featStart+1, featEnd - featStart-1).Split(',').Select(int.Parse).ToArray();
        return features;
    }
    
    private void SetJunctionFeatures(Vector2I pos, int[] features)
    {
        if (!_junctionMap.TryGetValue(pos, out var junction)) return;
        junction.E.GetComponent<JunctionComp>()
            .Features = features;
        junction.E.GetComponent<MarkableComp>()
            .WasMarked = true;
        GD.Print($"为位于({pos.X},{pos.Y})的衔接处设置特性组[{features.Join(",")}]");
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
        foreach (var line in lines)
        {
            if (!line.StartsWith("player(") || !line.EndsWith(']')) continue;
            var pos = GetPlayerPos(line);
            var items = GetPlayerItems(line);
            CreatePlayer(pos,items);
            break;
        }
    }
    
    private Vector2I GetPlayerPos(string line)
    {
        var posEnd = line.IndexOf(')');
        if (posEnd == -1) return Vector2I.One;
        var rawStr = line[7..posEnd]; 
        var pos = rawStr.Split(',').Select(int.Parse).ToArray();
        return new Vector2I(pos[0], pos[1]) * 2 + Vector2I.One;
    }
    
    private int[] GetPlayerItems(string line)
    {
        var itemStart = line.IndexOf('[');
        var itemEnd = line.IndexOf(']');
        if (itemStart == -1 || itemEnd == -1) return [];
        var rawStr = line.Substring(itemStart + 1, itemEnd - itemStart - 1);
        return rawStr.Split(',').Select(int.Parse).ToArray();
    }
    
    private void CreatePlayer(Vector2I pos,int[] items)
    {
        if (ResourceLoader.Load<PackedScene>(Consts.Player)
                .Instantiate().Duplicate() is not Player player) return;
        Player ??= player;
        
        _mapNode.AddChild(Player);
        
        Player.E.GetComponent<MapCompositionComp>()
            .Coordinate = pos;
        player.E.GetComponent<PocketComp>()
            .Items = items;
    }
    #endregion
    
    private void ParseMapData(string[] lines)
    {
        GD.Print("正在读取地图……");
        var mapLines = GetMapLines(lines);
        GD.Print("地图轮廓如下：");
        PrintLines(mapLines);
        GD.Print("正在生成地板……");
        ParseFloorData(mapLines);
        GD.Print("正在生成衔接处……");
        ParseJunctionData();
        GD.Print("正在读取衔接处特性列表……");
        var featsLines = GetJunctionFeaturesLines(lines);
        GD.Print("衔接处特性列表如下：");
        PrintLines(featsLines);
        GD.Print("正在为衔接处添加特性……");
        ParseJunctionFeatures(featsLines);
        GD.Print("正在查看物品白名单……");
        ParseItemData(lines);
        GD.Print("物品白名单如下……");
        PrintItemWhitelist();
        GD.Print("正在放置玩家……");
        ParsePlayerData(lines);
        GD.Print("地图生成完毕！\n");
    }

    private static string[] GetMapLines(string[] lines)
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

    private static string[] GetJunctionFeaturesLines(string[] lines)
    {
        List<string> featuresLines = [];
        featuresLines.AddRange(lines.Where(IdentifyFeaturesLine));
        return featuresLines.ToArray();
    }
    
    private static bool IdentifyMapYEnds(string line) => !string.IsNullOrEmpty(line) && line.StartsWith('#') && line.Distinct().Count() == 1;
    private static bool IdentifyMapHEnds(string line) => line.StartsWith('#') && line.EndsWith('#');
    private static bool IdentifyFeaturesLine(string line) => line.StartsWith("@(") && line.EndsWith(']');
    #endregion

    #region save
    public void Save(string mapFilePath)
    {
        TextFileUtil.Write(mapFilePath, FormText());
    }

    private string FormText()
    {
        var sb = new StringBuilder();
        GD.Print("正在保存地图数据……");
        sb.Append(FormMap());
        GD.Print("正在保存玩家数据……");
        sb.Append(FormPlayer());
        GD.Print("正在保存物品白名单……");
        sb.Append(FormItem());
        GD.Print("正在保存衔接处数据……");
        sb.Append(FormJunction());
        return sb.ToString();
    }

    private string FormMap()
    {
        var floorsPos = _floorMap.Keys.ToList();
        var xs = floorsPos.Select(p => p.X).ToArray();
        var ys = floorsPos.Select(p => p.Y).ToArray();
        
        var minX = (xs.Min() - 1) / 2;
        var maxX = (xs.Max() - 1) / 2;
        var minY = (ys.Min() - 1) / 2;
        var maxY = (ys.Max() - 1) / 2;
        var width = maxX - minX + 1 + 2;
        var height = maxY - minY + 1 + 2;

        var map = new char[height, width];
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                map[y, x] = '#';
        
        var offset = new Vector2I(minX, minY);
        foreach (var rawPos in floorsPos)
        {
            var pos = (rawPos - Vector2I.One) / 2 - offset + Vector2I.One;
            var floorCh = _floorMap[rawPos].E.GetComponent<MarkableComp>().WasMarked
                ? 'X'
                : 'O';
            map[pos.Y, pos.X] = floorCh;
        }

        var sb = new StringBuilder(height * (width + 1));
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++) sb.Append(map[y, x]); 
            sb.Append('\n');
        }
        return sb.ToString();
    }

    private string FormJunction()
    {
        var junctions = _junctionMap.Values.ToList();
        var sb = new StringBuilder();
        foreach (var junction in junctions)
        {
            if (!junction.E.GetComponent<MarkableComp>().WasMarked) continue;
            var junctionFeatures = junction.E.GetComponent<JunctionComp>().Features;
            if (junctionFeatures.Length == 0) continue;
            var junctionPos = junction.E.GetComponent<MapCompositionComp>().Coordinate;
            sb.Append($"@({junctionPos.X},{junctionPos.Y})[{junctionFeatures.Join(",")}]\n");
        }
        return sb.ToString();
    }

    private string FormPlayer()
    {
        if (Player is null) return "player(0,0)";
        var pos = Player.E.GetComponent<MapCompositionComp>().Coordinate;
        pos = (pos - Vector2I.One) / 2;
        var items = Player.E.GetComponent<PocketComp>().Items;
        return $"player({pos.X},{pos.Y})[{items.ToArray().Join(",")}]\n";
    }

    private string FormItem()
    {
        if (_itemWhitelist is null || _itemWhitelist.Count == 0) return "items[]\n";
        return $"items[{_itemWhitelist.ToArray().Join(",")}]\n";
    }
    #endregion
    
    #region debug
    private static void PrintLines(string[] lines)
    {
        foreach (var line in lines) GD.Print(line);
    }
    
    private void PrintItemWhitelist()
    { 
        GD.Print(_itemWhitelist.ToArray().Join(","));
    }
    #endregion
}