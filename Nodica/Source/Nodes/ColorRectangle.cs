namespace Nodica;

public class ColorRectangle : ClickableRectangle
{
    public RectangleStyle Style = new();

    public ColorRectangle()
    {
        Size = new(32, 32);
        OriginPreset = OriginPreset.TopLeft;
    }

    protected override void Draw()
    {
        DrawBorderedRectangle(
            GlobalPosition - Origin,
            Size,
            Style);
    }
}