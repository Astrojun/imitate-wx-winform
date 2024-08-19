using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsFormsTest2.ClassInfo
{
    class FriendInfo
    {
        public FriendInfo(string nickName)
        {
            this.nickName = nickName;
        }

        private string nickName;

        public string NickName
        {
            get { return nickName; }
            set { nickName = value; }
        }
    }
}
