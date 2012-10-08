<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/DefaultMasterPage.Master" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="JDBaconWebsite.HomePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
<title>Home Page</title>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="TitleBar" runat="server">
<h1>JD Bacon The Game</h1>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageLogo" runat="server">
    <img id="HomePageLogo" src="../Content/Rogue 6 Studios Logo.jpg" alt="Rogue 6 Studios Logo" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<%--

<div>
Regular contents here. Header does not expand or contact.
</div>

<p>Assuming the current page is named "current.htm", the below links when navigated to expands a particular header on that page:</p>
<p>
- <a href="">Expand 1st header within "expandable" header group</a><br />
- <a href="current.htm?expandable=1&subexpandable=0">Expand 2nd header within "expandable" header group and 1st header within nested "subexpandable" group</a><br />
</p>--%>
</asp:Content>
