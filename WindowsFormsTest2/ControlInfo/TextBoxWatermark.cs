﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsFormsTest2.ControlInfo
{
    public partial class TextBoxWatermark : TextBox
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern IntPtr GetWindowDC(IntPtr hWnd);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        const int WM_PAINT = 0x000F;
        const int WM_NCPAINT = 0x0085;

        private string hintText = string.Empty;
        private Font hintFont = SystemFonts.DefaultFont;

        [Description("水印文本")]
        public string HintText
        {
            get { return this.hintText; }
            set
            {
                this.hintText = value;
                this.Invalidate();
            }
        }

        [Description("用于显示水印文本的字体")]
        public Font HintFont
        {
            get { return this.hintFont; }
            set
            {
                this.hintFont = value;
                this.Invalidate();
            }
        }

        public TextBoxWatermark()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            ToolStripMenuItem ToolStripMenuItemPaste = new ToolStripMenuItem();
            ToolStripMenuItemPaste.Name = "ToolStripMenuItemPaste";
            ToolStripMenuItemPaste.Size = new System.Drawing.Size(152, 22);
            ToolStripMenuItemPaste.Text = "粘贴";
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            ToolStripMenuItemPaste});
            contextMenuStrip.Name = "contextMenuStrip";
            contextMenuStrip.Size = new System.Drawing.Size(153, 48);
            ToolStripMenuItemPaste.Enabled = Clipboard.ContainsText();
            contextMenuStrip.Renderer = new ToolStripRendererEx();
            contextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(contextMenuStrip_ItemClicked);

            this.ContextMenuStrip = contextMenuStrip;
        }

        private void contextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "ToolStripMenuItemPaste":
                    this.Text = Clipboard.GetText(TextDataFormat.Text);
                    break;
                default:
                    break;
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_PAINT || m.Msg == WM_NCPAINT)
            {
                IntPtr hDC = GetWindowDC(m.HWnd);
                if (hDC.ToInt32() == 0)
                {
                    return;
                }
                WmPaint(hDC);

                //返回结果 
                m.Result = IntPtr.Zero;
                //释放 
                ReleaseDC(m.HWnd, hDC);
            }
        }

        //水印
        private void WmPaint(IntPtr hDC)
        {
            using (Graphics graphics = Graphics.FromHdc(hDC))
            {
                if (Text.Length == 0 && !string.IsNullOrEmpty(hintText) && !Focused)
                {
                    TextFormatFlags format = TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                    if (RightToLeft == RightToLeft.Yes)
                    {
                        format |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;
                    }
                    TextRenderer.DrawText(graphics, this.hintText, this.hintFont, new Rectangle(-2, -2, ClientSize.Width, ClientSize.Height), Color.Gray, format);
                }
            }
        }

    }
}
