using Raylib_cs;

namespace Nodica;

public class CheckButton : Control
{
    public enum ActionMode { Release, Press }
    public enum ButtonState { Normal, Hover, Pressed, Focused }
    public enum ClickBehavior { Left, Right, Both }

    #region [ - - - Properties & Fields - - - ]

    public ButtonThemePack Styles { get; set; } = new();
    public ActionMode LeftClickActionMode { get; set; } = ActionMode.Release;
    public ActionMode RightClickActionMode { get; set; } = ActionMode.Release;
    public bool StayPressed { get; set; } = false;
    public ButtonState State { get; private set; } = ButtonState.Normal;
    public ClickBehavior Behavior { get; set; } = ClickBehavior.Both;

    public bool Toggled { get; set; } = false;

    public bool PressedLeft = false;
    public bool PressedRight = false;

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

        // No text to draw
    }
}