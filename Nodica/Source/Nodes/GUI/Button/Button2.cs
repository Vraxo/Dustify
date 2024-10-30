//using Raylib_cs;
//
//namespace Nodica;
//
//public class OptionButtonButton : Control
//{
//    public enum ClickMode { Limited, Limitless }
//    public enum ActionMode { Release, Press }
//    public enum ClickBehavior { Left, Right, Both }
//
//    #region [ - - - Properties & Fields - - - ]
//
//    public Vector2 TextPadding { get; set; } = Vector2.Zero;
//    public Vector2 TextOrigin { get; set; } = Vector2.Zero;
//    public OriginPreset TextOriginPreset { get; set; } = OriginPreset.Center;
//    public ButtonThemePack Themes { get; set; } = new();
//    public float AvailableWidth { get; set; } = 0;
//    public ActionMode LeftClickActionMode { get; set; } = ActionMode.Release;
//    public ActionMode RightClickActionMode { get; set; } = ActionMode.Release;
//    public bool StayPressed { get; set; } = false;
//    public bool ClipText { get; set; } = false;
//    public bool AutoWidth { get; set; } = false;
//    public Vector2 TextMargin { get; set; } = new(10, 5);
//    public string Ellipsis { get; set; } = "...";
//    public ClickBehavior Behavior { get; set; } = ClickBehavior.Left;
//    public float IconMargin { get; set; } = 12;
//
//    public Texture2D Icon { get; set; } = Raylib.LoadTexture("");
//    public bool HasIcon = false;
//
//    public bool PressedLeft = false;
//    public bool PressedRight = false;
//
//    public Action<OptionButtonButton> OnUpdate = (button) => { };
//
//    public event EventHandler? LeftClicked;
//    public event EventHandler? RightClicked;
//
//    private bool _disabled = false;
//    public bool Disabled
//    {
//        get => _disabled;
//        set
//        {
//            _disabled = value;
//            Themes.Current = Themes.Disabled;
//        }
//    }
//
//    private string displayedText = "";
//
//    private string _text = "";
//    public string Text
//    {
//        get => _text;
//
//        set
//        {
//            _text = value;
//            displayedText = value;
//            if (AutoWidth)
//            {
//                ResizeToFitText();
//            }
//        }
//    }
//
//    private string _themeFile = "";
//    public string ThemeFile3
//    {
//        get => _themeFile;
//
//        set
//        {
//            _themeFile = value;
//            Themes = PropertyLoader.Load<ButtonThemePack>(value);
//        }
//    }
//
//    #endregion
//
//    public OptionButtonButton()
//    {
//        Size = new(100, 26);
//    }
//
//    public override void Update()
//    {
//        if (!Disabled)
//        {
//            OnUpdate(this);
//            ClipDisplayedText();
//            HandleClicks();
//            HandleKeyboardInput();
//        }
//
//        UpdateTextOrigin();
//        Draw();
//        base.Update();
//    }
//
//    private void HandleKeyboardInput()
//    {
//        if (Focused && Raylib.IsKeyPressed(KeyboardKey.Enter))
//        {
//            LeftClicked?.Invoke(this, EventArgs.Empty);
//            RightClicked?.Invoke(this, EventArgs.Empty);
//        }
//    }
//
//    private void HandleClicks()
//    {
//        if (Disabled) return;
//
//        bool mouseOver = IsMouseOver();
//        bool anyPressed = false;
//
//        if (Behavior == ClickBehavior.Left || Behavior == ClickBehavior.Both)
//        {
//            HandleClick(
//                ref PressedLeft,
//                MouseButton.Left,
//                LeftClickActionMode,
//                LeftClicked);
//
//            if (PressedLeft) anyPressed = true;
//        }
//
//        if (Behavior == ClickBehavior.Right || Behavior == ClickBehavior.Both)
//        {
//            HandleClick(
//                ref PressedRight,
//                MouseButton.Right,
//                RightClickActionMode,
//                RightClicked);
//
//            if (PressedRight) anyPressed = true;
//        }
//
//        if (StayPressed && (PressedLeft || PressedRight))
//        {
//            Themes.Current = Themes.Pressed;
//        }
//        else if (Focused)
//        {
//            if (mouseOver)
//            {
//                Themes.Current = anyPressed ? Themes.Pressed : Themes.Focused;
//            }
//            else
//            {
//                Themes.Current = Focused ? Themes.Focused : Themes.Normal;
//            }
//        }
//        else if (mouseOver)
//        {
//            Themes.Current = anyPressed ? Themes.Pressed : Themes.Hover;
//        }
//        else
//        {
//            Themes.Current = Themes.Normal;
//        }
//    }
//
//    private void HandleClick(ref bool pressed, MouseButton button, ActionMode actionMode, EventHandler? clickHandler)
//    {
//        if (Disabled) return;
//
//        bool mouseOver = IsMouseOver();
//
//        if (mouseOver)
//        {
//            if (Raylib.IsMouseButtonPressed(button))
//            {
//                pressed = true;
//                HandleClickFocus();
//
//                if (actionMode == ActionMode.Press)
//                {
//                    clickHandler?.Invoke(this, EventArgs.Empty);
//                }
//            }
//        }
//
//        if (Raylib.IsMouseButtonReleased(button))
//        {
//            if (mouseOver && pressed && actionMode == ActionMode.Release) // (mouseOver || StayPressed)
//            {
//                clickHandler?.Invoke(this, EventArgs.Empty);
//            }
//
//            pressed = false;
//        }
//    }
//
//    protected override void Draw()
//    {
//        DrawBox();
//        DrawIcon();
//        DrawText();
//    }
//
//    private void DrawBox()
//    {
//        DrawBorderedRectangle(
//            GlobalPosition - Origin,
//            Size,
//            Themes.Current);
//    }
//
//    private void DrawIcon()
//    {
//        var iconOrigin = new Vector2(Icon.Width, Icon.Height) / 2f;
//
//        Raylib.DrawTextureV(
//            Icon,
//            GlobalPosition - new Vector2(Origin.X - IconMargin, 0) - iconOrigin,
//            Color.White);
//    }
//
//    private void DrawText()
//    {
//        Raylib.DrawTextEx(
//            Themes.Current.Font,
//            displayedText,
//            GetTextPosition(),
//            Themes.Current.FontSize,
//            1,
//            Themes.Current.FontColor);
//    }
//
//    private Vector2 GetTextPosition()
//    {
//        Vector2 fontDimensions = Raylib.MeasureTextEx(
//            Themes.Current.Font,
//            Text,
//            Themes.Current.FontSize,
//            1);
//
//        Vector2 center = Size / 2;
//
//        Vector2 alignmentAdjustment = new(
//            TextOrigin.X < center.X ? 0 : TextOrigin.X > center.X ? -fontDimensions.X : -fontDimensions.X / 2,
//            TextOrigin.Y < center.Y ? 0 : TextOrigin.Y > center.Y ? -fontDimensions.Y : -fontDimensions.Y / 2
//        );
//
//        return GlobalPosition + TextOrigin + alignmentAdjustment - Origin + TextPadding;
//    }
//
//    private void UpdateTextOrigin()
//    {
//        if (TextOriginPreset == OriginPreset.None)
//        {
//            return;
//        }
//
//        TextOrigin = TextOriginPreset switch
//        {
//            OriginPreset.Center => Size / 2,
//            OriginPreset.CenterLeft => new(0, Size.Y / 2),
//            OriginPreset.CenterRight => new(Size.X, Size.Y / 2),
//            OriginPreset.TopLeft => new(0, 0),
//            OriginPreset.TopRight => new(Size.X, 0),
//            OriginPreset.TopCenter => new(Size.X / 2, 0),
//            OriginPreset.BottomLeft => new(0, Size.Y),
//            OriginPreset.BottomRight => Size,
//            OriginPreset.BottomCenter => new(Size.X / 2, Size.Y),
//            OriginPreset.None => Origin,
//            _ => Origin,
//        };
//    }
//
//    private void ResizeToFitText()
//    {
//        if (!AutoWidth)
//        {
//            return;
//        }
//
//        int textWidth = (int)Raylib.MeasureTextEx(
//            Themes.Current.Font,
//            Text,
//            Themes.Current.FontSize,
//            1).X;
//
//        Size = new(textWidth + TextPadding.X * 2 + TextMargin.X, Size.Y + TextMargin.Y);
//    }
//
//    private void ClipDisplayedText()
//    {
//        if (!ClipText) return;
//
//        float characterWidth = GetCharacterWidth();
//        int numFittingCharacters = (int)(AvailableWidth / characterWidth);
//
//        if (numFittingCharacters <= 0)
//        {
//            displayedText = "";
//        }
//        else if (numFittingCharacters < Text.Length)
//        {
//            string trimmedText = Text[..numFittingCharacters];
//            displayedText = ClipTextWithEllipsis(trimmedText);
//        }
//        else
//        {
//            displayedText = Text;
//        }
//    }
//
//    private float GetCharacterWidth()
//    {
//        return Raylib.MeasureTextEx(
//            Themes.Current.Font,
//            " ",
//            Themes.Current.FontSize,
//            1).X;
//    }
//
//    private string ClipTextWithEllipsis(string input)
//    {
//        return input.Length > 3 ?
//               input[..^Ellipsis.Length] + Ellipsis :
//               input;
//    }
//}
