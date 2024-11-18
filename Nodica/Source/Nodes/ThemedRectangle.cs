namespace Nodica;

public class ThemedRectangle : ClickableRectangle
{
    public BoxTheme Style = new();

    public ThemedRectangle()
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