using Nodica;

namespace Dustify;

public class EntryPoint
{
    [STAThread]
    public static void Main(string[] args)
    {
        App.Instance.Initialize(1280, 720, "Dustify", args);

        var rootNode = new Scene("MainScene.txt").Instantiate<MainScene>(true);

        App.Instance.Run();
    }
}