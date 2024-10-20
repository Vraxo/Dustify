namespace Nodica;

public class Style
{
    public float Roundness        { get; set; } = 0;
    public float OutlineThickness { get; set; } = 0;
    public Color FillColor        { get; set; } = ThemeLoader.Instance.Colors["DefaultFill"];
    public Color OutlineColor     { get; set; } = ThemeLoader.Instance.Colors["DefaultOutline"];
}