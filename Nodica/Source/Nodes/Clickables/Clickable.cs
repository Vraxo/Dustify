namespace Nodica;

public abstract class Clickable : VisualItem
{
    public bool OnTopLeft = false;
    public bool OnTopRight = false;

    public override void Start()
    {
        LayeredClickManager.Instance.Register(this);
    }

    public override void Destroy()
    {
        LayeredClickManager.Instance.Unregister(this);
        base.Destroy();
    }

    public abstract bool IsMouseOver();
}