using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsTest2.FormInfo
{
    public partial class FormSetting : Form
    {
        public FormSetting()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            Font font = new Font(this.Font.FontFamily, 12);
            e.Graphics.DrawString("设置", font, Brushes.Black,
                new Rectangle(5, 1, this.Width - buttonClose.Width - buttonMin.Width - 2, buttonMin.Height), format);
            Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            e.Graphics.DrawRectangle(Pens.Silver, rect);
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0201:                //鼠标左键按下的消息
                    m.Msg = 0x00A1;         //更改消息为非客户区按下鼠标
                    m.LParam = IntPtr.Zero; //默认值
                    m.WParam = new IntPtr(2);//鼠标放在标题栏内
                    base.WndProc(ref m);
                    break;
                //case 0x00A3:  //WM_NCLBUTTONDBLCLK=163 <0xA3>拦截鼠标非客户区左键双击消息,决定窗体是否最大化显示
                //    Console.WriteLine(0x00A3);
                //    base.WndProc(ref m);  //这种方法的好处是自己不用处理鼠标形状了.
                //    return;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonMin_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Minimized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

    }
}
