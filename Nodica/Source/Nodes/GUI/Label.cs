using Raylib_cs;

namespace Nodica;

public class Label : VisualItem
{
    public class LabelStyle
    {
        public Font Font { get; set; } = FontLoader.Instance.Fonts["RobotoMono 32"];
        public Color FontColor { get; set; } = ThemeLoader.Instance.Colors["Text"];
        public uint FontSize { get; set; } = 16;
        public int FontSpacing { get; set; } = 0;
    }

    public enum TextCase
    {
        Upper,
        Lower,
        Both
    }

    public LabelStyle Style { get; set; } = new();
    public bool Clip { get; set; } = true;
    public string Ellipsis { get; set; } = "...";
    public TextCase Case { get; set; } = TextCase.Both;

    private string displayedText = "";

    private string _text = "";
    public string Text
    {
        get => _text;

        set
        {
            _text = value;
        }
    }

    public Label()
    {
        OriginPreset = OriginPreset.CenterLeft;
    }

    public override void Update()
    {
        LimitDisplayedText();
        ApplyCase();
        base.Update();
    }

    protected override void Draw()
    {
        Raylib.DrawTextEx(
            Style.Font,
            displayedText,
            GlobalPosition - Origin,
            Style.FontSize,
            Style.FontSpacing,
            Style.FontColor);
    }

    private void LimitDisplayedText()
    {
        if (!Clip)
        {
            return;
        }

        int numFittingCharacters = (int)(Size.X / (GetCharacterWidth() + Style.FontSpacing));

        if (numFittingCharacters <= 0)
        {
            displayedText = "";
        }
        else if (numFittingCharacters < _text.Length)
        {
            string trimmedText = _text[..numFittingCharacters];
            displayedText = ClipTextWithEllipsis(trimmedText);
        }
        else
        {
            displayedText = _text;
        }
    }

    private float GetCharacterWidth()
    {
        float width = Raylib.MeasureTextEx(
            Style.Font,
            " ",
            Style.FontSize,
            Style.FontSpacing).X;

        return width;
    }

    private string ClipTextWithEllipsis(string input)
    {
        if (input.Length > 3)
        {
            string trimmedText = input[..^Ellipsis.Length];
            return trimmedText + Ellipsis;
        }
        else
        {
            return input;
        }
    }

    private void ApplyCase()
    {
        displayedText = Case switch
        {
            TextCase.Upper => displayedText.ToUpper(),
            TextCase.Lower => displayedText.ToLower(),
            _ => displayedText
        };
    }
}