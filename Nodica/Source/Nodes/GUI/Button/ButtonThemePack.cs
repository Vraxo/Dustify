using Nodica;

public class ButtonThemePack
{
    // States

    public ButtonTheme Current { get; set; } = new();

    public ButtonTheme Normal { get; set; } = new();

    public ButtonTheme Hover { get; set; } = new()
    {
        FillColor = DefaultTheme.HoverFill
    };

    public ButtonTheme Pressed { get; set; } = new()
    {
        FillColor = DefaultTheme.Accent
    };

    public ButtonTheme Disabled { get; set; } = new()
    {
        FillColor = DefaultTheme.DisabledFill,
        BorderColor = DefaultTheme.DisabledBorder,
        FontColor = DefaultTheme.DisabledText
    };

    public ButtonTheme Focused { get; set; } = new()
    {
        BorderColor = DefaultTheme.FocusBorder,
        BorderLength = 1
    };

    // Setters

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
            Current.BorderColor = value;
            Normal.BorderColor = value;
            Hover.BorderColor = value;
            Pressed.BorderColor = value;
        }
    }

    public float BorderLengthUp
    {
        set
        {
            Current.BorderLengthUp = value;
            Normal.BorderLengthUp = value;
            Hover.BorderLengthUp = value;
            Pressed.BorderLengthUp = value;
        }
    }
}