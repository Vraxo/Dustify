using Nodica;
using Raylib_cs;
using SixLabors.ImageSharp;

namespace Dustify;

public partial class MainScene : Node
{
    private ImageSelectionButton imageSelectionButton;
    private RenderButton renderButton;
    private ProgressBar progressBar;

    // Public

    public override void Start()
    {
        imageSelectionButton = GetNode<ImageSelectionButton>("ImageSelectionButton");
        renderButton = GetNode<RenderButton>("RenderButton");
        progressBar = GetNode<ProgressBar>("ProgressBar");

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
        base.Update();
    }

    private void UpdateProgressBar()
    {
        progressBar.Position = new(Window.Size.X / 2, Window.Size.Y * 0.8f);
    }
}