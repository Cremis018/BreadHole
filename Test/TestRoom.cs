using Godot;

public partial class TestRoom : Node2D
{
    public override void _Ready()
    {
        // var textureConverter = new TextureConverter();
        // var map = textureConverter.FolderToTextureMap(UnitTextureStorage.InnerPath);
        // UnitTextureStorage.UpdateData(map);
        var builder = new UnitsBuilder(this);
        var mc = new MapContentConverter();
        var storage = mc.Read([
            "=========",
            "]O+O+O+O[",
            "]+ + = +[",
            "]O+O|#|O[",
            "]= = = +[",
            "]X+O+O+X[",
            "========="
        ]);
        builder.Build(storage);

        GD.Print(mc.Write(storage));
    }
}
