namespace Nodica;

public class RenderServer
{
    public static RenderServer Instance => instance ??= new();
    private static RenderServer? instance;

    public GraphicsBackend Backend;

    private List<DrawCommand> drawCommands = [];

    public void Add(DrawCommand drawCommand)
    {
        drawCommands.Add(drawCommand);
    }

    public void Remove(DrawCommand drawCommand)
    {
        drawCommands.Remove(drawCommand);
    }

    public void Process()
    {
        for (int i = 0; i < drawCommands.Count; i++)
        {
            drawCommands[i].Draw();
            drawCommands.Remove(drawCommands[i]);
        }
    }
}