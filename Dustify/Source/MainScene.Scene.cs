using Nodica;

namespace Dustify;

public partial class MainScene : Node
{
    public override void Build()
    {
        var imageSelectionButton = AddChild(new Button
        {
            Size = new(500, 500),
            Themes = new()
            {
                Roundness = 0
            }
        }, "ImageSelectionButton");

        var aspectRatioContainer = imageSelectionButton.AddChild(new AspectRatioContainer
        {
            Size = new(500, 500)
        }, "AspectRatioContainer");

        var imageDisplayer = aspectRatioContainer.AddChild(new ImageDisplayer(), "ImageDisplayer");

        var renderButton = AddChild(new Button
        {
            Position = new(500, 500),
            Themes = new()
            {
                Roundness = 0
            },
            Text = "Render",
            RightControlPath = "/root/OptionButton",
            Icon = TextureLoader.Instance.Get("Resources/ReturnIcon.png"),
            FocusOnClick = true
        }, "RenderButton");

        var focusTestButton = AddChild(new CheckBox
        {
            Position = new(600, 500),
            LeftControlPath = "/root/RenderButton",
            FocusOnClick = true
        }, "FocusTestButton");

        var optionButton = AddChild(new OptionButton
        {
            Position = new(500, 600),
            Text = "..."
        }, "OptionButton");

        var progressBar = AddChild(new ProgressBar
        {
            Position = new(500, 500),
            Size = new(500, 10)
        }, "ProgressBar");

        var label = AddChild(new Nodica.Label
        {
            Position = new(10, 10),
            Size = new(100, 16),
            Text = "Go fuck yourself you fucking retarded piece of shit"
        }, "Label");
    }
}