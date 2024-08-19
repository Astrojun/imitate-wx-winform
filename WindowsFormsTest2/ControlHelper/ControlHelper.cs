using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace WindowsFormsTest2.ControlHelper
{
    class ControlHelper
    {

        public static void DoubleBuffered(System.Windows.Forms.Control obj, bool setting)
        {
            Type dgvType = obj.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                  BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(obj, setting, null);
        }
    }
}
