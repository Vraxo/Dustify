using Raylib_cs;

namespace Nodica;

public abstract class VisualItem : Node2D
{
    public override void Update()
    {
        if (Visible && ReadyForVisibility)
        {
            Draw();
        }

        base.Update();
    }

    protected virtual void Draw() { }

    protected void DrawOutlinedRectangle(Vector2 position, Vector2 size, Style style)
    {
        if (style.OutlineThickness > 0)
        {
            for (int i = 1; i <= style.OutlineThickness; i++)
            {
                Vector2 offset = new(i / 2f, i / 2f);

                Rectangle rectangle = new()
                {
                    Position = position - offset,
                    Size = new(size.X + i, size.Y + i)
                };

                Raylib.DrawRectangleRounded(
                    rectangle,
                    style.Roundness,
                    (int)size.Y,
                    style.OutlineColor);
            }
        }

        Rectangle innerRectangle = new()
        {
            Position = position,
            Size = size
        };

        Raylib.DrawRectangleRounded(
            innerRectangle,
            style.Roundness,
            (int)size.Y,
            style.FillColor);
    }
}