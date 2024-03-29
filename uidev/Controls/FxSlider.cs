﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace uidev.Controls
{
    public partial class FxSlider : FxBaseControl, Interface.FxUiBase
    {

        public delegate void SlideEvent(object sender, Class.SlideArgs e);
        public SlideEvent Slide;

        private float penWidth = 2F;

        const int knobMoveRange = 2;

        private int knobSize = 7;
        private int knobPoint = 7 + 2;

        private float yp
        {
            get
            {
                return (float)(Height - 1) / 2F;
            }
        }

        private bool MouseIsEnter = false;
        private bool MouseIsDown = false;

        private Color knobColor = Class.uiCustoms.MainColor;
        private Color mouseEnterKnobColor = Class.uiCustoms.MainEnterColor;
        private Color mouseClickKnobColor = Class.uiCustoms.MainClickColor;

        private Color mainColor = Color.SlateBlue;
        private Color mouseEnterColor = Color.FromArgb(170, 160, 226);
        private Color mouseClickColor = Color.GhostWhite;

        private Color borderColor = Class.uiCustoms.BorderPen.Color;

        private int _value;
        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                int v = value < MinimumValue ? MinimumValue : MaximumValue < value ? MaximumValue : value;
                _value = v;
                SetValue(v);
                Refresh();
            }
        }

        private int _minValue = 0;
        public int MinimumValue
        {
            get
            {
                return _minValue;
            }
            set
            {
                if (MaximumValue < value) return;
                _minValue = value;
            }
        }

        private int _maxValue = 100;
        public int MaximumValue
        {
            get
            {
                return _maxValue;
            }
            set
            {
                if (value < MinimumValue) return;
                _maxValue = value;
            }
        }

        private bool _border = false;
        public bool Border
        {
            get
            {
                return _border;
            }
            set
            {
                _border = value;
                Refresh();
            }
        }

        private Cursor _mouseOnTheLineCursor = Cursors.Default;
        public Cursor MouseOnTheLineCursor
        {
            get
            {
                return _mouseOnTheLineCursor;
            }
            set
            {
                _mouseOnTheLineCursor = value;
            }
        }

        public FxSlider()
        {

            InitializeComponent();

            this.MinimumSize = new Size(knobSize * 2 + knobMoveRange * 2, knobSize * 2);
        }

        public int GetValue()
        {
            int val = knobPoint - knobSize;
            if (val == 0) return MinimumValue;

            return (int)(val / ((this.Width - 1 - knobSize * 2)
                / (float)(MaximumValue - MinimumValue)) + MinimumValue);
        }
        public float GetValueFloat()
        {
            int val = knobPoint - knobSize;
            if (val == 0) return MinimumValue;

            return ((float)val / ((float)(this.Width - 1 - knobSize * 2)
                / (float)(MaximumValue - MinimumValue)) + (float)MinimumValue);
        }

        private int SetValue(int __value)
        {
            int v;

            if (__value < MinimumValue) { v = MinimumValue; Console.WriteLine("min"); }
            else if (MaximumValue < __value) { v = MaximumValue; Console.WriteLine("max"); }
            else
            {
                v = (int)((__value - MinimumValue) * ((Width - 1 - knobSize * 2)
              / (float)(MaximumValue - MinimumValue)) + knobSize);
            }

            knobPoint = v;

            Refresh();

            return v;
        }

        private void FxSlider_Paint(object sender, PaintEventArgs e)
        {

            e.Graphics.Clear(Class.uiCustoms.BackColor);

            PointF[] dps = new PointF[4];
            dps[0] = new PointF((float)knobPoint - (float)knobSize, yp);
            dps[1] = new PointF((float)knobPoint, yp - (float)knobSize);
            dps[2] = new PointF((float)knobPoint + (float)knobSize, yp);
            dps[3] = new PointF((float)knobPoint, yp + (float)knobSize);

            Pen linepen = new Pen(Color.Gray, penWidth);
            e.Graphics.DrawLine(
                linepen,
                new PointF(knobSize + 2, Height / 2 + 0.05F),
                new PointF(Width - 1 - (knobSize + 2), Height / 2 + 0.05F)
                );

            if (MouseIsDown)
            {
                e.Graphics.FillPolygon(new SolidBrush(mouseClickColor), dps);
            }
            else if (MouseIsEnter)
            {
                e.Graphics.FillPolygon(new SolidBrush(mouseEnterColor), dps);
            }
            else
            {
                e.Graphics.FillPolygon(new SolidBrush(knobColor), dps);
            }

            if (Border) e.Graphics.DrawRectangle(new Pen(borderColor), 0, 0, Width - 1, Height - 1);

        }

        private int ParentX = 0;
        private int GetParentX(Control c)
        {
            ParentX -= c.Location.X;
            return 0;
        }
        /*
        private void FxSlider_ThreadMainLoop(Point point)
        {

            ParentX = point.X;
            Class.Tools.DoAllParent(this, GetParentX);
            int futureX = ParentX - this.Location.X - knobSize;

            if (MouseIsDown)
            {
                knobPoint = knobSize < futureX ? (futureX < (this.Width - 1 - knobSize) ? futureX : this.Width - knobSize) : knobSize;
                _value = GetValue();
                Slide?.Invoke(_value, new EventArgs());
                // Console.WriteLine(GetValue());
            }
        }
        */

        private bool GetMouseInKnob(Point mouseP)
        {
            return  mouseP.X >= this.knobPoint - this.knobSize &&
                    mouseP.X <= this.knobPoint + this.knobSize &&
                    mouseP.Y >= yp - this.knobSize &&
                    mouseP.Y <= yp + this.knobSize;
        }

        private bool GetMouseOnTheLine(Point mouseP)
        {
            return mouseP.X >= knobSize &&
                    mouseP.X <= Width - (1 + knobSize) &&
                    mouseP.Y >= (Height / 2 + 0.05F) - penWidth &&
                    mouseP.Y <= (Height / 2 + 0.05F) + penWidth;
        }

        private void FxSlider_MouseMove(object sender, MouseEventArgs e)
        {
            Point mouseP = e.Location;

            if (Enabled && GetMouseOnTheLine(e.Location))
            {
                Cursor = MouseOnTheLineCursor;
            }
            else
            {
                Cursor = Cursors.Default;
            }

            if (MouseIsDown)
            {
                knobPoint = knobSize < mouseP.X ? (mouseP.X < (this.Width - 1 - knobSize) ? mouseP.X : this.Width - knobSize) : knobSize;
                _value = GetValue();
                Slide?.Invoke(_value, new Class.SlideArgs(GetValue()));
            }
            if (GetMouseInKnob(mouseP))
            {
                MouseIsEnter = true;
            }
            else
            {
                MouseIsEnter = false;
            }
            Refresh();
        }

        private void FxSlider_MouseDown(object sender, MouseEventArgs e) {
            if (Enabled && GetMouseInKnob(e.Location) && e.Button == MouseButtons.Left)
            {
                MouseIsDown = true;
            }
            else if (Enabled && GetMouseOnTheLine(e.Location))
            {
                knobPoint = e.X;
                if (e.Button == MouseButtons.Left)
                {
                    MouseIsDown = true;
                }
                _value = GetValue();
                Slide?.Invoke(_value, new Class.SlideArgs(GetValue()));
            }
            else
            {
                FindForm().ActiveControl = null;
            }
            Refresh();
        }

        private void FxSlider_MouseUp(object sender, MouseEventArgs e)
        {
            MouseIsDown = false;

            if (!GetMouseInKnob(e.Location))
            {
                MouseIsEnter = false;
            }

            Refresh();
        }

        private void FxSlider_Leave(object sender, EventArgs e)
        {
            MouseIsEnter = false;
            Refresh();
        }

        private void FxSlider_EnabledChanged(object sender, EventArgs e)
        {
            Color _knobColor = Class.uiCustoms.MainColor;
            Color _mouseEnterKnobColor = Class.uiCustoms.MainEnterColor;
            Color _mouseClickKnobColor = Class.uiCustoms.MainClickColor;

            Color _mainColor = Color.SlateBlue;
            Color _mouseEnterColor = Color.FromArgb(170, 160, 226);
            Color _mouseClickColor = Color.GhostWhite;

            Color _borderColor = Class.uiCustoms.BorderPen.Color;

            if (!Enabled)
            {
                knobColor = Class.Tools.rgb2gray(_knobColor);
                mouseEnterKnobColor = Class.Tools.rgb2gray(_knobColor);
                mouseClickKnobColor = Class.Tools.rgb2gray(_knobColor);

                mainColor = Class.Tools.rgb2gray(_mainColor);
                mouseEnterColor = Class.Tools.rgb2gray(_mainColor);
                mouseClickColor = Class.Tools.rgb2gray(_mainColor);

                borderColor = Class.Tools.rgb2gray(_borderColor);
            }
            else
            {
                knobColor = _knobColor;
                mouseEnterKnobColor = _mouseEnterKnobColor;
                mouseClickKnobColor = _mouseClickKnobColor;

                mainColor = _mainColor;
                mouseEnterColor = _mouseEnterColor;
                mouseClickColor = _mouseClickColor;

                borderColor = Class.uiCustoms.BorderPen.Color;
            }
            Refresh();
        }

        private void FxSlider_MouseLeave(object sender, EventArgs e)
        {
            MouseIsEnter = false;
            Refresh();
        }

        /*
        private void FxSlider_TheMouseMove(Point point)
        {
            int Lx = FindForm().Location.X;
            int Ly = FindForm().Location.Y;
            
            this.Location = new Point(point.X - Lx, point.Y - Ly);

            FindForm().Refresh();
        }
        */
    }
}
