using Nodica;
using Raylib_cs;
using SixLabors.ImageSharp;
using Button = Nodica.Button;

namespace Dustify;

public class MainScene : Node
{
    private Button imageSelectionButton;
    private ImageDisplayer imageDisplayer;
    private Button renderButton;
    private ProgressBar progressBar;

    public override void Start()
    {
        imageSelectionButton = GetNode<Button>("ImageSelectionButton");
        imageSelectionButton.LeftClicked += OnImageSelectionButtonLeftClicked;

        imageDisplayer = GetNode<ImageDisplayer>("ImageSelectionButton/ARC/TextureRectangle");

        renderButton = GetNode<Button>("RenderButton");
        renderButton.LeftClicked += OnRenderButtonLeftClicked;
        renderButton.RightClicked += OnRenderButtonLeftClicked;

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
                HandleCommandLineArgs(args);
            }
        }

        base.Start();
    }

    public override void Update()
    {
        UpdateImageSelectionButton();
        UpdateRenderButton();
        UpdateProgressBar();
        base.Update();
    }

    private void OnImageSelectionButtonLeftClicked(object? sender, EventArgs e)
    {
        OpenFileDialog openFileDialog = new();
        openFileDialog.Filter = "Images|*.jpg;*.jpeg;*.gif;*.bmp;*.tiff;*.tif;*.png";
        openFileDialog.Title = "Select an Image";

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            string selectedFile = openFileDialog.FileName;
            LoadImage(selectedFile);
        }
    }

    private void OnRenderButtonLeftClicked(object? sender, EventArgs e)
    {
        StartRendering(Renderer.VideoQuality.Low, Renderer.VideoQuality.Medium, Renderer.VideoQuality.High);
    }

    private void StartRendering(params Renderer.VideoQuality[] qualities)
    {
        if (!imageDisplayer.HasTexture)
        {
            Console.WriteLine("can't render");
            return;
        }

        AddChild(new Renderer());

        Texture2D texture = imageDisplayer.Texture;
        GetNode<Renderer>("Renderer").Render(texture, qualities);
    }

    private void LoadImage(string selectedFile)
    {
        Bitmap bitmap;

        if (Path.GetExtension(selectedFile).ToLower() != ".png")
        {
            string pngPath = ConvertToPng(selectedFile);
            TextureLoader.Instance.Add(pngPath, pngPath);
            imageDisplayer.LoadTexture(pngPath, true);

            using (var tempBitmap = (Bitmap)System.Drawing.Image.FromFile(pngPath))
            {
                bitmap = new(tempBitmap); // Clone the bitmap to memory
            }

            imageDisplayer.BitmapData = bitmap;
            //imageDisplayer.OriginalImage = bitmap;
            File.Delete(pngPath);  // Now you can safely delete the PNG
        }
        else
        {
            TextureLoader.Instance.Add(selectedFile, selectedFile);
            imageDisplayer.LoadTexture(selectedFile, true);

            using (var tempBitmap = (Bitmap)System.Drawing.Image.FromFile(selectedFile))
            {
                bitmap = new Bitmap(tempBitmap); // Clone the bitmap to memory
            }

            imageDisplayer.BitmapData = bitmap;
        }
    }

    private void HandleCommandLineArgs(string[] args)
    {
        string imageFilePath = args[1];
        var qualities = new List<Renderer.VideoQuality>();

        for (int i = 2; i < args.Length; i++)
        {
            string quality = args[i].ToLower();

            switch (quality)
            {
                case "low":
                    qualities.Add(Renderer.VideoQuality.Low);
                    break;

                case "medium":
                    qualities.Add(Renderer.VideoQuality.Medium);
                    break;

                case "high":
                    qualities.Add(Renderer.VideoQuality.High);
                    break;

                default:
                    Console.WriteLine($"Warning: Unrecognized quality '{args[i]}'. Defaulting to all qualities.");

                    qualities.Add(Renderer.VideoQuality.Low);
                    qualities.Add(Renderer.VideoQuality.Medium);
                    qualities.Add(Renderer.VideoQuality.High);
                    break;
            }
        }

        if (qualities.Any())
        {
            LoadImage(imageFilePath);
            StartRendering(qualities.ToArray());
        }
    }

    private void UpdateImageSelectionButton()
    {
        imageSelectionButton.Position = new(Window.Size.X * 0.5f, Window.Size.Y / 2.5f);
    }

    private void UpdateRenderButton()
    {
        renderButton.Position = new(Window.Size.X / 2, Window.Size.Y * 0.85f);
    }

    private void UpdateProgressBar()
    {
        progressBar.Position = new(Window.Size.X / 2, Window.Size.Y * 0.8f);
    }

    private static string ConvertToPng(string imagePath)
    {
        string nameWithoutExtension = Path.GetFileNameWithoutExtension(imagePath);
        string pngPath = $"Resources/{nameWithoutExtension}.png";

        var image = SixLabors.ImageSharp.Image.Load(imagePath);
        image.SaveAsPng(pngPath);

        return pngPath;
    }
}