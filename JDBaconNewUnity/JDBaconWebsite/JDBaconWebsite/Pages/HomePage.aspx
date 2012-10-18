<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/DefaultMasterPage.Master" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="JDBaconWebsite.HomePage" %>

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
<section class="Default" style="width:380px;float:right;height:465px;">
<div class="marquee" behavior="scroll" direction="up" scrollamount="1" height="300" width="350">
<asp:PlaceHolder ID="ContributorsPlaceholder" runat="server"></asp:PlaceHolder>
</div>
<script>
    $("div.marquee").marquee();
</script>
</section>
</asp:Content>