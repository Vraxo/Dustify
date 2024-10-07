using Raylib_cs;

namespace Nodica;

public partial class LineEdit : ClickableRectangle
{
    protected class Caret
    {
        #region [ - - - Properties & Fields - - - ]

        private Vector2 position { get; set; } = Vector2.Zero;

        public float MaxTime = 0.5F;
        private const int minTime = 0;
        private const byte minAlpha = 0;
        private const byte maxAlpha = 255;
        private float timer = 0;
        private byte alpha = 255;
        private LineEdit parent;

        private float arrowKeyTimer = 0f;
        private const float arrowKeyDelay = 0.5f; 
        private const float arrowKeySpeed = 0.05f;

        private bool arrowKeyHeld = false;
        private bool movingRight = false;

        private int _x = 0;
        public int X
        {
            get => _x;

            set
            {
                if (value > _x)
                {
                    if (_x < Math.Min(parent.Text.Length, parent.GetDisplayableCharactersCount()))
                    {
                        _x = value;
                    }
                }
                else
                {
                    if (value >= 0)
                    {
                        _x = value;
                    }
                }

                alpha = maxAlpha;
            }
        }

        private Vector2 GlobalPosition => parent.GlobalPosition + position;

        #endregion

        // Public

        public Caret(LineEdit parent)
        {
            this.parent = parent;
        }

        public void Update()
        {
            if (!parent.Selected)
            {
                return;
            }

            HandleInput();
            Draw();
            UpdateAlpha();
        }

        private void Draw()
        {
            Raylib.DrawTextEx(
                parent.Style.Current.Font,
                "|",
                GetPosition(),
                parent.Style.Current.FontSize,
                1,
                GetColor());
        }

        // Movement

        private void HandleInput()
        {
            // Check for initial arrow key press
            if (Raylib.IsKeyPressed(KeyboardKey.Right))
            {
                arrowKeyHeld = true;
                movingRight = true;
                arrowKeyTimer = 0f;
                MoveCaretRight();
            }
            else if (Raylib.IsKeyPressed(KeyboardKey.Left))
            {
                arrowKeyHeld = true;
                movingRight = false;
                arrowKeyTimer = 0f;
                MoveCaretLeft();
            }

            // Check if arrow key is held down
            if (Raylib.IsKeyDown(KeyboardKey.Right) || Raylib.IsKeyDown(KeyboardKey.Left))
            {
                arrowKeyTimer += Raylib.GetFrameTime();

                if (arrowKeyTimer >= arrowKeyDelay)
                {
                    if (arrowKeyTimer % arrowKeySpeed < Raylib.GetFrameTime())
                    {
                        if (movingRight)
                        {
                            MoveCaretRight();
                        }
                        else
                        {
                            MoveCaretLeft();
                        }
                    }
                }
            }

            // Reset on key release
            if (Raylib.IsKeyReleased(KeyboardKey.Right) || Raylib.IsKeyReleased(KeyboardKey.Left))
            {
                arrowKeyHeld = false;
            }

            // Handle mouse click for caret positioning
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                MoveIntoPosition(Raylib.GetMousePosition().X);
            }
        }

        private void MoveCaretRight()
        {
            if (X == parent.GetDisplayableCharactersCount() && X < parent.Text.Length - parent.TextStartIndex)
            {
                parent.TextStartIndex++;
            }

            X++;
        }

        private void MoveCaretLeft()
        {
            if (X == 0 && parent.Text.Length > parent.GetDisplayableCharactersCount() && parent.TextStartIndex > 0)
            {
                parent.TextStartIndex--;
            }

            X--;
        }

        public void MoveIntoPosition(float mouseX)
        {
            if (parent.Text.Length == 0)
            {
                X = 0;
            }
            else
            {
                float x = mouseX - (parent.GlobalPosition.X - parent.Origin.X) - parent.TextOrigin.X;

                float characterWidth = GetSize().X;
                X = (int)MathF.Floor(x / characterWidth);

                int maxX = Math.Min(parent.GetDisplayableCharactersCount(), parent.Text.Length);
                X = Math.Clamp(X, 0, maxX);
            }
        }

        // Position & size

        private Vector2 GetPosition()
        {
            Vector2 size = GetSize();

            int x = (int)(GlobalPosition.X - parent.Origin.X + parent.TextOrigin.X + X * size.X - size.X / 2) + X;
            int y = (int)(GlobalPosition.Y + parent.Size.Y / 2 - size.Y / 2 - parent.Origin.Y);

            return new(x, y);
        }

        private Vector2 GetSize()
        {
            Font font = parent.Style.Current.Font;
            float fontSize = parent.Style.Current.FontSize;

            int width = (int)Raylib.MeasureTextEx(font, "|", fontSize, 1).X;
            int fontHeight = (int)(Raylib.MeasureTextEx(font, parent.Text, fontSize, 1).Y);

            return new(width, fontHeight);
        }

        // Alpha

        private Color GetColor()
        {
            Color color = parent.Style.Current.TextColor;
            color.A = alpha;

            return color;
        }

        private void UpdateAlpha()
        {
            if (timer > MaxTime)
            {
                alpha = alpha == maxAlpha ? minAlpha : maxAlpha;
                timer = minTime;
            }

            timer += Raylib.GetFrameTime();
        }
    }
}