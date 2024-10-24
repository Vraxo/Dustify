//using Raylib_cs;
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
//    #region [ - - - Properties & Fields - - - ]
//
//    public Vector2 TextPadding { get; set; } = Vector2.Zero;
//    public Vector2 TextOrigin { get; set; } = Vector2.Zero;
//    public OriginPreset TextOriginPreset { get; set; } = OriginPreset.Center;
//    public ButtonThemePack BackgroundStyles { get; set; } = new();
//    public bool PressedLeft { get; set; } = false;
//    public bool PressedRight { get; set; } = false;
//    public bool ClipText { get; set; } = false;
//    public float AvailableWidth { get; set; } = 0;
//    public ClickMode _LeftClickMode { get; set; } = ClickMode.Limitless;
//    public ClickMode _RightClickMode { get; set; } = ClickMode.Limitless;
//
//    public Action<Button> OnUpdate = (button) => { };
//
//    public event EventHandler? LeftClicked;
//    public event EventHandler? RightClicked;
//
//    private bool alreadyClicked = false;
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
//        HandleLeftClicks();
//        HandleRightClicks();
//    }
//
//    // Left click handling
//
//    private void HandleLeftClicks()
//    {
//        if (LeftClicked is null)
//        {
//            return;
//        }
//
//        if (_LeftClickMode == ClickMode.Limitless)
//        {
//            HandleLeftClickLimitless();
//        }
//        else
//        {
//            HandleLeftClickLimited();
//        }
//    }
//
//    private void HandleLeftClickLimitless()
//    {
//        if (IsMouseOver())
//        {
//            if (Raylib.IsMouseButtonReleased(MouseButton.Left))
//            {
//                if (PressedLeft)
//                {
//                    PressedLeft = false;
//                    LeftClicked.Invoke(this, EventArgs.Empty);
//                }
//            }
//        }
//
//        if (Raylib.IsMouseButtonDown(MouseButton.Left))
//        {
//            if (!IsMouseOver())
//            {
//                alreadyClicked = true;
//            }
//        }
//
//        if (IsMouseOver())
//        {
//            BackgroundStyles.Current = BackgroundStyles.Hover;
//            
//            if (Raylib.IsMouseButtonDown(MouseButton.Left))
//            {
//                if (!alreadyClicked && OnTopLeft)
//                {
//                    OnTopLeft = false;
//                    PressedLeft = true;
//                    alreadyClicked = true;
//                }
//
//                if (PressedLeft)
//                {
//                    BackgroundStyles.Current = BackgroundStyles.Pressed;
//                }
//            }
//        }
//        else
//        {
//            BackgroundStyles.Current = BackgroundStyles.Normal;
//        }
//
//        if (Raylib.IsMouseButtonReleased(MouseButton.Left))
//        {
//            if (IsMouseOver() && PressedLeft)
//            {
//                LeftClicked?.Invoke(this, EventArgs.Empty);
//            }
//
//            PressedLeft = false;
//            alreadyClicked = false;
//            BackgroundStyles.Current = BackgroundStyles.Normal;
//        }
//    }
//
//    private void HandleLeftClickLimited()
//    {
//        if (IsMouseOver())
//        {
//            BackgroundStyles.Current = BackgroundStyles.Hover;
//
//            if (Raylib.IsMouseButtonPressed(MouseButton.Left) && OnTopLeft)
//            {
//                PressedLeft = true;
//                OnTopLeft = false;
//            }
//
//            if (PressedLeft)
//            {
//                BackgroundStyles.Current = BackgroundStyles.Pressed;
//            }
//        }
//        else
//        {
//            PressedLeft = false;
//            BackgroundStyles.Current = BackgroundStyles.Normal;
//        }
//
//        if (Raylib.IsMouseButtonReleased(MouseButton.Left))
//        {
//            if (IsMouseOver() && PressedLeft)
//            {
//                LeftClicked?.Invoke(this, EventArgs.Empty);
//            }
//
//            PressedLeft = false;
//            BackgroundStyles.Current = BackgroundStyles.Normal;
//        }
//    }
//
//    // Right click handling
//
//    private void HandleRightClicks()
//    {
//        if (RightClicked is null)
//        {
//            return;
//        }
//
//        if (_RightClickMode == ClickMode.Limitless)
//        {
//            HandleRightClickLimitless();
//        }
//        else
//        {
//            HandleRightClickLimited();
//        }
//    }
//
//    private void HandleRightClickLimitless()
//    {
//        if (Raylib.IsMouseButtonDown(MouseButton.Right))
//        {
//            if (!IsMouseOver())
//            {
//                alreadyClicked = true;
//            }
//        }
//
//        if (IsMouseOver())
//        {
//            if (!PressedLeft)
//            {
//                BackgroundStyles.Current = BackgroundStyles.Hover;
//            }
//
//            if (Raylib.IsMouseButtonDown(MouseButton.Right))
//            {
//                if (!alreadyClicked && OnTopRight)
//                {
//                    OnTopRight = false;
//                    PressedRight = true;
//                    alreadyClicked = true;
//                }
//
//                if (PressedRight)
//                {
//                    BackgroundStyles.Current = BackgroundStyles.Pressed;
//                }
//            }
//        }
//        else
//        {
//            BackgroundStyles.Current = BackgroundStyles.Normal;
//        }
//
//        if (Raylib.IsMouseButtonReleased(MouseButton.Right))
//        {
//            if (IsMouseOver() && PressedRight)
//            {
//                RightClicked?.Invoke(this, EventArgs.Empty);
//            }
//
//            PressedRight = false;
//            alreadyClicked = false;
//            BackgroundStyles.Current = BackgroundStyles.Normal;
//        }
//    }
//
//    private void HandleRightClickLimited()
//    {
//        if (IsMouseOver())
//        {
//            BackgroundStyles.Current = BackgroundStyles.Hover;
//
//            if (Raylib.IsMouseButtonPressed(MouseButton.Right) && OnTopRight)
//            {
//                PressedRight = true;
//                OnTopRight = false;
//            }
//
//            if (PressedRight)
//            {
//                BackgroundStyles.Current = BackgroundStyles.Pressed;
//            }
//        }
//        else
//        {
//            PressedRight = false;
//            BackgroundStyles.Current = BackgroundStyles.Normal;
//        }
//
//        if (Raylib.IsMouseButtonReleased(MouseButton.Right))
//        {
//            if (IsMouseOver() && PressedRight)
//            {
//                RightClicked?.Invoke(this, EventArgs.Empty);
//            }
//
//            PressedRight = false;
//            BackgroundStyles.Current = BackgroundStyles.Normal;
//        }
//    }
//
//    // Drawing
//
//    private void Draw()
//    {
//        if (!(Visible && ReadyForVisibility))
//        {
//            return;
//        }
//
//        DrawShape();
//        DrawText();
//    }
//
//    private void DrawShape()
//    {
//        DrawShapeOutline();
//        DrawShapeInside();
//    }
//
//    private void DrawShapeInside()
//    {
//        Rectangle rectangle = new()
//        {
//            Position = GlobalPosition - Origin * Scale,
//            Size = Size * Scale
//        };
//
//        Raylib.DrawRectangleRounded(
//            rectangle,
//            BackgroundStyles.Current.Roundness,
//            (int)Size.Y,
//            BackgroundStyles.Current.FillColor);
//    }
//
//    private void DrawShapeOutline()
//    {
//        if (BackgroundStyles.Current.BorderLength <= 0)
//        {
//            return;
//        }
//
//        Rectangle rectangle = new()
//        {
//            Position = GlobalPosition - Origin * Scale,
//            Size = Size * Scale
//        };
//
//        for (int i = 0; i <= BackgroundStyles.Current.BorderLength; i++)
//        {
//            Rectangle outlineRectangle = new()
//            {
//                Position = rectangle.Position - new Vector2(i, i),
//                Size = new(rectangle.Size.X + i + 1, rectangle.Size.Y + i + 1)
//            };
//
//            Raylib.DrawRectangleRounded(
//                outlineRectangle,
//                BackgroundStyles.Current.Roundness,
//                (int)rectangle.Size.X,
//                BackgroundStyles.Current.BorderColor);
//        }
//    }
//
//    private void DrawText()
//    {
//        Raylib.DrawTextEx(
//            BackgroundStyles.Current.Font,
//            displayedText,
//            GetTextPosition(),
//            BackgroundStyles.Current.FontSize,
//            1,
//            BackgroundStyles.Current.FontColor);
//    }
//
//    // Text positioning
//
//    private Vector2 GetTextPosition()
//    {
//        // Measure the dimensions of the TextDisplayer
//        Vector2 fontDimensions = Raylib.MeasureTextEx(
//            BackgroundStyles.Current.Font,
//            Text,
//            BackgroundStyles.Current.FontSize,
//            1
//        );
//
//        // Evaluate the center of the Button
//        Vector2 center = Size / 2;
//
//        // Determine the alignment adjustment based on the TextOrigin
//        Vector2 alignmentAdjustment = new(
//            TextOrigin.X < center.X ? 0 : TextOrigin.X > center.X ? -fontDimensions.X : -fontDimensions.X / 2,
//            TextOrigin.Y < center.Y ? 0 : TextOrigin.Y > center.Y ? -fontDimensions.Y : -fontDimensions.Y / 2
//        );
//
//        // Evaluate the TextDisplayer position based on the alignment and origin
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
//            BackgroundStyles.Current.Font,
//            " ",
//            BackgroundStyles.Current.FontSize,
//            1).X;
//
//        return width;
//    }
//
//    private static string ClipTextWithEllipsis(string input)
//    {
//        if (input.Length > 3)
//        {
//            string trimmedText = input[..^3];
//            return trimmedText + "...";
//        }
//        else
//        {
//            return input;
//        }
//    }
//}