using Raylib_cs;

namespace Nodica;

public class TextureLoader
{
    public static TextureLoader Instance => instance ??= new();
    private static TextureLoader? instance;

    public Dictionary<string, Texture2D> Textures = [];

    private TextureLoader() { }

    public void Add(string name, string path)
    {
        if (!Textures.ContainsKey(name))
        {
            Textures.Add(name, Raylib.LoadTexture(path));
        }
    }

    public void Remove(string name)
    {
        if (Textures.ContainsKey(name))
        {
            Textures.Remove(name);
        }
    }

    public bool Contains(string name)
    {
        return Textures.ContainsKey(name);
    }
}