using Raylib_cs;

namespace Nodica;

public partial class LineEdit : ClickableRectangle
{
    #region [ - - - Properties & Fields - - - ]

    public static readonly Vector2 DefaultSize = new(300, 25);

    public string Text { get; set; } = "";
    public string DefaultText { get; set; } = "";
    public string PlaceholderText { get; set; } = "";
    public Vector2 TextOrigin { get; set; } = new(8, 0);
    public int MaxCharacters { get; set; } = int.MaxValue;
    public int MinCharacters { get; set; } = 0;
    public List<char> ValidCharacters { get; set; } = [];
    public ButtonStylePack Style { get; set; } = new();
    public bool Selected { get; set; } = false;
    public bool Editable { get; set; } = true;
    public bool ExpandToText { get; set; } = false;
    public bool RevertToDefaultText { get; set; } = true;
    public bool TemporaryDefaultText { get; set; } = true;
    public bool Secret { get; set; } = false;
    public char SecretCharacter { get; set; } = '*';

    public int TextStartIndex = 0;

    public Action<LineEdit> OnUpdate = (textBox) => { };

    public event EventHandler? FirstCharacterEntered;
    public event EventHandler? Cleared;
    public event EventHandler<string>? TextChanged;
    public event EventHandler<string>? Confirmed;

    protected Caret caret;
    private Shape shape;
    private TextDisplayer textDisplayer;
    private PlaceholderTextDisplayer placeholderTextDisplayer;

    private const int minAscii = 32;
    private const int maxAscii = 125;
    private const float backspaceDelay = 0.5f;
    private const float backspaceSpeed = 0.05f;

    private float backspaceTimer = 0f;
    private bool backspaceHeld = false;

    private float previousWidth = 0;

    #endregion

    public LineEdit()
    {
        Size = DefaultSize;
    }

    public override void Build()
    {
        //AddChild(new TextDisplayer());
        //AddChild(new PlaceholderTextDisplayer());
    }

    public override void Start()
    {
        shape = new(this);
        caret = new(this);
        textDisplayer = new(this);
        placeholderTextDisplayer = new(this);

        SizeChanged += OnSizeChanged;

        Style.Pressed.FillColor = ThemeLoader.Instance.Colors["TextBoxPressedFill"];
        Style.Pressed.BorderLength = 1;
        Style.Pressed.OutlineColor = ThemeLoader.Instance.Colors["Accent"];

        base.Start();
    }

    public override void Update()
    {
        OnUpdate(this);
        HandleInput();
        PasteText();
        UpdateSizeToFitText();

        shape.Update();
        caret.Update();
        textDisplayer.Update();
        placeholderTextDisplayer.Update();

        base.Update();
    }

    private void OnSizeChanged(object? sender, Vector2 e)
    {
        if (previousWidth != e.X)
        {
            previousWidth = e.X;
            TextStartIndex = 0;
        }
    }

    private void UpdateSizeToFitText()
    {
        if (!ExpandToText)
        {
            return;
        }

        int textWidth = (int)Raylib.MeasureTextEx(
            Style.Current.Font,
            Text,
            Style.Current.FontSize,
            Style.Current.FontSpacing).X;

        Size = new Vector2(textWidth + TextOrigin.X * 2, Size.Y);
    }

    public void Insert(string input)
    {
        if (!Editable)
        {
            return;
        }

        InsertTextAtCaret(input);
    }

    private void HandleInput()
    {
        if (!Editable)
        {
            return;
        }

        HandleClicks();

        if (!Selected)
        {
            return;
        }

        GetTypedCharacters();
        HandleBackspace();
        Confirm();
    }

    private void HandleClicks()
    {
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            if (!IsMouseOver())
            {
                Selected = false;
                Style.Current = Style.Normal;
            }
        }

        if (IsMouseOver())
        {
            if (Raylib.IsMouseButtonDown(MouseButton.Left))
            {
                if (OnTopLeft)
                {
                    Selected = true;
                    Style.Current = Style.Pressed;
                }
            }
        }
        else
        {
            if (Raylib.IsMouseButtonDown(MouseButton.Left))
            {
                Selected = false;
                Style.Current = Style.Normal;
            }
        }
    }

    private void GetTypedCharacters()
    {
        int key = Raylib.GetCharPressed();

        while (key > 0)
        {
            InsertCharacter(key);
            key = Raylib.GetCharPressed();
        }
    }

    private void InsertCharacter(int key)
    {
        bool isKeyInRange = key >= minAscii && key <= maxAscii;
        bool isSpaceLeft = Text.Length < MaxCharacters;

        if (isKeyInRange && isSpaceLeft)
        {
            if (ValidCharacters.Count > 0 && !ValidCharacters.Contains((char)key))
            {
                return;
            }

            if (TemporaryDefaultText && Text == DefaultText)
            {
                Text = "";
            }

            if (caret.X < 0 || caret.X > Text.Length)
            {
                caret.X = Text.Length;
            }

            Text = Text.Insert(caret.X + TextStartIndex, ((char)key).ToString());

            // Check if caret is out of view, and adjust TextStartIndex
            if (caret.X >= GetDisplayableCharactersCount())
            {
                TextStartIndex++;
            }
            else
            {
                caret.X++;
            }

            TextChanged?.Invoke(this, Text);

            if (Text.Length == 1)
            {
                FirstCharacterEntered?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void InsertTextAtCaret(string text)
    {
        bool isSpaceLeft = Text.Length + text.Length <= MaxCharacters;

        if (isSpaceLeft)
        {
            if (TemporaryDefaultText && Text == DefaultText)
            {
                Text = "";
            }

            if (caret.X < 0 || caret.X > Text.Length)
            {
                caret.X = Text.Length;
            }

            Text = Text.Insert(caret.X + TextStartIndex, text);
            caret.X += text.Length;

            // Shift text if caret moves out of view
            if (caret.X > GetDisplayableCharactersCount())
            {
                TextStartIndex = caret.X - GetDisplayableCharactersCount();
            }

            TextChanged?.Invoke(this, Text);

            if (Text.Length == text.Length)
            {
                FirstCharacterEntered?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void HandleBackspace()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Backspace))
        {
            backspaceHeld = true;
            backspaceTimer = 0f;
            DeleteLastCharacter();
        }
        else if (Raylib.IsKeyDown(KeyboardKey.Backspace) && backspaceHeld)
        {
            backspaceTimer += Raylib.GetFrameTime();

            if (backspaceTimer >= backspaceDelay)
            {
                if (backspaceTimer % backspaceSpeed < Raylib.GetFrameTime())
                {
                    DeleteLastCharacter();
                }
            }
        }

        if (Raylib.IsKeyReleased(KeyboardKey.Backspace))
        {
            backspaceHeld = false;
        }
    }

    private void DeleteLastCharacter()
    {
        int textLengthBeforeDeletion = Text.Length;

        if (Text.Length > 0)
        {
            // Remove the character before the caret
            Text = Text.Remove(caret.X - 1 + TextStartIndex, 1);

            // Adjust TextStartIndex if necessary
            if (caret.X == GetDisplayableCharactersCount() && TextStartIndex > 0)
            {
                TextStartIndex--;
            }

            // Move caret left after deletion, but don't exceed bounds
            caret.X = Math.Clamp(caret.X - 1, 0, Math.Min(Text.Length, GetDisplayableCharactersCount()));
        }

        // Revert text to default if it's empty
        RevertTextToDefaultIfEmpty();

        // Fire the TextChanged event
        TextChanged?.Invoke(this, Text);

        // Fire Cleared event if the text was just cleared
        if (Text.Length == 0 && textLengthBeforeDeletion != 0)
        {
            Cleared?.Invoke(this, EventArgs.Empty);
        }
    }

    //private void DeleteLastCharacter()
    //{
    //    int textLengthBeforeDeletion = Text.Length;
    //
    //    if (Text.Length > 0 && caret.X > 0)
    //    {
    //        Text = Text.Remove(caret.X - 1 + TextStartIndex, 1);
    //
    //        if (Text.Length % GetDisplayableCharactersCount() < GetDisplayableCharactersCount())
    //        {
    //            if (TextStartIndex > 0)
    //            {
    //                TextStartIndex--;
    //            }
    //        }
    //
    //        //if (caret.X != GetDisplayableCharactersCount() || Text.Length < GetDisplayableCharactersCount())
    //        //{
    //        //    caret.X--;
    //        //}
    //
    //        if (caret.X != GetDisplayableCharactersCount() && TextStartIndex == 0)
    //        {
    //            caret.X--;
    //        }
    //    }
    //
    //    RevertTextToDefaultIfEmpty();
    //
    //    TextChanged?.Invoke(this, Text);
    //
    //    if (Text.Length == 0 && textLengthBeforeDeletion != 0)
    //    {
    //        Cleared?.Invoke(this, EventArgs.Empty);
    //    }
    //}

    private void PasteText()
    {
        bool pressedLeftControl = Raylib.IsKeyDown(KeyboardKey.LeftControl);
        bool pressedV = Raylib.IsKeyPressed(KeyboardKey.V);

        if (pressedLeftControl && pressedV)
        {
            char[] clipboardContent = [.. Raylib.GetClipboardText_()];

            foreach (char character in clipboardContent)
            {
                InsertCharacter(character);
            }
        }
    }

    private void Confirm()
    {
        if (Raylib.IsKeyDown(KeyboardKey.Enter))
        {
            Selected = false;
            Style.Current = Style.Normal;
            Confirmed?.Invoke(this, Text);
        }
    }

    private void RevertTextToDefaultIfEmpty()
    {
        if (Text.Length == 0)
        {
            Text = DefaultText;
        }
    }

    private int GetDisplayableCharactersCount()
    {
        float oneCharacterWidth = Raylib.MeasureTextEx(
            Style.Current.Font,
            ".",
            Style.Current.FontSize,
            Style.Current.FontSpacing).X;

        float avaiableWidth = Size.X * 0.85f;

        int displayableCharactersCount = (int)(avaiableWidth / oneCharacterWidth);

        return displayableCharactersCount;
    }
}