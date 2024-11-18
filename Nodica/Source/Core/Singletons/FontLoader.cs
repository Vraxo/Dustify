namespace Nodica;

public class FontLoader
{
    public static FontLoader Instance => instance ??= new();
    private static FontLoader? instance;

    private Dictionary<string, Font> fonts = new();

    private FontLoader() { }

    public Font Get(string fullName)
    {
        if (fonts.ContainsKey(fullName))
            return fonts[fullName];

        (string fontName, int fontSize) = ParseFontName(fullName);

        string fontPath = $"Resources/Fonts/{fontName}.ttf";

        Font textFont = new(fontPath, fontSize);

        fonts.Add(fullName, textFont);

        return textFont;
    }

    private (string fontName, int fontSize) ParseFontName(string fullName)
    {
        int lastSpaceIndex = fullName.LastIndexOf(' ');

        if (lastSpaceIndex == -1)
            throw new ArgumentException($"Invalid font name format: {fullName}. Expected format: 'FontName Size'.");

        string fontName = fullName[..lastSpaceIndex];
        string sizeString = fullName[(lastSpaceIndex + 1)..];

        if (!int.TryParse(sizeString, out int fontSize))
            throw new ArgumentException($"Invalid font size in: {fullName}. Size must be a number.");

        return (fontName, fontSize);
    }
}
