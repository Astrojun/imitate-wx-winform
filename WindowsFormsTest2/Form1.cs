using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WindowsFormsTest2.ClassInfo;

namespace WindowsFormsTest2
{
    public partial class Form1 : Form
    {
        private BindingCollection<TransactionInfo> _bdList;

        public Form1()
        {
            InitializeComponent();
            //
            //dataGridViewSummary1
            //
            this.dataGridViewSummary1.SummaryColumns = new string[] { ColumnNewData.Name, ColumnTransactionsMoney.Name, ColumnPrice.Name, ColumnLastIncome.Name };
            this.dataGridViewSummary1.AutoGenerateColumns = false;
            this.ColumnId.DefaultCellStyle.ForeColor = Color.FromArgb(254, 222, 111);
            this.dataGridViewSummary1.Columns[1].DefaultCellStyle.ForeColor = Color.FromArgb(254, 222, 111);
            this.dataGridViewSummary1.Columns[3].DefaultCellStyle.ForeColor = Color.FromArgb(220, 82, 82);
            this.dataGridViewSummary1.Columns[4].DefaultCellStyle.ForeColor = Color.White;
            this.dataGridViewSummary1.Columns[5].DefaultCellStyle.ForeColor = Color.FromArgb(126, 162, 98);
            this.dataGridViewSummary1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewSummary1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewSummary1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewSummary1.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewSummary1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            _bdList = new BindingCollection<TransactionInfo>();
            _bdList.Add(new TransactionInfo("100000", "综合指数", 210.29f, 167633f, 199.77f, 0));
            _bdList.Add(new TransactionInfo("600001", "西游记", 596.51f, 596.57f, 542.34f, 6f));
            _bdList.Add(new TransactionInfo("600002", "防止大气污染", 48.18f, 65400f, 48.18f, 14f));
            _bdList.Add(new TransactionInfo("002001", "美丽中国", 54.22f, 216.88f, 49.29f, 55.8f));
            _bdList.Add(new TransactionInfo("002003", "湖北风光A", 88.89f, 27400f, 98.77f, 0));
            this.dataGridViewSummary1.DataSource = _bdList;
        }

        private void dataGridViewSummary1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
            if (e.ColumnIndex == ColumnNewData.Index)
            {
                TransactionInfo trans = (this.dataGridViewSummary1.Rows[e.RowIndex].DataBoundItem as TransactionInfo);
                int compare = trans.NewData.CompareTo(trans.LastIncome);
                switch (compare)
                {
                    case 0:
                        this.dataGridViewSummary1[2, e.RowIndex].Style.ForeColor = Color.White;
                        break;
                    case 1:
                        this.dataGridViewSummary1[2, e.RowIndex].Style.ForeColor = Color.FromArgb(181, 19, 60);
                        break;
                    case -1:
                        this.dataGridViewSummary1[2, e.RowIndex].Style.ForeColor = Color.FromArgb(126, 162, 98);
                        break;
                    default: break;
                }
            }

            if (e.ColumnIndex == ColumnTransactionsMoney.Index)
            {
                e.Value = string.Format(new MyFormatter(), "{0:MyFormatter}", e.Value);
                e.FormattingApplied = true;
            }
            if (e.ColumnIndex == ColumnPrice.Index)
            {
                float value = (float)e.Value;
                e.Value = value > 0 ? value.ToString() : "-";
                e.FormattingApplied = true;
            }
        }

        private void dataGridViewSummary1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridView myDataGrid = sender as DataGridView;
            SolidBrush b = new SolidBrush(myDataGrid.RowHeadersDefaultCellStyle.ForeColor);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(System.Globalization.CultureInfo.CurrentUICulture),
                myDataGrid.DefaultCellStyle.Font, b,
                e.RowBounds.Location.X + myDataGrid.Rows[e.RowIndex].HeaderCell.Size.Width / 2 - 5,
                e.RowBounds.Location.Y + e.RowBounds.Height / 2 - 6);
        }

    }
}
