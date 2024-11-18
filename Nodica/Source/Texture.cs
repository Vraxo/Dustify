using Raylib_cs;

namespace Nodica;

public class Texture
{
    public Vector2 Size { get; private set; } = Vector2.Zero;

    private Texture2D texture;

    public Texture(string filePath)
    {
        texture = TextureLoader.Instance.Get(filePath);
        Size = new(texture.Width, texture.Height);
    }

    public static implicit operator Texture2D(Texture texture)
    {
        return texture.texture;
    }
}