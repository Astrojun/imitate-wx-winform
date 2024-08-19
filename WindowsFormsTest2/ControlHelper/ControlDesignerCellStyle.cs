using System.Windows.Forms.Design;
using System.ComponentModel.Design;
using System.ComponentModel;
using System;

namespace WindowsFormsTest2.ControlHelper
{
    public sealed class ControlDesignerCellStyle : ControlDesigner
    {
        //DesignerActionListCollection _actionLists;

        //public override SelectionRules SelectionRules
        //{
        //    get
        //    {
        //        if (TheStackPanel.AutoSize) // 如果AutoSize
        //        {
        //            return SelectionRules.Moveable; // 则只能移动, 不能手动改变大小
        //        }
        //        else // 否则可以改变大小
        //        {
        //            return SelectionRules.Moveable | SelectionRules.AllSizeable;
        //        }
        //    }
        //}

        //public override System.ComponentModel.Design.DesignerActionListCollection ActionLists
        //{
        //    get
        //    {
        //        if (_actionLists == null)
        //        {
        //            _actionLists = new DesignerActionListCollection();
        //            // 给 Smart Tag 添加 StackPanelActionList 里的选项
        //            _actionLists.Add(new StackPanelActionList(this.Component));
        //            // 给 Smart Tag 添加 LinearGradientPanelActionList 里的选项 , 因为 StackPanel 继承了 LinearGradientPanel 
        //            _actionLists.Add(new LinearGradientPanelActionList(this.Component));
        //        }
        //        return _actionLists;
        //    }
        //}

    }


    //internal class StackPanelActionList : DesignerActionList
    //{
        //private StackPanel _stackPanel;

        //public StackPanelActionList(IComponent component)
        //    : base(component)
        //{
        //    this._stackPanel = component as StackPanel;
        //}

        //private PropertyDescriptor GetPropertyByName(string propName)
        //{
        //    PropertyDescriptor prop = TypeDescriptor.GetProperties(_stackPanel)[propName];
        //    if (null == prop)
        //        throw new ArgumentException("Matching StackPanel property not found!", propName);
        //    else
        //        return prop;
        //}

        //private void SetValue(string propertyName, object value)
        //{
        //    GetPropertyByName(propertyName).SetValue(_panel, value);
        //}

        ////要有哪些属性的选项, 都得这样写一遍
        //public bool AutoSize
        //{
        //    get { return _stackPanel.AutoSize; }
        //    set { SetValue("AutoSize", value); }
        //}

        //public bool CenterAlign
        //{
        //    get { return _stackPanel.CenterAlign; }
        //    set { SetValue("CenterAlign", value); }
        //}

        ////这个方法在下面用到了, 可以用来调用控件的方法
        //public void RefreshChildrenPosition()
        //{
        //    _stackPanel.RefreshChildrenPosition();
        //}

        ////这个方法指定了Smart Tag到底有哪些选项
        //public override DesignerActionItemCollection GetSortedActionItems()
        //{
        //    DesignerActionItemCollection items = new DesignerActionItemCollection();

        //    string headerName = "StackPanel Options";

        //    //添加一个选项 Category
        //    items.Add(new DesignerActionHeaderItem(headerName));

        //    //下面的各种选项都属于这个Category, 用headerName来指定
        //    items.Add(new DesignerActionPropertyItem("AutoSize", "Auto Size", headerName));
        //    items.Add(new DesignerActionPropertyItem("CenterAlign", "Center Align", headerName));
        //    items.Add(new DesignerActionPropertyItem("Orientation", "Orientation", headerName));

        //    //上面三个都是控件属性的设置, 这个则可以给Smart Tag添加一个LinkButton来执行某些方法
        //    //this, 说明了"RefreshChildrenPosition"这个方法在这个类中实现
        //    items.Add(new DesignerActionMethodItem(this, "RefreshChildrenPosition", "Refresh children position", headerName));

        //    return items;
        //}
    //}

}
