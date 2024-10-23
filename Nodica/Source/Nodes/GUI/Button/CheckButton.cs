using Raylib_cs;

namespace Nodica;

public class CheckButton : Control
{
    public enum ButtonState { Normal, Focused, Pressed, Disabled }

    public enum ActionMode { Release, Press }

    public enum ClickableButton { Left, Right, Both }

    #region [ - - - Properties & Fields - - - ]

    public ButtonThemePack Styles { get; set; } = new();
    public float AvailableWidth { get; set; } = 0;
    public ActionMode LeftClickActionMode { get; set; } = ActionMode.Release;
    public ActionMode RightClickActionMode { get; set; } = ActionMode.Release;
    public bool StayPressed { get; set; } = false;
    public bool Toggled { get; set; } = false;

    public ButtonState State { get; private set; } = ButtonState.Normal;

    public bool PressedLeft = false;
    public bool PressedRight = false;

    public event EventHandler? LeftClicked;
    public event EventHandler? RightClicked;

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

    private bool _disabled = false;
    public bool Disabled
    {
        get => _disabled;
        set
        {
            _disabled = value;
            State = value ? ButtonState.Disabled : ButtonState.Normal;
        }
    }

    public ClickableButton Clickable { get; set; } = ClickableButton.Left;

    #endregion

    public CheckButton()
    {
        Size = new(100, 26);
        FocusChanged += OnFocusChanged;
    }

    public override void Update()
    {
        if (!Disabled)
        {
            HandleClicks();
            HandleKeyboardInput();
        }

        UpdateCurrentStyle();
        Draw();
        base.Update();
    }

    public void Toggle()
    {
        Toggled = !Toggled;
        Console.WriteLine("Toggled: " + Toggled);
    }

    private void OnFocusChanged(bool focused)
    {
        State = focused ? ButtonState.Focused : ButtonState.Normal;
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
        if (Clickable == ClickableButton.Left || Clickable == ClickableButton.Both)
        {
            HandleClick(ref PressedLeft, MouseButton.Left, LeftClickActionMode, LeftClicked);
        }

        if (Clickable == ClickableButton.Right || Clickable == ClickableButton.Both)
        {
            HandleClick(ref PressedRight, MouseButton.Right, RightClickActionMode, RightClicked);
        }
    }

    private void HandleClick(ref bool pressed, MouseButton button, ActionMode actionMode, EventHandler? clickHandler)
    {
        bool mouseOver = IsMouseOver();

        if (mouseOver)
        {
            State = pressed ? ButtonState.Pressed : State;

            if (Raylib.IsMouseButtonPressed(button))
            {
                pressed = true;
                HandleClickFocus();

                if (actionMode == ActionMode.Press)
                {
                    clickHandler?.Invoke(this, EventArgs.Empty);
                    Toggle();
                }
            }
        }
        else if (!pressed || !StayPressed)
        {
            State = Focused ? ButtonState.Focused : ButtonState.Normal;
        }

        if (Raylib.IsMouseButtonReleased(button))
        {
            if (mouseOver && pressed && actionMode == ActionMode.Release)
            {
                clickHandler?.Invoke(this, EventArgs.Empty);
                Toggle();
            }

            pressed = false;
        }
    }

    // Update current style based on the state

    private void UpdateCurrentStyle()
    {
        Styles.Current = State switch
        {
            ButtonState.Normal => Styles.Normal,
            ButtonState.Focused => Styles.Focused,
            ButtonState.Pressed => Styles.Pressed,
            ButtonState.Disabled => Styles.Disabled,
            _ => Styles.Normal
        };
    }

    // Drawing

    protected override void Draw()
    {
        DrawBorderedRectangle(
            GlobalPosition - Origin,
            Size,
            Styles.Current);
    }
}