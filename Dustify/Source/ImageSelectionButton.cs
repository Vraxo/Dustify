using Nodica;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Dustify;

public class ImageSelectionButton : Button
{
    private ImageDisplayer imageDisplayer;

    public override void Start()
    {
        LeftClicked += OnLeftClicked;
        base.Start();
    }

    public override void Ready()
    {
        imageDisplayer = GetNode<ImageDisplayer>("/root/ImageSelectionButton/AspectRatioContainer/ImageDisplayer");
        base.Ready();
    }

    public override void Update()
    {
        UpdatePosition();
        base.Update();
    }

    private void UpdatePosition()
    {
        Position = new(Window.Size.X * 0.5f, Window.Size.Y / 2.5f);
    }

    public void LoadImage(string selectedFile)
    {
        string pngPath = Path.GetExtension(selectedFile).ToLower() != ".png"
            ? ConvertToPng(selectedFile)
            : selectedFile;

        Bitmap bitmap = ResizeImageIfNeeded(pngPath, out string resizedPngPath);
        TextureLoader.Instance.Get(resizedPngPath);
        imageDisplayer.LoadTexture(resizedPngPath, true);
        imageDisplayer.BitmapData = bitmap;

        Console.WriteLine("Loaded image from: " + resizedPngPath);

        if (Path.GetExtension(selectedFile).ToLower() != ".png")
        {
            File.Delete(pngPath);
        }
        File.Delete(resizedPngPath);
    }

    private Bitmap ResizeImageIfNeeded(string imagePath, out string resizedPngPath)
    {
        using var image = SixLabors.ImageSharp.Image.Load(imagePath);
        SixLabors.ImageSharp.Size newSize;

        if (image.Width > 1024 || image.Height > 1024)
        {
            float scale = Math.Min(1024f / image.Width, 1024f / image.Height);
            newSize = new SixLabors.ImageSharp.Size((int)(image.Width * scale), (int)(image.Height * scale));
            image.Mutate(x => x.Resize(newSize.Width, newSize.Height));

            string nameWithoutExtension = Path.GetFileNameWithoutExtension(imagePath);
            resizedPngPath = $"Resources/Temporary/{nameWithoutExtension} - Resized.png";
            image.SaveAsPng(resizedPngPath);
        }
        else
        {
            resizedPngPath = imagePath;
            newSize = new SixLabors.ImageSharp.Size(image.Width, image.Height);
        }

        using var memoryStream = new MemoryStream();
        image.SaveAsPng(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return new Bitmap(System.Drawing.Image.FromStream(memoryStream));
    }

    private void OnLeftClicked(object? sender, EventArgs e)
    {
        OpenFileDialog openFileDialog = new()
        {
            Filter = "Images|*.jpg;*.jpeg;*.gif;*.bmp;*.tiff;*.tif;*.png",
            Title = "Select an Image"
        };

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            string selectedFile = openFileDialog.FileName;
            LoadImage(selectedFile);
        }
    }

    private static string ConvertToPng(string imagePath)
    {
        if (!Directory.Exists("Resources/Temporary"))
        {
            Directory.CreateDirectory("Resources/Temporary");
        }

        string nameWithoutExtension = Path.GetFileNameWithoutExtension(imagePath);
        string pngPath = $"Resources/Temporary/{nameWithoutExtension}.png";

        var image = SixLabors.ImageSharp.Image.Load(imagePath);
        image.SaveAsPng(pngPath);

        return pngPath;
    }
}