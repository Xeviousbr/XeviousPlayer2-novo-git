#region Usings

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

#endregion

// This has been copied from PVSPlayerExample to display custom buttons on the main form

namespace XeviousPlayer2
{
    #region Global Color Scheme

    internal static class UIColors
    {
        /* ... under construction ... */

        // Buttons, DropDownButtons
        static internal Color BorderColor = Color.FromArgb(64, 64, 64); // 100
        //static internal Pen FocusPen = new Pen(Color.DarkGoldenrod);
        static internal Pen FocusPen = new Pen(Color.FromArgb(169, 163, 136));

        static internal Color NormalColor1A = Color.Gray;
        static internal Color NormalColor1B = Color.FromArgb(32, 32, 32);
        static internal Color NormalColor2A = Color.Black;
        static internal Color NormalColor2B = Color.FromArgb(48, 48, 48);

        static internal Color HotColor1A = Color.FromArgb(148, 148, 148);
        static internal Color HotColor1B = Color.FromArgb(32, 32, 32);
        static internal Color HotColor2A = Color.Black;
        static internal Color HotColor2B = Color.FromArgb(60, 60, 60); //60

        static internal Color PressedColor1A = Color.FromArgb(180, 180, 180); // 164
        static internal Color PressedColor1B = Color.FromArgb(40, 40, 40); // 48
        static internal Color PressedColor2A = Color.Black; // 18
        static internal Color PressedColor2B = Color.FromArgb(72, 72, 72); // 72

        // Sliders Thumb
        static internal Pen ThumbBorderPen = new Pen(Color.FromArgb(80, 80, 80));

        static internal Color NormalThumbColor1 = Color.FromArgb(132, 132, 132);
        static internal Color NormalThumbColor2 = Color.Black;

        static internal Color HotThumbColor1 = Color.FromArgb(148, 148, 148);
        static internal Color HotThumbColor2 = Color.FromArgb(18, 18, 18);

        static internal Color PressedThumbColor1 = Color.FromArgb(180, 180, 180); // 164
        static internal Color PressedThumbColor2 = Color.FromArgb(18, 18, 18);

        // Menus
        static internal Color MenuBackgroundColor = Color.FromArgb(32, 32, 32);
        static internal Color MenuMarginColor = Color.FromArgb(48, 48, 48);
        //static internal Color MenuBorderColor = Color.DimGray;
        static internal Color MenuBorderColor = Color.FromArgb(64, 64, 64);
        //static internal Color MenuBorderColor = Color.FromArgb(56, 56, 56);

        static internal Color MenuSeparatorDarkColor = Color.FromArgb(80, 80, 80);
        static internal Color MenuSeparatorLightColor = Color.DimGray;

        static internal Color MenuHighlightColor = Color.FromArgb(64, 24, 24);
        //static internal Color MenuHighlightColor2 = Color.FromArgb(18, 18, 18);
        //static internal Color MenuHighlightColor = Color.FromArgb(48, 48, 48);
        //static internal Color MenuHighlightBorderColor = Color.FromArgb(80, 80, 80);
        static internal Color MenuHighlightBorderColor = Color.FromArgb(64, 64, 64);

        //static internal Color MenuTextEnabledColor = Color.Goldenrod;
        static internal Color MenuTextEnabledColor = Color.FromArgb(169, 163, 136);
        static internal Color MenuTextDisabledColor = Color.DimGray;
    }

    #endregion

    #region Custom Control: CustomButton

    internal sealed class CustomButton : Button
    {
        // ************************************ Fields

        #region Fields

        private LinearGradientBrush _normalBrush1;
        private LinearGradientBrush _normalBrush2;
        private LinearGradientBrush _hotBrush1;
        private LinearGradientBrush _hotBrush2;
        private LinearGradientBrush _pressedBrush1;
        private LinearGradientBrush _pressedBrush2;

        private Rectangle _borderRect;
        private Rectangle _buttonRect1;
        private Rectangle _buttonRect2;

        private Pen _borderPen;
        private bool _hotButton;
        private bool _pressedButton;
        private bool _notifyDefault;
        private bool _notifyDefaultDraw;

        private TextFormatFlags _textFlags;

        private bool _disposed;

        #endregion

        // ************************************ Main

        #region Main

        public CustomButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.Opaque, true);
            TextAlign = ContentAlignment.MiddleCenter;
            _textFlags = new TextFormatFlags();
            _textFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;

            _borderPen = new Pen(UIColors.BorderColor);
            SetButtonRectangle();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control's focus border is drawn.
        /// </summary>
        [Category("Appearance"), Description("Indicates whether the control's focus border is drawn."), DefaultValue(false)]
        public bool FocusBorder
        {
            get { return _notifyDefaultDraw; }
            set { _notifyDefaultDraw = value; }
        }

        /// <summary>
        /// Gets or sets the control's border color.
        /// </summary>
        [Category("Appearance"), Description("The control's border color.")]
        public Color BorderColor
        {
            get { return _borderPen.Color; }
            set { _borderPen.Color = value; }
        }

        public override void NotifyDefault(bool value)
        {
            _notifyDefault = value;
            base.NotifyDefault(value);
        }

        // need this for strange spacing of WebDings chars
        [DefaultValue(ContentAlignment.MiddleCenter)]
        public override ContentAlignment TextAlign
        {
            get
            {
                return base.TextAlign;
            }
            set
            {
                switch (value)
                {
                    case ContentAlignment.MiddleLeft:
                        _textFlags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter;
                        break;
                    case ContentAlignment.MiddleCenter:
                        _textFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
                        break;
                    case ContentAlignment.MiddleRight:
                        _textFlags = TextFormatFlags.Right | TextFormatFlags.VerticalCenter;
                        break;

                    // This is only to get the pause/next/previous/stop symbols centered!
                    case ContentAlignment.BottomCenter:
                        _textFlags = TextFormatFlags.Bottom;
                        break;
                }
                base.TextAlign = value;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            SetButtonRectangle();
        }

        protected override void OnMouseEnter(EventArgs eventargs)
        {
            _hotButton = true;
            base.OnMouseEnter(eventargs);
        }

        protected override void OnMouseLeave(EventArgs eventargs)
        {
            _hotButton = false;
            base.OnMouseLeave(eventargs);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            _pressedButton = true;
            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            _pressedButton = false;
            Invalidate();
            base.OnMouseUp(mevent);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {

                if (disposing)
                {
                    _borderPen.Dispose();
                    _normalBrush1.Dispose();
                    _normalBrush2.Dispose();
                    _hotBrush1.Dispose();
                    _hotBrush2.Dispose();
                    _pressedBrush1.Dispose();
                    _pressedBrush2.Dispose();
                }
                base.Dispose(disposing);
                _disposed = true;
            }
        }

        #endregion

        // ************************************ OnPaint

        #region OnPaint

        protected override void OnPaint(PaintEventArgs pevent)
        {
            // erase background - dropped (dropped rounded corners)
            // pevent.Graphics.FillRectangle(eraseBrush, this.ClientRectangle);

            // draw fill
            if (_hotButton)
            {
                if (_pressedButton)
                {
                    pevent.Graphics.FillRectangle(_pressedBrush1, _buttonRect1);
                    pevent.Graphics.FillRectangle(_pressedBrush2, _buttonRect2);
                }
                else
                {
                    pevent.Graphics.FillRectangle(_hotBrush1, _buttonRect1);
                    pevent.Graphics.FillRectangle(_hotBrush2, _buttonRect2);
                }
            }
            else
            {
                pevent.Graphics.FillRectangle(_normalBrush1, _buttonRect1);
                pevent.Graphics.FillRectangle(_normalBrush2, _buttonRect2);
            }

            // draw border
            if (_notifyDefault && _notifyDefaultDraw) pevent.Graphics.DrawRectangle(UIColors.FocusPen, _borderRect);
            else pevent.Graphics.DrawRectangle(_borderPen, _borderRect);

            // draw text
            if (_textFlags == TextFormatFlags.Bottom)
            {
                // This is only to get the pause/next/previous/stop symbols centered!
                TextRenderer.DrawText(pevent.Graphics, Text, Font, new Point(5, 1),
                    Enabled ? ForeColor : Color.DimGray, Color.Transparent);
            }
            else
            {
                TextRenderer.DrawText(pevent.Graphics, Text, Font, ClientRectangle,
                    Enabled ? ForeColor : Color.DimGray, Color.Transparent, _textFlags);
            }
        }

        #endregion

        // ************************************ SetButtonRectangle

        #region SetButtonRectangle

        private void SetButtonRectangle()
        {
            // dropped rounded corners

            if (_normalBrush1 != null)
            {
                _normalBrush1.Dispose();
                _normalBrush2.Dispose();
                _hotBrush1.Dispose();
                _hotBrush2.Dispose();
                _pressedBrush1.Dispose();
                _pressedBrush2.Dispose();
            }
            _borderRect.Width = ClientRectangle.Width - 1;
            _borderRect.Height = ClientRectangle.Height - 1;

            _buttonRect1 = new Rectangle(0, 0, ClientRectangle.Width, ClientRectangle.Height / 2);
            // gradient fix with odd heights
            _buttonRect2 = _buttonRect1.Height % 2 == 0 ? new Rectangle(0, ClientRectangle.Height / 2, ClientRectangle.Width, ClientRectangle.Height / 2) : new Rectangle(0, ClientRectangle.Height / 2, ClientRectangle.Width, ClientRectangle.Height / 2 + 2);

            _normalBrush1 = new LinearGradientBrush(_buttonRect1, UIColors.NormalColor1A, UIColors.NormalColor1B, LinearGradientMode.Vertical);
            _normalBrush2 = new LinearGradientBrush(_buttonRect2, UIColors.NormalColor2A, UIColors.NormalColor2B, LinearGradientMode.Vertical);
            _hotBrush1 = new LinearGradientBrush(_buttonRect1, UIColors.HotColor1A, UIColors.HotColor1B, LinearGradientMode.Vertical);
            _hotBrush2 = new LinearGradientBrush(_buttonRect2, UIColors.HotColor2A, UIColors.HotColor2B, LinearGradientMode.Vertical);
            _pressedBrush1 = new LinearGradientBrush(_buttonRect1, UIColors.PressedColor1A, UIColors.PressedColor1B, LinearGradientMode.Vertical);
            _pressedBrush2 = new LinearGradientBrush(_buttonRect2, UIColors.PressedColor2A, UIColors.PressedColor2B, LinearGradientMode.Vertical);
        }

        #endregion
    }

    #endregion
}
