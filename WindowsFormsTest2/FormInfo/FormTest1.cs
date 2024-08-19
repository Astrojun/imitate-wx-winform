using System.Windows.Forms;
using WindowsFormsTest2.ControlInfo;
using WindowsFormsTest2.Properties;
using System;
using System.Threading.Tasks;

namespace WindowsFormsTest2.FormInfo
{
    public partial class FormTest1 : Form
    {
        public FormTest1()
        {
            InitializeComponent();

//            this.richTextBox1.Rtf = @"{\rtf1\ansi\ansicpg936\deff0\deflang1033\deflangfe2052
//{\fonttbl{\f0\fmodern\fprq6\fcharset134 \'cb\'ce\'cc\'e5;}{\f1\fnil\fcharset134 \'cb\'ce\'cc\'e5;}}
//{\colortbl;\red255\green0\blue0;\red0\green0\blue255;\red0\green255\blue0;}
//\viewkind4\uc1\pard\cf0\lang2052\f0\fs20 sss\par
//\cf1 ddd\par}";
            
            ToolStripManager.Renderer = new ToolStripRendererEx();
            
//            this.richTextBox1.Rtf = @"{\rtf1\ansi\ansicpg936\deff0\deflang1033\deflangfe2052
//{\fonttbl{\f0\fmodern\fprq6\fcharset134 \'cb\'ce\'cc\'e5;}{\f1\fnil\fcharset134 \'cb\'ce\'cc\'e5;}}
//{\colortbl;\red190\green190\blue190;}
//\viewkind4\uc1\pard\cf0\lang2052\f0\fs20 ssss\par
//\cf1\fs18 ddddd\par}";
            //Console.WriteLine(richTextBox1.Rtf);

            string d = "";
            int2string(10, out d);
        }
        private void int2string(int n,out string k)
        {
            k = n.ToString();
        }

        private void toolStripButton1_MouseEnter(object sender, System.EventArgs e)
        {
        }

        private void toolStripSplitButton1_ButtonClick(object sender, System.EventArgs e)
        {
        }

    }
}
