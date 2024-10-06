using Nodica;
using SixLabors.ImageSharp;
using Button = Nodica.Button;
using Image = SixLabors.ImageSharp.Image;

namespace Dustify;

public class MainScene : Node
{
    private Button imageSelectionButton;
    private TexturedRectangle texturedRectangle;
    private Node2D rightPanel;

    public override void Start()
    {
        imageSelectionButton = GetNode<Button>("ImageSelectionButton");
        imageSelectionButton.LeftClicked += OnImageSelectionButtonLeftClicked;

        texturedRectangle = GetNode<TexturedRectangle>("ImageSelectionButton/ARC/TexturedRectangle");

        rightPanel = GetNode<Node2D>("RightPanel");

        base.Start();
    }

    public override void Update()
    {
        //GetNode<AspectRatioContainer>("ImageSelectionButton/ARC").Position = Window.Size / 2;

        UpdateImageSelectionButton();
        UpdateRightPanel();

        base.Update();
    }

    private void OnImageSelectionButtonLeftClicked(object? sender, EventArgs e)
    {
        // Open file dialog for selecting an image
        OpenFileDialog openFileDialog = new();
        openFileDialog.Filter = "Images|*.jpg;*.jpeg;*.gif;*.bmp;*.tiff;*.tif;*.png"; // Filter for image files
        openFileDialog.Title = "Select an Image";
        
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            string selectedFile = openFileDialog.FileName;
        
            // Check if the file is a PNG
            if (Path.GetExtension(selectedFile).ToLower() != ".png")
            {
                // Convert the image to PNG and save it to Resources/
                string pngPath = ConvertToPng(selectedFile);
                TextureLoader.Instance.Add(pngPath, pngPath);
                texturedRectangle.LoadTexture(pngPath);
                File.Delete(pngPath); // Delete the converted file after loading
            }
            else
            {
                TextureLoader.Instance.Add(selectedFile, selectedFile);
                texturedRectangle.LoadTexture(selectedFile, true);
            }
        }
    }

    private void UpdateImageSelectionButton()
    {
        imageSelectionButton.Position = new(Window.Size.X * 0.25f, Window.Size.Y / 2);
    }

    private void UpdateRightPanel()
    {
        rightPanel.Position = new(Window.Size.X * 0.8f, Window.Size.Y * 0.25f);
    }

    private static string ConvertToPng(string imagePath)
    {
        string pngPath = Path.Combine("Resources", Path.GetFileNameWithoutExtension(imagePath) + ".png");

        using (Image image = Image.Load(imagePath))
        {
            image.SaveAsPng(pngPath); // Save as PNG
        }

        return pngPath;
    }
}