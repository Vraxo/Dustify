using Raylib_cs;

namespace Nodica;

public abstract class VisualItem : Node2D
{
    public bool Visible { get; set; } = true;
    public bool ReadyForVisibility = false;

    public override void Update()
    {
        if (Visible && ReadyForVisibility)
        {
            Draw();
        }
        base.Update();
        ReadyForVisibility = true;
    }

    protected virtual void Draw() { }

    protected void DrawRoundedRectangle(Vector2 position, Vector2 size, float roundness, int segments, Color color)
    {
        Rectangle rectangle = new()
        {
            Position = position,
            Size = size
        };

        Raylib.DrawRectangleRounded(
            rectangle,
            roundness,
            segments,
            color);
    }

    protected void DrawThemedRectangle(Vector2 position, Vector2 size, BoxTheme theme)
    {
        float top = theme.BorderLengthUp;
        float right = theme.BorderLengthRight;
        float bottom = theme.BorderLengthDown;
        float left = theme.BorderLengthLeft;

        Vector2 outerRectanglePosition = new(position.X - left, position.Y - top);
        Vector2 outerRectangleSize = new(size.X + left + right, size.Y + top + bottom);

        if (top > 0 || right > 0 || bottom > 0 || left > 0)
        {
            DrawRoundedRectangle(
                outerRectanglePosition,
                outerRectangleSize,
                theme.Roundness,
                (int)size.Y,
                theme.BorderColor);
        }

        DrawRoundedRectangle(
            position,
            size,
            theme.Roundness,
            (int)size.Y,
            theme.FillColor);
    }

    protected void DrawTexture(Texture2D texture, Vector2 position, Color tint)
    {
        Raylib.DrawTextureV(
            texture,
            position,
            tint);
    }

    protected void DrawText(string text, Vector2 position, Raylib_cs.Font font, float fontSize, float spacing, Color color)
    {
        Raylib.DrawTextEx(
            font,
            text,
            position,
            fontSize,
            spacing,
            color);
    }
}