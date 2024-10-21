using Nodica;

public class ButtonStylePack
{
    // States

    public ButtonStyle Current { get; set; } = new();

    public ButtonStyle Normal { get; set; } = new();

    public ButtonStyle Hover { get; set; } = new()
    {
        FillColor = ThemeLoader.Instance.Colors["HoverFill"]
    };

    public ButtonStyle Pressed { get; set; } = new()
    {
        FillColor = ThemeLoader.Instance.Colors["Accent"]
    };

    public float FontSpacing
    {
        set
        {
            Normal.FontSpacing = value;
            Hover.FontSpacing = value;
            Pressed.FontSpacing = value;
        }
    }

    public float FontSize
    {
        set
        {
            Normal.FontSize = value;
            Hover.FontSize = value;
            Pressed.FontSize = value;
        }
    }

    public Font Font
    {
        set
        {
            Normal.Font = value;
            Hover.Font = value;
            Pressed.Font = value;
        }
    }

    public Color FontColor
    {
        set
        {
            Normal.FontColor = value;
            Hover.FontColor = value;
            Pressed.FontColor = value;
        }
    }

    public float Roundness
    {
        set
        {
            Current.Roundness = value;
            Normal.Roundness = value;
            Hover.Roundness = value;
            Pressed.Roundness = value;
        }
    }

    public float OutlineThickness
    {
        set
        {
            Current.BorderLength = value;
            Normal.BorderLength = value;
            Hover.BorderLength = value;
            Pressed.BorderLength = value;
        }
    }

    public Color FillColor
    {
        set
        {
            Current.FillColor = value;
            Normal.FillColor = value;
            Hover.FillColor = value;
            Pressed.FillColor = value;
        }
    }

    public Color OutlineColor
    {
        set
        {
            Current.OutlineColor = value;
            Normal.OutlineColor = value;
            Hover.OutlineColor = value;
            Pressed.OutlineColor = value;
        }
    }
}