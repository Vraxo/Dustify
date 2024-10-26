﻿using Raylib_cs;

namespace Nodica;

public class CheckBox : Control
{
    public enum ClickMode { Limited, Limitless }
    public enum ActionMode { Release, Press }
    public enum ButtonState { Normal, Hover, Pressed, Focused }
    public enum ClickBehavior { Left, Right, Both }

    #region [ - - - Properties & Fields - - - ]

    public ButtonThemePack BackgroundStyles { get; set; } = new();
    public BoxStyle CheckStyles { get; set; } = new();
    public Vector2 CheckSize { get; set; } = new();
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
        }
    }

    private string _themeFile = "";
    public string ThemeFile
    {
        get => _themeFile;

        set
        {
            _themeFile = value;
            BackgroundStyles = StyleLoader.LoadStyle<ButtonThemePack>(value);
        }
    }

    public bool Toggled { get; set; } = false;

    #endregion

    public CheckBox()
    {
        Size = new(26, 26);
        FocusChanged += OnFocusChanged;
        CheckSize = Size / 2;
        BackgroundStyles.Roundness = 1;
        CheckStyles.Roundness = 1;
        CheckStyles.FillColor = DefaultTheme.Accent;
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
        BackgroundStyles.Current = focused ? 
                                   BackgroundStyles.Focused : 
                                   BackgroundStyles.Normal;
    }

    private void HandleKeyboardInput()
    {
        if (Focused && Raylib.IsKeyPressed(KeyboardKey.Enter))
        {
            Toggle();
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
                MouseButton.Left,
                LeftClickActionMode);

            if (PressedLeft) anyPressed = true;
        }

        if (Behavior == ClickBehavior.Right || Behavior == ClickBehavior.Both)
        {
            HandleClick(
                ref PressedRight,
                MouseButton.Right,
                RightClickActionMode);

            if (PressedRight) anyPressed = true;
        }

        if (StayPressed && (PressedLeft || PressedRight))
        {
            BackgroundStyles.Current = BackgroundStyles.Pressed;
        }
        else if (Focused)
        {
            if (mouseOver)
            {
                BackgroundStyles.Current = anyPressed ? BackgroundStyles.Pressed : BackgroundStyles.Focused;
            }
            else
            {
                BackgroundStyles.Current = Focused ? BackgroundStyles.Focused : BackgroundStyles.Normal;
            }
        }
        else if (mouseOver)
        {
            BackgroundStyles.Current = anyPressed ? BackgroundStyles.Pressed : BackgroundStyles.Hover;
        }
        else
        {
            BackgroundStyles.Current = BackgroundStyles.Normal;
        }
    }

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
                    Toggle();
                }
            }
        }

        if (Raylib.IsMouseButtonReleased(button))
        {
            if (mouseOver && pressed && actionMode == ActionMode.Release)
            {
                Toggle();
            }

            pressed = false;
        }
    }

    protected override void Draw()
    {
        DrawBorderedRectangle(
            GlobalPosition - Origin,
            Size,
            BackgroundStyles.Current);

        if (Toggled)
        {
            DrawBorderedRectangle(
                GlobalPosition - Origin / 2,
                CheckSize,
                CheckStyles);
        }
    }
}