using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace WindowsFormsTest2.ControlInfo
{
    /// <summary>
    /// Todo. Add RightToLeft Support for ReadOnlyTextbox
    /// </summary>
    [ToolboxItem(true)]
    public partial class DataGridViewSummary : DataGridView, ISupportInitialize
    {

        #region Browsable properties 属性

        /// <summary>
        /// If true a row header at the left side 
        /// of the summaryboxes is displayed.
        /// </summary>
        private bool displaySumRowHeader = false;
        [Browsable(true), Category("Summary"), Description("是否显示统计行头标题")]
        public bool DisplaySumRowHeader
        {
            get { return displaySumRowHeader; }
            set
            {
                displaySumRowHeader = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Text displayed in the row header
        /// of the summary row.
        /// </summary>
        private string sumRowHeaderText;
        [Browsable(true), Category("Summary"), Description("统计行头标题文本")]
        public string SumRowHeaderText
        {
            get { return sumRowHeaderText; }
            set { sumRowHeaderText = value; }
        }

        /// <summary>
        /// Text displayed in the row header
        /// of the summary row.
        /// </summary>
        private bool sumRowHeaderTextBold = false;
        [Browsable(true), Category("Summary"), Description("统计行头标题文本是否为粗体")]
        public bool SumRowHeaderTextBold
        {
            get { return sumRowHeaderTextBold; }
            set { sumRowHeaderTextBold = value; }
        }

        /// <summary>
        /// Add columns to sum up in text form
        /// </summary>
        private string[] summaryColumns;
        [Browsable(true), Category("Summary"), Description("需统计的列名")]
        public string[] SummaryColumns
        {
            get { return summaryColumns; }
            set { summaryColumns = value; }
        }

        /// <summary>
        /// Display the summary Row
        /// </summary>
        private bool summaryRowVisible;
        [Browsable(true), Category("Summary"), Description("是否显示统计行")]
        public bool SummaryRowVisible
        {
            get { return summaryRowVisible; }
            set
            {
                summaryRowVisible = value;
                if (summaryControl != null && spacePanel != null)
                {
                    summaryControl.Visible = value;
                    spacePanel.Visible = value;
                }
            }
        }

        private int summaryRowSpace = 0;
        [Browsable(true), Category("Summary"), Description("统计行与DataGridView工作区距离")]
        public int SummaryRowSpace
        {
            get { return summaryRowSpace; }
            set { summaryRowSpace = value; }
        }

        private string formatString = "F02";
        [Browsable(false), Category("Summary"), DefaultValue("F02"), Description("统计行文本默认格式化")]
        public string FormatString
        {
            get { return formatString; }
            set { formatString = value; }
        }

        [Browsable(true), Category("Summary"), Description("统计行背景色")]
        public Color SummaryRowBackColor
        {
            get { return this.summaryControl.SummaryRowBackColor; }
            set { summaryControl.SummaryRowBackColor = value; }
        }

        private Color barColor = Color.FromArgb(147, 147, 147);
        [Browsable(true), Category("外观"), DefaultValue(typeof(Color), "147, 147, 147"), Description("滚动条颜色")]
        public Color BarColor
        {
            get { return barColor; }
            set
            {
                barColor = value;
            }
        }

        /// <summary>
        /// advoid user from setting the scrollbars manually
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new ScrollBars ScrollBars
        {
            get { return base.ScrollBars; }
            set { base.ScrollBars = value; }
        }

        #endregion

        #region Declare variables 声明变量

        public event EventHandler CreateSummary;
        private VScrollBarEx vScrollBar;
        private HScrollBarEx hScrollBar;

        private SummaryControlContainer summaryControl;
        private Panel panel, spacePanel;
        private TextBox refBox;

        private int columnsWidth = 0;
        private int rowsHeight = 0;
        private int preferredWidth = 0;
        private int preferredHeight = 0;

        #endregion

        #region Constructor 构造函数

        public DataGridViewSummary()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.ScrollBars = System.Windows.Forms.ScrollBars.None;
            
            refBox = new TextBox();
            panel = new Panel();
            spacePanel = new Panel();
            hScrollBar = new HScrollBarEx();
            vScrollBar = new VScrollBarEx();

            summaryControl = new SummaryControlContainer(this);
            summaryControl.VisibilityChanged += new EventHandler(summaryControl_VisibilityChanged);

            Resize += new EventHandler(DataGridControlSum_Resize);
            ColumnAdded += new DataGridViewColumnEventHandler(DataGridControlSum_ColumnAdded);
            ColumnRemoved += new DataGridViewColumnEventHandler(DataGridControlSum_ColumnRemoved);
            RowsAdded += new DataGridViewRowsAddedEventHandler(DataGridViewSummary_RowsAdded);
            RowsRemoved += new DataGridViewRowsRemovedEventHandler(DataGridViewSummary_RowsRemoved);
            ColumnWidthChanged += new DataGridViewColumnEventHandler(DataGridViewSummary_ColumnWidthChanged);
            RowHeadersWidthChanged += new EventHandler(DataGridViewSummary_RowHeadersWidthChanged);
            RowHeightChanged += new DataGridViewRowEventHandler(DataGridViewSummary_RowHeightChanged);
            ColumnHeadersHeightChanged += new EventHandler(DataGridViewSummary_ColumnHeadersHeightChanged);
            DataBindingComplete += new DataGridViewBindingCompleteEventHandler(DataGridViewSummary_DataBindingComplete);

            hScrollBar.Scroll += new ScrollEventHandler(scrollBar_Scroll);
            hScrollBar.VisibleChanged += new EventHandler(scrollBar_VisibleChanged);
            hScrollBar.Value = 0;
            hScrollBar.Height = 15;
            hScrollBar.Visible = false;

            vScrollBar.Scroll += new ScrollEventHandler(scrollBar_Scroll);
            vScrollBar.VisibleChanged += new EventHandler(scrollBar_VisibleChanged);
            vScrollBar.Value = 0;
            vScrollBar.Width = 15;
            vScrollBar.Visible = false;
        }

        #endregion
        
        #region public functions 公共方法

        /// <summary>
        /// Refresh the summary 刷新统计行
        /// </summary>
        public void RefreshSummary()
        {
            if (this.summaryControl != null)
                this.summaryControl.RefreshSummary();
        }

        #endregion

        #region Calculate Columns, Rows and Scrollbars width 计算行高、列宽并处理行列变化事件



        private void DataGridControlSum_ColumnRemoved(object sender, DataGridViewColumnEventArgs e)
        {
            calculateColumnsWidth(false);
            summaryControl.Width = preferredWidth;
            if (hScrollBar.Visible || vScrollBar.Visible)
                adjustScrollbarToPanel();
        }

        private void DataGridControlSum_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            calculateColumnsWidth(false);
            summaryControl.Width = preferredWidth;
            if (hScrollBar.Visible || vScrollBar.Visible)
                adjustScrollbarToPanel();
        }

        private void DataGridViewSummary_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            calculateColumnsWidth(true);
            if (hScrollBar.Visible || vScrollBar.Visible)
                adjustScrollbarToPanel();
        }

        private void DataGridViewSummary_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            calculateColumnsWidth(true);
            if (hScrollBar.Visible || vScrollBar.Visible)
                adjustScrollbarToPanel();
        }

        private void DataGridViewSummary_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            adjustScrollBarToView(sender, e, false);
        }

        private void DataGridViewSummary_RowHeadersWidthChanged(object sender, EventArgs e)
        {
            adjustScrollBarToView(sender, e, false);
        }

        private void DataGridViewSummary_ColumnHeadersHeightChanged(object sender, EventArgs e)
        {
            adjustScrollBarToView(sender, e, true);
        }

        private void DataGridViewSummary_RowHeightChanged(object sender, DataGridViewRowEventArgs e)
        {
            adjustScrollBarToView(sender, e, true);
        }

        #endregion

        #region Other Events and delegates 其他事件和委托

        /// <summary>
        /// Moves viewable area of DataGridView according to the position of the scrollbar 滚动条滚动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                //按像素滚动
                HorizontalScrollingOffset = e.NewValue;
            }
            else
            {
                //按行滚动
                int vPosition = (int)Math.Ceiling(Convert.ToDouble(e.NewValue) * Convert.ToDouble(Rows.Count) / Convert.ToDouble(vScrollBar.Maximum));
                if (vPosition < Rows.Count)
                {
                    FirstDisplayedScrollingRowIndex = vPosition;
                }
            }
        }

        /// <summary>
        /// 创建统计行时发生
        /// </summary>
        public void CreateSummaryRow()
        {
            OnCreateSummary(this, EventArgs.Empty);
        }

        /// <summary>
        /// Calls the CreateSummary event 唤起创建统计行事件
        /// </summary>
        private void OnCreateSummary(object sender, EventArgs e)
        {
            if (CreateSummary != null)
                CreateSummary(sender, e);
        }

        /// <summary>
        /// 数据表调整大小时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridControlSum_Resize(object sender, EventArgs e)
        {
            Console.WriteLine("DataGridControlSum_Resize");
            if (!DesignMode && Parent != null)
            {
                resizeScrollBar();
                if(summaryControl.Visible)
                    adjustSumControlToGrid();
                if (hScrollBar.Visible || vScrollBar.Visible)
                    adjustScrollbarToPanel();
            }
        }

        /// <summary>
        /// 滚动条可见性变化时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scrollBar_VisibleChanged(object sender, EventArgs e)
        {
            Console.WriteLine("scrollBar_VisibleChanged");
            if (!DesignMode && Parent != null)
            {
                if (Parent.Visible)
                {
                    this.Height = panel.Height - (summaryControl.Visible ? summaryControl.Height : 0) - 
                        (hScrollBar.Visible ? hScrollBar.Height : 0) - summaryRowSpace;
                    this.Width = panel.Width - (vScrollBar.Visible ? vScrollBar.Width : 0);
                    hScrollBar.IsGotParentMouseWheel = (hScrollBar.Visible && !vScrollBar.Visible) ? true : false;
                    vScrollBar.IsGotParentMouseWheel = vScrollBar.Visible ? true : false;
                }
            }
        }

        /// <summary>
        /// 统计行可见性变化时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void summaryControl_VisibilityChanged(object sender, EventArgs e)
        {
            Console.WriteLine("summaryControl_VisibilityChanged");
            this.Height = panel.Height - (summaryControl.Visible ? summaryControl.Height : 0) - summaryRowSpace;
        }

        #endregion

        #region Adjust summaryControl, scrollbar  调整统计行和滚动条属性

        /// <summary>
        /// Position the summaryControl under the DataGridView  调整统计行位置
        /// </summary>
        private void adjustSumControlToGrid()
        {
            Console.WriteLine("adjustSumControlToGrid");
            if (summaryControl == null || Parent == null)
                return;
            summaryControl.Top = panel.Height - summaryControl.Height;
            summaryControl.Left = Left;
            summaryControl.Width = Width;
        }

        /// <summary>
        /// Position the hScrollbar under the summaryControl  调整横向滚动条适应统计行
        /// </summary>
        private void adjustScrollbarToPanel()
        {
            Console.WriteLine("adjustScrollbarToPanel");
            if (Parent != null)
            {
                if (hScrollBar.Visible)
                {
                    hScrollBar.Top = Bottom;
                    hScrollBar.Width = Width;
                    hScrollBar.Left = Left;
                }

                if (vScrollBar.Visible)
                {
                    vScrollBar.Left = Right;
                    vScrollBar.Height = Height;
                    vScrollBar.Top = Top;
                }
            }
        }

        /// <summary>
        /// 调整滚动条的属性值自适应
        /// </summary>
        private void resizeScrollBar()
        {
            Console.WriteLine("resizeScrollBar");
            if (columnsWidth > 0)
            {
                if (preferredWidth < Width || Width < preferredWidth - columnsWidth
                    || panel.Height < (ColumnHeadersVisible ? ColumnHeadersHeight : 0) + hScrollBar.Height + summaryRowSpace + (summaryControl.Visible ? summaryControl.Height : 0))
                {
                    if (hScrollBar.Visible)
                    {
                        hScrollBar.Visible = false;
                    }
                    hScrollBar.Value = hScrollBar.Minimum;
                }
                else
                {
                    if (!hScrollBar.Visible)
                    {
                        hScrollBar.Visible = true;
                    }
                }
                if (hScrollBar.Visible)
                {
                    hScrollBar.LargeChange = Width - (RowHeadersVisible ? RowHeadersWidth : 0) - Columns.GetColumnsWidth(DataGridViewElementStates.Frozen);
                    hScrollBar.SmallChange = Convert.ToInt32(hScrollBar.LargeChange / 6.0);
                    HorizontalScrollingOffset = hScrollBar.Value;
                }
            }
            else
            {
                hScrollBar.Visible = false;
            }

            if (rowsHeight > 0)
            {
                if (preferredHeight < Height || Height < preferredHeight - rowsHeight
                    || panel.Width < vScrollBar.Width + (RowHeadersVisible ? RowHeadersWidth : 0))
                {
                    if (vScrollBar.Visible)
                    {
                        vScrollBar.Visible = false;
                    }
                    vScrollBar.Value = vScrollBar.Minimum;
                }
                else
                {
                    if (!vScrollBar.Visible)
                    {
                        vScrollBar.Visible = true;
                    }
                }
                if (vScrollBar.Visible)
                {
                    vScrollBar.LargeChange = Height - (ColumnHeadersVisible ? ColumnHeadersHeight : 0) - Rows.GetRowsHeight(DataGridViewElementStates.Frozen);
                    vScrollBar.SmallChange = Convert.ToInt32(vScrollBar.LargeChange / 6.0);
                }
            }
            else
            {
                vScrollBar.Visible = false;
            }
        }

        /// <summary>
        /// scrollbar自适应行高列宽的变化
        /// </summary>
        /// <param name="sender">事件对象</param>
        /// <param name="e">事件内容</param>
        /// <param name="isHScrollOrVScroll">true为VScrollBar变化，false为HScrollBar变化</param>
        private void adjustScrollBarToView(object sender, EventArgs e, bool isVScrollOrHScroll)
        {
            calculateColumnsWidth(isVScrollOrHScroll);
            summaryControl.Width = preferredWidth;
            resizeScrollBar();
            if (summaryControl.Visible)
                adjustSumControlToGrid();
            if (hScrollBar.Visible || vScrollBar.Visible)
                adjustScrollbarToPanel();
        }

        /// <summary>
        /// 计算行高、列宽
        /// </summary>
        /// <param name="isRowOrColumn">true为求RowsHeight，false为ColumnsWidth</param>
        private void calculateColumnsWidth(bool isRowOrColumn)
        {
            Console.WriteLine("calculateColumnsWidth");
            if (!isRowOrColumn)
            {
                columnsWidth = Columns.GetColumnsWidth(DataGridViewElementStates.Visible) - Columns.GetColumnsWidth(DataGridViewElementStates.Frozen);
                hScrollBar.Maximum = columnsWidth;
                preferredWidth = Columns.GetColumnsWidth(DataGridViewElementStates.Visible) + (RowHeadersVisible ? RowHeadersWidth : 0);
            }
            else
            {
                rowsHeight = Rows.GetRowsHeight(DataGridViewElementStates.Visible) - Rows.GetRowsHeight(DataGridViewElementStates.Frozen);
                vScrollBar.Maximum = rowsHeight;
                preferredHeight = Rows.GetRowsHeight(DataGridViewElementStates.Visible) + (ColumnHeadersVisible ? ColumnHeadersHeight : 0);
            }
        }

        private void DataGridViewSummary_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            Console.WriteLine("DataGridViewSummary_DataBindingComplete");
            calculateColumnsWidth(true);
            calculateColumnsWidth(false);
            resizeScrollBar();
            if (summaryControl.Visible)
                adjustSumControlToGrid();
            if (hScrollBar.Visible || vScrollBar.Visible)
                adjustScrollbarToPanel();
        }

        /// <summary>
        /// When the DataGridView is visible for the first time a panel is created.
        /// The DataGridView is then removed from the parent control and added as 
        /// child to the newly created panel   
        /// 初始化数据表和其他控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeParent()
        {
            if (!DesignMode && Parent != null)
            {
                Console.WriteLine("changeParent");
                summaryControl.InitialHeight = this.refBox.Height;
                summaryControl.Height = summaryControl.InitialHeight;
                summaryControl.BackColor = this.RowHeadersDefaultCellStyle.BackColor;
                summaryControl.ForeColor = Color.Transparent;
                summaryControl.RightToLeft = this.RightToLeft;
                summaryControl.Dock = DockStyle.Bottom;

                panel.Bounds = this.Bounds;
                panel.BackColor = this.BackgroundColor;
                panel.Dock = this.Dock;
                panel.Anchor = this.Anchor;
                panel.Padding = this.Padding;
                panel.Margin = this.Margin;
                panel.Top = this.Top;
                panel.Left = this.Left;
                panel.BorderStyle = this.BorderStyle;

                Margin = new Padding(0);
                Padding = new Padding(0);
                Top = 0;
                Left = 0;
                Dock = DockStyle.Fill;

                if (this.Parent is TableLayoutPanel)
                {
                    int rowSpan, colSpan;

                    TableLayoutPanel tlp = this.Parent as TableLayoutPanel;
                    TableLayoutPanelCellPosition cellPos = tlp.GetCellPosition(this);

                    rowSpan = tlp.GetRowSpan(this);
                    colSpan = tlp.GetColumnSpan(this);

                    tlp.Controls.Remove(this);
                    tlp.Controls.Add(panel, cellPos.Column, cellPos.Row);
                    tlp.SetRowSpan(panel, rowSpan);
                    tlp.SetColumnSpan(panel, colSpan);
                }
                else
                {
                    Control parent = this.Parent;
                    parent.Controls.Remove(this);
                    parent.Controls.Add(panel);
                }

                hScrollBar.Width = this.Width;
                hScrollBar.Left = this.Left;
                hScrollBar.BringToFront();
                hScrollBar.Dock = DockStyle.Bottom;
                hScrollBar.BarColor = barColor;

                vScrollBar.Height = this.Height;
                vScrollBar.Top = this.Top;
                vScrollBar.BringToFront();
                vScrollBar.Dock = DockStyle.Right;
                vScrollBar.BarColor = barColor;

                spacePanel = new Panel();
                spacePanel.BackColor = panel.BackColor;
                spacePanel.Height = summaryRowSpace;
                spacePanel.Dock = DockStyle.Bottom;

                panel.BringToFront();
                panel.Controls.Add(this);
                panel.Controls.Add(hScrollBar);
                panel.Controls.Add(vScrollBar);
                panel.Controls.Add(spacePanel);
                panel.Controls.Add(summaryControl);

            }
        }

        #endregion

        #region ISupportInitialzie 通知初始化事宜

        public void BeginInit()
        {
        }

        public void EndInit()
        {
            changeParent();
        }

        #endregion
    }
}