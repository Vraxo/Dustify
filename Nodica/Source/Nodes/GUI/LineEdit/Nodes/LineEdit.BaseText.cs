using Raylib_cs;

namespace Nodica;

public partial class LineEdit : Control
{
    private abstract class BaseText
    {
        protected LineEdit parent;

        private Vector2 position = Vector2.Zero;

        private Vector2 GlobalPosition => parent.GlobalPosition + position;

        public BaseText(LineEdit parent)
        {
            this.parent = parent;
        }

        public void Update()
        {
            Draw();
        }

        protected void Draw()
        {
            if (!parent.Visible || ShouldSkipDrawing())
            {
                return;
            }

            Raylib.DrawTextEx(
                parent.Theme.Current.Font,
                GetText(),
                GetPosition(),
                parent.Theme.Current.FontSize,
                parent.Theme.Current.FontSpacing,
                parent.Theme.Current.FontColor);
        }

        protected Vector2 GetPosition()
        {
            Vector2 position = new(GetX(), GetY());
            return position;
        }

        private int GetX()
        {
            int x = (int)(GlobalPosition.X - parent.Origin.X + parent.TextOrigin.X);
            return x;
        }

        private int GetY()
        {
            int halfFontHeight = GetHalfFontHeight();
            int y = (int)(GlobalPosition.Y + (parent.Size.Y / 2) - halfFontHeight - parent.Origin.Y);
            return y;
        }

        private int GetHalfFontHeight()
        {
            Font font = parent.Theme.Current.Font;
            string text = GetText();
            uint fontSize = (uint)parent.Theme.Current.FontSize;

            int halfFontHeight = (int)(Raylib.MeasureTextEx(font, text, fontSize, 1).Y / 2);
            return halfFontHeight;
        }

        protected abstract string GetText();

        protected abstract bool ShouldSkipDrawing();
    }
}