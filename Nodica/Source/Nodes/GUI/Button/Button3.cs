using Raylib_cs;
using System.Security.AccessControl;

namespace Nodica;

public class Button : Control
{
    public enum ClickMode { Limited, Limitless }
    public enum ActionMode { Release, Press }
    public enum ClickBehavior { Left, Right, Both }

    #region [ - - - Properties & Fields - - - ]

    public Vector2 TextPadding { get; set; } = Vector2.Zero;
    public Vector2 TextOrigin { get; set; } = Vector2.Zero;

    public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Center;
    public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Center;

    public OriginPreset TextOriginPreset { get; set; } = OriginPreset.Center;
    public ButtonThemePack Styles { get; set; } = new();
    public float AvailableWidth { get; set; } = 0;
    public ActionMode LeftClickActionMode { get; set; } = ActionMode.Release;
    public ActionMode RightClickActionMode { get; set; } = ActionMode.Release;
    public bool StayPressed { get; set; } = false;
    public bool ClipText { get; set; } = false;
    public bool AutoWidth { get; set; } = false;
    public Vector2 TextMargin { get; set; } = new(10, 5);
    public string Ellipsis { get; set; } = "...";
    public ClickBehavior Behavior { get; set; } = ClickBehavior.Left;
    public float IconMargin { get; set; } = 12;

    public Texture2D Icon { get; set; }
    public bool HasIcon = false;

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
            if (AutoWidth)
            {
                ResizeToFitText();
            }
        }
    }

    private string _themeFile = "";
    public string ThemeFile3
    {
        get => _themeFile;

        set
        {
            _themeFile = value;
            Styles = PropertyLoader.Load<ButtonThemePack>(value);
        }
    }

    public ButtonThemePack ThemeFile
    {
        set
        {
            Styles = value;
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

        base.Update();
    }

    private void HandleKeyboardInput()
    {
        if (Focused && Raylib.IsKeyPressed(KeyboardKey.Enter))
        {
            LeftClicked?.Invoke(this, EventArgs.Empty);
            RightClicked?.Invoke(this, EventArgs.Empty);
        }
    }

    private void HandleClicks()
    {
        if (Disabled) return;

        bool mouseOver = IsMouseOver();
        bool anyPressed = false;

        if (Behavior == ClickBehavior.Left || Behavior == ClickBehavior.Both)
        {
            HandleClick(
                ref PressedLeft,
                ref OnTopLeft,
                MouseButton.Left,
                LeftClickActionMode,
                LeftClicked);

            if (PressedLeft) anyPressed = true;
        }

        if (Behavior == ClickBehavior.Right || Behavior == ClickBehavior.Both)
        {
            HandleClick(
                ref PressedRight,
                ref OnTopRight,
                MouseButton.Right,
                RightClickActionMode,
                RightClicked);

            if (PressedRight) anyPressed = true;
        }

        if (StayPressed && (PressedLeft || PressedRight))
        {
            Styles.Current = Styles.Pressed;
        }
        else if (Focused)
        {
            if (mouseOver)
            {
                Styles.Current = anyPressed ? Styles.Pressed : Styles.Focused;
            }
            else
            {
                Styles.Current = Focused ? Styles.Focused : Styles.Normal;
            }
        }
        else if (mouseOver)
        {
            Styles.Current = anyPressed ? Styles.Pressed : Styles.Hover;
        }
        else
        {
            Styles.Current = Styles.Normal;
        }
    }

    private void HandleClick(ref bool pressed, ref bool onTop, MouseButton button, ActionMode actionMode, EventHandler? clickHandler)
    {
        if (Disabled) return;

        bool mouseOver = IsMouseOver();

        if (mouseOver)
        {
            if (Raylib.IsMouseButtonPressed(button) && onTop)
            {
                pressed = true;
                HandleClickFocus();

                if (actionMode == ActionMode.Press)
                {
                    clickHandler?.Invoke(this, EventArgs.Empty);
                    onTop = false;
                }
            }
        }

        if (Raylib.IsMouseButtonReleased(button))
        {
            if (mouseOver && pressed && onTop && actionMode == ActionMode.Release) // (mouseOver || StayPressed)
            {
                clickHandler?.Invoke(this, EventArgs.Empty);
            }

            onTop = false;
            pressed = false;
        }
    }

    protected override void Draw()
    {
        DrawBox();
        DrawIcon();
        DrawText();
    }

    private void DrawBox()
    {
        DrawBorderedRectangle(
            GlobalPosition - Origin,
            Size,
            Styles.Current);
    }

    private void DrawIcon()
    {
        var iconOrigin = new Vector2(Icon.Width, Icon.Height) / 2f;

        Raylib.DrawTextureV(
            Icon,
            GlobalPosition - new Vector2(Origin.X - IconMargin, 0) - iconOrigin,
            Color.White);
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

    private Vector2 GetTextPosition()
    {
        Vector2 textSize = Raylib.MeasureTextEx(
            Styles.Current.Font,
            Text,
            Styles.Current.FontSize,
            1);

        float x = HorizontalAlignment switch
        {
            HorizontalAlignment.Center => Size.X / 2,
            HorizontalAlignment.Right => Size.X - textSize.X / 2
        };

        float y = VerticalAlignment switch
        {
            VerticalAlignment.Center => Size.Y / 2
        };

        Vector2 origin = new(x, y);

        return GlobalPosition - Origin + origin - textSize / 2 + TextOrigin;
    }

    private void ResizeToFitText()
    {
        if (!AutoWidth)
        {
            return;
        }

        int textWidth = (int)Raylib.MeasureTextEx(
            Styles.Current.Font,
            Text,
            Styles.Current.FontSize,
            1).X;

        Size = new(textWidth + TextPadding.X * 2 + TextMargin.X, Size.Y + TextMargin.Y);
    }

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
