
public class Tile
{
    enum Type
    {
        Empty,
        Full
    }

    Type type;

    public int x { get; private set; }
    public int y { get; private set; }

    public Tile(int _x, int _y)
    {
        this.x = _x;
        this.y = _y;

        type = Type.Full;
    }
}