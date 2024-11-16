using Nodica;
using System.Reflection;

namespace Nodica;

public class App
{
    private static App? instance;
    public static App Instance => instance ??= new();

    private GraphicsBackendBase graphicsBackend;
    public Node RootNode;

    private App() { }

    public void Initialize(int width, int height, string title, GraphicsBackend backend)
    {
        graphicsBackend = backend switch
        {
            GraphicsBackend.Raylib => new RaylibBackend(),
            GraphicsBackend.SDL2 => new SDL2Backend(),
            _ => throw new NotImplementedException("Unsupported graphics backend")
        };

        graphicsBackend.Initialize(width, height, title);
        SetCurrentDirectory();
        SetWindowFlags();
    }

    public void Run()
    {
        graphicsBackend.Run();
    }

    public void SetRootNode(Node node, bool packedScene = false)
    {
        RootNode = node;

        if (!packedScene)
        {
            RootNode.Build();
        }
    }

    private static void SetCurrentDirectory()
    {
        string assemblyLocation = Assembly.GetEntryAssembly().Location;
        Environment.CurrentDirectory = Path.GetDirectoryName(assemblyLocation);
    }

    private static void SetWindowFlags()
    {
        // Platform-specific window flags (this might be Raylib-specific, and would be updated for SDL2)
    }

    private void ProcessSingletons()
    {
        ClickManager.Instance.Process();
        RenderServer.Instance.Process();
    }
}