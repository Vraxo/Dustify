using Raylib_cs;

namespace Nodica
{
    public class TextureLoader
    {
        public static TextureLoader Instance => instance ??= new();
        private static TextureLoader? instance;

        private Dictionary<string, Texture2D> textures = new();

        private TextureLoader() { }

        public Texture2D Get(string path)
        {
            if (!textures.ContainsKey(path))
            {
                textures[path] = Raylib.LoadTexture(path);
            }
            return textures[path];
        }

        public void Remove(string path)
        {
            if (textures.ContainsKey(path))
            {
                Raylib.UnloadTexture(textures[path]); // Unload the texture to free memory
                textures.Remove(path);
            }
        }

        public bool Contains(string path)
        {
            return textures.ContainsKey(path);
        }
    }
}