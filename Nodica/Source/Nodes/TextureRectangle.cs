using Raylib_cs;

namespace Nodica;

public class TextureRectangle : VisualItem
{
    public Texture2D Texture { get; set; } = Raylib.LoadTexture("");
    public bool HasTexture = false;
    public int Height = 0;

    public TextureRectangle()
    {
        Size = new(32, 32);
    }

    public override void Update()
    {
        Draw();
        base.Update();
    }

    public void LoadTexture(string name, bool resize = false)
    {
        Texture = TextureLoader.Instance.Get(name);
        HasTexture = true;

        if (resize)
        {
            Size = new(Texture.Width, Texture.Height);
            Console.WriteLine("Size: " + Size);
        }
    }

    protected override void Draw()
    {
        Rectangle source = new(0, Height, Texture.Width, Texture.Height);
        Rectangle destination = new(GlobalPosition, Size);

        Raylib.DrawTexturePro(
            Texture,
            source,
            destination,
            Origin,
            0,
            Color.White);
    }
}