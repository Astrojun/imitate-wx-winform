using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsTest2.ControlHelper
{
    public static class TextHelper
    {
        internal static StringAlignment TranslateAligment(HorizontalAlignment aligment)
        {
            if (aligment == HorizontalAlignment.Left)
                return StringAlignment.Near;
            else if (aligment == HorizontalAlignment.Right)
                return StringAlignment.Far;
            else
                return StringAlignment.Center;
        }

        internal static HorizontalAlignment TranslateGridColumnAligment(DataGridViewContentAlignment aligment)
        {
            if (aligment == DataGridViewContentAlignment.BottomLeft || aligment == DataGridViewContentAlignment.MiddleLeft || aligment == DataGridViewContentAlignment.TopLeft)
                return HorizontalAlignment.Left;
            else if (aligment == DataGridViewContentAlignment.BottomRight || aligment == DataGridViewContentAlignment.MiddleRight || aligment == DataGridViewContentAlignment.TopRight  )
                return HorizontalAlignment.Right;
            else
                return HorizontalAlignment.Left;
        }

        internal static TextFormatFlags TranslateAligmentToFlag(HorizontalAlignment aligment)
        {
            if (aligment == HorizontalAlignment.Left)
                return TextFormatFlags.Left;
            else if (aligment == HorizontalAlignment.Right)
                return TextFormatFlags.Right;
            else
                return TextFormatFlags.HorizontalCenter;
        }

        internal static TextFormatFlags TranslateTrimmingToFlag(StringTrimming trimming)
        {
            if (trimming == StringTrimming.EllipsisCharacter)
                return TextFormatFlags.EndEllipsis;
            else if (trimming == StringTrimming.EllipsisPath)
                return TextFormatFlags.PathEllipsis;
            if (trimming == StringTrimming.EllipsisWord)
                return TextFormatFlags.WordEllipsis;
            if (trimming == StringTrimming.Word)
                return TextFormatFlags.WordBreak;
            else
                return TextFormatFlags.Default;
        }

        internal static TextFormatFlags TabAlignmentToFlag(TabAlignment alignment)
        {
            TextFormatFlags flag = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
            switch (alignment)
            {
                case TabAlignment.Left:
                case TabAlignment.Right:
                    flag |= TextFormatFlags.WordBreak;
                    break;
                case TabAlignment.Bottom:
                case TabAlignment.Top:
                    flag |= (TextFormatFlags.SingleLine);
                    break;
            }
            return flag;
        }

        internal static TextFormatFlags ComputeTextFormatFlagsForCellStyleAlignment(bool rightToLeft, DataGridViewContentAlignment alignment, DataGridViewTriState wrapMode)
        {
            TextFormatFlags horizontalCenter;
            switch (alignment)
            {
                case DataGridViewContentAlignment.TopLeft:
                    horizontalCenter = TextFormatFlags.Default;
                    if (rightToLeft)
                    {
                        horizontalCenter |= TextFormatFlags.Right;
                    }
                    break;

                case DataGridViewContentAlignment.TopCenter:
                    horizontalCenter = TextFormatFlags.HorizontalCenter;
                    break;

                case DataGridViewContentAlignment.TopRight:
                    horizontalCenter = TextFormatFlags.Default;
                    if (!rightToLeft)
                    {
                        horizontalCenter |= TextFormatFlags.Right;
                    }
                    break;

                case DataGridViewContentAlignment.MiddleLeft:
                    horizontalCenter = TextFormatFlags.VerticalCenter;
                    if (rightToLeft)
                    {
                        horizontalCenter |= TextFormatFlags.Right;
                    }
                    break;

                case DataGridViewContentAlignment.MiddleCenter:
                    horizontalCenter = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
                    break;

                case DataGridViewContentAlignment.BottomCenter:
                    horizontalCenter = TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
                    break;

                case DataGridViewContentAlignment.BottomRight:
                    horizontalCenter = TextFormatFlags.Bottom;
                    if (!rightToLeft)
                    {
                        horizontalCenter |= TextFormatFlags.Right;
                    }
                    break;

                case DataGridViewContentAlignment.MiddleRight:
                    horizontalCenter = TextFormatFlags.VerticalCenter;
                    if (!rightToLeft)
                    {
                        horizontalCenter |= TextFormatFlags.Right;
                    }
                    break;

                case DataGridViewContentAlignment.BottomLeft:
                    horizontalCenter = TextFormatFlags.Bottom;
                    if (rightToLeft)
                    {
                        horizontalCenter |= TextFormatFlags.Right;
                    }
                    break;

                default:
                    horizontalCenter = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
                    break;
            }
            if (wrapMode == DataGridViewTriState.False)
            {
                horizontalCenter |= TextFormatFlags.SingleLine;
            }
            else
            {
                horizontalCenter |= TextFormatFlags.WordBreak;
            }
            horizontalCenter |= TextFormatFlags.NoPrefix;
            horizontalCenter |= TextFormatFlags.PreserveGraphicsClipping;
            if (rightToLeft)
            {
                horizontalCenter |= TextFormatFlags.RightToLeft;
            }
            return horizontalCenter;
        }

    }
}
