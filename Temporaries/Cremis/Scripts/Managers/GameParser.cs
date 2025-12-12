//TODO:游戏解析器系列

using System.Collections.Generic;
using System.Linq;
using Godot;

public static class GameParser
{
    #region load
    public static void Load(string mapFilePath)
    {
        var rawContent = TextFileUtil.ReadLines(mapFilePath);
        if (rawContent.Length == 0) return;
        Game.Instance.Map = GameMapParser.Load(rawContent);
    }
    #endregion

    #region save
    
    #endregion
}

public static class GameGlobalParser
{
    #region load
    
    #endregion

    #region save
    
    #endregion
}

public static class GameMapParser
{
    #region parse
    public static GameMapWorld Load(string[] rawContent)
    {
        var map = new GameMapWorld();
        var mapContent = FindMapContent(rawContent);
        GD.Print(mapContent.ToArray().Join("\n"));
        map.FloorSet = LoadFloorSet(mapContent);
        
        return map;
    }

    public static string Save(GameMapWorld world, string mapFilePath)
    {
        return null;
    }
    #endregion

    #region floor
    private static Dictionary<Vector2I, Floor> LoadFloorSet(List<string> lines)
    {
        var dic = new Dictionary<Vector2I, Floor>();
        for (var row = 0; row < lines.Count; row++)
        {
            var line = lines[row];
            for (int col = 0; col < line.Length; col++)
            {
                var ch = line[col];
                var kv = GetFloorKV(ch, row, col);
                if (kv.Item1.Sign()<=Vector2I.Zero || kv.Item2 is null) continue;
                dic.Add(kv.Item1, kv.Item2);
            }
        }
        return dic;
    }
    
    private static (Vector2I,Floor) GetFloorKV(char ch,int row,int col)
    {
        var rawPos = new Vector2I(col, row);
        var pos = (rawPos - Vector2I.One) * 2 + Vector2I.One;
        var floor = Floor.Scene.Instantiate().Duplicate() as Floor;
        floor.Name = $"Floor({pos.X},{pos.Y})";
        switch (ch)
        {
            case 'O':
            case 'o':
                break;
            case 'X':
            case 'x':
                break;
        }

        return (pos, floor);
    }
    
    private static string[] SaveFloorSet(Dictionary<Vector2I, Floor> floorSet)
    {
        return [];
    }
    #endregion

    #region finder
    private static List<string> FindMapContent(string[] content)
    {
        List<string> mapContent = [];
        mapContent.AddRange(content.Where(s => s.StartsWith('#') && s.EndsWith('#')));
        return mapContent;
    }
    #endregion
}

public static class GameMiscParser
{
    #region load
    public static void Load(GameMapWorld world, string[] content)
    {
        
    }
    #endregion

    #region save
    
    #endregion
}

public static class EntityFeaturesParser
{
    
}

public static class TriggerEventsParser
{
    
}