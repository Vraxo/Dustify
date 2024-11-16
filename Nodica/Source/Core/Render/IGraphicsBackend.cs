namespace Nodica;

public abstract class GraphicsBackendBase
{
    public abstract void Initialize(int width, int height, string title);
    public abstract void Run();
    public abstract void SetWindowIcon(string iconPath);
    public abstract void BeginDrawing();
    public abstract void EndDrawing();
    public abstract void ClearBackground(Color color);
    public abstract bool WindowShouldClose();
}