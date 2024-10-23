﻿using Raylib_cs;

namespace Nodica;

public class Control : ClickableRectangle
{
    public bool FocusOnClick { get; set; } = false;
    public bool EnableArrowNavigation { get; set; } = true;
    public string? UpControlPath { get; set; }
    public string? DownControlPath { get; set; }
    public string? LeftControlPath { get; set; }
    public string? RightControlPath { get; set; }

    public event Action<bool>? FocusChanged;

    private bool _focused = false;
    public bool Focused
    {
        get => _focused;
        set
        {
            if (_focused != value)
            {
                _focused = value;
                FocusChanged?.Invoke(_focused);
            }
        }
    }

    public override void Update()
    {
        if (EnableArrowNavigation && Focused)
        {
            HandleArrowNavigation();
        }

        UpdateFocusOnMouseOut();
        base.Update();
    }

    private void HandleArrowNavigation()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Up) && !string.IsNullOrEmpty(UpControlPath))
        {
            NavigateToControl(UpControlPath);
        }
        else if (Raylib.IsKeyPressed(KeyboardKey.Down) && !string.IsNullOrEmpty(DownControlPath))
        {
            NavigateToControl(DownControlPath);
        }
        else if (Raylib.IsKeyPressed(KeyboardKey.Left) && !string.IsNullOrEmpty(LeftControlPath))
        {
            NavigateToControl(LeftControlPath);
        }
        else if (Raylib.IsKeyPressed(KeyboardKey.Right) && !string.IsNullOrEmpty(RightControlPath))
        {
            NavigateToControl(RightControlPath);
        }
    }

    private void NavigateToControl(string controlPath)
    {
        Control? targetControl = GetNode<Control>(controlPath);
        if (targetControl != null)
        {
            Focused = false;
            targetControl.Focused = true;
        }
    }

    private void UpdateFocusOnMouseOut()
    {
        if (!IsMouseOver() && Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            Focused = false;
        }
    }

    protected virtual void HandleClickFocus()
    {
        if (FocusOnClick && IsMouseOver())
        {
            Focused = true;
        }
    }
}