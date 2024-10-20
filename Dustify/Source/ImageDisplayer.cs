using Nodica;

namespace Dustify;
// test
public class ImageDisplayer : CustomTexturedRectangle
{
    public Bitmap BitmapData;
    public float Speed = 5;

    private bool disintegrating = false;
    private int step = 4;
    private int currentRow = 0;

    private Timer timer;

    public override void Update()
    {
        if (disintegrating)
        {
            Disintegrate();
        }

        base.Update();
    }

    public void StartDisintegration()
    {
        disintegrating = true;
        currentRow = 0;
    }

    private void OnTimerTimedOut(object? sender, EventArgs e)
    {
        Height = 0;
    }

    private void Disintegrate()
    {
        if (currentRow * step < Size.Y)
        {
            for (int x = 0; x < Size.X; x += step)
            {
                System.Drawing.Color color = BitmapData.GetPixel(x, currentRow * step);
                Vector2 position = Position - Origin + new Vector2(x, currentRow * step);

                Particle particle = new()
                {
                    Position = position,
                    Style = new()
                    {
                        FillColor = new(color.R, color.G, color.B, color.A)
                    },
                    Speed = Speed
                };

                AddChild(particle);
            }

            currentRow++;
            Height += step;
        }
        else
        {
            disintegrating = false;
        }
    }
}