using System.Linq;
using System.Text;
using Godot;

internal class MapConverter
{
    #region read
    #endregion

    #region write
    
    #endregion
}

internal class MapContentConverter
{
    #region read
    public CoordUnitStorage Read(string[] lines)
    {
        var content = new CoordUnitStorage();
        for (int y = 0; y < lines.Length; y++)
        {
            var line = lines[y];
            for (int x = 0; x < line.Length; x++)
            {
                if (y % 2 == 0 && x % 2 == 0)
                {
                    content.SetUnit(new(x, y), null);
                    continue;
                }
                var ch = line[x];
                var unit = CharToUnit(ch);
                if (unit is null) continue;
                unit.E.GetComponent<CoordinateComp>().Coordinate = new(x, y);
                content.SetUnit(new(x, y), unit);
            }
        }
        return content;
    }
    #endregion

    #region write
    public string Write(CoordUnitStorage content)
    {
        var sb = new StringBuilder();
        var coords = content.GetCoords<IUnit>(true,false);
        GD.Print(content.Size);
        var y = -1;
        foreach (var coord in coords)
        {
            if (coord.Y != y)
            {
                y = coord.Y;
                sb.Append('\n');
            }
            sb.Append(UnitToChar(content.GetUnit(coord)));
        }
        return sb.ToString();
    }
    #endregion

    private IUnit CharToUnit(char ch) =>
        ch switch
        {
            'E' or 'e' or '=' or '[' or ']' or '|' => UnitFacade.CreateUnit<Edge>(true),
            'C' or 'c' or 'O' or 'o' => UnitFacade.CreateUnit<Cell>(),
            'M' or 'm' or 'X' or 'x' => UnitFacade.CreateUnit<Cell>(true),
            'V' or 'v' or '#' or '@' => UnitFacade.CreateUnit<Void>(),
            'J' or 'j' or '+' or '*' => UnitFacade.CreateUnit<Junction>(),
            '?' => UnitFacade.CreateUnit<Placeholder>(),
            _ => null
        };

    private char UnitToChar(IUnit unit) =>
        unit switch
        {
            Cell cell => cell.E.GetComponent<MarkableComp>().IsMarked ? 'X' : 'O',
            Edge => '=',
            Void => '#',
            Junction => '+',
            Placeholder => '?',
            _ => ' '
        };
}