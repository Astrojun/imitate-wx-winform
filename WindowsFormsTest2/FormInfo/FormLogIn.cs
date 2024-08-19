using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WindowsFormsTest2.Properties;
using WindowsFormsTest2.ClassInfo;
using WindowsFormsTest2.ControlInfo;
using Newtonsoft.Json.Linq;
using DevCapture;
using WindowsFormsTest2.ControlHelper;

namespace WindowsFormsTest2.FormInfo
{
    public partial class FormLogIn : Form
    {

        private Point _movePoint;
        private bool _moveFlag = false;
        private bool _sizeChangeFlag = false; //表示鼠标当前是否处于按下状态，初始值为否
        private Point dataGridViewChatPoint;
        private BindingList<ContactsInfo> contactsInfoList;
        private BindingList<FriendInfo> friendList;
        private FrmCapture m_frmCapture;


        #region 构造函数
        public FormLogIn()
        {
            InitializeComponent();
            this.MaximumSize = new System.Drawing.Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
            panelFormButton.BringToFront();
            FrmCapture.LoadPlugins(@"D:\ChromeDownload\capture\Plugins");

            contactsInfoList = new BindingList<ContactsInfo>();
            friendList = new BindingList<FriendInfo>();
            for (int i = 0; i < imageList.Images.Count; i++)
            {
                contactsInfoList.Add(new ContactsInfo(imageList.Images[i], "昵称" + i.ToString() + "\n" + "消息" + i.ToString(), DateTime.Now.ToShortTimeString()));
                if (i == 1 || i == 4 || i == 8)
                    friendList.Add(new FriendInfo("{image:,text:'昵称" + i.ToString() + "'}"));
                else
                    friendList.Add(new FriendInfo("{image:'PictureLogIn',text:'昵称" + i.ToString() + "'}"));
            }

            this.dataGridViewChat.DataSource = contactsInfoList;
            resizeDataGridViewScrollBar(vScrollBarExChat, dataGridViewChat);

            this.dataGridViewFriend.DataSource = friendList;
            resizeDataGridViewScrollBar(vScrollBarExFriend, dataGridViewFriend);

            this.radioButtonChat.Checked = true;
            ToolStripManager.Renderer = new WindowsFormsTest2.ControlInfo.ToolStripRendererEx();
            //this.toolStrip.Renderer = new ToolStripRendererEx();
            //this.contextMenuStrip.Renderer = new ToolStripRendererEx();
        }

        #endregion

        private void button_MouseClick(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            switch (button.Name)
            {
                case "buttonMin":
                    this.WindowState = FormWindowState.Minimized;
                    break;
                case "buttonMax":
                    if (this.WindowState != FormWindowState.Maximized)
                        this.WindowState = FormWindowState.Maximized;
                    else
                        this.WindowState = FormWindowState.Normal;
                    break;
                case "buttonClose":
                    this.Close();
                    break;
            }
        }

        private void buttonSetting_Click(object sender, EventArgs e)
        {
            FormSetting form = new FormSetting();
            form.Show();
        }

        private void dataGridViewChat_SizeChanged(object sender, EventArgs e)
        {
            if (dataGridViewChat.Rows.GetRowsHeight(DataGridViewElementStates.Visible) > dataGridViewChat.Height)
            {
                vScrollBarExChat.Maximum = dataGridViewChat.Rows.GetRowsHeight(DataGridViewElementStates.Visible);
                vScrollBarExChat.LargeChange = Height - (dataGridViewChat.ColumnHeadersVisible ? dataGridViewChat.ColumnHeadersHeight : 0) - dataGridViewChat.Rows.GetRowsHeight(DataGridViewElementStates.Frozen);
                vScrollBarExChat.SmallChange = vScrollBarExChat.LargeChange / 5;
                vScrollBarExChat.Visible = true;
            }
            else
            {
                vScrollBarExChat.Visible = false;
            }
        }

        #region datagridview行选择事件
        private void dataGridViewChat_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewChat.SelectedRows.Count > 0)
                buttonNickName.Text = (dataGridViewChat.SelectedRows[0].DataBoundItem as ContactsInfo).Name.Split(new string[] { "\n" }, 2, StringSplitOptions.RemoveEmptyEntries)[0];
        }

        private void dataGridViewFriend_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewFriend.SelectedRows.Count > 0)
            {
                JObject json = JObject.Parse((dataGridViewFriend.SelectedRows[0].DataBoundItem as FriendInfo).NickName);
                labelName.Text = json["text"].ToString();
            }
        }

        #endregion

        #region 窗体的移动和缩放
        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (!(sender is Form))
                {
                    _movePoint = new Point(-e.X - (sender as Control).Location.X, -e.Y - (sender as Control).Location.Y);
                }
                else
                {
                    _movePoint = new Point(-e.X, -e.Y);
                }
                _moveFlag = true;
            }
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left && _moveFlag)
            {
                Point myPosittion = Control.MousePosition;//获取当前鼠标的屏幕坐标
                myPosittion.Offset(_movePoint.X, _movePoint.Y);//重载当前鼠标的位置
                this.DesktopLocation = myPosittion;//设置当前窗体在屏幕上的位置
            }
        }

        private void panel2_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left && _moveFlag)
            {
                _moveFlag = false;
            }
        }

        private void panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left && _moveFlag)
            {
                Point myPosittion = Control.MousePosition;//获取当前鼠标的屏幕坐标
                myPosittion.Offset(_movePoint.X, _movePoint.Y);//重载当前鼠标的位置
                this.DesktopLocation = myPosittion;//设置当前窗体在屏幕上的位置
            }
        }

        private void panel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (!(sender is Form))
                {
                    _movePoint = new Point(-e.X - (sender as Control).Location.X, -e.Y - (sender as Control).Location.Y);
                }
                else
                {
                    _movePoint = new Point(-e.X, -e.Y);
                }
                _moveFlag = true;
            }
        }

        private void panel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                _moveFlag = false;
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                //WM_NCHITTEST = 0x0084,  当用户在在非客户区移动鼠标、按住或释放鼠标时发送本消息(击中测试);若鼠标没有被捕获,则本消息在窗口得到光标之后发出,否则消息发送到捕获到鼠标的窗口
                case 0x0084:
                    base.WndProc(ref m);
                    Point vPoint = new Point((int)m.LParam & 0xFFFF,
                        (int)m.LParam >> 16 & 0xFFFF);
                    vPoint = PointToClient(vPoint);
                    if (vPoint.X <= 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)FormHelper.HTTOPLEFT;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)FormHelper.HTBOTTOMLEFT;
                        else m.Result = (IntPtr)FormHelper.HTLEFT;
                    else if (vPoint.X >= ClientSize.Width - 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)FormHelper.HTTOPRIGHT;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)FormHelper.HTBOTTOMRIGHT;
                        else m.Result = (IntPtr)FormHelper.HTRIGHT;
                    else if (vPoint.Y <= 5)
                        m.Result = (IntPtr)FormHelper.HTTOP;
                    else if (vPoint.Y >= ClientSize.Height - 5)
                        m.Result = (IntPtr)FormHelper.HTBOTTOM;
                    break;
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

        #endregion

        #region panelSendMsg高度改变

        private void panelSendMsg_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = PointToClient(Cursor.Position);
            if (Math.Abs(p.Y - panelSendMsg.Top) <= 4)
            {
                Cursor = Cursors.HSplit;
            }
            else
            {
                Cursor = Cursors.Arrow;
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Left && _sizeChangeFlag)
            {
                panelSendMsg.Height = panelSendMsg.Bottom - p.Y;
            }
        }

        private void panelSendMsg_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                _sizeChangeFlag = false;
            }
        }

        private void panelSendMsg_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                _sizeChangeFlag = true;
            }
        }

        private void panelSendMsg_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        #endregion

        #region 其他事件

        private void panelSendMsg_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawLine(Pens.Gainsboro, 0, 0, panelSendMsg.Width, 0);
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            switch (radioButton.Name)
            {
                case "radioButtonChat":
                    radioButton.Image = radioButton.Checked ? Resources.btChatSel : Resources.btChat;
                    tabControl.SelectedIndex = 0;
                    buttonNickName.Visible = true;
                    labelName.Visible = false;
                    break;
                case "radioButtonFriend":
                    radioButton.Image = radioButton.Checked ? Resources.btFriendSel : Resources.btFriend;
                    tabControl.SelectedIndex = 1;
                    buttonNickName.Visible = false;
                    labelName.Visible = true;
                    break;
                case "radioButtonFavorite":
                    radioButton.Image = radioButton.Checked ? Resources.btFavoriteSel : Resources.btFavorite;
                    tabControl.SelectedIndex = 2;
                    break;
            }
        }

        private void buttonName_TextChanged(object sender, EventArgs e)
        {
            Control c = sender as Control;
            c.Location = new Point((panelContext.Width - c.Width) / 2, 5);
        }

        #endregion

        #region ToolStripMenuItem事件

        private void cutScreen_Click(object sender, EventArgs e)
        {
            if ((sender as ToolStripItem).Name.Equals(toolStripMenuItemCutScreen.Name))
            {
                this.WindowState = FormWindowState.Minimized;
            }

            if (m_frmCapture == null || m_frmCapture.IsDisposed)
                m_frmCapture = new FrmCapture(false, false);  //bCaptureCursor 是否捕获鼠标, bFromClipBoard 是否从剪切板截图
            m_frmCapture.Show();

            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void toolStripButtonSendFile_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
        }

        #endregion

        #region 滚动条事件
        private void vScrollBarExChat_Scroll(object sender, ScrollEventArgs e)
        {
            int vPosition = (int)Math.Ceiling(Convert.ToDouble(e.NewValue) * Convert.ToDouble(dataGridViewChat.Rows.Count) / Convert.ToDouble(vScrollBarExChat.Maximum));
            if (vPosition < dataGridViewChat.Rows.Count)
            {
                dataGridViewChat.FirstDisplayedScrollingRowIndex = vPosition;
            }
        }

        private void vScrollBarExFriend_Scroll(object sender, ScrollEventArgs e)
        {
            int vPosition = (int)Math.Ceiling(Convert.ToDouble(e.NewValue) * Convert.ToDouble(dataGridViewFriend.Rows.Count) / Convert.ToDouble(vScrollBarExFriend.Maximum));
            if (vPosition < dataGridViewFriend.Rows.Count)
            {
                dataGridViewFriend.FirstDisplayedScrollingRowIndex = vPosition;
            }
        }

        private void vScrollBarExChat_VisibleChanged(object sender, EventArgs e)
        {
            VScrollBarEx vScrollBarEx = sender as VScrollBarEx;
            if (vScrollBarEx.Visible)
            {
                vScrollBarEx.Maximum = dataGridViewChat.Rows.GetRowsHeight(DataGridViewElementStates.Visible);
                vScrollBarEx.LargeChange = Height - (dataGridViewChat.ColumnHeadersVisible ? dataGridViewChat.ColumnHeadersHeight : 0) - dataGridViewChat.Rows.GetRowsHeight(DataGridViewElementStates.Frozen);
                vScrollBarEx.SmallChange = vScrollBarEx.LargeChange / 5;
                vScrollBarEx.ResizeBarRectangle();
            }
        }

        private void vScrollBarExFriend_VisibleChanged(object sender, EventArgs e)
        {
            VScrollBarEx vScrollBarEx = sender as VScrollBarEx;
            if (vScrollBarEx.Visible)
            {
                vScrollBarEx.Maximum = dataGridViewFriend.Rows.GetRowsHeight(DataGridViewElementStates.Visible);
                vScrollBarEx.LargeChange = Height - (dataGridViewFriend.ColumnHeadersVisible ? dataGridViewFriend.ColumnHeadersHeight : 0) - dataGridViewFriend.Rows.GetRowsHeight(DataGridViewElementStates.Frozen);
                vScrollBarEx.SmallChange = vScrollBarEx.LargeChange / 5;
                vScrollBarEx.ResizeBarRectangle();
            }
        }

        #endregion

        #region 行删除事件
        private void ToolStripMenuItemDelMsg_Click(object sender, EventArgs e)
        {
            int oldValue = vScrollBarExChat.Value;
            Point p = dataGridViewChat.PointToClient(dataGridViewChatPoint);
            DataGridView.HitTestInfo hitInfo = dataGridViewChat.HitTest(p.X, p.Y);
            if (hitInfo.RowIndex > 0)
                this.contactsInfoList.RemoveAt(hitInfo.RowIndex);
            resizeDataGridViewScrollBar(vScrollBarExChat, dataGridViewChat);
            if (oldValue != vScrollBarExChat.Value)
            {
                vScrollBarExChat_Scroll(vScrollBarExChat,
                    new ScrollEventArgs(ScrollEventType.EndScroll, oldValue, vScrollBarExChat.Value, ScrollOrientation.VerticalScroll));
            }
            this.dataGridViewChat.Invalidate();
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            dataGridViewChatPoint = Cursor.Position;
            Point p = dataGridViewChat.PointToClient(dataGridViewChatPoint);
            DataGridView.HitTestInfo hitInfo = dataGridViewChat.HitTest(p.X, p.Y);
        }

        #endregion

        #region 其他方法

        private void resizeDataGridViewScrollBar(VScrollBarEx vScrollBarEx, DataGridView dataGridView)
        {
            vScrollBarEx.Visible = (dataGridView.Rows.GetRowsHeight(DataGridViewElementStates.Visible) > dataGridView.Height) ? true : false;
            if (dataGridView != null)
            {
                vScrollBarEx.Maximum = dataGridViewChat.Rows.GetRowsHeight(DataGridViewElementStates.Visible);
                vScrollBarEx.LargeChange = Height - (dataGridViewChat.ColumnHeadersVisible ? dataGridViewChat.ColumnHeadersHeight : 0) - dataGridViewChat.Rows.GetRowsHeight(DataGridViewElementStates.Frozen);
                vScrollBarEx.SmallChange = vScrollBarEx.LargeChange / 5;
                vScrollBarEx.ResizeBarRectangle();
            }
        }

        #endregion

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBoxSearch_EndEdit(object sender, EventArgs e)
        {
            if (textBoxSearch.Text != null && textBoxSearch.Text.Length > 0)
            {
                switch (this.tabControl.SelectedIndex)
                {
                    case 0:
                    case 1:
                        for (int i = 0; i < contactsInfoList.Count; i++)
                        {
                            if (contactsInfoList[i].Name.Contains(textBoxSearch.Text))
                            {
                                Console.WriteLine(contactsInfoList[i].Name.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)[0]);
                            }
                        }
                        for (int i = 0; i < friendList.Count; i++)
                        {
                            if (friendList[i].NickName.Contains(textBoxSearch.Text))
                            {
                                JObject json = JObject.Parse(friendList[i].NickName);
                                Console.WriteLine(json["text"].ToString());
                            }
                        }
                        break;
                    case 2:
                        break;
                    default:
                        break;
                }
            }
        }

    }
}
