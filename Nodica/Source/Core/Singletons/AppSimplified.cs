using Nodica;
using System.Reflection;

namespace Nodica;

public class App
{
    private static App? instance;
    public static App Instance => instance ??= new();

    public GraphicsBackendBase GraphicsBackend;
    public Node RootNode;

    private App() { }

    public void Initialize(int width, int height, string title, GraphicsBackend backend)
    {
        GraphicsBackend = backend switch
        {
            Nodica.GraphicsBackend.Raylib => new RaylibBackend(),
            Nodica.GraphicsBackend.SDL2 => new SDL2Backend(),
            _ => throw new NotImplementedException("Unsupported graphics backend")
        };

        RenderServer.Instance.Backend = backend;

        GraphicsBackend.Initialize(width, height, title);
        SetCurrentDirectory();
        SetWindowFlags();
    }

    public void Run()
    {
        GraphicsBackend.Run();
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