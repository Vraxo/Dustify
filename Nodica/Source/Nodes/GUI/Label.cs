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

    private int _visibleCharacters = -1;  // -1 means show all characters
    public int VisibleCharacters
    {
        get => _visibleCharacters;

        set
        {
            _visibleCharacters = value;
            UpdateVisibleRatio();  // Keep the ratio updated when characters change
        }
    }

    private float _visibleRatio = 1.0f;  // 1.0 means all characters are displayed
    public float VisibleRatio
    {
        get => _visibleRatio;

        set
        {
            _visibleRatio = value;
            UpdateVisibleCharacters();  // Keep the characters updated when ratio changes
        }
    }

    private string displayedText = "";

    private string _text = "";
    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            UpdateVisibleCharacters();  // Recalculate when text changes
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
        // Determine how much of the text is visible, either by VisibleCharacters or VisibleRatio
        string textToConsider = VisibleCharacters == -1 ? _text : _text[..Math.Min(VisibleCharacters, _text.Length)];

        if (!Clip)
        {
            displayedText = textToConsider;
            return;
        }

        int numFittingCharacters = (int)(Size.X / (GetCharacterWidth() + Style.FontSpacing));

        // Limit the number of characters we consider based on VisibleCharacters
        if (VisibleCharacters != -1)
        {
            numFittingCharacters = Math.Min(numFittingCharacters, VisibleCharacters);
        }

        if (numFittingCharacters <= 0)
        {
            displayedText = "";
        }
        else if (numFittingCharacters < textToConsider.Length)
        {
            string trimmedText = textToConsider[..numFittingCharacters];
            displayedText = ClipTextWithEllipsis(trimmedText);
        }
        else
        {
            displayedText = textToConsider;
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
        if (input.Length > Ellipsis.Length)
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

    private void UpdateVisibleCharacters()
    {
        // If VisibleRatio is set, recalculate the number of characters to display based on the ratio
        if (_text.Length > 0)
        {
            VisibleCharacters = (int)(_text.Length * VisibleRatio);
        }
    }

    private void UpdateVisibleRatio()
    {
        // Update VisibleRatio when VisibleCharacters changes
        if (_text.Length > 0)
        {
            _visibleRatio = (float)VisibleCharacters / _text.Length;
        }
        else
        {
            _visibleRatio = 1.0f;  // If there's no text, set it to full
        }
    }
}
