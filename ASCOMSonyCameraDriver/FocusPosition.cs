using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.SonyMirrorless
{
    public partial class FocusPosition : UserControl
    {
        public FocusPosition()
        {
            InitializeComponent();
        }

        private double position = 0;

        public double Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            double MarkerWidth = Width * 0.05;
            double WidthScale = Width - MarkerWidth;

            // Draw position as 5% of total width
            Rectangle pos = new Rectangle((int)(ClientRectangle.X + (Position * WidthScale)), ClientRectangle.Y, (int)MarkerWidth, ClientRectangle.Height);

            Brush fore = new SolidBrush(ForeColor);
            e.Graphics.FillRectangle(new SolidBrush(ForeColor), pos);
            fore.Dispose();
            /*StringFormat style = new StringFormat();
            style.Alignment = StringAlignment.Near;
            switch (alignmentValue)
            {
                case ContentAlignment.MiddleLeft:
                    style.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.MiddleRight:
                    style.Alignment = StringAlignment.Far;
                    break;
                case ContentAlignment.MiddleCenter:
                    style.Alignment = StringAlignment.Center;
                    break;
            }

            // Call the DrawString method of the System.Drawing class to write
            // text. Text and ClientRectangle are properties inherited from
            // Control.
            e.Graphics.DrawString(
                Text,
                Font,
                new SolidBrush(ForeColor),
                ClientRectangle, style);*/
        }
    }
}
