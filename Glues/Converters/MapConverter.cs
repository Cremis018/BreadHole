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
        for (var row = 0; row < lines.Length; row++)
        {
            var line = lines[row];
            line = line.Substring(1, line.Length - 2);
            for (var col = 0; col < line.Length; col++)
            {
                var ch = line[col];
                var unit = CharToCell(ch);
                if (unit is null) continue;
                content.SetUnit(GetCoord(row, col), unit);
            }
        }
        FillJunction(content);
        return content;
    }
    #endregion

    #region write
    public string Write(CoordUnitStorage content)
    {
        var coords = content.GetCellVec2Coords(true);
        var rect = content.GetRect();
        var sb = new StringBuilder();
        sb.AppendLine(new string('#', rect.Size.X + 2));
        foreach (var row in coords)
        {
            sb.Append('#');
            var chars = new string('#',rect.Size.X).ToCharArray();
            foreach (var unit in row)
            {
                chars[unit.X] = CellToChar(content.GetUnit((unit-Vector2I.One)/2));
            }
            sb.Append("#\n");
        }
        sb.Append(new string('#', rect.Size.X + 2));
        return null;
    }
    #endregion

    private Vector2I GetCoord(int row, int col) => new(row * 2 + 1, col * 2 + 1);
    private (int row, int col) GetRowAndCol(Vector2I coord) => ((coord.X - 1) / 2, (coord.Y - 1) / 2);

    private IUnit CharToCell(char ch)
    {
        switch (ch)
        {
            case '#':
                return NodeUtil.Create<Void>();
            case 'O' or 'o':
                var cell1 = NodeUtil.Create<Cell>();
                return cell1;
            case 'X' or 'x':
                var cell2 = NodeUtil.Create<Cell>();
                var markableComp = cell2.E.GetComponent<MarkableComp>();
                markableComp.IsMarked = true;
                markableComp.ActivateItems = [new Crumbs()];
                return cell2;
            default:
                return null;
        }
    }
    
    private char CellToChar(IUnit unit) =>
        unit switch
        {
            Void => '#',
            Cell cell => cell.E.GetComponent<MarkableComp>().IsMarked ? 'X' : 'O',
            _ => ' '
        };

    private void FillJunction(CoordUnitStorage content)
    {
        FillHorizontalJunction(content);
        FillVerticalJunction(content);
    }

    private void FillHorizontalJunction(CoordUnitStorage content)
    {
        var coords = content.GetCellVec2Coords();
        foreach (var row in coords)
        {
            var mark = true;
            for (int i = 0; i < row.Length; i++)
            {
                var cell = row[i];
                if (i >= row.Length) content.SetUnit(row[i]+Vector2I.Right,NodeUtil.Create<Edge>());
                if (mark)
                {
                    content.SetUnit(cell + Vector2I.Left, NodeUtil.Create<Edge>());
                    mark = false;
                }
                var next = row[i + 1];
                if (cell.X - next.X > 2)
                {
                    content.SetUnit(cell + Vector2I.Right, NodeUtil.Create<Edge>());
                    mark = true;
                }
                else
                    content.SetUnit(cell + Vector2I.Right, NodeUtil.Create<Junction>());
            }
        }
    }
    
    private void FillVerticalJunction(CoordUnitStorage content)
    { 
        var coords = content.GetCellVec2Coords(true);
        foreach (var col in coords)
        {
            var mark = true;
            for (int i = 0; i < col.Length; i++)
            {
                var cell = col[i];
                if (i >= col.Length) content.SetUnit(col[i]+Vector2I.Down,NodeUtil.Create<Edge>());
                if (mark)
                {
                    content.SetUnit(cell + Vector2I.Up, NodeUtil.Create<Edge>());
                    mark = false;
                }
                var next = col[i + 1];
                if (cell.X - next.X > 2)
                {
                    content.SetUnit(cell + Vector2I.Down, NodeUtil.Create<Edge>());
                    mark = true;
                }
                else
                    content.SetUnit(cell + Vector2I.Down, NodeUtil.Create<Junction>());
            }
        }
    }
}