<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProgressUpdates.ascx.cs" Inherits="JDBaconWebsite.Controls.ProgressUpdates" %>
<div style="border:1px solid black;border-left:0px; border-right:0px; border-bottom:0px">
Weekly Updates:
    <div class="marquee" behavior="scroll" direction="up" scrollamount="1" height="250" width="500" >
        <asp:PlaceHolder ID="UpdatesPlaceholder" runat="server"></asp:PlaceHolder>
    </div>
</div>

<div style="border:1px solid black;border-left:0px; border-right:0px; border-bottom:0px">
Contributors:
    <div class="marquee" behavior="scroll" direction="up" scrollamount="1" height="200" width="500">
        <asp:PlaceHolder ID="ContributorsPlaceholder" runat="server"></asp:PlaceHolder>
    </div>
</div>
<script type="text/javascript">
    $("div.marquee").marquee();
</script>