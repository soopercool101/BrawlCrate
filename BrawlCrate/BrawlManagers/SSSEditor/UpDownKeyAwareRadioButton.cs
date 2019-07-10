using System.Windows.Forms;

namespace BrawlCrate.SSSEditor
{
    public class UpDownKeyAwareRadioButton : RadioButton
    {
        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Right:
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                    return true;
            }

            return base.IsInputKey(keyData);
        }
    }
}