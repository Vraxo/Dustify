using Nodica;
using Raylib_cs;

namespace Dustify;

public class RenderButton : Button
{
    private ImageSelectionButton imageSelectionButton;
    private ImageDisplayer imageDisplayer;

    public override void Start()
    {
        LeftClicked += OnLeftClicked;
        
        imageSelectionButton = GetNode<ImageSelectionButton>("/root/ImageSelectionButton");
        imageDisplayer = GetNode<ImageDisplayer>("/root/ImageSelectionButton/AspectRatioContainer/ImageDisplayer");

        base.Start();
    }

    public override void Update()
    {
        Position = new(Window.Size.X / 2, Window.Size.Y * 0.9f);

        base.Update();
    }

    public void HandleCommandLineArgs(string[] args)
    {
        string imageFilePath = args[1];
        List<VideoQuality> qualities = ParseQualitiesFromArgs(args.Skip(2).ToArray());

        if (qualities.Count == 0)
        {
            qualities = new() { VideoQuality.Low, VideoQuality.Medium, VideoQuality.High };
        }

        imageSelectionButton.LoadImage(imageFilePath);
        StartRendering(qualities);
    }

    private void OnLeftClicked(object? sender, EventArgs e)
    {
        if (!imageDisplayer.HasTexture || GetNode<Renderer>("Renderer") is not null)
        {
            return;
        }

        List<VideoQuality> qualities = GetSelectedQualities();
        StartRendering(qualities);
    }

    private void StartRendering(List<VideoQuality> qualities)
    {
        AddChild(new Renderer());

        Texture2D texture = imageDisplayer.Texture;

        var optionButton = GetNode<OptionButton>("/root/OptionButton");

        var disintegrationMode = optionButton.Text == "Row by row" ? 
                                                      DisintegrationMode.RowByRow : 
                                                      DisintegrationMode.AllAtOnce;

        GetNode<Renderer>("Renderer").Render(texture, qualities, disintegrationMode);
    }

    private List<VideoQuality> ParseQualitiesFromArgs(string[] args)
    {
        List<VideoQuality> qualities = new();

        foreach (string arg in args)
        {
            switch (arg.ToLower())
            {
                case "low":
                case "l":
                    qualities.Add(VideoQuality.Low);
                    break;
                case "medium":
                case "m":
                    qualities.Add(VideoQuality.Medium);
                    break;
                case "high":
                case "h":
                    qualities.Add(VideoQuality.High);
                    break;
                default:
                    Console.WriteLine($"Warning: Unrecognized quality '{arg}'. Ignoring this argument.");
                    break;
            }
        }

        return qualities;
    }

    private List<VideoQuality> GetSelectedQualities()
    {
        List<VideoQuality> selectedQualities = new();

        if (GetNode<CheckBox>("/root/QualityCheckBoxes/HighQualityCheckBox").Checked)
        {
            selectedQualities.Add(VideoQuality.High);
        }

        if (GetNode<CheckBox>("/root/QualityCheckBoxes/MediumQualityCheckBox").Checked)
        {
            selectedQualities.Add(VideoQuality.Medium);
        }

        if (GetNode<CheckBox>("/root/QualityCheckBoxes/LowQualityCheckBox").Checked)
        {
            selectedQualities.Add(VideoQuality.Low);
        }

        if (selectedQualities.Count == 0)
        {
            selectedQualities = new() { VideoQuality.Low, VideoQuality.Medium, VideoQuality.High };
        }

        foreach (var q in selectedQualities)
        {
            Console.WriteLine(q);
        }

        Console.WriteLine("stop");

        return selectedQualities;
    }
}