<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/DefaultMasterPage.Master" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="JDBaconWebsite.HomePage" %>

<%@ Register Src="~/Controls/ProgressUpdates.ascx" TagName="ProgressUpdate" tagprefix="JDSite" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
<title>JD Bacon The Game Home Page!</title>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="TitleBar" runat="server">
<h1>Home Page</h1>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageLogo" runat="server">
    <img id="HomePageLogo" src="../Content/Rogue 6 Studios Logo.jpg" alt="Rogue 6 Studios Logo" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">

<section class="Default" style="width:760px;height:980px;">
<div style="padding-left:75px">
    <iframe width="640" height="480" src="http://www.kickstarter.com/projects/206354589/jd-bacon-the-game/widget/video.html" frameborder="0"> </iframe>
</div>
    <JDSite:ProgressUpdate ID="JDSideBar" runat="server"/> 
</section>
</asp:Content>