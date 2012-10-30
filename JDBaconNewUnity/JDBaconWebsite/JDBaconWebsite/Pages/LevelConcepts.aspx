<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/DefaultMasterPage.Master" AutoEventWireup="true" CodeBehind="LevelConcepts.aspx.cs" Inherits="JDBaconWebsite.Pages.LevelConcepts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
<title>Level Concepts</title>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageLogo" runat="server">
    <img id="HomePageLogo" src="../Content/Rogue 6 Studios Logo.jpg" alt="Rogue 6 Studios Logo" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleBar" runat="server">
<h1>Level Concepts</h1>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<section class="Default">
    <table class="LevelConceptMap">
        <tr>
            <td >
                <img src="../Content/Level 1 Room 2.jpg" alt=""/>
            </td>

            <td>
                <img src="../Content/Level 1 Room 3.jpg" alt=""/>
            </td>

            <td>
                <img src="../Content/Level 2 Factory 2.jpg" alt=""/>
            </td>
        </tr>
        <tr>
            <td>
                <img src="../Content/Level 2 Factory.jpg" alt=""/>
            </td>
            <td>
                <img src="../Content/JDs_wingedLogo Fade_LevelScale.jpg" alt=""/>
            </td>
            <td>
                <img src="../Content/Level 3 Kali Flower Float.jpg" alt=""/>
            </td>
        </tr>
        <tr>
            <td>
                <img src="../Content/Level 3 Tofu Float.jpg" alt=""/>
            </td>
            <td>
                <img src="../Content/Level 4 City Shot.jpg" alt=""/>
            </td>

            <td>
                <img src="../Content/Level 5 Shot 1.jpg" alt=""/>
            </td>
        </tr>
    </table>
</section>
<script type="text/javascript">
    $(function () {
        $('table');
    })
</script>
</asp:Content>