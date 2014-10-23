﻿using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;


namespace ZD.Gui.Zen
{
    public partial class ZenTabbedForm : ZenControlBase, IDisposable, IZenTabsChangedListener
    {
        private readonly ZenTimer zenTimer;
        private readonly ZenWinForm form;
        private Size logicalMinimumSize = Size.Empty;

        private readonly int headerHeight;
        private readonly int innerPadding;
        private string header;
        private string headerEllipsed = null;
        private ZenCloseControl ctrlClose;
        private ZenTabControl mainTabCtrl;
        private readonly List<ZenTabControl> contentTabControls = new List<ZenTabControl>();
        private bool controlsCreated = false;
        private ZenTab mainTab;
        private readonly ZenTabCollection tabs;
        private int activeTabIdx = 0;
        private Cursor desiredCursor = Cursors.Arrow;
        private readonly int tooltipPadding;
        private readonly Dictionary<ZenControlBase, TooltipInfo> tooltipInfos = new Dictionary<ZenControlBase, TooltipInfo>();

        public ZenTabbedForm()
            : base(null)
        {
            zenTimer = new ZenTimer(this);
            form = new ZenWinForm(DoPaint);

            headerHeight = (int)(ZenParams.HeaderHeight * Scale);
            innerPadding = (int)(ZenParams.InnerPadding * Scale);
            tooltipPadding = (int)(ZenParams.TooltipPadding * Scale);

            createZenControls();
            tabs = new ZenTabCollection(this);

            form.SizeChanged += onFormSizeChanged;
            form.MouseDown += onFormMouseDown;
            form.MouseMove += onFormMouseMove;
            form.MouseUp += onFormMouseUp;
            form.MouseClick += onFormMouseClick;
            form.MouseEnter += onFormMouseEnter;
            form.MouseLeave += onFormMouseLeave;
            form.FormClosed += onFormClosed;
            form.Load += onFormLoaded;
        }

        public Form WinForm
        {
            get { return form; }
        }

        /// <summary>
        /// Gets or sets the form's cursor.
        /// </summary>
        public override sealed Cursor Cursor
        {
            get { return form.Cursor; }
            set { desiredCursor = value; form.Cursor = value; }
        }

        public override float Scale
        {
            get { return form.Scale; }
        }

        private Size ContentSize
        {
            get
            {
                return new Size(
                    form.Width - 2 * (int)ZenParams.InnerPadding,
                    form.Height - (int)ZenParams.InnerPadding - (int)ZenParams.HeaderHeight);
            }
        }

        private Point ContentLocation
        {
            get
            {
                return new Point((int)ZenParams.InnerPadding, (int)(ZenParams.HeaderHeight + ZenParams.InnerPadding));
            }
        }

        protected ZenTab MainTab
        {
            get { return mainTab; }
            set
            {
                mainTab = value;
                mainTab.Ctrl.AbsLocation = ContentLocation;
                mainTab.Ctrl.Size = ContentSize;
                mainTabCtrl.Text = mainTab.Header;
                mainTabCtrl.Size = new Size(mainTabCtrl.PreferredWidth, mainTabCtrl.Height);
            }
        }

        protected ZenTabCollection Tabs
        {
            get { return tabs; }
        }

        protected string Header
        {
            get { return header; }
            set { header = value; headerEllipsed = null; doRepaint(); form.Invalidate(); }
        }

        /// <summary>
        /// Index of the active tab. 0 is first content tab; -1 is main tab.
        /// </summary>
        protected int ActiveTabIdx
        {
            get { return activeTabIdx; }
        }

        public override sealed Size LogicalSize
        {
            set
            {
                float w = value.Width;
                float h = value.Height;
                Size s = new Size((int)(w * Scale), (int)(h * Scale));
                form.Size = s;
            }
            get
            {
                Rectangle rect = form.DisplayRectangle;
                return new Size((int)Math.Ceiling((rect.Width / Scale)), (int)Math.Ceiling(rect.Height / Scale));
            }
        }

        public Size LogicalMinimumSize
        {
            get { return logicalMinimumSize; }
            set { logicalMinimumSize = value; }
        }

        protected override sealed Point MousePositionAbs
        {
            get { return form.PointToClient(form.MousePosition); }
        }

        public override sealed Rectangle AbsRect
        {
            get { return new Rectangle(0, 0, form.Width, form.Height); }
        }

        void IZenTabsChangedListener.ZenTabsChanged()
        {
            // Remove old tab controls in header, re-add them
            List<ZenControl> toRemove = new List<ZenControl>();
            foreach (ZenControl zc in ZenChildren)
            {
                ZenTabControl ztc = zc as ZenTabControl;
                if (ztc == null || ztc.IsMain) continue;
                toRemove.Add(zc);
            }
            foreach (ZenControl zc in toRemove)
            {
                RemoveChild(zc);
                zc.Dispose();
            }
            contentTabControls.Clear();
            // Recreate all header tabs; add content control to form's children
            int posx = mainTabCtrl.AbsLocation.X + mainTabCtrl.Width;
            for (int i = 0; i != tabs.Count; ++i)
            {
                ZenTabControl tc = new ZenTabControl(this, false);
                tc.Text = tabs[i].Header;
                tc.LogicalSize = new Size(80, 30);
                tc.Size = new Size(tc.PreferredWidth, tc.Height);
                tc.AbsLocation = new Point(posx, headerHeight - tc.Height);
                posx += tc.Width;
                tc.MouseClick += tabCtrl_MouseClick;
                contentTabControls.Add(tc);
            }
            // If this is the first content tab being added, this will be the active (visible) one
            if (tabs.Count == 1)
            {
                RemoveChild(mainTab.Ctrl);
                contentTabControls[0].Selected = true;
            }
            // Must arrange controls - so newly added, and perhaps displayed, content tab
            // gets sized and placed
            arrangeControls();
            // Redraw
            doRepaint();
            form.Invalidate();
        }

        private void onFormClosed(object sender, FormClosedEventArgs e)
        {
            Dispose();
        }

        public override void Dispose()
        {
            form.Dispose();
            base.Dispose();
        }

        private void createZenControls()
        {
            ctrlClose = new ZenCloseControl(this);
            ctrlClose.LogicalSize = new Size(40, 20);
            ctrlClose.AbsLocation = new Point(form.Width - ctrlClose.Width - innerPadding, 0);
            ctrlClose.MouseClick += ctrlClose_MouseClick;

            mainTabCtrl = new ZenTabControl(this, true);
            mainTabCtrl.Text = "Main";
            mainTabCtrl.LogicalSize = new Size(80, 30);
            mainTabCtrl.Size = new Size(mainTabCtrl.PreferredWidth, mainTabCtrl.Height);
            mainTabCtrl.AbsLocation = new Point(1, headerHeight - mainTabCtrl.Height);
            mainTabCtrl.MouseClick += tabCtrl_MouseClick;

            controlsCreated = true;
        }

        private void arrangeControls()
        {
            if (!controlsCreated) return;

            headerEllipsed = null;
            
            // Resize and place active content tab, if any
            foreach (ZenTab zt in tabs)
            {
                if (!form.Created || ZenChildren.Contains(zt.Ctrl))
                {
                    zt.Ctrl.AbsLocation = new Point(innerPadding, headerHeight + innerPadding);
                    zt.Ctrl.Size = new Size(
                        form.Width - 2 * innerPadding,
                        form.Height - 2 * innerPadding - headerHeight);
                }
            }
            // Resize main tab, if active
            if (mainTab != null && (!form.Created || ZenChildren.Contains(mainTab.Ctrl)))
            {
                mainTab.Ctrl.AbsLocation = new Point(innerPadding, headerHeight + innerPadding);
                mainTab.Ctrl.Size = new Size(
                    form.Width - 2 * innerPadding,
                    form.Height - 2 * innerPadding - headerHeight);
            }

            ctrlClose.AbsLocation = new Point(form.Width - ctrlClose.Size.Width - innerPadding, 0);
        }

        private void ctrlClose_MouseClick(ZenControlBase sender)
        {
            form.Close();
        }


        private void tabCtrl_MouseClick(ZenControlBase sender)
        {
            ZenTabControl ztc = sender as ZenTabControl;
            // Click on active tab - nothing to do
            if (ztc.IsMain && activeTabIdx == -1) return;
            int idx = contentTabControls.IndexOf(ztc);
            if (idx == activeTabIdx) return;
            // Switching to main
            if (idx == -1)
            {
                contentTabControls[activeTabIdx].Selected = false;
                mainTabCtrl.Selected = true;
                RemoveChild(tabs[activeTabIdx].Ctrl);
                AddChild(mainTab.Ctrl);
                activeTabIdx = -1;
            }
            // Switching away to a content tab
            else
            {
                mainTabCtrl.Selected = false;
                contentTabControls[idx].Selected = true;
                RemoveChild(mainTab.Ctrl);
                AddChild(tabs[idx].Ctrl);
                activeTabIdx = idx;
            }
            // Newly active contol still has old size if window was resized
            arrangeControls();
            // Refresh
            doRepaint();
            form.Invalidate();
        }

        private void onFormSizeChanged(object sender, EventArgs e)
        {
            arrangeControls();
            doRepaint();
            form.Update();
        }

        /// <summary>
        /// Seals chain. Returns this form's zen timer.
        /// </summary>
        internal sealed override ZenTimer Timer
        {
            get { return zenTimer; }
        }

        protected override sealed void RegisterWinFormsControl(Control c)
        {
            AddWinFormsControl(c);
        }

        internal sealed override void AddWinFormsControl(Control c)
        {
            form.Controls.Add(c);
        }

        internal sealed override void RemoveWinFormsControl(Control c)
        {
            form.Controls.Remove(c);
        }

        protected override sealed void InvokeOnForm(Delegate method)
        {
            form.Invoke(method);
        }

        /// <summary>
        /// Handles the timer event for animations. Unsubscribes when timer no longer needed.
        /// </summary>
        public override void DoTimer(out bool? needBackground, out RenderMode? renderMode)
        {
            bool timerNeeded = false;
            bool paintNeeded = false;
            {
                bool tn, pn;
                doTimerTooltip(out tn, out pn);
                timerNeeded |= tn;
                paintNeeded |= pn;
            }
            if (!timerNeeded) UnsubscribeFromTimer();
            if (paintNeeded) { needBackground = false; renderMode = RenderMode.Invalidate; }
            else { needBackground = null; renderMode = null; }
        }

        private void doRepaint()
        {
            ZenWinForm.PaintCanvas pc = null;
            try
            {
                pc = form.GetBitmapRenderer();
                if (pc == null) return;
                Graphics g = pc.Graphics;
                DoPaint(g);
            }
            finally
            {
                if (pc != null) pc.Dispose();
            }
        }

        /// <summary>
        /// Calls each control's paint function. Or repaints entire canvas (all controls) if needed.
        /// </summary>
        internal override sealed void MakeControlsPaint(ReadOnlyCollection<ControlToPaint> ctrls)
        {
            ZenWinForm.PaintCanvas pc = null;
            // Strongest (most immediate) render mode requested
            RenderMode rm = RenderMode.None;
            try
            {
                pc = form.GetBitmapRenderer();
                if (pc == null) return;
                Graphics g = pc.Graphics;
                // Paint only individual controls, or full canvas?
                bool needBackground = false;
                // If a control requests its own repaint (e.g., from animation), AND there are tooltips
                // -> We must repaint full canvas and put tooltips on top, to avoid crazy flicker
                List<TooltipToPaint> ttpList = getTooltipsToPaint();
                if (ttpList.Count != 0)
                {
                    needBackground = true;
                    rm = RenderMode.Invalidate;
                }
                // Any control specifically requesting background paint?
                if (!needBackground)
                    foreach (ControlToPaint ctp in ctrls)
                        if (ctp.NeedBackground || ctp.Ctrl == this) { needBackground = true; break; }

                // Do the painting
                if (needBackground) DoPaint(g);
                // Collect control's render mode wishes; paint them individually if needed
                foreach (ControlToPaint ctp in ctrls)
                {
                    if (ctp.RenderMode > rm) rm = ctp.RenderMode;
                    if (!needBackground)
                    {
                        g.ResetTransform(); g.ResetClip();
                        g.TranslateTransform(ctp.Ctrl.AbsLeft, ctp.Ctrl.AbsTop);
                        g.Clip = new Region(new Rectangle(0, 0, ctp.Ctrl.Width, ctp.Ctrl.Height));
                        ctp.Ctrl.DoPaint(g);
                    }
                }
            }
            finally
            {
                if (pc != null) pc.Dispose();
            }
            if (rm == RenderMode.None) return;
            if (form.InvokeRequired)
            {
                try
                {
                    form.Invoke((MethodInvoker)delegate
                    {
                        if (rm == RenderMode.Invalidate) form.Invalidate();
                        else form.Refresh();
                    });
                }
                catch (ObjectDisposedException)
                {
                    // We just swallow this.
                    // Cannot prevent timer threads from requesting a repaint
                    // As window gets disposed in GUI thread
                }
            }
            else
            {
                if (rm == RenderMode.Invalidate) form.Invalidate();
                else form.Refresh();
            }
        }


        private enum DragMode
        {
            None,
            ResizeW,
            ResizeE,
            ResizeN,
            ResizeS,
            ResizeNW,
            ResizeSE,
            ResizeNE,
            ResizeSW,
            Move
        }

        private DragMode dragMode = DragMode.None;
        private Point dragStart;
        private Point formBeforeDragLocation;
        private Size formBeforeDragSize;

        private DragMode getDragArea(Point p)
        {
            Rectangle rHeader = new Rectangle(
                innerPadding,
                innerPadding,
                form.Width - 2 * innerPadding,
                headerHeight - innerPadding);
            if (rHeader.Contains(p))
                return DragMode.Move;
            Rectangle rEast = new Rectangle(
                form.Width - innerPadding,
                0,
                innerPadding,
                form.Height);
            if (rEast.Contains(p))
            {
                if (p.Y < 2 * innerPadding) return DragMode.ResizeNE;
                if (p.Y > form.Height - 2 * innerPadding) return DragMode.ResizeSE;
                return DragMode.ResizeE;
            }
            Rectangle rWest = new Rectangle(
                0,
                0,
                innerPadding,
                form.Height);
            if (rWest.Contains(p))
            {
                if (p.Y < 2 * innerPadding) return DragMode.ResizeNW;
                if (p.Y > form.Height - 2 * innerPadding) return DragMode.ResizeSW;
                // On the west, do not use border right next to main tab - 1px hot area looks silly
                if (p.X == 0 && p.Y >= mainTabCtrl.AbsTop && p.Y <= mainTabCtrl.AbsBottom)
                    return DragMode.None;
                return DragMode.ResizeW;
            }
            Rectangle rNorth = new Rectangle(
                0,
                0,
                form.Width,
                innerPadding);
            if (rNorth.Contains(p))
            {
                if (p.X < 2 * innerPadding) return DragMode.ResizeNW;
                if (p.X > form.Width - 2 * innerPadding) return DragMode.ResizeNE;
                return DragMode.ResizeN;
            }
            Rectangle rSouth = new Rectangle(
                0,
                form.Height - innerPadding,
                form.Width,
                innerPadding);
            if (rSouth.Contains(p))
            {
                if (p.X < 2 * innerPadding) return DragMode.ResizeSW;
                if (p.X > form.Width - 2 * innerPadding) return DragMode.ResizeSE;
                return DragMode.ResizeS;
            }
            return DragMode.None;
        }

        public override void DoMouseLeave()
        {
            base.DoMouseLeave();
            // After dragging window border, reset cursor
            if (dragMode == DragMode.None || dragMode == DragMode.Move)
                form.Cursor = Cursors.Arrow;
        }

        private void onFormMouseLeave(object sender, EventArgs e)
        {
            DoMouseLeave();
        }

        private void onFormMouseEnter(object sender, EventArgs e)
        {
            DoMouseEnter();
        }

        private void onFormMouseClick(object sender, MouseEventArgs e)
        {
            DoMouseClick(e.Location, e.Button);
        }

        private void onFormMouseDown(object sender, MouseEventArgs e)
        {
            // Over any control? Not our job to handle.
            if (DoMouseDown(e.Location, e.Button))
                return;

            // Resizing at window border
            var area = getDragArea(e.Location);
            if (area == DragMode.Move)
            {
                dragMode = DragMode.Move;
                dragStart = form.PointToScreen(e.Location);
                formBeforeDragLocation = form.Location;
            }
            else if (area != DragMode.None && area != DragMode.Move)
            {
                dragMode = area;
                dragStart = form.PointToScreen(e.Location);
                formBeforeDragSize = form.Size;
                formBeforeDragLocation = form.Location;
            }
        }

        private void onFormMouseUp(object sender, MouseEventArgs e)
        {
            // Have been resizing or moving? Indicate it's now done.
            if (dragMode != DragMode.None) DoMoveResizeFinished();
            // Resize by dragging border ends
            dragMode = DragMode.None;
            // Handle otherwise
            DoMouseUp(e.Location, e.Button);
        }

        /// <summary>
        /// Takes a size, and calculates difference from real (scaled) minimum size if smaller.
        /// In dimension that's OK, returns 0.
        /// </summary>
        private Size clipDiffFromMinSize(Size sz)
        {
            int w = (int)(((float)logicalMinimumSize.Width) * Scale);
            int h = (int)(((float)logicalMinimumSize.Height) * Scale);
            if (sz.Width < w) w = w - sz.Width;
            else w = 0;
            if (logicalMinimumSize.Width == 0) w = 0;
            if (sz.Height < h) h = h - sz.Height;
            else h = 0;
            if (logicalMinimumSize.Height == 0) h = 0;
            return new Size(w, h);
        }

        /// <summary>
        /// Called when form has finished moving or resizing.
        /// </summary>
        protected virtual void DoMoveResizeFinished()
        {
        }

        public override bool DoMouseMove(Point p, MouseButtons button)
        {
            // Window being resized by dragging at border
            Point loc = form.PointToScreen(p);
            if (dragMode == DragMode.Move)
            {
                int dx = loc.X - dragStart.X;
                int dy = loc.Y - dragStart.Y;
                Point newLocation = new Point(formBeforeDragLocation.X + dx, formBeforeDragLocation.Y + dy);
                ((Form)form.TopLevelControl).Location = newLocation;
                return true;
            }
            else if (dragMode == DragMode.ResizeE)
            {
                int dx = loc.X - dragStart.X;
                Size sz = new Size(formBeforeDragSize.Width + dx, formBeforeDragSize.Height);
                Size dmin = clipDiffFromMinSize(sz);
                form.Size = sz + dmin;
                form.Refresh();
                return true;
            }
            else if (dragMode == DragMode.ResizeW)
            {
                int dx = loc.X - dragStart.X;
                form.Left = formBeforeDragLocation.X + dx;
                form.Size = new Size(formBeforeDragSize.Width - dx, formBeforeDragSize.Height);
                form.Refresh();
                return true;
            }
            else if (dragMode == DragMode.ResizeN)
            {
                int dy = loc.Y - dragStart.Y;
                form.Top = formBeforeDragLocation.Y + dy;
                form.Size = new Size(formBeforeDragSize.Width, formBeforeDragSize.Height - dy);
                form.Refresh();
                return true;
            }
            else if (dragMode == DragMode.ResizeS)
            {
                int dy = loc.Y - dragStart.Y;
                Size sz = new Size(formBeforeDragSize.Width, formBeforeDragSize.Height + dy);
                Size dmin = clipDiffFromMinSize(sz);
                form.Size = sz + dmin;
                form.Refresh();
                return true;
            }
            else if (dragMode == DragMode.ResizeNW)
            {
                int dx = loc.X - dragStart.X;
                int dy = loc.Y - dragStart.Y;
                form.Size = new Size(formBeforeDragSize.Width - dx, formBeforeDragSize.Height - dy);
                form.Location = new Point(formBeforeDragLocation.X + dx, formBeforeDragLocation.Y + dy);
                form.Refresh();
                return true;
            }
            else if (dragMode == DragMode.ResizeSE)
            {
                int dx = loc.X - dragStart.X;
                int dy = loc.Y - dragStart.Y;
                form.Size = new Size(formBeforeDragSize.Width + dx, formBeforeDragSize.Height + dy);
                form.Refresh();
                return true;
            }
            else if (dragMode == DragMode.ResizeNE)
            {
                int dx = loc.X - dragStart.X;
                int dy = loc.Y - dragStart.Y;
                form.Size = new Size(formBeforeDragSize.Width + dx, formBeforeDragSize.Height - dy);
                form.Location = new Point(formBeforeDragLocation.X, formBeforeDragLocation.Y + dy);
                form.Refresh();
                return true;
            }
            else if (dragMode == DragMode.ResizeSW)
            {
                int dx = loc.X - dragStart.X;
                int dy = loc.Y - dragStart.Y;
                form.Size = new Size(formBeforeDragSize.Width - dx, formBeforeDragSize.Height + dy);
                form.Location = new Point(formBeforeDragLocation.X + dx, formBeforeDragLocation.Y);
                form.Refresh();
                return true;
            }

            // Over a control of ours? If yes, we're done.
            if (base.DoMouseMove(p, button))
            {
                form.Cursor = desiredCursor;
                return true;
            }

            // Switch cursor when moving over resize hot areas in border
            var area = getDragArea(p);
            if (area == DragMode.ResizeW || area == DragMode.ResizeE)
                form.Cursor = Cursors.SizeWE;
            else if (area == DragMode.ResizeN || area == DragMode.ResizeS)
                form.Cursor = Cursors.SizeNS;
            else if (area == DragMode.ResizeNW || area == DragMode.ResizeSE)
                form.Cursor = Cursors.SizeNWSE;
            else if (area == DragMode.ResizeNE || area == DragMode.ResizeSW)
                form.Cursor = Cursors.SizeNESW;
            else form.Cursor = Cursors.Arrow;

            return true;
        }

        private void onFormMouseMove(object sender, MouseEventArgs e)
        {
            DoMouseMove(e.Location, e.Button);
        }

        private void onFormLoaded(object sender, EventArgs e)
        {
            OnFormLoaded();
        }
    }
}