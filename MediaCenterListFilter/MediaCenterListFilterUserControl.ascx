<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Taxonomy" Namespace="Microsoft.SharePoint.Taxonomy" Assembly="Microsoft.SharePoint.Taxonomy, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MediaCenterListFilterUserControl.ascx.cs" Inherits="Demo.Webparts.VisualWebParts.MediaCenterListFilter.MediaCenterListFilterUserControl" %>

<style>
    td {
        padding: 5px;
    }
</style>

<table>
    <tr>
        <td>Enterprise Keywords
        </td>
        <td>
            <Taxonomy:TaxonomyWebTaggingControl runat="server" ID="MediaTaxonomyWebTaggingControl"
                Visible="true"
                IsUseCommaAsDelimiter="true"
                IsSpanTermSets="true"
                IsSpanTermStores="false"
                IsDisplayPickerButton="true"
                IsMulti="true"
                AllowFillIn="true"
                IsAddTerms="false"
                IsIncludePathData="false">
            </Taxonomy:TaxonomyWebTaggingControl>
        </td>
    </tr>
    <tr>
        <td>Category
        </td>
        <td>
            <asp:DropDownList ID="ImageCategory" runat="server" AutoPostBack="false" Width="364px">
                <asp:ListItem Enabled="true" Text="Select Category" Value="-1"></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>File Name
        </td>
        <td>
            <asp:TextBox ID="FileLeafRef" autocomplete="off" MaxLength="100" runat="server" Width="355px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:UpdatePanel ID="btApplyFilterPanel" runat="server">
                <ContentTemplate>
                    <asp:Button ID="btApplyFilter" OnClick="btApplyFilter_Click" runat="server" Text="Apply Filter" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btApplyFilter" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </td>
        <td>
            <asp:Button ID="btClearFilter" OnClick="btClearFilter_Click" runat="server" Text="Clear Filter" />
        </td>
    </tr>
</table>
