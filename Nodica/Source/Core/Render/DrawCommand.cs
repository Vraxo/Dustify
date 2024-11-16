namespace Nodica;

public abstract class DrawCommand
{
    public void Draw()
    {
        switch (RenderServer.Instance.Backend)
        {
            case GraphicsBackend.Raylib:
                DrawRaylib();
                break;

            case GraphicsBackend.SDL2:
                DrawSDL2();
                break;
        }
    }

    protected virtual void DrawRaylib() { }

    protected virtual void DrawSDL2() { }
}