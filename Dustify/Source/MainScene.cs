using Nodica;
using Raylib_cs;

namespace Dustify;

public partial class MainScene : Node
{
    private ImageSelectionButton imageSelectionButton;
    private RenderButton renderButton;
    private ProgressBar progressBar;

    // Public

    public override void Start()
    {
        // Clear the contents of the Resources/Temporary directory
        //ClearTemporaryDirectory();

        imageSelectionButton = GetNode<ImageSelectionButton>("ImageSelectionButton");
        renderButton = GetNode<RenderButton>("RenderButton");
        progressBar = GetNode<ProgressBar>("ProgressBar");

        var optionButton = GetNode<OptionButton>("OptionButton");
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
        Raylib.DrawFPS(10, 10);
        UpdateProgressBar();
        base.Update();
    }

    private void UpdateProgressBar()
    {
        progressBar.Position = new(Window.Size.X / 2, Window.Size.Y * 0.8f);
    }

    private void ClearTemporaryDirectory()
    {
        string tempDir = Path.Combine("Resources", "Temporary");

        if (Directory.Exists(tempDir))
        {
            // Delete all files in the directory
            foreach (var file in Directory.GetFiles(tempDir))
            {
                File.Delete(file);
            }

            // Optionally, delete subdirectories and their contents
            foreach (var dir in Directory.GetDirectories(tempDir))
            {
                Directory.Delete(dir, true); // true to delete recursively
            }
        }
    }
}
