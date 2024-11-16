using Nodica;

namespace Dustify;

public class EntryPoint
{
    [STAThread]
    public static void Main(string[] args)
    {
        App.Instance.Initialize(640, 720, "Dustify");





        // Approach 1 - instantiate from a PackedScene file

        var rootNode = new PackedScene("MainScene.txt").Instantiate<MainScene>(true);
        //App.Instance.SetRootNode(rootNode, true);



        // Approach 2 - insantiate from an object
        //MainScene mainScene = new();
        //App.Instance.SetRootNode(mainScene);





        App.Instance.Run();
    }
}