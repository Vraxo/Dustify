namespace Nodica;

public class ProgressBar : VisualItem
{
    public float Percentage { get; set; } = 0f;
    public Style EmptyStyle = new();
    public Style FilledStyle = new();

    public ProgressBar()
    {
        Size = new(250, 10);
        FilledStyle.FillColor = ThemeLoader.Instance.Colors["Accent"];
    }

    protected override void Draw()
    {
        DrawOutlinedRectangle(GlobalPosition - Origin, Size, EmptyStyle);

        Vector2 filledSize = new(Size.X * Percentage, Size.Y);
        DrawOutlinedRectangle(GlobalPosition - Origin, filledSize, FilledStyle);
    }
}