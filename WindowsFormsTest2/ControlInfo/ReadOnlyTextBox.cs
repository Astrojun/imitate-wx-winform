using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsTest2.ControlHelper;
using System.ComponentModel;

namespace WindowsFormsTest2.ControlInfo
{
    [ToolboxItem(false)]
    public partial class ReadOnlyTextBox : Control
    {

        #region  �����ֶ�
        private StringFormat format;

        #endregion

        #region  ���캯��

        public ReadOnlyTextBox()
        {            
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            format = new StringFormat( StringFormatFlags.NoWrap  | StringFormatFlags.FitBlackBox | StringFormatFlags.MeasureTrailingSpaces);
            format.LineAlignment = StringAlignment.Center;            

            this.Height = 10;
            this.Width = 10;

            this.Padding = new Padding(2);
            this.TextChanged += new EventHandler(ReadOnlyTextBox_TextChanged);           
        }

        #endregion

        #region  ����

        private bool isSummary;
        public bool IsSummary
        {
            get { return isSummary; }
            set { isSummary = value; }
        }

        private bool isLastColumn;
        public bool IsLastColumn
        {
            get { return isLastColumn; }
            set { isLastColumn = value; }
        }

        private string formatString;
        public string FormatString
        {
            get { return formatString; }
            set { formatString = value; }
        }

        private HorizontalAlignment textAlign = HorizontalAlignment.Left;
        [DefaultValue(HorizontalAlignment.Left)]
        public HorizontalAlignment TextAlign
        {
            get { return textAlign; }
            set 
            {
                textAlign = value;
                setFormatFlags();
            }
        }

        private StringTrimming trimming = StringTrimming.None;
        [DefaultValue(StringTrimming.None)]
        public StringTrimming Trimming
        {
            get { return trimming; }
            set
            {
                trimming = value;
                setFormatFlags();
            }
        }

        private Color borderColor = Color.Black;
        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; }
        }

        #endregion

        #region  ��������

        private void setFormatFlags()
        {
            format.Alignment = TextHelper.TranslateAligment(TextAlign);
            format.Trimming = trimming;
        }

        #endregion

        #region  �¼���ί��

        private void ReadOnlyTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(formatString) && !string.IsNullOrEmpty(Text))
            {
                Text = string.Format(formatString, Text);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //int subWidth = 0;
            Rectangle textBounds;

            //if (!string.IsNullOrEmpty(formatString) && !string.IsNullOrEmpty(Text))
            //{
            //    Text = String.Format("{0:" + formatString + "}", Convert.ToDecimal(Text));
            //}

            textBounds = new Rectangle(this.ClientRectangle.X + 2, this.ClientRectangle.Y + 2, this.ClientRectangle.Width - 2 , this.ClientRectangle.Height - 2 );
            using(Pen pen = new Pen(borderColor))
            {
                //if (isLastColumn)
                //    subWidth = 1;

                e.Graphics.FillRectangle(new SolidBrush(this.BackColor), this.ClientRectangle);
                //e.Graphics.DrawRectangle(pen, this.ClientRectangle.X, this.ClientRectangle.Y, this.ClientRectangle.Width - subWidth , this.ClientRectangle.Height - 1);             
                e.Graphics.DrawString(Text, Font, Brushes.White, textBounds , format);
            }
        }

        #endregion

    }
}


