using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls
{
    public class NumericInputBox : TextBox
    {
        public float _previousValue;
        public float? _value;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float Value
        {
            get
            {
                if (_value == null)
                {
                    Apply();
                }

                return _value.Value;
            }
            set
            {
                _value = value; //Clamp(_minValue, _maxValue);
                UpdateText();
            }
        }

        public NumericInputBox()
        {
            UpdateText();
        }

        public float MinimumValue
        {
            get => _minValue;
            set => _minValue = value;
        }

        public float MaximumValue
        {
            get => _maxValue;
            set => _maxValue = value;
        }

        public bool Integral
        {
            get => _integral;
            set => _integral = value;
        }

        public bool Integer
        {
            get => _integer;
            set => _integer = value;
        }

        public float _minValue = float.MinValue;
        public float _maxValue = float.MaxValue;
        public bool _integral;
        public bool _integer;

        public event EventHandler ValueChanged;

        protected override void OnLostFocus(EventArgs e)
        {
            Apply();
            base.OnLostFocus(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            float val;
            switch (e.KeyCode)
            {
                case Keys.D0:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                case Keys.NumPad0:
                case Keys.NumPad1:
                case Keys.NumPad2:
                case Keys.NumPad3:
                case Keys.NumPad4:
                case Keys.NumPad5:
                case Keys.NumPad6:
                case Keys.NumPad7:
                case Keys.NumPad8:
                case Keys.NumPad9:
                    if (Text.IndexOf('-') == 0 && SelectionStart == 0 && SelectionLength == 0)
                    {
                        e.SuppressKeyPress = true;
                    }
                    break;
                case Keys.Back:
                    break;

                case Keys.Left:
                    if (float.TryParse(Text, out val))
                    {
                        if (e.Control)
                        {
                            Text = (val - 90f).Clamp(_minValue, _maxValue).ToString();
                            Apply();
                            e.Handled = true;
                            e.SuppressKeyPress = true;
                        }

                        if (e.Shift)
                        {
                            Text = (val - 180f).Clamp(_minValue, _maxValue).ToString();
                            Apply();
                            e.Handled = true;
                            e.SuppressKeyPress = true;
                        }
                    }

                    break;

                case Keys.Right:
                    if (float.TryParse(Text, out val))
                    {
                        if (e.Control)
                        {
                            Text = (val + 90f).Clamp(_minValue, _maxValue).ToString();
                            Apply();
                            e.Handled = true;
                            e.SuppressKeyPress = true;
                        }

                        if (e.Shift)
                        {
                            Text = (val + 180f).Clamp(_minValue, _maxValue).ToString();
                            Apply();
                            e.Handled = true;
                            e.SuppressKeyPress = true;
                        }
                    }

                    break;

                case Keys.Up:
                    if (float.TryParse(Text, out val))
                    {
                        if (e.Shift || _integral)
                        {
                            Text = (val + 1.0f).Clamp(_minValue, _maxValue).ToString();
                        }
                        else
                        {
                            Text = (val + 0.1f).Clamp(_minValue, _maxValue).ToString();
                        }

                        Apply();
                    }

                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;

                case Keys.Down:
                    if (float.TryParse(Text, out val))
                    {
                        if (e.Shift || _integral)
                        {
                            Text = (val - 1.0f).Clamp(_minValue, _maxValue).ToString();
                        }
                        else
                        {
                            Text = (val - 0.1f).Clamp(_minValue, _maxValue).ToString();
                        }

                        Apply();
                    }

                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;

                case Keys.Subtract:
                case Keys.OemMinus:
                    if (SelectionStart != 0 || Text.Contains('-') && !SelectedText.Contains('-'))
                    {
                        e.SuppressKeyPress = true;
                    }

                    break;

                case Keys.Decimal:
                    if (CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.Contains(","))
                    {
                        goto case Keys.Oemcomma;
                    }

                    goto case Keys.OemPeriod;
                case Keys.OemPeriod:
                    if (Text.IndexOf('.') != -1 || Integer)
                    {
                        e.SuppressKeyPress = true;
                    }

                    break;
                case Keys.Oemcomma:
                    if (!CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.Contains(",") ||
                        Text.IndexOf(',') != -1 || Integer)
                    {
                        e.SuppressKeyPress = true;
                    }

                    break;
                case Keys.Escape:
                    UpdateText();
                    e.SuppressKeyPress = true;
                    break;

                case Keys.Enter:
                    Apply();
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;

                case Keys.X:
                    if (e.Control)
                    {
                        if (float.TryParse(Text, out val))
                        {
                            val = float.NaN;
                            Text = val.ToString();
                            Apply();
                        }
                    }

                    break;

                case Keys.V:
                case Keys.C:
                    if (!e.Control)
                    {
                        goto default;
                    }

                    break;
                case Keys.Delete:
                    break;
                default:
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
            }

            base.OnKeyDown(e);
        }

        private void UpdateText()
        {
            if (_value == float.NaN)
            {
                Text = "";
            }
            else
            {
                Text = _value.ToString();
            }
        }

        private void Apply()
        {
            float val = _value ?? 0.0f;
            int val2 = (int) val;

            if (_value != null && val.ToString() == Text)
            {
                return;
            }

            if (Text == "")
            {
                val = float.NaN;
            }
            else if (!_integral)
            {
                float.TryParse(Text, out val);
                val = val.Clamp(_minValue, _maxValue);
            }
            else
            {
                int.TryParse(Text, out val2);
                val2 = val2.Clamp(_minValue == float.MinValue ? int.MinValue : (int)_minValue, _maxValue == float.MaxValue ? int.MaxValue : (int)_maxValue);
            }

            if (!_integral)
            {
                if (_value != val)
                {
                    _previousValue = _value ?? 0.0f;
                    _value = val;
                    ValueChanged?.Invoke(this, null);
                }
            }
            else
            {
                if (_value != val2)
                {
                    _previousValue = _value ?? 0.0f;
                    _value = val2;
                    ValueChanged?.Invoke(this, null);
                }
            }

            UpdateText();
        }
    }
}