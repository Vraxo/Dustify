namespace Nodica;

public class ColorRectangle : ClickableRectangle
{
    public BoxTheme Style = new();

    public ColorRectangle()
    {
        Size = new(32, 32);
        OriginPreset = OriginPreset.TopLeft;
    }

    protected override void Draw()
    {
        DrawThemedRectangle(
            GlobalPosition - Offset,
            Size,
            Style);
    }
}