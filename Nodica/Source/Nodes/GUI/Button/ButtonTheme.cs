namespace Nodica;

public class ButtonTheme : BoxStyle
{
    public float FontSpacing { get; set; } = 0;
    public float FontSize { get; set; } = 16;
    public Font Font { get; set; } = FontLoader.Instance.Fonts["RobotoMono 32"];
    public Color FontColor { get; set; } = DefaultTheme.Text;
}