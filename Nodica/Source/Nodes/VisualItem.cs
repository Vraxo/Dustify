namespace Nodica;

public abstract class VisualItem : Node2D
{
    public override void Update()
    {
        if (Visible && ReadyForVisibility)
        {
            Draw();
        }

        base.Update();
    }

    protected abstract void Draw();
}