using Raylib_cs;

namespace Nodica;

public class CheckButton : Control
{
    public enum ClickMode { Limited, Limitless }
    public enum ActionMode { Release, Press }
    public enum ButtonState { Normal, Hover, Pressed, Focused }

    #region [ - - - Properties & Fields - - - ]

    public ButtonThemePack Styles { get; set; } = new();

    public ActionMode LeftClickActionMode { get; set; } = ActionMode.Release;
    public ActionMode RightClickActionMode { get; set; } = ActionMode.Release;
    public bool StayPressed { get; set; } = false;

    public bool PressedLeft = false;
    public bool PressedRight = false;

    public Action<CheckButton> OnUpdate = (button) => { };

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

    // Public state property with a private setter
    public ButtonState State { get; private set; } = ButtonState.Normal;

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
            OnUpdate(this);
            HandleClicks();
        }

        Draw();
        base.Update();
    }

    private void OnFocusChanged(bool focused)
    {
        State = focused ? ButtonState.Focused : ButtonState.Normal;
        UpdateStyle();
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
            State = pressed ? ButtonState.Pressed : ButtonState.Hover;
            UpdateStyle();

            if (Raylib.IsMouseButtonPressed(button))
            {
                pressed = true;
                HandleClickFocus();

                if (actionMode == ActionMode.Press)
                {
                    clickHandler?.Invoke(this, EventArgs.Empty);
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
    }
}
