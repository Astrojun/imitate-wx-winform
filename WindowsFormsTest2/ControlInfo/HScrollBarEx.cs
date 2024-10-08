﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WindowsFormsTest2.ControlInfo
{
    public partial class HScrollBarEx : HScrollBar
    {
        #region  字段

        private const int RADIUS = 8;  //滚动条四角绘制直径
        private const int SMALLLENGTH = 20;  //滚动条最短长度
        private bool _isBarGotLButton = false;  //滚动条是否获取到焦点
        private Point _movePoint;  //移动的位置
        private Rectangle _barRectangle;  //滚动条区域
        private ScrollOrientation scrollOrientation;
        private int wheelDelta;

        private int value = 0;
        private int minimum = 0;
        private int maximum = 100;
        private int largeChange = 10;
        private int smallChange = 1;
        private Color barColor = Color.FromArgb(147, 147, 147);
        private bool isGotParentMouseWheel = true;

        #endregion

        #region  属性

        /// <summary>
        /// 滚动条颜色
        /// </summary>
        [DefaultValue(typeof(Color), "147,147,147"), RefreshProperties(RefreshProperties.Repaint), Description("滚动条颜色")]
        public Color BarColor
        {
            get
            {
                return barColor;
            }
            set
            {
                barColor = value;
                this.Invalidate(_barRectangle);
            }
        }

        /// <summary>
        /// 滚动条当前位置表示的值
        /// </summary>
        [DefaultValue(0), RefreshProperties(RefreshProperties.Repaint), Description("滚动条当前位置表示的值")]
        public new int Value
        {
            get { return value; }
            set
            {
                if (this.value != value)
                {
                    if ((value < this.minimum) || (value > this.maximum))
                    {
                        throw new ArgumentOutOfRangeException("Value", "Value应介于Maximum和Minimum之间");
                    }
                    this.value = Math.Min(value, this.maximum - this.minimum - this.LargeChange);
                    _barRectangle.X = (this.maximum == this.minimum || this.maximum == this.LargeChange) ? 0 : this.value * (Width - (_barRectangle.Width > SMALLLENGTH ? 
                        _barRectangle.Width : SMALLLENGTH)) / (this.maximum - this.minimum - this.LargeChange);
                    base.OnValueChanged(EventArgs.Empty);
                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// 可滚动的下限值
        /// </summary>
        [RefreshProperties(RefreshProperties.Repaint), DefaultValue(0), Description("可滚动的下限值")]
        public new int Minimum
        {
            get
            {
                return this.minimum;
            }
            set
            {
                if (this.minimum != value)
                {
                    if (this.maximum < value)
                    {
                        this.maximum = value;
                    }
                    if (value > this.value)
                    {
                        this.value = value;
                    }
                    this.minimum = value;
                }
            }
        }

        /// <summary>
        /// 可滚动的上限值
        /// </summary>
        [RefreshProperties(RefreshProperties.Repaint), DefaultValue(100), Description("可滚动的上限值")]
        public new int Maximum
        {
            get
            {
                return this.maximum;
            }
            set
            {
                if (this.maximum != value)
                {
                    if (this.minimum > value)
                    {
                        this.minimum = value;
                    }
                    if (value < this.value)
                    {
                        this.Value = value;
                    }
                    this.maximum = value;
                    int barWidth = maximum == minimum ? SMALLLENGTH : largeChange * Width / (maximum - minimum);
                    _barRectangle.Width = barWidth >= SMALLLENGTH ? barWidth : SMALLLENGTH;
                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// 当用户点击滚动条背景区域或按Page Up、Page Down键时，滚动条位置变化的幅度
        /// </summary>
        [DefaultValue(10), Description("当用户点击滚动条背景区域或按Page Up、Page Down键时，滚动条位置变化的幅度")]
        public new int LargeChange
        {
            get
            {
                return Math.Min(this.largeChange, (this.maximum - this.minimum));
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("LargeChange", "LargeChange必须大于或等于0");
                }
                if (this.largeChange != value)
                {
                    this.largeChange = value;
                    int barWidth = maximum == minimum ? SMALLLENGTH : largeChange * Width / (maximum - minimum);
                    _barRectangle.Width = barWidth >= SMALLLENGTH ? barWidth : SMALLLENGTH;
                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// 当用户使用滚轮时，滚动条位置变化的幅度
        /// </summary>
        [DefaultValue(1), Description("当用户使用滚轮时，滚动条位置变化的幅度")]
        public new int SmallChange
        {
            get
            {
                return Math.Min(this.smallChange, this.LargeChange);
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("SmallChange", "SmallChange必须大于或等于0");
                }
                if (this.smallChange != value)
                {
                    this.smallChange = value;
                }
            }
        }

        /// <summary>
        /// 是否获取父容器滚轮焦点
        /// </summary>
        [DefaultValue(true), Browsable(false)]
        public bool IsGotParentMouseWheel
        {
            get { return isGotParentMouseWheel; }
            set { isGotParentMouseWheel = value; }
        }

        #endregion

        #region  构造函数

        /// <summary>
        /// 构造垂直滚动条新实例
        /// </summary>
        public HScrollBarEx()
        {
            InitializeComponent();
            scrollOrientation = ScrollOrientation.HorizontalScroll;
            SetStyle(ControlStyles.UserPaint | ControlStyles.UserMouse | ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.StandardClick, true);

            _barRectangle = new Rectangle(0, 0, SMALLLENGTH, Height);
        }

        #endregion

        #region  方法

        public void ResizeBarRectangle()
        {
            if (Visible)
            {
                if (value > maximum - LargeChange && maximum - LargeChange >= minimum)
                    Value = maximum - LargeChange;
                int barWidth = maximum == minimum ? SMALLLENGTH : largeChange * Width / (maximum - minimum);
                _barRectangle.Width = barWidth >= SMALLLENGTH ? barWidth : SMALLLENGTH;
                _barRectangle.X = (maximum == minimum || maximum == LargeChange) ? 0 : value * (Width - (_barRectangle.Width > SMALLLENGTH ?
                        _barRectangle.Width : SMALLLENGTH)) / (maximum - minimum - LargeChange);
                _barRectangle.Height = Height;
                this.Invalidate();
            }
        }

        public void ResizeScrollVisible(Size preferredSize, Size size, Control parent, int otherLength, bool isResetValue)
        {
            if (!preferredSize.IsEmpty && !size.IsEmpty)
            {
                if (preferredSize.Width > size.Width && size.Width > SMALLLENGTH)
                {
                    if (!this.Visible)
                    {
                        this.Visible = true;
                    }
                }
                else
                {
                    if (this.Visible)
                    {
                        this.Visible = false;
                    }
                }
            }
            if (!this.Visible && isResetValue)
            {
                this.Value = this.minimum;
            }
        }

        #endregion

        #region 重写事件

        protected override void OnParentChanged(EventArgs e)
        {
            if (!DesignMode && Parent != null)
            {
                Parent.MouseWheel += new MouseEventHandler(parent_MouseWheel);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            Console.WriteLine("HScrollBar_OnResize");
            ResizeBarRectangle();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (_barRectangle.Contains(e.X, e.Y))
                {
                    _isBarGotLButton = true;
                    _movePoint = new Point(_barRectangle.X - e.X, _barRectangle.Y - e.Y);
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_isBarGotLButton)
            {
                Point p = PointToClient(MousePosition);
                p.Offset(_movePoint);
                if (p.X >= (maximum - minimum - (largeChange > SMALLLENGTH ? largeChange : SMALLLENGTH)) * Width / (maximum - minimum))
                {
                    Value = maximum - minimum - largeChange;
                }
                else if (p.X <= 0)
                {
                    Value = minimum;
                }
                else
                {
                    //根据鼠标位置设置value值
                    //p.X / Width为当前X坐标对于总长度的所占比,再与(maximum - minimum)全长相乘得到当前value值
                    Value = p.X * (maximum - minimum) / Width;
                }
                this.Invalidate();
                base.OnScroll(new ScrollEventArgs(ScrollEventType.EndScroll, this.value, this.scrollOrientation));
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                _isBarGotLButton = false;
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (!this.isGotParentMouseWheel) return;
            this.wheelDelta += e.Delta;
            bool flag = false;
            while (Math.Abs(this.wheelDelta) >= 120)
            {
                if (this.wheelDelta > 0)
                {
                    this.wheelDelta -= 120;
                    this.DoScroll(ScrollEventType.SmallDecrement);
                    flag = true;
                }
                else
                {
                    this.wheelDelta += 120;
                    this.DoScroll(ScrollEventType.SmallIncrement);
                    flag = true;
                }
            }
            if (flag)
            {
                this.DoScroll(ScrollEventType.EndScroll);
            }
            if (e is HandledMouseEventArgs)
            {
                ((HandledMouseEventArgs)e).Handled = true;
            }
            this.Invalidate();          
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (!_barRectangle.Contains(e.X, e.Y) && ClientRectangle.Contains(e.X, e.Y))
                {
                    if (e.X > _barRectangle.Right)
                        Value = Math.Min(value + LargeChange, maximum - minimum - LargeChange);
                    else if (e.X < _barRectangle.X)
                        Value = Math.Max(value - LargeChange, minimum);
                    this.Invalidate();
                    base.OnScroll(new ScrollEventArgs(ScrollEventType.EndScroll, this.value, this.scrollOrientation));
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Visible)
            {
                Graphics g = e.Graphics;
                g.InterpolationMode = InterpolationMode.NearestNeighbor; 
                g.SmoothingMode = SmoothingMode.HighQuality;
                SolidBrush sb = new SolidBrush(barColor);
                GraphicsPath gPath = new GraphicsPath();
                Rectangle rect = new Rectangle(_barRectangle.X, _barRectangle.Y, RADIUS, RADIUS);

                // 左上角
                gPath.AddArc(rect, 180, 90);
                // 右上角  
                rect.X = _barRectangle.Right - RADIUS - 1;
                gPath.AddArc(rect, 270, 90);
                // 右下角  
                rect.Y = _barRectangle.Bottom - RADIUS;
                gPath.AddArc(rect, 0, 90);
                // 左下角  
                rect.X = _barRectangle.X;
                gPath.AddArc(rect, 90, 90);
                gPath.CloseFigure();
                g.FillPath(sb, gPath);
            }

        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg != 0x7b) base.WndProc(ref m);
        }

        #endregion

        #region  其他事件

        private void parent_MouseWheel(object sender, MouseEventArgs e)
        {
            if (isGotParentMouseWheel)
                this.OnMouseWheel(e);
        }

        private void DoScroll(ScrollEventType type)
        {
            if (this.RightToLeft == RightToLeft.Yes)
            {
                switch (type)
                {
                    case ScrollEventType.SmallDecrement:
                        type = ScrollEventType.SmallIncrement;
                        break;

                    case ScrollEventType.SmallIncrement:
                        type = ScrollEventType.SmallDecrement;
                        break;

                    case ScrollEventType.LargeDecrement:
                        type = ScrollEventType.LargeIncrement;
                        break;

                    case ScrollEventType.LargeIncrement:
                        type = ScrollEventType.LargeDecrement;
                        break;

                    case ScrollEventType.First:
                        type = ScrollEventType.Last;
                        break;

                    case ScrollEventType.Last:
                        type = ScrollEventType.First;
                        break;
                }
            }
            int newValue = this.value;
            int oldValue = this.value;
            switch (type)
            {
                case ScrollEventType.SmallDecrement:
                    newValue = Math.Max(this.value - this.SmallChange, this.minimum);
                    break;

                case ScrollEventType.SmallIncrement:
                    newValue = Math.Min((this.value + this.SmallChange), (this.maximum - this.LargeChange));
                    break;

                case ScrollEventType.LargeDecrement:
                    newValue = Math.Max(this.value - this.LargeChange, this.minimum);
                    break;

                case ScrollEventType.LargeIncrement:
                    newValue = Math.Min((this.value + this.LargeChange), ((this.maximum - this.LargeChange) + 1));
                    break;

                case ScrollEventType.ThumbPosition:
                case ScrollEventType.ThumbTrack:
                    {
                        Point p = PointToClient(MousePosition);
                        p.Offset(_movePoint);
                        newValue = p.X * (this.maximum - this.minimum) / ((Width - _barRectangle.Width));
                        break;
                    }
                case ScrollEventType.First:
                    newValue = this.minimum;
                    break;

                case ScrollEventType.Last:
                    newValue = (this.maximum - this.LargeChange) + 1;
                    break;
            }
            base.OnScroll(new ScrollEventArgs(type, oldValue, newValue, this.scrollOrientation));
            this.Value = newValue;
        }

        #endregion

    }
}
