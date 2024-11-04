namespace Nodica;

public class ColorRectangle : ClickableRectangle
{
    public BoxStyle Style = new();

    public ColorRectangle()
    {
        Size = new(32, 32);
        OriginPreset = OriginPreset.TopLeft;
    }

    protected override void Draw()
    {
        DrawBorderedRectangle(
            GlobalPosition - Offset,
            Size,
            Style);
    }
}