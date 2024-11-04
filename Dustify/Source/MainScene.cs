using Nodica;

namespace Dustify;

public partial class MainScene : Node
{
    private ImageSelectionButton imageSelectionButton;
    private RenderButton renderButton;
    private ProgressBar progressBar;
    private OptionButton optionButton;

    public override void Start()
    {
        imageSelectionButton = GetNode<ImageSelectionButton>("ImageSelectionButton");
        renderButton = GetNode<RenderButton>("RenderButton");
        progressBar = GetNode<ProgressBar>("ProgressBar");

        optionButton = GetNode<OptionButton>("OptionButton");
        optionButton.Add("Row by row");
        optionButton.Add("All at once");

        string[] args = Environment.GetCommandLineArgs();

        if (args.Length > 1)
        {
            if (!File.Exists(args[1]))
            {
                Console.WriteLine("Error: The provided image file does not exist.");
                Environment.Exit(1);
            }
            else
            {
                renderButton.HandleCommandLineArgs(args);
            }
        }

        base.Start();
    }

    public override void Update()
    {
        UpdateProgressBar();
        UpdateOptionButton();
        base.Update();
    }

    private void UpdateProgressBar()
    {
        progressBar.Position = new(Window.Size.X / 2, Window.Size.Y * 0.8f);
    }

    private void UpdateOptionButton()
    {
        optionButton.Position = new(Window.Size.X / 2 + 150, Window.Size.Y * 0.84f);
    }

    private void ClearTemporaryDirectory()
    {
        string temporaryDirectory = Path.Combine("Resources", "Temporary");

        if (Directory.Exists(temporaryDirectory))
        {
            foreach (var file in Directory.GetFiles(temporaryDirectory))
            {
                File.Delete(file);
            }
        }
    }
}