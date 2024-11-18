namespace Nodica;

public class TextureRectangle : VisualItem
{
    public Texture? Texture { get; set; }

    public TextureRectangle()
    {
        Size = new(32, 32);
    }

    public override void Update()
    {
        Draw();
        base.Update();
    }

    protected override void Draw()
    {
        DrawTexture(
            Texture,
            GlobalPosition - Origin,
            Color.White);
    }
}