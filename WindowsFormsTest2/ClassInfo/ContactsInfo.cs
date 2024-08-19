using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace WindowsFormsTest2.ClassInfo
{
    class ContactsInfo : IDisposable
    {
        public ContactsInfo(Image icon, string name, string time)
        {
            this.Icon = icon;
            this.Time = time;
            this.Name = name;
        }

        ~ContactsInfo()
        {
            Dispose();
        }

        public Image Icon
        {
            get;
            set;
        }

        private string name;
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public string Time
        {
            get;
            set;
        }

//        private string nameRtf;
//        public string NameRtf
//        {
//            get { return this.nameRtf; }
//            set
//            {
//                this.name = value;
//                string[] strValue = value.Split(new string[] { "\n" }, 2, StringSplitOptions.RemoveEmptyEntries);
//                //string[] str = new string[] { (strValue[0].Length > 6 ? strValue[0].Substring(0, 6) + "..." : strValue[0]), (strValue[1].Length > 9 ? strValue[1].Substring(0, 9) + "..." : strValue[1]) };
////                this.nameRtf = @"{\rtf1\ansi\ansicpg936\deff0\deflang1033\deflangfe2052
////{\fonttbl{\f0\fmodern\fprq6\fcharset134 \'cb\'ce\'cc\'e5;}{\f1\fnil\fcharset134 \'cb\'ce\'cc\'e5;}}
////{\colortbl;\red190\green190\blue190;}
////\viewkind4\uc1\pard\cf0\lang2052\f0\fs20" + strValue[0] + @"\par
////\cf1\fs18" + strValue[1] + @"\par}";
//            }
//        }


        public void Dispose()
        {
            Icon.Dispose();
        }
    }
}
