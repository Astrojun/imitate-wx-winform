namespace WindowsFormsTest2
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridViewSummary1 = new WindowsFormsTest2.ControlInfo.DataGridViewSummary();
            this.ColumnId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnNewData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTransactionsMoney = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnLastIncome = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSummary1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewSummary1
            // 
            this.dataGridViewSummary1.AllowUserToAddRows = false;
            this.dataGridViewSummary1.AllowUserToDeleteRows = false;
            this.dataGridViewSummary1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.dataGridViewSummary1.BarColor = System.Drawing.Color.Silver;
            this.dataGridViewSummary1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewSummary1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridViewSummary1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(52)))), ((int)(((byte)(88)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewSummary1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewSummary1.ColumnHeadersHeight = 25;
            this.dataGridViewSummary1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewSummary1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnId,
            this.ColumnProductName,
            this.ColumnNewData,
            this.ColumnTransactionsMoney,
            this.ColumnLastIncome,
            this.ColumnPrice});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.NullValue = null;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(52)))), ((int)(((byte)(88)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewSummary1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewSummary1.DisplaySumRowHeader = true;
            this.dataGridViewSummary1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewSummary1.EnableHeadersVisualStyles = false;
            this.dataGridViewSummary1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            this.dataGridViewSummary1.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewSummary1.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridViewSummary1.Name = "dataGridViewSummary1";
            this.dataGridViewSummary1.ReadOnly = true;
            this.dataGridViewSummary1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(147)))), ((int)(((byte)(147)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(52)))), ((int)(((byte)(88)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewSummary1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewSummary1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridViewSummary1.RowTemplate.Height = 23;
            this.dataGridViewSummary1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewSummary1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewSummary1.Size = new System.Drawing.Size(701, 395);
            this.dataGridViewSummary1.SummaryColumns = new string[0];
            this.dataGridViewSummary1.SummaryRowBackColor = System.Drawing.Color.Empty;
            this.dataGridViewSummary1.SummaryRowSpace = 2;
            this.dataGridViewSummary1.SummaryRowVisible = true;
            this.dataGridViewSummary1.SumRowHeaderText = "合计";
            this.dataGridViewSummary1.SumRowHeaderTextBold = false;
            this.dataGridViewSummary1.TabIndex = 3;
            this.dataGridViewSummary1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridViewSummary1_CellFormatting);
            this.dataGridViewSummary1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridViewSummary1_RowPostPaint);
            // 
            // ColumnId
            // 
            this.ColumnId.DataPropertyName = "ID";
            this.ColumnId.Frozen = true;
            this.ColumnId.HeaderText = "代码";
            this.ColumnId.Name = "ColumnId";
            this.ColumnId.ReadOnly = true;
            // 
            // ColumnProductName
            // 
            this.ColumnProductName.DataPropertyName = "ProductName";
            this.ColumnProductName.Frozen = true;
            this.ColumnProductName.HeaderText = "名称";
            this.ColumnProductName.Name = "ColumnProductName";
            this.ColumnProductName.ReadOnly = true;
            // 
            // ColumnNewData
            // 
            this.ColumnNewData.DataPropertyName = "NewData";
            this.ColumnNewData.HeaderText = "最新";
            this.ColumnNewData.Name = "ColumnNewData";
            this.ColumnNewData.ReadOnly = true;
            // 
            // ColumnTransactionsMoney
            // 
            this.ColumnTransactionsMoney.DataPropertyName = "TransactionsMoney";
            this.ColumnTransactionsMoney.HeaderText = "成交量";
            this.ColumnTransactionsMoney.Name = "ColumnTransactionsMoney";
            this.ColumnTransactionsMoney.ReadOnly = true;
            // 
            // ColumnLastIncome
            // 
            this.ColumnLastIncome.DataPropertyName = "LastIncome";
            this.ColumnLastIncome.HeaderText = "昨收";
            this.ColumnLastIncome.Name = "ColumnLastIncome";
            this.ColumnLastIncome.ReadOnly = true;
            // 
            // ColumnPrice
            // 
            this.ColumnPrice.DataPropertyName = "Price";
            this.ColumnPrice.HeaderText = "委卖价";
            this.ColumnPrice.Name = "ColumnPrice";
            this.ColumnPrice.ReadOnly = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(701, 395);
            this.Controls.Add(this.dataGridViewSummary1);
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSummary1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ControlInfo.DataGridViewSummary dataGridViewSummary1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNewData;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTransactionsMoney;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnLastIncome;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPrice;


    }
}

