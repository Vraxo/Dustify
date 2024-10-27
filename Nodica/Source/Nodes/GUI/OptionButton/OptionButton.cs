using Raylib_cs;
using System.Collections.Generic;

namespace Nodica;

public partial class OptionButton : Button
{
    public bool Open { get; set; } = false;
    public int Choice { get; set; } = -1;

    // List to keep track of Option children
    private readonly List<Option> optionChildren = new();

    public OptionButton()
    {
        LeftClicked += OnLeftClicked;
    }

    private void OnLeftClicked(object? sender, EventArgs e)
    {
        if (!Open)
        {
            DropDown();
        }
        else
        {
            Close();
        }
    }

    public override void Update()
    {
        if (Open)
        {
            bool isMouseOverAnyOption = IsMouseOver() || optionChildren.Any(option => option.IsMouseOver());

            if (Raylib.IsMouseButtonPressed(MouseButton.Left) && !isMouseOverAnyOption)
            {
                Close();
            }
        }

        base.Update();
    }

    private void DropDown()
    {
        for (int i = 0; i < 3; i++)
        {
            var option = new Option
            {
                Position = new(0, 25 * (i + 1)),
                Styles = new()
                {
                    Roundness = 0
                },
                Text = $"Option {i}",
                Checked = i == Choice,
                Index = i
            };

            AddChild(option);
            optionChildren.Add(option); // Add to our tracked list
        }

        Open = true;
    }

    private void Close()
    {
        foreach (var option in optionChildren)
        {
            option.Destroy();
        }

        optionChildren.Clear(); // Clear the list once options are removed
        Open = false;
    }
}
