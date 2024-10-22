using Raylib_cs;

namespace Nodica;

public class Button : ClickableRectangle
{
    public enum ClickMode { Limited, Limitless }
    public enum ActionMode { Release, Press }

    #region [ - - - Properties & Fields - - - ]

    public Vector2 TextPadding { get; set; } = Vector2.Zero;
    public Vector2 TextOrigin { get; set; } = Vector2.Zero;
    public OriginPreset TextOriginPreset { get; set; } = OriginPreset.Center;
    public ButtonThemePack Styles { get; set; } = new();
    public float AvailableWidth { get; set; } = 0;
    public ActionMode LeftClickActionMode { get; set; } = ActionMode.Release;
    public ActionMode RightClickActionMode { get; set; } = ActionMode.Release;
    public bool StayPressed { get; set; } = false;
    public bool ClipText { get; set; } = false;
    public bool ExpandWidthToText { get; set; } = true;
    public Vector2 TextMargin { get; set; } = new(10, 5);
    public string Ellipsis { get; set; } = "...";
    public bool FocusOnClick { get; set; } = true;

    public bool PressedLeft = false;
    public bool PressedRight = false;

    public Action<Button> OnUpdate = (button) => { };

    public event EventHandler? LeftClicked;
    public event EventHandler? RightClicked;

    private bool _focused = false;
    public bool Focused
    {
        get => _focused;
        set
        {
            if (_focused != value)
            {
                _focused = value;
                Styles.Current = _focused ? Styles.Focused : Styles.Normal;
            }
        }
    }

    private bool _disabled = false;
    public bool Disabled
    {
        get => _disabled;

        set
        {
            _disabled = value;
            Styles.Current = Styles.Disabled;
        }
    }

    private string displayedText = "";

    private string _text = "";
    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            displayedText = value;
            if (ExpandWidthToText)
            {
                UpdateSizeToFitText();
            }
        }
    }

    private string _themeFile = "";
    public string ThemeFile
    {
        get => _themeFile;

        set
        {
            _themeFile = value;
            Styles = StyleLoader.LoadStyle<ButtonThemePack>("Resources/ButtonTheme.txt");
        }
    }

    #endregion

    public Button()
    {
        Size = new(100, 26);
    }

    public override void Update()
    {
        if (!Disabled)
        {
            OnUpdate(this);
            ClipDisplayedText();
            HandleClicks();
            HandleKeyboardInput();
        }
        
        UpdateFocusOnMouseOut();
        UpdateTextOrigin();
        Draw();
        base.Update();
    }

    private void UpdateFocusOnMouseOut()
    {
        if (!IsMouseOver() && Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            Focused = false;
        }
    }

    private void HandleKeyboardInput()
    {
        if (Focused && Raylib.IsKeyPressed(KeyboardKey.Enter))
        {
            LeftClicked?.Invoke(this, EventArgs.Empty);
            RightClicked?.Invoke(this, EventArgs.Empty);
        }
    }

    // Click handling
    private void HandleClicks()
    {
        if (Disabled) return;

        HandleClick(
            ref PressedLeft,
            MouseButton.Left,
            LeftClickActionMode,
            LeftClicked);

        HandleClick(
            ref PressedRight,
            MouseButton.Right,
            RightClickActionMode,
            RightClicked);
    }

    private void HandleClick(ref bool pressed, MouseButton button, ActionMode actionMode, EventHandler? clickHandler)
    {
        if (clickHandler is null || Disabled) return;

        bool mouseOver = IsMouseOver();

        if (mouseOver)
        {
            Styles.Current = pressed ? Styles.Pressed : (Focused ? Styles.Focused : Styles.Hover);

            if (Raylib.IsMouseButtonPressed(button) && OnTopLeft)
            {
                pressed = true;

                // Set focus based on FocusOnClick
                if (FocusOnClick)
                {
                    Focused = true;
                }

                if (actionMode == ActionMode.Press)
                {
                    clickHandler?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        else if (!pressed || !StayPressed)
        {
            Styles.Current = Focused ? Styles.Focused : Styles.Normal;
        }

        if (Raylib.IsMouseButtonReleased(button))
        {
            if (mouseOver && pressed && actionMode == ActionMode.Release)
            {
                clickHandler?.Invoke(this, EventArgs.Empty);
            }

            pressed = false;
        }
    }

    // Drawing
    protected override void Draw()
    {
        DrawBorderedRectangle(
            GlobalPosition - Origin,
            Size,
            Disabled ? Styles.Disabled : Styles.Current);

        DrawText();
    }

    private void DrawText()
    {
        Raylib.DrawTextEx(
            Styles.Current.Font,
            displayedText,
            GetTextPosition(),
            Styles.Current.FontSize,
            1,
            Styles.Current.FontColor);
    }

    // Text positioning
    private Vector2 GetTextPosition()
    {
        Vector2 fontDimensions = Raylib.MeasureTextEx(
            Styles.Current.Font,
            Text,
            Styles.Current.FontSize,
            1);

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

    // Text resizing
    private void UpdateSizeToFitText()
    {
        int textWidth = (int)Raylib.MeasureTextEx(
            Styles.Current.Font,
            Text,
            Styles.Current.FontSize,
            1).X;

        Size = new(textWidth + TextPadding.X * 2 + TextMargin.X, Size.Y + TextMargin.Y);
    }

    // Displayed text truncating
    private void ClipDisplayedText()
    {
        if (!ClipText) return;

        float characterWidth = GetCharacterWidth();
        int numFittingCharacters = (int)(AvailableWidth / characterWidth);

        if (numFittingCharacters <= 0)
        {
            displayedText = "";
        }
        else if (numFittingCharacters < Text.Length)
        {
            string trimmedText = Text[..numFittingCharacters];
            displayedText = ClipTextWithEllipsis(trimmedText);
        }
        else
        {
            displayedText = Text;
        }
    }

    private float GetCharacterWidth()
    {
        return Raylib.MeasureTextEx(
            Styles.Current.Font,
            " ",
            Styles.Current.FontSize,
            1).X;
    }

    private string ClipTextWithEllipsis(string input)
    {
        return input.Length > 3 ?
               input[..^Ellipsis.Length] + Ellipsis :
               input;
    }
}