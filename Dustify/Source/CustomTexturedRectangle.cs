using Nodica;
using Raylib_cs;

namespace Dustify;

public class CustomTexturedRectangle : VisualItem
{
    public Texture2D Texture { get; set; }
    public bool HasTexture = false;
    public int Height = 0;

    public CustomTexturedRectangle()
    {
        Size = new(32, 32);
    }

    public void LoadTexture(string name, bool resize = false)
    {
        Console.WriteLine("calling from Custom: ");
        Texture = TextureLoader.Instance.Get(name);
        HasTexture = true;

        Size = new(Texture.Width, Texture.Height);
    }

    protected override void Draw()
    {
        Rectangle source = new(
            0,
            Height,
            Texture.Width,
            Texture.Height - Height);

        Rectangle destination = new(
            GlobalPosition + new Vector2(0, Height) - Origin,
            Size - new Vector2(0, Height));

        Raylib.DrawTexturePro(
            Texture,
            source,
            destination,
            Offset,
            0,
            Color.White);
    }
}