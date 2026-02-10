// TODO 行列坐标和游戏坐标互转，以及将行列坐标的内容扩展到其他代码中
public struct Ranks(int row, int col)
{
    public int Row { get; set; } = row;
    public int Col { get; set; } = col;
    public Direction Direction { get; set; } = Direction.None;
    
    public override string ToString()
    {
        return $"{Row},{Col}";
    }
}