﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WindowsFormsTest2.FormInfo;

namespace WindowsFormsTest2
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormLogIn());
        }
    }
}
