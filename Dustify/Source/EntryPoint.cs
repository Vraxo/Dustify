using Nodica;

namespace Dustify;

public class EntryPoint
{
    [STAThread]
    public static void Main(string[] args)
    {
        App.Instance.Initialize(640, 720, "Dustify");

        var rootNode = new PackedScene("MainScene.txt").Instantiate<MainScene>(true);

        App.Instance.Run();
    }
}