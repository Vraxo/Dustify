namespace Nodica;

public class ButtonTheme : RectangleStyle
{
    public float FontSpacing { get; set; } = 1;
    public float FontSize { get; set; } = 16;
    public Font Font { get; set; } = FontLoader.Instance.Fonts["RobotoMono 32"];
    public Color FontColor { get; set; } = DefaultTheme.Text;
}