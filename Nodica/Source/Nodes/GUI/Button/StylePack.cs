using Nodica;

public class StylePack<TStateStyle> where TStateStyle : Style, new()
{
    // States

    public TStateStyle Current { get; set; } = new();
    
    public TStateStyle Default { get; set; } = new();
    
    public TStateStyle Hover { get; set; } = new() 
    { 
        FillColor = ThemeLoader.Instance.Colors["HoverFill"] 
    };
    
    public TStateStyle Pressed { get; set; } = new()
    {
        FillColor = ThemeLoader.Instance.Colors["Accent"]
    };

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