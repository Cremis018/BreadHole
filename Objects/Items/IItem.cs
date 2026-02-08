internal interface IItem
{
    int Id { get; set; }
    void UsePrimarily(GameWorld world);
    void UseSecondary(GameWorld world);
}