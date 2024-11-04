using Nodica;
using Raylib_cs;
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
        string resizedPngPath = ResizeImageIfNeeded(selectedFile, out var bitmap);

        Texture2D texture = TextureLoader.Instance.Get(resizedPngPath);
        imageDisplayer.LoadTexture(resizedPngPath, true);
        imageDisplayer.BitmapData = bitmap;

        Console.WriteLine("Loaded image from: " + resizedPngPath);

        if (resizedPngPath != selectedFile)
        {
            File.Delete(resizedPngPath);
        }
    }

    private string ResizeImageIfNeeded(string imagePath, out Bitmap bitmap)
    {
        using var image = SixLabors.ImageSharp.Image.Load(imagePath);
        SixLabors.ImageSharp.Size newSize;

        if (image.Width > 1024 || image.Height > 1024)
        {
            float scale = Math.Min(1024f / image.Width, 1024f / image.Height);
            newSize = new SixLabors.ImageSharp.Size((int)(image.Width * scale), (int)(image.Height * scale));
            image.Mutate(x => x.Resize(newSize.Width, newSize.Height));

            string nameWithoutExtension = Path.GetFileNameWithoutExtension(imagePath);
            string resizedPngPath = $"Resources/Temporary/{nameWithoutExtension} - Resized.png";

            if (!Directory.Exists("Resources/Temporary"))
            {
                Directory.CreateDirectory("Resources/Temporary");
            }

            image.SaveAsPng(resizedPngPath);

            using var memoryStream = new MemoryStream();
            image.SaveAsPng(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            bitmap = new Bitmap(System.Drawing.Image.FromStream(memoryStream));

            return resizedPngPath;
        }

        bitmap = new Bitmap(imagePath);
        return imagePath;
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
}