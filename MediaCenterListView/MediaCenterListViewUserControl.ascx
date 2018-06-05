<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MediaCenterListViewUserControl.ascx.cs" Inherits="Demo.Webparts.VisualWebParts.MediaCenterListView.MediaCenterListViewUserControl" %>

<link href="/_layouts/15/1033/STYLES/Demo/assets/dist/assets/css/ListViewWebpart.css" rel="stylesheet" />
<script src="/_layouts/15/1033/STYLES/Demo/assets/dist/assets/js/jquery-3.3.1.min.js"></script>
<script src="/_layouts/15/1033/STYLES/Demo/assets/dist/assets/js/deleteDocument.js"></script>

<asp:UpdatePanel ID="ItemsGridViewPanel" runat="server">
    <ContentTemplate>
        <SharePoint:SPGridView EnableViewState="false"
            ID="ItemsGridView"
            AutoGenerateColumns="false"
            runat="server">
        </SharePoint:SPGridView>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:ObjectDataSource ID="objectDataSource" runat="server"></asp:ObjectDataSource>
