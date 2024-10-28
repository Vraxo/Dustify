namespace Nodica;

public partial class OptionButton : Button
{
    public class OptionButtonButton : Button
    {
        public int Index = 0;
        public bool Checked = false;

        public override void Build()
        {
            var parent = Parent as Node2D;
    
            AddChild(new CheckBox()
            {
                Position = new Vector2(-parent.Origin.X / 9 * 6, 0),
            });
        }
    
        public override void Start()
        {
            HorizontalAlignment = HorizontalAlignment.Right;
            TextOriginPreset = OriginPreset.None;
            TextOrigin = new(-4, 0);

            LeftClicked += OnLeftClicked;

            if (Checked)
            {
                GetNode<CheckBox>("CheckBox").Toggle();
            }

            base.Start();
        }

        private void OnLeftClicked(object? sender, EventArgs e)
        {
            (Parent as Nodica.OptionButton).Text = Text;
            (Parent as Nodica.OptionButton).Choice = Index;
            (Parent as Nodica.OptionButton).Close();
        }
    }
}