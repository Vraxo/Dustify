﻿using Raylib_cs;

namespace Nodica;

public partial class LineEdit
{
    private abstract class BaseText
    {
        public TextBoxStyle Style;

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
                parent.Style.Current.Font,
                GetText(),
                GetPosition(),
                parent.Style.Current.FontSize,
                parent.Style.Current.FontSpacing,
                parent.Style.Current.FontColor);
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
            Font font = parent.Style.Current.Font;
            string text = GetText();
            uint fontSize = (uint)parent.Style.Current.FontSize;

            int halfFontHeight = (int)(Raylib.MeasureTextEx(font, text, fontSize, 1).Y / 2);
            return halfFontHeight;
        }

        protected abstract string GetText();

        protected abstract bool ShouldSkipDrawing();
    }
}