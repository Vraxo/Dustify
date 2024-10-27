using Raylib_cs;

namespace Nodica;

public abstract class BaseButton : Control
{
    public enum ClickMode { Limited, Limitless }
    public enum ActionMode { Release, Press }
    public enum ClickBehavior { Left, Right, Both }

    #region [ - - - Properties & Fields - - - ]

    public ClickMode Mode { get; set; } = ClickMode.Limited;
    public ActionMode LeftClickActionMode { get; set; } = ActionMode.Release;
    public ActionMode RightClickActionMode { get; set; } = ActionMode.Release;
    public bool StayPressed { get; set; } = false;
    public ClickBehavior Behavior { get; set; } = ClickBehavior.Both;

    public bool PressedLeft = false;
    public bool PressedRight = false;

    private bool _disabled = false;
    public bool Disabled
    {
        get => _disabled;
        set
        {
            _disabled = value;
            OnDisable();
        }
    }

    protected abstract void OnDisable();

    #endregion

    public BaseButton()
    {
        // Base button initialization if needed
    }

    public override void Update()
    {
        if (!Disabled)
        {
            HandleClicks();
            HandleKeyboardInput();
        }

        base.Update();
    }

    private void HandleKeyboardInput()
    {
        if (Focused && Raylib.IsKeyPressed(KeyboardKey.Enter))
        {
            OnEnterKeyPressed();
        }
    }

    protected abstract void OnEnterKeyPressed();

    private void HandleClicks()
    {
        if (Disabled) return;

        bool mouseOver = IsMouseOver();
        bool anyPressed = false;

        if (Behavior == ClickBehavior.Left || Behavior == ClickBehavior.Both)
        {
            HandleClick(ref PressedLeft, MouseButton.Left, LeftClickActionMode);
            if (PressedLeft) anyPressed = true;
        }

        if (Behavior == ClickBehavior.Right || Behavior == ClickBehavior.Both)
        {
            HandleClick(ref PressedRight, MouseButton.Right, RightClickActionMode);
            if (PressedRight) anyPressed = true;
        }

        UpdateStyles(mouseOver, anyPressed);
    }

    protected abstract void UpdateStyles(bool mouseOver, bool anyPressed);

    private void HandleClick(ref bool pressed, MouseButton button, ActionMode actionMode)
    {
        if (Disabled) return;

        bool mouseOver = IsMouseOver();

        if (mouseOver)
        {
            if (Raylib.IsMouseButtonPressed(button))
            {
                pressed = true;
                HandleClickFocus();

                if (actionMode == ActionMode.Press)
                {
                    OnClick();
                }
            }
        }

        if (Raylib.IsMouseButtonReleased(button))
        {
            if (mouseOver && pressed && actionMode == ActionMode.Release)
            {
                OnClick();
            }

            pressed = false;
        }
    }

    protected abstract void OnClick();
}
