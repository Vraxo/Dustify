using Nodica;

namespace Dustify;

public class QualityCheckBoxes : Node2D
{
    private CheckBox highQualityCheckBox;
    private CheckBox mediumQualityCheckBox;
    private CheckBox lowQualityCheckBox;

    public override void Start()
    {
        highQualityCheckBox = GetNode<CheckBox>("HighQualityCheckBox");
        mediumQualityCheckBox = GetNode<CheckBox>("MediumQualityCheckBox");
        lowQualityCheckBox = GetNode<CheckBox>("LowQualityCheckBox");

        base.Start();
    }

    public override void Update()
    {
        UpdateHighQualityCheckBox();
        UpdateMediumQualityCheckBox();
        UpdateLowQualityCheckBox();
        base.Update();
    }

    private void UpdateHighQualityCheckBox()
    {
        float x = mediumQualityCheckBox.Position.X - 100;
        float y = Window.Size.Y * 0.84f;

        highQualityCheckBox.Position = new(x, y);
    }

    private void UpdateMediumQualityCheckBox()
    {
        float x = Window.Size.X / 2 - 100;
        float y = Window.Size.Y * 0.84f;

        mediumQualityCheckBox.Position = new(x, y);
    }

    private void UpdateLowQualityCheckBox()
    {
        float x = mediumQualityCheckBox.Position.X + 100;
        float y = Window.Size.Y * 0.84f;

        lowQualityCheckBox.Position = new(x, y);
    }
}