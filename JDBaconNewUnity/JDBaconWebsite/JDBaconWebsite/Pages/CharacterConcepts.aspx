<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/DefaultMasterPage.Master" AutoEventWireup="true" CodeBehind="CharacterConcepts.aspx.cs" Inherits="JDBaconWebsite.Pages.CharacterConcepts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
<title>Character Concepts</title>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageLogo" runat="server">
    <img id="HomePageLogo" src="../Content/Rogue 6 Studios Logo.jpg" alt="Rogue 6 Studios Logo" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="TitleBar" runat="server">
<h1>Character Concepts</h1>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<section class="Default">
    <table class="CharacterConceptMap">
        <tr>
            <td>
               <img src="../Content/JD Bacon BodyParts.bmp" alt=""/>
            </td>

            <td>
                <img src="../Content/Zombie 2 Bodyparts.bmp" alt=""/>
            </td>

            <td>
                <img src="../Content/Super Bacon Bodyparts.bmp" alt="" />
            </td>
            <td rowspan="3">
                <img src="../Content/Kali Flower Concept 2.jpg" alt="" />
            </td>
        </tr>
        <tr>
            <td>
                <img src="../Content/Zombie 1Bodyparts.bmp" alt="" />
            </td>
            <td>
                <img src="../Content/Clam Boss and Turret Concept.jpeg" alt="" />
            </td>
            <td>
                <img src="../Content/Special Zombie Concept.jpeg"  alt=""/>
            </td>
        </tr>
        <tr>
            <td>
                <img src="../Content/Veggi Monster Concept.jpeg" alt="" />
            </td>
            <td>
                <img src="../Content/Tofu Concept.jpeg" alt="" />
            </td>

            <td>
                <img src="../Content/Kali Flower Bodyparts.bmp" alt="" />
            </td>
        </tr>
    </table>
    </section>
</asp:Content>
