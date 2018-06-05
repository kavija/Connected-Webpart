using System;
using System.Data;
using System.Web.UI;
using Microsoft.SharePoint;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.WebControls;
using Demo.Intranet.MediaCenter;
using System.Collections.Generic;

namespace Demo.Webparts.VisualWebParts.MediaCenterListView
{
    public partial class MediaCenterListViewUserControl : UserControl
    {
        private static string ConstLoadListItems = "LoadListItems";
        private static string ConstPreviewImage = "previewImage";
        private static string ConstDeleteItem = "deleteItem";
        private static string ConstFileName = "fileName";
        private static string ConstImageThumb = "{0}/Images/{1}";
        private static string ConstDownload = "Download";
        private static string ConstDownloadFile = "Download File";
        private static string ConstPlayVideoHeader = "Watch";
        private static string ConstPlayVideo = "Watch this Video";
        private static string ConstTarget = "_blank";
        private static string ConstID = "ID";
        private static string ConstEdit = "Edit";
        private static string ConstDelete = "Delete";
        private static string ConstName = "Name";
        private static string ConstDescription = "Description";
        private static string ConstDisplayTitle = "Display Title";
        private static string ConstFolderView = "Folder View";
        private static string ConstViewFolder = "View Folder";
        private static string ConstFolderViewUrl = "{0}?RootFolder={1}";
        private static string ConstEditFormUrl = "{0}?ID={1}&Source={2}";
        private MediaCenterListView _webpart = null;
        private string MediaCenterDeliveryUrl;
        private string MediaCenterImageDownloadUrl;
        private string MediaCenterVideoPlayUrl;

        internal void ConnectWebPart(MediaCenterListView webpart)
        {
            _webpart = webpart;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Dictionary<string, string> configurations = ConfigurationManager.GetConfigurations(SPContext.Current.Site.WebApplication, new List<string> {
                GlobalConstant.MediaCenterDeliveryUrl,
                GlobalConstant.MediaCenterImageDownloadUrl,
                GlobalConstant.MediaCenterVideoPlayUrl
            });
            MediaCenterDeliveryUrl = configurations[GlobalConstant.MediaCenterDeliveryUrl];
            MediaCenterImageDownloadUrl = configurations[GlobalConstant.MediaCenterImageDownloadUrl];
            MediaCenterVideoPlayUrl = configurations[GlobalConstant.MediaCenterVideoPlayUrl];

            ItemsGridView.AllowPaging = true;
            ItemsGridView.PagerTemplate = null;

            if (!_webpart.IsPreviewDisabled || !_webpart.IsLinkFileNameDisabled)
            {
                ItemsGridView.RowDataBound += ItemsGridView_RowDataBound;
            }
            ItemsGridView.PageIndexChanging += new GridViewPageEventHandler(ItemsGridView_PageIndexChanging);

            objectDataSource = new ObjectDataSource(this.GetType().AssemblyQualifiedName, ConstLoadListItems);
            objectDataSource.ObjectCreating += new ObjectDataSourceObjectEventHandler(objectDataSource_ObjectCreating);
        }

        protected void ItemsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (!_webpart.IsPreviewDisabled)
                {
                    Image imgThumb = e.Row.FindControl(ConstPreviewImage) as Image;
                    imgThumb.ImageUrl = string.Format(ConstImageThumb, MediaCenterDeliveryUrl, DataBinder.Eval(e.Row.DataItem, GlobalConstant.LinkFilename));
                }
                if (!_webpart.IsLinkFileNameDisabled)
                {
                    Label fileName = e.Row.FindControl(ConstFileName) as Label;
                    fileName.Text = Utilities.GetFileNameWithoutExtension(DataBinder.Eval(e.Row.DataItem, GlobalConstant.LinkFilename).ToString());
                }

                Label deleteItem = e.Row.FindControl(ConstDeleteItem) as Label;
                deleteItem.Text = ConstDelete;
                deleteItem.Attributes.Add("class", "span-on-hover");
                 
                deleteItem.Attributes.Add("onclick", "javascript:deleteDocument('" + DataBinder.Eval(e.Row.DataItem, GlobalConstant.FileRef).ToString() + "','#" + deleteItem.ClientID + "')");
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            var spSite = SPContext.Current.Site;
            var spWeb = SPContext.Current.Web;
            var spList = spWeb.Lists[_webpart.ListName];

            ItemsGridView.Columns.Clear();
            ItemsGridView.PageSize = _webpart.PageSize;

            HyperLinkField downloadFile = new HyperLinkField
            {
                Text = ConstDownloadFile,
                HeaderText = ConstDownload,
                DataNavigateUrlFields = new string[] { GlobalConstant.LinkFilename },
                DataNavigateUrlFormatString = string.Format(MediaCenterImageDownloadUrl, "{0}"),
                Target = ConstTarget
            };

            HyperLinkField playVideo = new HyperLinkField
            {
                Text = ConstPlayVideo,
                HeaderText = ConstPlayVideoHeader,
                DataNavigateUrlFields = new string[] { GlobalConstant.LinkFilename },
                DataNavigateUrlFormatString = string.Format(MediaCenterVideoPlayUrl, "{0}"),
                Target = ConstTarget
            };

            HyperLinkField editItem = new HyperLinkField
            {
                DataTextField = ConstID,
                HeaderText = ConstEdit,
                DataNavigateUrlFields = new string[] { ConstID },
                DataNavigateUrlFormatString = string.Format(ConstEditFormUrl, spList.DefaultEditFormUrl, "{0}", spList.DefaultViewUrl),
                Target = ConstTarget
            };

            TemplateField deleteItemTemplete = new TemplateField();
            deleteItemTemplete.ItemTemplate = new DeleteItemTemplate(DataControlRowType.DataRow);
            deleteItemTemplete.HeaderTemplate = new DeleteItemTemplate(DataControlRowType.Header);

            HyperLinkField folderView = new HyperLinkField
            {
                Text = ConstViewFolder,
                HeaderText = ConstFolderView,
                DataNavigateUrlFields = new string[] { GlobalConstant.FolderPath },
                DataNavigateUrlFormatString = string.Format(ConstFolderViewUrl, spList.DefaultViewUrl, "{0}"),
                Target = ConstTarget
            };

            SPBoundField imageCategory = new SPBoundField { DataField = GlobalConstant.ImageCategory, HeaderText = GlobalConstant.Category };

            ItemsGridView.Columns.Add(editItem);
            if (!_webpart.IsFileNameDisabled)
            {
                SPBoundField imageFileName = new SPBoundField { DataField = GlobalConstant.ImageFileName, HeaderText = ConstName };
                ItemsGridView.Columns.Add(imageFileName);
            }
            if (!_webpart.IsLinkFileNameDisabled)
            {
                TemplateField templateFileName = new TemplateField();
                templateFileName.ItemTemplate = new FileNameTemplate(DataControlRowType.DataRow);
                templateFileName.HeaderTemplate = new FileNameTemplate(DataControlRowType.Header);
                ItemsGridView.Columns.Add(templateFileName);
            }
            if (!_webpart.IsDisplayTitleDisabled)
            {
                SPBoundField imageDisplayTitle = new SPBoundField { DataField = GlobalConstant.Title, HeaderText = ConstDisplayTitle };
                ItemsGridView.Columns.Add(imageDisplayTitle);
            }
            ItemsGridView.Columns.Add(imageCategory);
            if (!_webpart.IsDescriptionDisabled)
            {
                SPBoundField imageFileName = new SPBoundField { DataField = GlobalConstant.MediaDescription, HeaderText = ConstDescription };
                ItemsGridView.Columns.Add(imageFileName);
            }
            if (!_webpart.IsDownloadDisabled)
            {
                ItemsGridView.Columns.Add(downloadFile);
            }
            if (!_webpart.IsPlayDisabled)
            {
                ItemsGridView.Columns.Add(playVideo);
            }
            ItemsGridView.Columns.Add(folderView);

            if (!_webpart.IsPreviewDisabled)
            {
                TemplateField templateEmail = new TemplateField();
                templateEmail.ItemTemplate = new PreviewTemplate(DataControlRowType.DataRow);
                templateEmail.HeaderTemplate = new PreviewTemplate(DataControlRowType.Header);
                ItemsGridView.Columns.Add(templateEmail);
            }
            ItemsGridView.Columns.Add(deleteItemTemplete);

            try
            {
                this.BindItems();
            }
            catch (Exception ex)
            {
                //TODO: Log Exception
            }
        }

        private void BindItems()
        {
            ItemsGridView.DataSource = objectDataSource;
            ItemsGridView.DataBind();
        }

        public DataTable LoadListItems()
        {
            var web = SPContext.Current.Web;
            var spList = web.Lists[_webpart.ListName];
            var spQuery = new SPQuery();
            spQuery.RowLimit = 100;
            spQuery.Query = string.Format("{0}{1}", _webpart.ListFilter, GlobalConstant.DefaultOrder);
            spQuery.ViewFields = @"<FieldRef Name='ID' /><FieldRef Name='LinkFilename' /><FieldRef Name='FileRef' /><FieldRef Name='ImageCategory' /><FieldRef Name='ImageFileName' /><FieldRef Name='MediaDescription' /><FieldRef Name='Title' /><FieldRef Name='FolderPath' />";
            spQuery.ViewAttributes = "Scope=\"Recursive\"";

            var spListItems = spList.GetItems(spQuery).GetDataTable();

            return spListItems;
        }

        void objectDataSource_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            e.ObjectInstance = this;
        }

        void ItemsGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ItemsGridView.PageIndex = e.NewPageIndex;
        }
    }

    public class PreviewTemplate : ITemplate
    {
        private DataControlRowType controlRowType;
        private static string ConstPreviewImgae = "previewImage";
        private static int Height = 175;
        private static int Width = 175;

        public PreviewTemplate(DataControlRowType conrolRowType)
        {
            this.controlRowType = conrolRowType;
        }

        public void InstantiateIn(Control container)
        {
            switch (controlRowType)
            {
                case DataControlRowType.Header:
                    Label previewHeader = new Label();
                    previewHeader.Text = GlobalConstant.Preview;
                    container.Controls.Add(previewHeader);
                    break;
                case DataControlRowType.DataRow:
                    Image previewImage = new Image();
                    previewImage.ID = ConstPreviewImgae;
                    previewImage.Height = Height;
                    previewImage.Width = Width;
                    container.Controls.Add(previewImage);
                    break;
                default:
                    break;
            }
        }
    }

    public class FileNameTemplate : ITemplate
    {
        private DataControlRowType controlRowType;
        private static string ConstFileName = "fileName";

        public FileNameTemplate(DataControlRowType conrolRowType)
        {
            this.controlRowType = conrolRowType;
        }

        public void InstantiateIn(Control container)
        {
            switch (controlRowType)
            {
                case DataControlRowType.Header:
                    Label fileNameHeader = new Label();
                    fileNameHeader.Text = GlobalConstant.Name;
                    container.Controls.Add(fileNameHeader);
                    break;
                case DataControlRowType.DataRow:
                    Label fileName = new Label();
                    fileName.ID = ConstFileName;
                    container.Controls.Add(fileName);
                    break;
                default:
                    break;
            }
        }
    }

    public class DeleteItemTemplate : ITemplate
    {
        private DataControlRowType controlRowType;
        private static string ConstDeleteItem = "deleteItem";

        public DeleteItemTemplate(DataControlRowType conrolRowType)
        {
            this.controlRowType = conrolRowType;
        }

        public void InstantiateIn(Control container)
        {
            switch (controlRowType)
            {
                case DataControlRowType.Header:
                    Label deleteItemHeader = new Label();
                    deleteItemHeader.Text = GlobalConstant.Delete;
                    container.Controls.Add(deleteItemHeader);
                    break;
                case DataControlRowType.DataRow:
                    Label deleteItemLink = new Label();
                    deleteItemLink.ID = ConstDeleteItem;
                    container.Controls.Add(deleteItemLink);
                    break;
                default:
                    break;
            }
        }
    }
}