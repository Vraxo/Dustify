//using Raylib_cs;
//using System.Linq.Expressions;
//
//namespace Nodica;
//
//public class Button : ClickableRectangle
//{
//    public enum ClickMode
//    {
//        Limited,
//        Limitless
//    }
//
//    public enum ActionMode
//    {
//        Release,
//        Press
//    }
//
//    #region [ - - - Properties & Fields - - - ]
//
//    public Vector2         TextPadding          { get; set; } = Vector2.Zero;
//    public Vector2         TextOrigin           { get; set; } = Vector2.Zero;
//    public OriginPreset    TextOriginPreset     { get; set; } = OriginPreset.Center;
//    public ButtonThemePack Styles               { get; set; } = new();
//    public float           AvailableWidth       { get; set; } = 0;
//    public ClickMode       _LeftClickMode       { get; set; } = ClickMode.Limitless;
//    public ClickMode       _RightClickMode      { get; set; } = ClickMode.Limitless;
//    public ActionMode      LeftClickActionMode  { get; set; } = ActionMode.Release;
//    public ActionMode      RightClickActionMode { get; set; } = ActionMode.Release;
//    public bool            ClipText { get; set; } = false;
//    public string          Ellipsis { get; set; } = "...";
//
//    public bool PressedLeft  = false;
//    public bool PressedRight = false;
//
//    public Action<Button> OnUpdate = (button) => { };
//
//    public event EventHandler? LeftClicked;
//    public event EventHandler? RightClicked;
//
//    private bool   alreadyClicked = false;
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
//        }
//    }
//
//    #endregion
//
//    // Public
//
//    public Button()
//    {
//        Size = new(100, 26);
//    }
//
//    public override void Update()
//    {
//        OnUpdate(this);
//        ClipDisplayedText();
//        UpdateTextOrigin();
//        HandleClick();
//        Draw();
//        base.Update();
//    }
//
//    // Click handling
//
//    private void HandleClick()
//    {
//        HandleClick(
//            ref PressedLeft, 
//            MouseButton.Left, 
//            _LeftClickMode, 
//            OnTopLeft, 
//            LeftClicked, 
//            ref OnTopLeft);
//
//        HandleClick(
//            ref PressedRight, 
//            MouseButton.Right, 
//            _RightClickMode, 
//            OnTopRight, 
//            RightClicked, 
//            ref OnTopRight);
//    }
//
//    // Generic click handling (used for both left and right)
//
//    private void HandleClick(ref bool pressed, MouseButton button, ClickMode mode, bool onTop, EventHandler? clickedEvent, ref bool onTopFlag)
//    {
//        if (clickedEvent is null)
//        {
//            return;
//        }
//
//        if (mode == ClickMode.Limitless)
//        {
//            HandleClickLimitless(ref pressed, button, onTop, clickedEvent, ref onTopFlag);
//        }
//        else
//        {
//            HandleClickLimited(ref pressed, button, onTop, clickedEvent, ref onTopFlag);
//        }
//    }
//
//    private void HandleClickLimitless(ref bool pressed, MouseButton button, bool onTop, EventHandler? clickedEvent, ref bool onTopFlag)
//    {
//        if (Raylib.IsMouseButtonDown(button))
//        {
//            if (!IsMouseOver())
//            {
//                alreadyClicked = true;
//            }
//        }
//
//        if (IsMouseOver())
//        {
//            Styles.Current = Styles.Hover;
//
//            if (Raylib.IsMouseButtonReleased(button))
//            {
//                if (pressed)
//                {
//                    pressed = false;
//                    clickedEvent?.Invoke(this, EventArgs.Empty);
//                }
//            }
//
//            if (Raylib.IsMouseButtonDown(button))
//            {
//                if (!alreadyClicked && onTop)
//                {
//                    onTopFlag = false;
//                    pressed = true;
//                    alreadyClicked = true;
//                }
//
//                if (pressed)
//                {
//                    Styles.Current = Styles.Pressed;
//                }
//            }
//        }
//        else
//        {
//            Styles.Current = Styles.Normal;
//        }
//
//        if (Raylib.IsMouseButtonReleased(button))
//        {
//            if (IsMouseOver() && pressed)
//            {
//                clickedEvent?.Invoke(this, EventArgs.Empty);
//            }
//
//            pressed = false;
//            alreadyClicked = false;
//            Styles.Current = Styles.Normal;
//        }
//    }
//
//    private void HandleClickLimited(ref bool pressed, MouseButton button, bool onTop, EventHandler? clickedEvent, ref bool onTopFlag)
//    {
//        if (IsMouseOver())
//        {
//            Styles.Current = Styles.Hover;
//
//            if (Raylib.IsMouseButtonPressed(button) && onTop)
//            {
//                pressed = true;
//                onTopFlag = false;
//            }
//
//            if (pressed)
//            {
//                Styles.Current = Styles.Pressed;
//            }
//        }
//        else
//        {
//            pressed = false;
//            Styles.Current = Styles.Normal;
//        }
//
//        if (Raylib.IsMouseButtonReleased(button))
//        {
//            if (IsMouseOver() && pressed)
//            {
//                clickedEvent?.Invoke(this, EventArgs.Empty);
//            }
//
//            pressed = false;
//            Styles.Current = Styles.Normal;
//        }
//    }
//
//    // Drawing
//
//    protected override void Draw()
//    {
//        DrawBorderedRectangle(GlobalPosition - Origin, Size, Styles.Current);
//        DrawText();
//    }
//
//    private void DrawText()
//    {
//        Raylib.DrawTextEx(
//            Styles.Current.Font,
//            displayedText,
//            GetTextPosition(),
//            Styles.Current.FontSize,
//            1,
//            Styles.Current.FontColor);
//    }
//
//    // Text positioning
//
//    private Vector2 GetTextPosition()
//    {
//        Vector2 fontDimensions = Raylib.MeasureTextEx(
//            Styles.Current.Font,
//            Text,
//            Styles.Current.FontSize,
//            1
//        );
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
//    // Displayed text truncating
//
//    private void ClipDisplayedText()
//    {
//        if (!ClipText)
//        {
//            return;
//        }
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
//        float width = Raylib.MeasureTextEx(
//            Styles.Current.Font,
//            " ",
//            Styles.Current.FontSize,
//            1).X;
//
//        return width;
//    }
//
//    private string ClipTextWithEllipsis(string input)
//    {
//        return input.Length > 3 ? 
//               input[..^Ellipsis.Length] + Ellipsis : 
//               input;
//    }
//}