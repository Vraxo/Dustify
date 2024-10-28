using Raylib_cs;

namespace Nodica;

public partial class LineEdit
{
    private class Shape : VisualItem
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

            DrawBorderedRectangle(
                parent.GlobalPosition - parent.Origin,
                parent.Size,
                parent.Theme.Current);

            //DrawOutline();
            //DrawInside();
        }

        //private void DrawOutline()
        //{
        //    if (parent.Theme.Current.BorderLength <= 0)
        //    {
        //        return;
        //    }
        //
        //    for (int i = 1; i <= parent.Theme.Current.BorderLength; i++)
        //    {
        //        Vector2 offset = new(i / 2f, i / 2f);
        //
        //        Rectangle rectangle = new()
        //        {
        //            Position = parent.GlobalPosition - parent.Origin - offset,
        //            Size = new(parent.Size.X + i, parent.Size.Y + i)
        //        };
        //
        //        Raylib.DrawRectangleRounded(
        //            rectangle,
        //            parent.Theme.Current.Roundness,
        //            (int)parent.Size.Y,
        //            parent.Theme.Current.BorderColor);
        //    }
        //}
        //
        //private void DrawInside()
        //{
        //    Rectangle rectangle = new()
        //    {
        //        Position = parent.GlobalPosition - parent.Origin,
        //        Size = parent.Size
        //    };
        //
        //    Raylib.DrawRectangleRounded(
        //        rectangle,
        //        parent.Theme.Current.Roundness,
        //        (int)parent.Size.Y,
        //        parent.Theme.Current.FillColor);
        //}
    }
}