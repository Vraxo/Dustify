using Nodica;

namespace Dustify;

public class ImageDisplayer : CustomTexturedRectangle
{
    public Bitmap BitmapData;
    public float Speed = 5;
    public DisintegrationMode Mode = DisintegrationMode.AllAtOnce;

    private bool disintegrating = false;
    private int step = 4;
    private int currentRow = 0;

    public override void Update()
    {
        if (disintegrating)
        {
            Disintegrate();
        }

        base.Update();
    }

    public void StartDisintegration(DisintegrationMode mode)
    {
        Mode = mode;
        disintegrating = true;
        currentRow = 0;
    }

    private void OnTimerTimedOut(object? sender, EventArgs e)
    {
        Height = 0;
    }

    private void Disintegrate()
    {
        if (Mode == DisintegrationMode.RowByRow)
        {
            DisintegrateRowByRow();
        }
        else if (Mode == DisintegrationMode.AllAtOnce)
        {
            DisintegrateAllAtOnce();
        }
    }

    private void DisintegrateRowByRow()
    {
        if (currentRow * step < BitmapData.Height)
        {
            for (int x = 0; x < BitmapData.Width; x += step)
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

    private void DisintegrateAllAtOnce()
    {
        for (int y = 0; y < BitmapData.Height; y += step)
        {
            for (int x = 0; x < BitmapData.Width; x += step)
            {
                System.Drawing.Color color = BitmapData.GetPixel(x, y);
                Vector2 position = Position - Origin + new Vector2(x, y);

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
        }

        Visible = false;
        disintegrating = false;
    }
}