using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls
{
    public class StringInputComboBox : ComboBox
    {
        public string _oldValue;
        private bool check = true;
        public string _value;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Value
        {
            get => _value;
            set
            {
                if (_value == value)
                {
                    return;
                }

                if (check == false)
                {
                    _oldValue = Text;
                }

                _value = value;

                UpdateText();

                if (check)
                {
                    _oldValue = Text;
                    check = false;
                }

                Apply();
            }
        }

        public StringInputComboBox()
        {
            UpdateText();
        }

        protected override void OnTextUpdate(EventArgs e)
        {
            base.OnTextUpdate(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            Apply();
            base.OnTextChanged(e);
        }

        public event EventHandler ValueChanged;

        protected override void OnLostFocus(EventArgs e)
        {
            Apply();
            base.OnLostFocus(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    UpdateText();
                    e.SuppressKeyPress = true;
                    break;

                case Keys.Enter:
                    Apply();
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;

                //case Keys.Space | Keys.Back:
                //case Keys.Space | Keys.MButton | Keys.RButton:
                //    break;

                case Keys.X:
                    if (e.Control)
                    {
                        Text = null;
                        Apply();
                    }

                    break;

                //case Keys.V:
                //case Keys.C:
                //    if (!e.Control)
                //        goto default;
                //    break;

                default:
                    //e.Handled = true;
                    //e.SuppressKeyPress = true;
                    break;
            }

            base.OnKeyDown(e);
        }

        private void UpdateText()
        {
            if (string.IsNullOrEmpty(_value))
            {
                Text = "";
            }
            else
            {
                Text = _value;
            }
        }

        private void Apply()
        {
            string val = _value;

            if (val == Text)
            {
                return;
            }

            val = Text;

            if (_value != val)
            {
                _value = val;
                ValueChanged?.Invoke(this, null);
            }

            UpdateText();
        }
    }
}