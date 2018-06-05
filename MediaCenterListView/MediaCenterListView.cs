using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint.WebControls;
using Demo.Intranet.MediaCenter;

namespace Demo.Webparts.VisualWebParts.MediaCenterListView
{
    [ToolboxItemAttribute(false)]
    public class MediaCenterListView : WebPart
    {
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/Demo.Webparts.VisualWebParts/MediaCenterListView/MediaCenterListViewUserControl.ascx";
        private static string ConstFilterStringForPaging = "FilterStringForPaging";
        private static string ConstListFilter = "<Where><Eq><FieldRef Name='" + GlobalConstant.IsProcessed + "'/><Value Type='Boolean'>1</Value></Eq></Where>";
        public IListFilterString _provider;

        private string FilterStringForPaging
        {
            get
            {
                if (ViewState[ConstFilterStringForPaging] == null)
                    return string.Empty;
                else
                    return (string)ViewState[ConstFilterStringForPaging];
            }
            set { ViewState[ConstFilterStringForPaging] = value; }
        }

        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable(true)]
        [WebDisplayName("Page Size")]
        [Category("Custom")]
        public int PageSize { get; set; }

        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable(true)]
        [WebDisplayName("List Name")]
        [Category("Custom")]
        public string ListName { get; set; }

        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable(true)]
        [WebDisplayName("Hide Display Title")]
        [Category("Custom")]
        public bool IsDisplayTitleDisabled { get; set; }

        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable(true)]
        [WebDisplayName("Hide FileName")]
        [Category("Custom")]
        public bool IsFileNameDisabled { get; set; }

        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable(true)]
        [WebDisplayName("Hide Link FileName")]
        [Category("Custom")]
        public bool IsLinkFileNameDisabled { get; set; }

        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable(true)]
        [WebDisplayName("Hide Description")]
        [Category("Custom")]
        public bool IsDescriptionDisabled { get; set; }

        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable(true)]
        [WebDisplayName("Hide Preview")]
        [Category("Custom")]
        public bool IsPreviewDisabled { get; set; }

        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable(true)]
        [WebDisplayName("Hide Download")]
        [Category("Custom")]
        public bool IsDownloadDisabled { get; set; }

        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable(true)]
        [WebDisplayName("Hide Watch Video")]
        [Category("Custom")]
        public bool IsPlayDisabled { get; set; }

        public string ListFilter { get; set; }

        public MediaCenterListView()
        {
            if (this.ListName == null)
            {
                this.ListName = GlobalConstant.LIST_Demo_MEDIAIMAGE;
            }

            if(ListFilter ==null)
            {
                ListFilter = ConstListFilter;
            }

            if (PageSize == 0)
            {
                PageSize = GlobalConstant.ListViewPaging;
            }
        }

        protected override void CreateChildControls()
        {
            var control = Page.LoadControl(_ascxPath) as MediaCenterListViewUserControl;
            control.ConnectWebPart(this);
            Controls.Add(control);
        }

        [ConnectionConsumer("List Filter")]
        public void RegisterListFilterProvider(IListFilterString provider)
        {
            _provider = provider;

            if (_provider != null)
            {
                var providerListFilterString = _provider.ListFilterString;
                var providerListNameString = _provider.ListNameString;

                if (!string.IsNullOrEmpty(providerListFilterString))
                {
                    var control = Page.LoadControl(_ascxPath) as MediaCenterListViewUserControl;
                    var itemsGridView = control.FindControl("ItemsGridView") as SPGridView;
                    itemsGridView.PageIndex = 0;

                    this.ListFilter = providerListFilterString;
                    FilterStringForPaging = providerListFilterString;
                }
                else if(!string.IsNullOrEmpty(FilterStringForPaging))
                {
                    this.ListFilter = FilterStringForPaging;
                }

                if (!string.IsNullOrEmpty(providerListNameString))
                {
                    this.ListName = providerListNameString;
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnLoad(e);

            if (_provider != null && _provider.TriggerControl != null)
            {
                var smgr = ScriptManager.GetCurrent(this.Page);
                smgr.RegisterAsyncPostBackControl(_provider.TriggerControl);
            }
        }
    }
}
