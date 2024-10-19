using Raylib_cs;
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
        DrawEmpty();
        DrawFilled();
    }

    private void DrawEmpty()
    {
        Raylib.DrawRectangleV(
            GlobalPosition - Origin,
            Size,
            EmptyStyle.FillColor);
    }

    private void DrawFilled()
    {
        Vector2 size = new(Size.X * Percentage, Size.Y);

        Raylib.DrawRectangleV(
            GlobalPosition - Origin,
            size,
            FilledStyle.FillColor);
    }
}