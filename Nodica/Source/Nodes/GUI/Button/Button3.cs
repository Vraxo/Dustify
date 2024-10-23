using Raylib_cs;

namespace Nodica;

public class Button : Control
{
    public enum ClickMode { Limited, Limitless }
    public enum ActionMode { Release, Press }
    public enum ButtonState { Normal, Hover, Pressed, Focused }
    public enum ClickBehavior { Left, Right, Both }

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
    public bool AutoWidth { get; set; } = true;
    public Vector2 TextMargin { get; set; } = new(10, 5);
    public string Ellipsis { get; set; } = "...";
    public ButtonState State { get; private set; } = ButtonState.Normal;
    public ClickBehavior Behavior { get; set; } = ClickBehavior.Both;

    public bool PressedLeft = false;
    public bool PressedRight = false;

    public Action<Button> OnUpdate = (button) => { };

    public event EventHandler? LeftClicked;
    public event EventHandler? RightClicked;

    private bool _disabled = false;
    public bool Disabled
    {
        get => _disabled;
        set
        {
            _disabled = value;
            UpdateStyle();
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
            if (AutoWidth)
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

    // Public state property with a private setter

    // New property to set click behavior

    #endregion

    public Button()
    {
        Size = new(100, 26);
        FocusChanged += OnFocusChanged;
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

        UpdateTextOrigin();
        Draw();
        base.Update();
    }

    private void OnFocusChanged(bool focused)
    {
        State = focused ? ButtonState.Focused : ButtonState.Normal;
        UpdateStyle();
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

        // Check if the button is currently being pressed
        bool mouseOver = IsMouseOver();
        bool anyPressed = false;

        if (Behavior == ClickBehavior.Left || Behavior == ClickBehavior.Both)
        {
            HandleClick(
                ref PressedLeft,
                MouseButton.Left,
                LeftClickActionMode,
                LeftClicked);

            // Update anyPressed if left button is pressed
            if (PressedLeft) anyPressed = true;
        }

        if (Behavior == ClickBehavior.Right || Behavior == ClickBehavior.Both)
        {
            HandleClick(
                ref PressedRight,
                MouseButton.Right,
                RightClickActionMode,
                RightClicked);

            // Update anyPressed if right button is pressed
            if (PressedRight) anyPressed = true;
        }

        // Update state based on whether any button is pressed
        if (mouseOver)
        {
            State = anyPressed ? ButtonState.Pressed : ButtonState.Hover;
        }
        else
        {
            State = Focused ? ButtonState.Focused : ButtonState.Normal;
        }

        UpdateStyle();
    }

    private void HandleClick(ref bool pressed, MouseButton button, ActionMode actionMode, EventHandler? clickHandler)
    {
        bool mouseOver = IsMouseOver();

        if (mouseOver)
        {
            State = pressed ? ButtonState.Pressed : ButtonState.Hover;
            UpdateStyle();

            if (Raylib.IsMouseButtonPressed(button))
            {
                pressed = true;
                HandleClickFocus();

                if (actionMode == ActionMode.Press)
                {
                    clickHandler?.Invoke(this, EventArgs.Empty);
                    Styles.Current = Styles.Pressed;
                }
            }
        }
        else if (!pressed || !StayPressed)
        {
            State = Focused ? ButtonState.Focused : ButtonState.Normal;
            UpdateStyle();
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

    // Update the style based on the current state
    private void UpdateStyle()
    {
        if (Disabled)
        {
            Styles.Current = Styles.Disabled;
        }
        else
        {
            Styles.Current = State switch
            {
                ButtonState.Pressed => Styles.Pressed,
                ButtonState.Hover => Styles.Hover,
                ButtonState.Focused => Styles.Focused,
                _ => Styles.Normal
            };
        }
    }

    // Drawing
    protected override void Draw()
    {
        DrawBorderedRectangle(
            GlobalPosition - Origin,
            Size,
            Styles.Current);

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
