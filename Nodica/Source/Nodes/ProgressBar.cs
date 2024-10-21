namespace Nodica;

public class ProgressBar : VisualItem
{
    public BoxStyle EmptyStyle = new();
    public BoxStyle FilledStyle = new();

    private float _percentage = 0;
    public float Percentage
    {
        get => _percentage;

        set
        {
            _percentage = Math.Clamp(value, 0, 1);
        }
    }

    public ProgressBar()
    {
        Size = new(250, 10);
        FilledStyle.FillColor = ThemeLoader.Instance.Colors["Accent"];
    }

    protected override void Draw()
    {
        DrawBorderedRectangle(GlobalPosition - Origin, Size, EmptyStyle);

        if (Percentage == 0)
        {
            return;
        }

        Vector2 filledSize = new(Size.X * Percentage, Size.Y);
        DrawBorderedRectangle(GlobalPosition - Origin, filledSize, FilledStyle);
    }
}