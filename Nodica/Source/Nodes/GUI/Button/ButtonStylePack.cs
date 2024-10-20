using Nodica;

public class ButtonStylePack
{
    // States

    public ButtonStyle Current { get; set; } = new();
    
    public ButtonStyle Default { get; set; } = new();
    
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

    public float Roundness
    {
        set
        {
            Current.Roundness = value;
            Default.Roundness = value;
            Hover.Roundness = value;
            Pressed.Roundness = value;
        }
    }

    public float OutlineThickness
    {
        set
        {
            Current.OutlineThickness = value;
            Default.OutlineThickness = value;
            Hover.OutlineThickness = value;
            Pressed.OutlineThickness = value;
        }
    }

    public Color FillColor
    {
        set
        {
            Current.FillColor = value;
            Default.FillColor = value;
            Hover.FillColor = value;
            Pressed.FillColor = value;
        }
    }

    public Color OutlineColor
    {
        set
        {
            Current.OutlineColor = value;
            Default.OutlineColor = value;
            Hover.OutlineColor = value;
            Pressed.OutlineColor = value;
        }
    }
}