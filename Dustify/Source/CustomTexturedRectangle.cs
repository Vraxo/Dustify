using Nodica;
using Raylib_cs;

namespace Dustify;

public class CustomTexturedRectangle : Node2D
{
    public Texture2D Texture { get; set; } = Raylib.LoadTexture("");
    public bool HasTexture = false;
    public int Height = 0;

    public CustomTexturedRectangle()
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
        Texture = TextureLoader.Instance.Textures[name];
        HasTexture = true;

        Size = new(Texture.Width, Texture.Height);
    }

    public void Draw()
    {
        Rectangle source = new(
            0,
            Height,
            Texture.Width,
            Texture.Height - Height);

        Rectangle destination = new(
            GlobalPosition + new Vector2(0, Height),
            Size - new Vector2(0, Height));

        Raylib.DrawTexturePro(
            Texture,
            source,
            destination,
            Origin,
            0,
            Color.White);
    }
}