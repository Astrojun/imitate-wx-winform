using System.Windows.Forms;
using System;
using System.Text;
using System.Globalization;
using System.ComponentModel;
using System.Drawing;
using WindowsFormsTest2.ControlHelper;

namespace WindowsFormsTest2.ControlInfo
{
    [ToolboxBitmap(typeof(DataGridViewTextBoxColumn), "DataGridViewTextBoxColumn.bmp")]
    public sealed partial class DataGridViewRichTextBoxColumn : DataGridViewColumn
    {

        public DataGridViewRichTextBoxColumn()
            : base(new DataGridViewRichTextBoxCell())
        {
        }

        public override object Clone()
        {
            DataGridViewColumn column;
            Type type = base.GetType();
            if (type == typeof(DataGridViewRichTextBoxColumn))
            {
                column = base.Clone() as DataGridViewRichTextBoxColumn;
            }
            else
            {
                column = (DataGridViewRichTextBoxColumn)Activator.CreateInstance(type);
            }
            return column;
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                if (value != null && !value.GetType().IsAssignableFrom(typeof(DataGridViewRichTextBoxCell)))
                {
                    throw new InvalidCastException("必须是一个双行文本框单元格");
                }
                base.CellTemplate = value;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(0x40);
            builder.Append("DataGridViewTextBoxColumn { Name=");
            builder.Append(base.Name);
            builder.Append(", Index=");
            builder.Append(base.Index.ToString(CultureInfo.CurrentCulture));
            builder.Append(" }");
            return builder.ToString();
        }
    }

    public class DataGridViewRichTextBoxCell : DataGridViewImageCell
    {
        private static readonly RichTextBox _editingControl = new RichTextBox();

        public override Type EditType
        {
            get
            {
                return typeof(DataGridViewRichTextBoxEditingControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                return typeof(string);
            }
            set
            {
                base.ValueType = value;
            }
        }

        public override Type FormattedValueType
        {
            get
            {
                return typeof(string);
            }
        }

        private static void SetRichTextBoxText(RichTextBox ctl, string text)
        {
            try
            {
                ctl.Rtf = text;
            }
            catch (ArgumentException)
            {
                ctl.Text = text;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        private Image GetRtfImage(int rowIndex, object value, bool selected, Rectangle cellBounds, DataGridViewCellStyle cellStyle)
        {
            Size cellSize = GetSize(rowIndex);

            if (cellSize.Width < 1 || cellSize.Height < 1)
                return null;

            RichTextBox ctl = _editingControl;

            ctl.Size = GetSize(rowIndex);
            ctl.WordWrap = false;
            ctl.Multiline = true;
            string[] str = (value as string).Split(new string[] { "\n" }, 2, StringSplitOptions.RemoveEmptyEntries);
            str[0] = (TextRenderer.MeasureText(str[0], cellStyle.Font).Width > cellBounds.Width - 20 && str[0].Length > 6) ? (str[0].Substring(0, 6) + "...") : str[0];
            if (str.Length == 2)
            {
                str[1] = (TextRenderer.MeasureText(str[1], cellStyle.Font).Width > cellBounds.Width && str[1].Length > 8) ? (str[1].Substring(0, 8) + "...") : str[1];
            }
            string rtf = @"{\rtf1\ansi\ansicpg936\deff0\deflang1033\deflangfe2052
{\fonttbl{\f0\fmodern\fprq6\fcharset134 \'cb\'ce\'cc\'e5;}{\f1\fnil\fcharset134 \'cb\'ce\'cc\'e5;}}
{\colortbl;\red190\green190\blue190;}
\viewkind4\uc1\pard\cf0\lang2052\f0\fs20" + str[0] + @"\par
\cf1\fs18" + (str.Length == 2 ? str[1] : "") + @"\par}";

            SetRichTextBoxText(ctl, rtf);

            if (ctl != null)
            {
                // Print the content of RichTextBox to an image.
                Size imgSize = new Size(cellSize.Width - 1, cellSize.Height - 1);
                Image rtfImg = null;

                if (selected)
                {
                    // Selected cell state
                    ctl.BackColor = cellStyle.SelectionBackColor;
                    ////ctl.ForeColor = cellStyle.SelectionForeColor;
                    ctl.Select(ctl.Lines[0].Length + 1, ctl.Lines[1].Length);
                    Color foreColor = ctl.SelectionColor;
                    ctl.SelectionColor = cellStyle.SelectionForeColor;

                    // Print image
                    rtfImg = RichTextBoxPrinter.Print(ctl, imgSize.Width, imgSize.Height);

                    // Restore RichTextBox
                    ctl.BackColor = cellStyle.BackColor;
                    //ctl.ForeColor = cellStyle.ForeColor;
                    //ctl.SelectionColor = foreColor;
                }
                else
                {
                    rtfImg = RichTextBoxPrinter.Print(ctl, imgSize.Width, imgSize.Height);
                }

                return rtfImg;
            }

            return null;
        }

        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);

            RichTextBox ctl = DataGridView.EditingControl as RichTextBox;

            if (ctl != null)
            {
                SetRichTextBoxText(ctl, Convert.ToString(initialFormattedValue));
            }
        }

        protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
        {
            return value;
        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, null, null, errorText, cellStyle, advancedBorderStyle, paintParts);


            Image img = GetRtfImage(rowIndex, value, base.Selected, cellBounds, cellStyle);

            if (img != null)
                graphics.DrawImage(img, cellBounds.Left, cellBounds.Top);
        }

        #region Handlers of edit events, copyied from DataGridViewTextBoxCell

        private byte flagsState;

        protected override void OnEnter(int rowIndex, bool throughMouseClick)
        {
            base.OnEnter(rowIndex, throughMouseClick);

            if ((base.DataGridView != null) && throughMouseClick)
            {
                this.flagsState = (byte)(this.flagsState | 1);
            }
        }

        protected override void OnLeave(int rowIndex, bool throughMouseClick)
        {
            base.OnLeave(rowIndex, throughMouseClick);

            if (base.DataGridView != null)
            {
                this.flagsState = (byte)(this.flagsState & -2);
            }
        }

        protected override void OnMouseDoubleClick(DataGridViewCellMouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (base.DataGridView != null)
            {
                Point currentCellAddress = base.DataGridView.CurrentCellAddress;

                if (((currentCellAddress.X == e.ColumnIndex) && (currentCellAddress.Y == e.RowIndex)) && (e.Button == MouseButtons.Left))
                {
                    if ((this.flagsState & 1) != 0)
                    {
                        this.flagsState = (byte)(this.flagsState & -2);
                    }
                    else if (base.DataGridView.EditMode != DataGridViewEditMode.EditProgrammatically)
                    {
                        base.DataGridView.BeginEdit(false);
                    }
                }
            }
        }

        public override bool KeyEntersEditMode(KeyEventArgs e)
        {
            return (((((char.IsLetterOrDigit((char)((ushort)e.KeyCode)) && ((e.KeyCode < Keys.F1) || (e.KeyCode > Keys.F24))) || ((e.KeyCode >= Keys.NumPad0) && (e.KeyCode <= Keys.Divide))) || (((e.KeyCode >= Keys.OemSemicolon) && (e.KeyCode <= Keys.OemBackslash)) || ((e.KeyCode == Keys.Space) && !e.Shift))) && (!e.Alt && !e.Control)) || base.KeyEntersEditMode(e));
        }

        #endregion
    }

    public class DataGridViewRichTextBoxEditingControl : RichTextBox, IDataGridViewEditingControl
    {
        private DataGridView _dataGridView;
        private int _rowIndex;
        private bool _valueChanged;

        public DataGridViewRichTextBoxEditingControl()
        {
            this.BorderStyle = BorderStyle.None;
            this.WordWrap = false;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            _valueChanged = true;
            EditingControlDataGridView.NotifyCurrentCellDirty(true);
        }

        protected override bool IsInputKey(Keys keyData)
        {
            Keys keys = keyData & Keys.KeyCode;
            if (keys == Keys.Return)
            {
                return this.Multiline;
            }

            return base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    // Control + B = Bold
                    case Keys.B:
                        if (this.SelectionFont.Bold)
                        {
                            this.SelectionFont = new Font(this.Font.FontFamily, this.Font.Size, ~FontStyle.Bold & this.Font.Style);
                        }
                        else
                            this.SelectionFont = new Font(this.Font.FontFamily, this.Font.Size, FontStyle.Bold | this.Font.Style);
                        break;
                    // Control + U = Underline
                    case Keys.U:
                        if (this.SelectionFont.Underline)
                        {
                            this.SelectionFont = new Font(this.Font.FontFamily, this.Font.Size, ~FontStyle.Underline & this.Font.Style);
                        }
                        else
                            this.SelectionFont = new Font(this.Font.FontFamily, this.Font.Size, FontStyle.Underline | this.Font.Style);
                        break;
                    // Control + I = Italic
                    // 有冲突
                    //case Keys.I:
                    //    if (this.SelectionFont.Italic)
                    //    {
                    //        this.SelectionFont = new Font(this.Font.FontFamily, this.Font.Size, ~FontStyle.Italic & this.Font.Style);
                    //    }
                    //    else
                    //        this.SelectionFont = new Font(this.Font.FontFamily, this.Font.Size, FontStyle.Italic | this.Font.Style);
                    //    break;
                    default:
                        break;
                }
            }
        }

        #region IDataGridViewEditingControl Members

        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.ForeColor = dataGridViewCellStyle.ForeColor;
            this.BackColor = dataGridViewCellStyle.BackColor;
        }

        public DataGridView EditingControlDataGridView
        {
            get
            {
                return _dataGridView;
            }
            set
            {
                _dataGridView = value;
            }
        }

        public object EditingControlFormattedValue
        {
            get
            {
                return this.Rtf;
            }
            set
            {
                if (value is string)
                    this.Text = value as string;
            }
        }

        public int EditingControlRowIndex
        {
            get
            {
                return _rowIndex;
            }
            set
            {
                _rowIndex = value;
            }
        }

        public bool EditingControlValueChanged
        {
            get
            {
                return _valueChanged;
            }
            set
            {
                _valueChanged = value;
            }
        }

        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            switch ((keyData & Keys.KeyCode))
            {
                case Keys.Return:
                    if ((((keyData & (Keys.Alt | Keys.Control | Keys.Shift)) == Keys.Shift) && this.Multiline))
                    {
                        return true;
                    }
                    break;
                case Keys.Left:
                case Keys.Right:
                case Keys.Up:
                case Keys.Down:
                    return true;
            }

            return !dataGridViewWantsInputKey;
        }

        public Cursor EditingPanelCursor
        {
            get { return this.Cursor; }
        }

        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return this.Text;
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {
        }

        public bool RepositionEditingControlOnValueChange
        {
            get { return false; }
        }

        #endregion
    }

    //[ToolboxItem(false)]
    //public sealed class DoubleTextBoxCell : DataGridViewCell
    //{
    //    // Methods
    //    public DoubleTextBoxCell()
    //    {
    //    }

    //    public override object Clone()
    //    {
    //        DataGridViewCell cell;
    //        Type type = base.GetType();
    //        if (type == typeof(DoubleTextBoxCell))
    //        {
    //            cell = base.Clone() as DoubleTextBoxCell;
    //        }
    //        else
    //        {
    //            cell = (DoubleTextBoxCell)Activator.CreateInstance(type);
    //        }
    //        return cell;
    //    }

    //    public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
    //    {
    //        base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
    //        RichTextBoxControl rtbControl = base.DataGridView.EditingControl as RichTextBoxControl;
    //        if (rtbControl != null)
    //        {
    //            rtbControl.BorderStyle = BorderStyle.None;
    //            rtbControl.Multiline = dataGridViewCellStyle.WrapMode == DataGridViewTriState.True;
    //            rtbControl.WordWrap = false;
    //            string str = initialFormattedValue as string;
    //            if (str == null)
    //            {
    //                rtbControl.Text = string.Empty;
    //            }
    //            else
    //            {
    //                rtbControl.Text = str;
    //            }
    //        }
    //    }

    //    protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
    //    {
    //        base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
           
    //        string[] ss = (formattedValue.ToString()).Split(new string[] { "\n" }, 2, StringSplitOptions.RemoveEmptyEntries);
    //        if (Selected)
    //        {
    //            graphics.FillRectangle(new SolidBrush(cellStyle.SelectionBackColor), cellBounds);
    //            graphics.DrawString(ss[0], cellStyle.Font, new SolidBrush(cellStyle.SelectionForeColor), 3.0f + cellBounds.X, (cellBounds.Height - cellStyle.Font.Size * 2) / 4.0f + cellBounds.Y);
    //            graphics.DrawString(ss[1], cellStyle.Font, new SolidBrush(Color.Black), 3.0f + cellBounds.X, (cellBounds.Height - cellStyle.Font.Height * 2) / 4.0f + cellBounds.Height / 2.0f + cellBounds.Y);
    //        }
    //        else
    //        {
    //            graphics.FillRectangle(new SolidBrush(cellStyle.BackColor), cellBounds);
    //            graphics.DrawString(ss[0], cellStyle.Font, new SolidBrush(cellStyle.ForeColor), 3.0f + cellBounds.X, (cellBounds.Height - cellStyle.Font.Size * 2) / 4.0f + cellBounds.Y);
    //            graphics.DrawString(ss[1], cellStyle.Font, new SolidBrush(Color.Gray), 3.0f + cellBounds.X, (cellBounds.Height - cellStyle.Font.Height * 2) / 4.0f + cellBounds.Height / 2.0f + cellBounds.Y);
    //        }
    //    }
        
    //    // Properties
    //    private DataGridViewTextBoxEditingControl EditingTextBox { get; set; }

    //    public override Type FormattedValueType
    //    {
    //        get
    //        {
    //            return typeof(string);
    //        }
    //    }

    //    //获取或设置单元格中值的数据类型
    //    public override Type ValueType
    //    {
    //        get
    //        {
    //            return typeof(string);
    //        }
    //    }

    //    //获取单元格的寄宿编辑控件的类型
    //    public override Type EditType
    //    {
    //        get
    //        {
    //            return typeof(TextBox);
    //        }
    //    }

    //    //获取新记录所在行中单元格的默认值
    //    public override object DefaultNewRowValue
    //    {
    //        get
    //        {
    //            return string.Empty;
    //        }
    //    }

    //}

    [ToolboxItem(false)]
    public sealed class DoubleTextBoxCellStyle : DataGridViewCellStyle
    {
        public DoubleTextBoxCellStyle()
        {
        }

        public override DataGridViewCellStyle Clone()
        {
            DataGridViewCellStyle cellStyle;
            Type type = base.GetType();
            if (type == typeof(DoubleTextBoxCellStyle))
            {
                cellStyle = base.Clone() as DoubleTextBoxCellStyle;
            }
            else
            {
                cellStyle = (DoubleTextBoxCellStyle)Activator.CreateInstance(type);
            }
            return cellStyle;
        }

        private DataGridViewTriState multiline = DataGridViewTriState.NotSet;
        public DataGridViewTriState Multiline
        {
            get { return multiline; }
            set { multiline = value; }
        }

        private Font font2 = SystemFonts.DefaultFont;
        public Font Font2
        {
            get { return font2; }
            set { font2 = value; }
        }

        private Color foreColor = SystemColors.Control;
        public Color ForeColor2
        {
            get { return foreColor; }
            set { foreColor = value; }
        }
    }

}
