﻿using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace uidev.Interface
{
    public class FxAlwaysMouseActive : Controls.FxBaseControl
    {

        private Thread th;
        private Point _mp;
        private Point _mousePoint 
        {
            get
            {
                return _mp;
            }
            set
            {
                _mp = value;
                ThreadMainLoop?.Invoke(_mp);
            }
        }

        public Point MousePoint
        {
            get
            {
                return _mousePoint;
            }
        }

        public delegate void ThreadMainLoopEvent(Point point);
        public ThreadMainLoopEvent ThreadMainLoop;

        public FxAlwaysMouseActive()
        {
            this.SuspendLayout();

            //this.MouseDown += am_MouseDown;
            //this.MouseUp += am_MouseUp;

            this.ResumeLayout(false);

            th = new Thread(new ThreadStart(GetMousePosition));
        }

        public void GetMousePosition()
        {

            while (true)
            //while (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
            {
                _mousePoint = Cursor.Position;
                Thread.Sleep(10);
            }

        }

        public void StartThread()
        {
            th.Start();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
            
            th.Abort();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }
    }
}
