using Raylib_cs;

namespace Nodica;

public class CheckBox : ClickableRectangle
{
    public Vector2 CheckSize = new(10, 10);
    public ButtonStylePack BackgroundStyle = new();
    public ButtonStylePack CheckStyle = new();
    public bool Checked = false;
    public Action<CheckBox> OnUpdate = (checkBox) => { };
    public event EventHandler? Toggled;

    public CheckBox()
    {
        Size = new(20, 20);
        OriginPreset = OriginPreset.Center;

        BackgroundStyle.Roundness = 1;

        CheckStyle.Normal.FillColor = new(71, 114, 179, 255);
        CheckStyle.Current = CheckStyle.Normal;
    }

    public override void Update()
    {
        Draw();
        HandleClicks();
        OnUpdate(this);
        base.Update();
    }

    protected override void Draw()
    {
        Rectangle rectangle = new()
        {
            Position = GlobalPosition - Origin,
            Size = Size
        };

        DrawInside(rectangle);
        DrawOutline(rectangle);
        DrawCheck();
    }

    private void DrawInside(Rectangle rectangle)
    {
        Raylib.DrawRectangleRounded(
            rectangle,
            BackgroundStyle.Current.Roundness,
            (int)Size.Y,
            BackgroundStyle.Current.FillColor);
    }

    private void DrawOutline(Rectangle rectangle)
    {
        if (BackgroundStyle.Current.BorderLength > 0)
        {
            Raylib.DrawRectangleRoundedLines(
                rectangle,
                BackgroundStyle.Current.Roundness,
                (int)Size.Y,
                BackgroundStyle.Current.BorderLength,
                BackgroundStyle.Current.OutlineColor);
        }
    }

    private void DrawCheck()
    {
        if (!Checked)
        {
            return;
        }

        Rectangle rectangle = new()
        {
            Position = GlobalPosition - Origin / 2,
            Size = CheckSize
        };

        Raylib.DrawRectangleRounded(
            rectangle,
            BackgroundStyle.Current.Roundness,
            (int)CheckSize.Y,
            CheckStyle.Current.FillColor);
    }

    private void HandleClicks()
    {
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            if (IsMouseOver() && OnTopLeft)
            {
                Checked = !Checked;
                Toggled?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}