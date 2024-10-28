using Nodica;
using SixLabors.ImageSharp;

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
        Bitmap bitmap;

        if (Path.GetExtension(selectedFile).ToLower() != ".png")
        {
            string pngPath = ConvertToPng(selectedFile);
            TextureLoader.Instance.Get(pngPath);
            imageDisplayer.LoadTexture(pngPath, true);

            using (var tempBitmap = (Bitmap)System.Drawing.Image.FromFile(pngPath))
            {
                bitmap = new(tempBitmap); // Clone the bitmap to memory
            }

            imageDisplayer.BitmapData = bitmap;
            Console.WriteLine("set bitmap 1");
            File.Delete(pngPath);
        }
        else
        {
            TextureLoader.Instance.Get(selectedFile);
            imageDisplayer.LoadTexture(selectedFile, true);

            using (var tempBitmap = (Bitmap)System.Drawing.Image.FromFile(selectedFile))
            {
                bitmap = new Bitmap(tempBitmap); // Clone the bitmap to memory
            }

            imageDisplayer.BitmapData = bitmap;
            Console.WriteLine("set bitmap 2");
        }
    }

    private void OnLeftClicked(object? sender, EventArgs e)
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

    private static string ConvertToPng(string imagePath)
    {
        string nameWithoutExtension = Path.GetFileNameWithoutExtension(imagePath);
        string pngPath = $"Resources/{nameWithoutExtension}.png";

        var image = SixLabors.ImageSharp.Image.Load(imagePath);
        image.SaveAsPng(pngPath);

        return pngPath;
    }
}