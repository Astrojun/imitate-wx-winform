using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using WindowsFormsTest2.Properties;

namespace WindowsFormsTest2.ControlInfo
{
    [ToolboxBitmap(typeof(DataGridViewImageColumn), "DataGridViewImageColumn.bmp")]
    public sealed partial class DataGridViewImageAndTextColumn : DataGridViewColumn
    {
        public DataGridViewImageAndTextColumn()
            : base(new DataGridViewImageAndTextCell())
        {
        }

        public override object Clone()
        {
            DataGridViewColumn column;
            Type type = base.GetType();
            if (type == typeof(DataGridViewImageAndTextColumn))
            {
                column = base.Clone() as DataGridViewImageAndTextColumn;
            }
            else
            {
                column = (DataGridViewImageAndTextColumn)Activator.CreateInstance(type);
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
                if (value != null && !value.GetType().IsAssignableFrom(typeof(DataGridViewImageAndTextCell)))
                {
                    throw new InvalidCastException("必须是一个单元格");
                }
                base.CellTemplate = value;
            }
        }

    }

    public class DataGridViewImageAndTextCell : DataGridViewTextBoxCell
    {
        private Image imageValue;
        private Size imageSize = new Size(33, 33);

        public override object Clone()
        {
            DataGridViewImageAndTextCell c = base.Clone() as DataGridViewImageAndTextCell;
            c.imageValue = this.imageValue;
            c.imageSize = this.imageSize;
            return c;
        }

        public DataGridViewImageAndTextCell()
        {
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

        public Image ImageValue
        {
            get
            {
                return this.imageValue;
            }
            set
            {
                if (this.imageValue != value)
                {
                    this.imageValue = value;
                }
            }
        }

        public Size ImageSize
        {
            get { return this.imageSize; }
            set { this.imageSize = value; }
        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates cellState, object value, object formattedValue, string errorText,
            DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            if (value != null)
            {
                try
                {
                    JObject jObject = JObject.Parse(value as string);
                    System.Resources.ResourceManager m = new System.Resources.ResourceManager("WindowsFormsTest2.Properties.Resources", typeof(Resources).Assembly);
                    this.ImageValue = m.GetObject(jObject["image"].ToString()) as Image;
                }
                catch (ArgumentNullException e)
                {
                    Console.WriteLine(e);
                }
                if (ImageValue == null)
                {
                    cellState = cellState & (~DataGridViewElementStates.Selected);
                }
            }
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, null, null, errorText, cellStyle, advancedBorderStyle, paintParts);

            if (value != null)
            {
                JObject jObject = JObject.Parse(value as string);
                bool flag3 = (cellState & DataGridViewElementStates.Selected) != DataGridViewElementStates.None;
                Rectangle rectImage = new Rectangle(cellBounds.X, cellBounds.Y, ImageSize.Width + 20, cellBounds.Height);
                Rectangle rect = new Rectangle(rectImage.Right, (cellBounds.Height - ImageSize.Height) / 2 + cellBounds.Y, cellBounds.Width - rectImage.Width, ImageSize.Height);
                rectImage.Inflate(-10, (ImageSize.Height - cellBounds.Height) / 2);
                
                try
                {
                    if (this.ImageValue != null)
                    {
                        GraphicsContainer container = graphics.BeginContainer();
                        graphics.DrawImage(this.ImageValue, rectImage);
                        graphics.EndContainer(container);
                    }
                }
                catch(ArgumentNullException e)
                {
                    Console.WriteLine(e);
                }
                
                TextFormatFlags flags = WindowsFormsTest2.ControlHelper.TextHelper.ComputeTextFormatFlagsForCellStyleAlignment(base.DataGridView.RightToLeft == RightToLeft.Yes, cellStyle.Alignment, cellStyle.WrapMode);
                if ((flags & TextFormatFlags.SingleLine) != TextFormatFlags.Default)
                {
                    flags |= TextFormatFlags.EndEllipsis;
                }
                if (this.ImageValue == null)
                {
                    int offset = rect.X / 2;
                    rect.Inflate(offset, 0);
                    rect.Offset(-offset + 10, 0);
                    graphics.FillRectangle(Brushes.Silver, cellBounds);
                    this.OwningRow.DataGridView.Rows[rowIndex].Height = TextRenderer.MeasureText(jObject["text"].ToString(), cellStyle.Font, rect.Size, flags).Height + 6;
                }
                
                TextRenderer.DrawText(graphics, jObject["text"].ToString(), cellStyle.Font, rect, flag3 ? cellStyle.SelectionForeColor : cellStyle.ForeColor, flags);
            }
        }
    }
}
