using Raylib_cs;

namespace Nodica;

public partial class LineEdit
{
    private class Shape
    {
        private LineEdit parent;

        public Shape(LineEdit parent)
        {
            this.parent = parent;
        }

        public void Update()
        {
            Draw();
        }

        private void Draw()
        {
            if (!(parent.Visible && parent.ReadyForVisibility))
            {
                return;
            }

            DrawOutline();
            DrawInside();
        }

        private void DrawOutline()
        {
            if (parent.Style.Current.BorderLength <= 0)
            {
                return;
            }

            for (int i = 1; i <= parent.Style.Current.BorderLength; i++)
            {
                Vector2 offset = new(i / 2f, i / 2f);

                Rectangle rectangle = new()
                {
                    Position = parent.GlobalPosition - parent.Origin - offset,
                    Size = new(parent.Size.X + i, parent.Size.Y + i)
                };

                Raylib.DrawRectangleRounded(
                    rectangle,
                    parent.Style.Current.Roundness,
                    (int)parent.Size.Y,
                    parent.Style.Current.BorderColor);
            }
        }

        private void DrawInside()
        {
            Rectangle rectangle = new()
            {
                Position = parent.GlobalPosition - parent.Origin,
                Size = parent.Size
            };

            Raylib.DrawRectangleRounded(
                rectangle,
                parent.Style.Current.Roundness,
                (int)parent.Size.Y,
                parent.Style.Current.FillColor);
        }
    }
}