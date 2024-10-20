using Raylib_cs;

namespace Nodica;

public class Button : ClickableRectangle
{
    public enum ClickMode
    {
        Limited,
        Limitless
    }

    #region [ - - - Properties & Fields - - - ]

    public Vector2 TextPadding { get; set; } = Vector2.Zero;
    public Vector2 TextOrigin { get; set; } = Vector2.Zero;
    public OriginPreset TextOriginPreset { get; set; } = OriginPreset.Center;
    public ButtonStylePack Style { get; set; } = new();
    public bool PressedLeft = false;
    public bool PressedRight = false;
    public bool TruncateText { get; set; } = false;
    public float AvailableWidth { get; set; } = 0;
    public ClickMode LeftClickMode { get; set; } = ClickMode.Limitless;
    public ClickMode RightClickMode { get; set; } = ClickMode.Limitless;

    public Action<Button> OnUpdate = (button) => { };

    public event EventHandler? LeftClicked;
    public event EventHandler? RightClicked;

    private bool alreadyClicked = false;
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

    #endregion

    // Public

    public Button()
    {
        Size = new(100, 26);
    }

    public override void Update()
    {
        OnUpdate(this);
        LimitDisplayedText();
        UpdateTextOrigin();
        HandleClicks();
        Draw();
        base.Update();
    }

    // Click handling

    private void HandleClicks()
    {
        HandleClick(
            ref PressedLeft, 
            MouseButton.Left, 
            LeftClickMode, 
            OnTopLeft, 
            LeftClicked, 
            ref OnTopLeft);

        HandleClick(
            ref PressedRight, 
            MouseButton.Right, 
            RightClickMode, 
            OnTopRight, 
            RightClicked, 
            ref OnTopRight);
    }

    // Generic click handling (used for both left and right)

    private void HandleClick(ref bool pressed, MouseButton button, ClickMode mode, bool onTop, EventHandler? clickedEvent, ref bool onTopFlag)
    {
        if (clickedEvent is null)
        {
            return;
        }

        if (mode == ClickMode.Limitless)
        {
            HandleClickLimitless(ref pressed, button, onTop, clickedEvent, ref onTopFlag);
        }
        else
        {
            HandleClickLimited(ref pressed, button, onTop, clickedEvent, ref onTopFlag);
        }
    }

    private void HandleClickLimitless(ref bool pressed, MouseButton button, bool onTop, EventHandler? clickedEvent, ref bool onTopFlag)
    {
        if (Raylib.IsMouseButtonDown(button))
        {
            if (!IsMouseOver())
            {
                alreadyClicked = true;
            }
        }

        if (IsMouseOver())
        {
            Style.Current = Style.Hover;

            if (Raylib.IsMouseButtonReleased(button))
            {
                if (pressed)
                {
                    pressed = false;
                    clickedEvent?.Invoke(this, EventArgs.Empty);
                }
            }

            if (Raylib.IsMouseButtonDown(button))
            {
                if (!alreadyClicked && onTop)
                {
                    onTopFlag = false;
                    pressed = true;
                    alreadyClicked = true;
                }

                if (pressed)
                {
                    Style.Current = Style.Pressed;
                }
            }
        }
        else
        {
            Style.Current = Style.Default;
        }

        if (Raylib.IsMouseButtonReleased(button))
        {
            if (IsMouseOver() && pressed)
            {
                clickedEvent?.Invoke(this, EventArgs.Empty);
            }

            pressed = false;
            alreadyClicked = false;
            Style.Current = Style.Default;
        }
    }

    private void HandleClickLimited(ref bool pressed, MouseButton button, bool onTop, EventHandler? clickedEvent, ref bool onTopFlag)
    {
        if (IsMouseOver())
        {
            Style.Current = Style.Hover;

            if (Raylib.IsMouseButtonPressed(button) && onTop)
            {
                pressed = true;
                onTopFlag = false;
            }

            if (pressed)
            {
                Style.Current = Style.Pressed;
            }
        }
        else
        {
            pressed = false;
            Style.Current = Style.Default;
        }

        if (Raylib.IsMouseButtonReleased(button))
        {
            if (IsMouseOver() && pressed)
            {
                clickedEvent?.Invoke(this, EventArgs.Empty);
            }

            pressed = false;
            Style.Current = Style.Default;
        }
    }

    // Drawing

    protected override void Draw()
    {
        DrawShape();
        DrawText();
    }

    private void DrawShape()
    {
        DrawShapeOutline();
        DrawShapeInside();
    }

    private void DrawShapeInside()
    {
        Rectangle rectangle = new()
        {
            Position = GlobalPosition - Origin * Scale,
            Size = Size * Scale
        };

        Raylib.DrawRectangleRounded(
            rectangle,
            Style.Current.Roundness,
            (int)Size.Y,
            Style.Current.FillColor);
    }

    private void DrawShapeOutline()
    {
        if (Style.Current.OutlineThickness <= 0)
        {
            return;
        }

        Rectangle rectangle = new()
        {
            Position = GlobalPosition - Origin * Scale,
            Size = Size * Scale
        };

        for (int i = 0; i <= Style.Current.OutlineThickness; i++)
        {
            Rectangle outlineRectangle = new()
            {
                Position = rectangle.Position - new Vector2(i, i),
                Size = new(rectangle.Size.X + i + 1, rectangle.Size.Y + i + 1)
            };

            Raylib.DrawRectangleRounded(
                outlineRectangle,
                Style.Current.Roundness,
                (int)rectangle.Size.X,
                Style.Current.OutlineColor);
        }
    }

    private void DrawText()
    {
        Raylib.DrawTextEx(
            Style.Current.Font,
            displayedText,
            GetTextPosition(),
            Style.Current.FontSize,
            1,
            Style.Current.FontColor);
    }

    // Text positioning

    private Vector2 GetTextPosition()
    {
        Vector2 fontDimensions = Raylib.MeasureTextEx(
            Style.Current.Font,
            Text,
            Style.Current.FontSize,
            1
        );

        Vector2 center = Size / 2;

        Vector2 alignmentAdjustment = new(
            TextOrigin.X < center.X ? 0 : TextOrigin.X > center.X ? -fontDimensions.X : -fontDimensions.X / 2,
            TextOrigin.Y < center.Y ? 0 : TextOrigin.Y > center.Y ? -fontDimensions.Y : -fontDimensions.Y / 2
        );

        return GlobalPosition + TextOrigin + alignmentAdjustment - Origin + TextPadding;
    }

    private void UpdateTextOrigin()
    {
        TextOrigin = TextOriginPreset switch
        {
            OriginPreset.Center => Size / 2,
            OriginPreset.CenterLeft => new(0, Size.Y / 2),
            OriginPreset.CenterRight => new(Size.X, Size.Y / 2),
            OriginPreset.TopLeft => new(0, 0),
            OriginPreset.TopRight => new(Size.X, 0),
            OriginPreset.TopCenter => new(Size.X / 2, 0),
            OriginPreset.BottomLeft => new(0, Size.Y),
            OriginPreset.BottomRight => Size,
            OriginPreset.BottomCenter => new(Size.X / 2, Size.Y),
            OriginPreset.None => Origin,
            _ => Origin,
        };
    }

    // Displayed text truncating

    private void LimitDisplayedText()
    {
        if (!TruncateText)
        {
            return;
        }

        float characterWidth = GetCharacterWidth();
        int numFittingCharacters = (int)(AvailableWidth / characterWidth);

        if (numFittingCharacters <= 0)
        {
            displayedText = "";
        }
        else if (numFittingCharacters < Text.Length)
        {
            string trimmedText = Text[..numFittingCharacters];
            displayedText = ReplaceLastThreeWithDots(trimmedText);
        }
        else
        {
            displayedText = Text;
        }
    }

    private float GetCharacterWidth()
    {
        float width = Raylib.MeasureTextEx(
            Style.Current.Font,
            " ",
            Style.Current.FontSize,
            1).X;

        return width;
    }

    private static string ReplaceLastThreeWithDots(string input)
    {
        if (input.Length > 3)
        {
            string trimmedText = input[..^3];
            return trimmedText + "...";
        }
        else
        {
            return input;
        }
    }
}