﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace uidev.Controls
{
    public partial class FxTextbox : FxBaseControl
    {
        private FxTextboxBase textBox = new FxTextboxBase();

        [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
        static extern void SetCursorPos(int X, int Y);

        [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
        static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [DllImport("user32.dll")]
        static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);

        private const int MOUSEEVENTF_LEFTDOWN = 0x2;
        private const int MOUSEEVENTF_LEFTUP = 0x4;

        private Pen borderPen = Class.uiCustoms.BorderPen;

        private void InitTextBox()
        {
            textBox.Font = Class.uiCustoms.Font;
            textBox.Visible = false;
            textBox.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            textBox.BorderStyle = BorderStyle.None;
            textBox.PreviewKeyDown += TextBox_PreviewKeyDown;
        }

        private void UpdateTextBox()
        {
            textBox.Text = this.Text;
            textBox.Location = new Point(1, 1);
            textBox.Size = new Size(Width - 2, Height - 2);
        }

        private void FocusTextBox()
        {
            UpdateTextBox();
            textBox.Visible = true;
            textBox.Focus();
            Refresh();
        }

        private void UnFocusTextBox()
        {
            textBox.Visible = false;
            this.Text = textBox.Text;
            Refresh();
        }

        public FxTextbox()
        {
            InitializeComponent();

            InitTextBox();
            UpdateTextBox();

            this.Controls.Add(textBox);
        }

        private Size GetTextSize(string text)
        {
            return TextRenderer.MeasureText(text, Class.uiCustoms.Font, Size, TextFormatFlags.NoPadding);
        }

        private int Mouse2StringPoint(int x)
        {
            if (0 < Text.Length)
            {
                if (1 < Text.Length)
                {
                    for (int i = 1; i <= Text.Length; i++)
                    {
                        int half = (int)Math.Round((double)GetTextSize(Text.Substring((i - 1), 1)).Width / 2.0);
                        if (x < GetTextSize(Text.Substring(0, i)).Width - half)
                        {
                            return i - 1;
                        }
                    }
                }
                return Text.Length;
            }
            return 0;
        }

        private void FxTextbox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Class.uiCustoms.DarkColor);

            TextRenderer.DrawText(
                e.Graphics,
                Text,
                Class.uiCustoms.Font,
                new Point(3, Height / 2),
                Color.White,
                TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding
                );

            if (Border)
            {
                e.Graphics.DrawRectangle(borderPen, 0, 0, Width - 1, Height - 1);
            }
        }

        private void TextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                UnFocusTextBox();
            }
        }

        private void FxTextbox_MouseDown(object sender, MouseEventArgs e)
        {
            FocusTextBox();

            textBox.SelectionStart = Mouse2StringPoint(e.X);
            textBox.SelectionLength = 0;
            //mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
        }

        private void FxTextbox_MouseUp(object sender, MouseEventArgs e)
        {
            //mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        private void FxTextbox_Enter(object sender, EventArgs e)
        {
            FocusTextBox();
        }

        private void FxTextbox_Leave(object sender, EventArgs e)
        {
            UnFocusTextBox();
        }

        private void FxTextbox_Resize(object sender, EventArgs e)
        {
            Size = new Size(Width, 18);

            UpdateTextBox();
        }

    }
}
