<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/DefaultMasterPage.Master" AutoEventWireup="true" CodeBehind="ProgressUpdates.aspx.cs" Inherits="JDBaconWebsite.Pages.ProgressUpdates" %>

<%@ Register Src="~/Controls/ProgressUpdates.ascx" TagName="ProgressUpdate" tagprefix="JDSite" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
<title>Progress Updates</title>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="TitleBar" runat="server">
<h1>Progress Updates</h1>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageLogo" runat="server">
    <img id="HomePageLogo" src="../Content/Rogue 6 Studios Logo.jpg" alt="Rogue 6 Studios Logo" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<section style="width:370px; float:left; margin-top:15px;">
<img alt="JDBacon Poster" src="../Content/JDBaconPoster.jpg" style="width:370px" />
</section>
<section class="Default" style="width:380px;float:right;height:500px;">
    <JDSite:ProgressUpdate ID="JDSideBar" runat="server"/>
</section>
</asp:Content>