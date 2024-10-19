using Nodica;

public class ButtonStyle : StylePack<ButtonStateStyle>
{
    // Setters

    public float FontSpacing
    {
        set
        {
            Default.FontSpacing = value;
            Hover.FontSpacing = value;
            Pressed.FontSpacing = value;
        }
    }

    public float FontSize
    {
        set
        {
            Default.FontSize = value;
            Hover.FontSize = value;
            Pressed.FontSize = value;
        }
    }

    public Font Font
    {
        set
        {
            Default.Font = value;
            Hover.Font = value;
            Pressed.Font = value;
        }
    }

    public Color FontColor
    {
        set
        {
            Default.FontColor = value;
            Hover.FontColor = value;
            Pressed.FontColor = value;
        }
    }
}