using System;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using WindowsFormsTest2.ControlHelper;
using WindowsFormsTest2.ClassInfo;

namespace WindowsFormsTest2.ControlInfo
{
    [System.ComponentModel.ToolboxItem(false)]
    public class SummaryControlContainer : Control
    {

        #region Declare variables 声明变量

        private Hashtable sumBoxHash;
        private DataGridViewSummary dgv;
        private Label sumRowHeaderLabel;

        #endregion

        #region 属性

        private int initialHeight;
        public int InitialHeight
        {
            get { return initialHeight; }
            set { initialHeight = value; }
        }

        private bool lastVisibleState;
        public bool LastVisibleState
        {
            get { return lastVisibleState; }
            set { lastVisibleState = value; }
        }        

        private Color summaryRowBackColor;
        public Color SummaryRowBackColor
        {
            get { return summaryRowBackColor; }
            set { summaryRowBackColor = value;}
        }

        #endregion

        #region 事件

        /// <summary>
        /// Event is raised when visibility changes and the
        /// lastVisibleState is not the new visible state
        /// </summary>
        public event EventHandler VisibilityChanged;

        #endregion

        #region Constructors 构造函数

        public SummaryControlContainer(DataGridViewSummary dgv)
        {
            if (dgv == null)
                throw new Exception("DataGridView is null!");

            this.dgv = dgv;
            this.Height = dgv.RowTemplate.Height;

            sumBoxHash = new Hashtable();
            sumRowHeaderLabel = new Label();

            sumRowHeaderLabel.Anchor = AnchorStyles.Left;
            sumRowHeaderLabel.TextAlign = ContentAlignment.MiddleCenter;
            sumRowHeaderLabel.Height = dgv.RowTemplate.Height;
            sumRowHeaderLabel.Width = dgv.RowHeadersWidth;
            sumRowHeaderLabel.Top = 0;
            sumRowHeaderLabel.Padding = new Padding(0, 4, 0, 0);
            sumRowHeaderLabel.Visible = false;
            this.Controls.Add(sumRowHeaderLabel);

            this.dgv.CreateSummary += new EventHandler(dgv_CreateSummary);
            this.dgv.RowsAdded += new DataGridViewRowsAddedEventHandler(dgv_RowsAdded);
            this.dgv.RowsRemoved += new DataGridViewRowsRemovedEventHandler(dgv_RowsRemoved);
            this.dgv.CellValueChanged += new DataGridViewCellEventHandler(dgv_CellValueChanged);

            this.dgv.Scroll += new ScrollEventHandler(dgv_Scroll);
            this.dgv.ColumnWidthChanged += new DataGridViewColumnEventHandler(dgv_ColumnWidthChanged);
            this.dgv.RowHeadersWidthChanged += new EventHandler(dgv_RowHeadersWidthChanged);
            this.VisibleChanged += new EventHandler(SummaryControlContainer_VisibleChanged);

            this.dgv.ColumnAdded += new DataGridViewColumnEventHandler(dgv_ColumnAdded);
            this.dgv.ColumnRemoved += new DataGridViewColumnEventHandler(dgv_ColumnRemoved);
            this.dgv.ColumnStateChanged += new DataGridViewColumnStateChangedEventHandler(dgv_ColumnStateChanged);
            this.dgv.ColumnDisplayIndexChanged += new DataGridViewColumnEventHandler(dgv_ColumnDisplayIndexChanged);

            this.dgv.DataSourceChanged += new EventHandler(dgv_DataSourceChanged);
        }

        void dgv_DataSourceChanged(object sender, EventArgs e)
        {
            reCreateSumBoxes();
        }

        private void dgv_ColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
        {
            reCreateSumBoxes();
        }

        private void dgv_ColumnStateChanged(object sender, DataGridViewColumnStateChangedEventArgs e)
        {
            resizeSumBoxes();
        }

        private void dgv_ColumnRemoved(object sender, DataGridViewColumnEventArgs e)
        {
            reCreateSumBoxes();
        }

        private void dgv_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            reCreateSumBoxes();
        }

        private void dgv_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            ReadOnlyTextBox roTextBox = (ReadOnlyTextBox)sumBoxHash[dgv.Columns[e.ColumnIndex]];
            if (roTextBox != null)
            {
                if (roTextBox.IsSummary)
                    calcSummaries();
            }
        }

        private void dgv_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            calcSummaries();
        }

        private void dgv_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            calcSummaries();
        }

        private void SummaryControlContainer_VisibleChanged(object sender, EventArgs e)
        {
            if (lastVisibleState != this.Visible)
            {
                OnVisiblityChanged(sender, e);
            }
        }

        protected void OnVisiblityChanged(object sender, EventArgs e)
        {
            if (VisibilityChanged != null)
                VisibilityChanged(sender, e);

            lastVisibleState = this.Visible;
        }

        #endregion

        #region Events and delegates 事件和委托

        private void dgv_CreateSummary(object sender, EventArgs e)
        {
            reCreateSumBoxes();
            calcSummaries();
        }

        private void dgv_Scroll(object sender, ScrollEventArgs e)
        {
            resizeSumBoxes();
        }

        private void dgv_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            resizeSumBoxes();
        }

        private void dgv_RowHeadersWidthChanged(object sender, EventArgs e)
        {
            resizeSumBoxes();
        }

        private void dgv_Resize(object sender, EventArgs e)
        {
            resizeSumBoxes();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            resizeSumBoxes();
        }

        #endregion

        #region Functions 其他方法

        /// <summary>
        /// Checks if passed object is of type of integer 输入是否为整数
        /// </summary>
        /// <param name="o">object</param>
        /// <returns>true/ false</returns>
        protected bool IsInteger(object o)
        {
            if (o is Int64)
                return true;
            if (o is Int32)
                return true;
            if (o is Int16)
                return true;
            return false;
        }

        /// <summary>
        /// Checks if passed object is of type of decimal/ double 输入是否为浮点型数
        /// </summary>
        /// <param name="o">object</param>
        /// <returns>true/ false</returns>
        protected bool IsDecimal(object o)
        {
            if (o is Decimal)
                return true;
            if (o is Single)
                return true;
            if (o is Double)
                return true;
            return false;
        }

        /// <summary>
        /// Enable manual refresh of the SummaryDataGridView 启用数据表的手动刷新
        /// </summary>
        internal void RefreshSummary()
        {
            calcSummaries();
        }

        /// <summary>
        /// Calculate the Sums of the summary columns 计算统计行数据总和
        /// </summary>
        private void calcSummaries()
        {
            foreach (ReadOnlyTextBox roTextBox in sumBoxHash.Values)
            {
                if (roTextBox.IsSummary)
                {
                    roTextBox.Tag = 0;
                    roTextBox.Text = "0";
                }
            }

            if (dgv.SummaryColumns != null && dgv.SummaryColumns.Length > 0 && sumBoxHash.Count > 0)
            {
                //System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                //stopwatch.Start(); //  开始监视代码运行时间
                ////......
                //stopwatch.Stop(); //  停止监视
                //TimeSpan timespan = stopwatch.Elapsed; //  获取当前实例测量得出的总时间

                foreach (DataGridViewColumn dgvColumn in sumBoxHash.Keys)
                {
                    ReadOnlyTextBox sumBox = (ReadOnlyTextBox)sumBoxHash[dgvColumn];

                    for (int i = 0; i < dgv.Rows.Count; i++)
                    {
                        DataGridViewCell dgvCell = dgv.Rows[i].Cells[dgvColumn.Index];
                        if (sumBox != null && sumBox.IsSummary)
                        {
                            if (dgvCell.Value != null && !(dgvCell.Value is DBNull))
                            {
                                if (IsInteger(dgvCell.Value))
                                {
                                    sumBox.Tag = Convert.ToInt64(sumBox.Tag) + Convert.ToInt64(dgvCell.Value);
                                }
                                else if (IsDecimal(dgvCell.Value))
                                {
                                    sumBox.Tag = Convert.ToDecimal(sumBox.Tag) + Convert.ToDecimal(dgvCell.Value);
                                }

                                if (dgvColumn.Name.Equals("ColumnTransactionsMoney"))
                                {
                                    sumBox.Text = string.Format(new MyFormatter(), "{0:MyFormatter}", sumBox.Tag);
                                }
                                else
                                {
                                    sumBox.Text = string.Format("{0}", sumBox.Tag);
                                }
                                sumBox.Invalidate();
                            }
                        }
                    }
                }

            }
        }

        /// <summary>
        /// Create summary boxes for each Column of the DataGridView  为数据表各列创建统计文本
        /// </summary>
        private void reCreateSumBoxes()
        {
            ReadOnlyTextBox sumBox;

            foreach (Control control in sumBoxHash.Values)
            {
                this.Controls.Remove(control);
            }
            sumBoxHash.Clear();

            int iCnt = 0;

            List<DataGridViewColumn> sortedColumns = SortedColumns;
            foreach (DataGridViewColumn dgvColumn in sortedColumns)
            {
                sumBox = new ReadOnlyTextBox();
                sumBoxHash.Add(dgvColumn, sumBox);

                sumBox.Top = 0;
                sumBox.Height = dgv.RowTemplate.Height;
                sumBox.BorderColor = dgv.GridColor;
                sumBox.ForeColor = Color.White;

                if (summaryRowBackColor == null || summaryRowBackColor == Color.Transparent)
                    sumBox.BackColor = dgv.DefaultCellStyle.BackColor;
                else
                    sumBox.BackColor = summaryRowBackColor;
                sumBox.BringToFront();

                if (dgv.ColumnCount - iCnt == 1)
                    sumBox.IsLastColumn = true;

                if (dgv.SummaryColumns != null && dgv.SummaryColumns.Length > 0)
                {
                    for (int iCntX = 0; iCntX < dgv.SummaryColumns.Length; iCntX++)
                    {
                        if (dgv.SummaryColumns[iCntX] == dgvColumn.DataPropertyName ||
                            dgv.SummaryColumns[iCntX] == dgvColumn.Name)
                        {
                            dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                            sumBox.TextAlign = TextHelper.TranslateGridColumnAligment(dgvColumn.DefaultCellStyle.Alignment);
                            sumBox.IsSummary = true;

                            sumBox.FormatString = dgvColumn.CellTemplate.Style.Format;
                            if (dgvColumn.ValueType == typeof(System.Int32) || dgvColumn.ValueType == typeof(System.Int16) ||
                                dgvColumn.ValueType == typeof(System.Int64) || dgvColumn.ValueType == typeof(System.Single) ||
                                dgvColumn.ValueType == typeof(System.Double) || dgvColumn.ValueType == typeof(System.Single) ||
                                dgvColumn.ValueType == typeof(System.Decimal))
                                sumBox.Tag = System.Activator.CreateInstance(dgvColumn.ValueType);
                        }
                    }
                }

                sumBox.BringToFront();
                this.Controls.Add(sumBox);

                iCnt++;
            }

            if (dgv.DisplaySumRowHeader)
            {
                sumRowHeaderLabel.Visible = true;
                sumRowHeaderLabel.Text = dgv.SumRowHeaderText;
                sumRowHeaderLabel.ForeColor = dgv.RowHeadersDefaultCellStyle.ForeColor;
                sumRowHeaderLabel.Font = new Font(dgv.DefaultCellStyle.Font, dgv.SumRowHeaderTextBold ? FontStyle.Bold : FontStyle.Regular);
                sumRowHeaderLabel.Height = dgv.RowTemplate.Height;
                sumRowHeaderLabel.Width = dgv.RowHeadersWidth;
            }
            calcSummaries();
            resizeSumBoxes();
        }

        /// <summary>
        /// Order the columns in the way they are displayed  对可见列进行统计
        /// </summary>
        private List<DataGridViewColumn> SortedColumns
        {
            get
            {
                List<DataGridViewColumn> result = new List<DataGridViewColumn>();
                DataGridViewColumn column = dgv.Columns.GetFirstColumn(DataGridViewElementStates.None);
                if (column == null)
                    return result;
                result.Add(column);
                while ((column = dgv.Columns.GetNextColumn(column, DataGridViewElementStates.None, DataGridViewElementStates.None)) != null)
                    result.Add(column);

                return result;
            }
        }

        /// <summary>
        /// Resize the summary Boxes (depending on the width of the Columns of the DataGridView)    
        /// 调整统计文本框大小（取决于数据表的列宽）
        /// </summary>
        private void resizeSumBoxes()
        {
            this.SuspendLayout();
            if (sumBoxHash != null && sumBoxHash.Count > 0)
                try
                {
                    int rowHeaderWidth = dgv.RowHeadersVisible ? dgv.RowHeadersWidth - 1: 0;
                    int sumLabelWidth = dgv.RowHeadersVisible ? dgv.RowHeadersWidth - 1 : 0;
                    int curPos = rowHeaderWidth;

                    if (dgv.DisplaySumRowHeader && sumLabelWidth > 0)
                    {
                        if(!sumRowHeaderLabel.Visible)
                            sumRowHeaderLabel.Visible = true;
                        sumRowHeaderLabel.Width = sumLabelWidth;

                        if (dgv.RightToLeft == RightToLeft.Yes)
                        {
                            if (sumRowHeaderLabel.Dock != DockStyle.Right)
                                sumRowHeaderLabel.Dock = DockStyle.Right;
                        }
                        else
                        {
                            if (sumRowHeaderLabel.Dock != DockStyle.Left)
                                sumRowHeaderLabel.Dock = DockStyle.Left;
                        }
                    }
                    else
                    {
                        if (sumRowHeaderLabel.Visible)
                            sumRowHeaderLabel.Visible = false;
                    }

                    int iCnt = 0;
                    Rectangle oldBounds;

                    foreach (DataGridViewColumn dgvColumn in SortedColumns) //dgv.Columns)
                    {
                        ReadOnlyTextBox sumBox = (ReadOnlyTextBox)sumBoxHash[dgvColumn];

                        if (sumBox != null)
                        {
                            oldBounds = sumBox.Bounds;
                            if (!dgvColumn.Visible)
                            {
                                sumBox.Visible = false;
                                continue;
                            }

                            int from = dgvColumn.Frozen ? curPos : curPos - dgv.HorizontalScrollingOffset;

                            int width = dgvColumn.Width + (iCnt == 0 ? 0 : 0);

                            if (from < rowHeaderWidth)
                            {
                                width -= rowHeaderWidth - from;
                                from = rowHeaderWidth;
                            }

                            if (from + width > this.Width)
                                width = this.Width - from;

                            if (width < 4)
                            {
                                if (sumBox.Visible)
                                    sumBox.Visible = false;
                            }
                            else
                            {
                                if (this.RightToLeft == RightToLeft.Yes)
                                    from = this.Width - from - width;


                                if (sumBox.Left != from || sumBox.Width != width)
                                    sumBox.SetBounds(from, 0, width, 0, BoundsSpecified.X | BoundsSpecified.Width);

                                if (!sumBox.Visible)
                                    sumBox.Visible = true;
                            }

                            curPos += dgvColumn.Width + (iCnt == 0 ? 0 : 0);
                            if (oldBounds != sumBox.Bounds)
                                sumBox.Invalidate();

                        }
                        iCnt++;
                    }
                }
                finally
                {
                    this.ResumeLayout();
                }
        }

        #endregion
    }
}
