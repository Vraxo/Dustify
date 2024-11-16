using Nodica;
using Raylib_cs;

public class RaylibBackend : GraphicsBackendBase
{
    public override void Initialize(int width, int height, string title)
    {
        Raylib.InitWindow(width, height, title);
        Raylib.SetWindowMinSize(width, height);
        Raylib.InitAudioDevice();
        SetWindowIcon("Resources/Icon/Icon.png");
    }

    public override void Run()
    {
        while (!Raylib.WindowShouldClose())
        {
            BeginDrawing();
            ClearBackground(DefaultTheme.Background);
            Raylib.EndDrawing();
        }
    }

    public override void SetWindowIcon(string iconPath)
    {
        Raylib.SetWindowIcon(Raylib.LoadImage(iconPath));
    }

    public override void BeginDrawing() => Raylib.BeginDrawing();
    public override void EndDrawing() => Raylib.EndDrawing();
    public override void ClearBackground(Color color) => Raylib.ClearBackground(color);
    public override bool WindowShouldClose() => Raylib.WindowShouldClose();
}