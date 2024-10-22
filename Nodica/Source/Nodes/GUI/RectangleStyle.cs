namespace Nodica;

public class RectangleStyle
{
    public float Roundness { get; set; } = 0.5f;
    public float BorderLength { get; set; } = 0;
    public Color FillColor { get; set; } = ThemeLoader.Instance.Colors["DefaultFill"];
    public Color BorderColor { get; set; } = ThemeLoader.Instance.Colors["DefaultOutline"];

    public float BorderLengthUp { get; set; } = 0;
    public float BorderLengthRight { get; set; } = 0;
    public float BorderLengthDown { get; set; } = 0;
    public float BorderLengthLeft { get; set; } = 0;
}