using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Demo.Intranet.MediaCenter;

namespace Demo.Webparts.VisualWebParts.MediaCenterListFilter
{
    [ToolboxItemAttribute(false)]
    public class MediaCenterListFilter : WebPart, IListFilterString
    {
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/Demo.Webparts.VisualWebParts/MediaCenterListFilter/MediaCenterListFilterUserControl.ascx";

        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable(true)]
        [WebDisplayName("List Name")]
        [Category("Custom")]
        public string ListName { get; set; }

        public String ListFilter = String.Empty;

        public MediaCenterListFilter()
        {
            if (this.ListName == null)
            {
                this.ListName = GlobalConstant.LIST_Demo_MEDIAIMAGE;
            }

            if (ListFilter == null)
            {
                ListFilter = string.Empty;
            }
        }

        //protected override void OnPreRender(EventArgs e)
        //{
        //    base.OnPreRender(e);
        //    var smgr = ScriptManager.GetCurrent(this.Page);
        //    smgr.RegisterAsyncPostBackControl(TriggerControl);
        //}

        protected override void CreateChildControls()
        {
            var control = Page.LoadControl(_ascxPath) as MediaCenterListFilterUserControl;
            control.ConnectWebPart(this);
            Controls.Add(control);
        }

        [ConnectionProvider("List Filter", AllowsMultipleConnections = true)]
        public IListFilterString GetListFilterProvider()
        {
            return this;
        }

        public Control TriggerControl
        {
            get
            {
                return (Page.LoadControl(_ascxPath) as MediaCenterListFilterUserControl).FindControl("btApplyFilter");
            }
        }

        public string ListNameString
        {
            get
            {
                return ListName;
            }
        }

        public string ListFilterString
        {
            get
            {
                return ListFilter;
            }
        }
    }
}
