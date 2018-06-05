using Microsoft.SharePoint;
using Microsoft.SharePoint.Taxonomy;
using Demo.Intranet.MediaCenter;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace Demo.Webparts.VisualWebParts.MediaCenterListFilter
{
    public partial class MediaCenterListFilterUserControl : UserControl
    {
        private MediaCenterListFilter _webpart = null;
        private static string ConstListFilter = "<Eq><FieldRef Name='" + GlobalConstant.IsProcessed + "'/><Value Type='Boolean'>1</Value></Eq>";

        internal void ConnectWebPart(MediaCenterListFilter webpart)
        {
            _webpart = webpart;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            TaxonomySession taxonomySession = new TaxonomySession(SPContext.Current.Site);
            MediaTaxonomyWebTaggingControl.SSPList = taxonomySession.TermStores[0].Id.ToString();

            if (Page.IsPostBack) return;

            var spWeb = SPContext.Current.Web;
            SPFieldChoice imageCategory = (SPFieldChoice)spWeb.Lists[_webpart.ListName].Fields[GlobalConstant.FieldImageCategory];
            for (int i = 0; i < imageCategory.Choices.Count; i++)
            {
                ImageCategory.Items.Add(imageCategory.Choices[i].ToString());
            }
        }

        protected void btApplyFilter_Click(object sender, EventArgs e)
        {
            _webpart.ListFilter = GetListFilter();
        }

        protected void btClearFilter_Click(object sender, EventArgs e)
        {
            FileLeafRef.Text = string.Empty;
            MediaTaxonomyWebTaggingControl.Text = string.Empty;
            ImageCategory.SelectedIndex = 0;
            _webpart.ListFilter = string.Format("<Where>{0}</Where>", ConstListFilter);
        }

        public string GetListFilter()
        {
            Dictionary<string, string> filters = new Dictionary<string, string>();
            string filterQuery = ConstListFilter;
            string validationMessage;
            string tagFilterQuery = string.Empty;
            bool tagOrContainer = false;

            if (!string.IsNullOrEmpty(FileLeafRef.Text))
            {
                filters.Add(FileLeafRef.ID, string.Format("<Contains><FieldRef Name='FileLeafRef'/><Value Type='File'>{0}</Value></Contains>", FileLeafRef.Text));
            }

            if (!string.IsNullOrEmpty(ImageCategory.SelectedValue) && !ImageCategory.SelectedValue.Equals("-1"))
            {
                filters.Add(ImageCategory.ID, string.Format("<Eq><FieldRef Name='ImageCategory' /><Value Type='Choice'>{0}</Value></Eq>", ImageCategory.SelectedValue));
            }

            var valid = MediaTaxonomyWebTaggingControl.Validate(out validationMessage);
            if (valid)
            {
                var values = new TaxonomyFieldValueCollection(string.Empty);
                values.PopulateFromLabelGuidPairs(MediaTaxonomyWebTaggingControl.Text);

                foreach (TaxonomyFieldValue value in values)
                {
                    if (tagOrContainer)
                    {
                        tagFilterQuery = string.Format("<Or>{0}<Contains><FieldRef Name='TaxKeyword' /><Value Type='Text'>{1}</Value></Contains></Or>", tagFilterQuery, value.Label);
                    }
                    else
                    {
                        tagFilterQuery = string.Format("<Contains><FieldRef Name='TaxKeyword' /><Value Type='Text'>{0}</Value></Contains>", value.Label);
                    }

                    tagOrContainer = true;
                }

            }

            if (!string.IsNullOrEmpty(tagFilterQuery))
            {
                filters.Add(MediaTaxonomyWebTaggingControl.ID, tagFilterQuery);
            }


            foreach (string key in filters.Keys)
            {
                filterQuery = string.Format("<And>{0}{1}</And>", filterQuery, filters[key]);
            }

            var listFilter = string.Format("<Where>{0}</Where>", filterQuery);

            return listFilter;
        }
    }
}
