namespace Nodica;

public partial class LineEdit : Button
{
    private class TextDisplayer : BaseText
    {
        public TextDisplayer(LineEdit parent) : base(parent)
        {
            this.parent = parent;
        }

        protected override string GetText()
        {
            return parent.Secret ?
                new string(parent.SecretCharacter, parent.Text.Length) :
                parent.Text.Substring(parent.TextStartIndex, Math.Min(parent.Text.Length - parent.TextStartIndex, parent.GetDisplayableCharactersCount()));
        }

        protected override bool ShouldSkipDrawing()
        {
            return false;
        }
    }
}