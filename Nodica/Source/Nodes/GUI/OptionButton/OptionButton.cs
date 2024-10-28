using Raylib_cs;
using System.Collections.Generic;

namespace Nodica;

public partial class OptionButton : Button
{
    public bool Open { get; set; } = false;
    public int Choice { get; set; } = -1;

    public List<string> Options { get; set; } = [];
    public int OptionsCount => Options.Count;

    private readonly List<OptionButtonButton> optionChildren = new();

    public OptionButton()
    {
        LeftClicked += OnLeftClicked;
    }

    public void Add(string option)
    {
        Options.Add(option);
    }

    public void Clear()
    {
        Options.Clear();
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
        for (int i = 0; i < Options.Count; i++)
        {
            var option = new OptionButtonButton
            {
                Position = new(0, 25 * (i + 1)),
                Styles = new()
                {
                    Roundness = 0
                },
                Text = Options[i],
                Checked = i == Choice,
                Index = i
            };

            AddChild(option);
            optionChildren.Add(option);
        }

        Open = true;
    }

    private void Close()
    {
        foreach (var option in optionChildren)
        {
            option.Destroy();
        }

        optionChildren.Clear();
        Open = false;
    }
}