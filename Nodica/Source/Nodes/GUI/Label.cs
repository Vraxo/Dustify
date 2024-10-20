using Raylib_cs;

namespace Nodica;

public class Label : VisualItem
{
    public Color  Color          { get; set; } = ThemeLoader.Instance.Colors["Text"];
    public uint   FontSize       { get; set; } = 16;
    public Font   Font           { get; set; } = FontLoader.Instance.Fonts["RobotoMono 32"];
    public int    MaxCharacters  { get; set; } = -1;
    public string Ellipsis       { get; set; } = "...";
    public bool   Clip           { get; set; } = true;

    private string displayedText = "";

    private string _text = "";
    public string Text
    {
        get => _text;

        set
        {
            _text = value;
            displayedText = value;
        }
    }

    public Label()
    {
        OriginPreset = OriginPreset.CenterLeft;
    }

    public override void Update()
    {
        LimitDisplayedText();
        base.Update();
    }

    protected override void Draw()
    {
        Raylib.DrawTextEx(
            Font,
            displayedText,
            GlobalPosition - Origin,
            FontSize,
            1,
            Color);
    }

    private void LimitDisplayedText()
    {
        if (!Clip)
        {
            return;
        }

        int numFittingCharacters = (int)(Size.X / (GetCharacterWidth() + 1));

        if (numFittingCharacters <= 0)
        {
            displayedText = "";
        }
        else if (numFittingCharacters < Text.Length)
        {
            string trimmedText = Text[..numFittingCharacters];
            displayedText = ReplaceTextEndWithEllipsis(trimmedText);
        }
        else
        {
            displayedText = Text;
        }
    }

    private float GetCharacterWidth()
    {
        float width = Raylib.MeasureTextEx(
            Font,
            " ",
            FontSize,
            1).X;

        return width;
    }

    private string ReplaceTextEndWithEllipsis(string input)
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
}