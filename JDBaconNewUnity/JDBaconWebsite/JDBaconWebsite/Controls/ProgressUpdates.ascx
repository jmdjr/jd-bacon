<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProgressUpdates.ascx.cs" Inherits="JDBaconWebsite.Controls.ProgressUpdates" %>
<div style="border:1px solid black;border-left:0px; border-right:0px; border-bottom:0px; overflow:hidden;">
Weekly Updates:
    <div class="marquee" behavior="scroll" direction="up" scrollamount="1" height="250" width="800" >
11/09/2012
<br />Our website is now up!
<br />Our programers are still hard at work getting the JD Bacon and
<br />zombie animations implemented for the gameplay video.
<br />Mike and Travis are working on fleshing out each of the levels.
<br />Brandon is working on enemy and Super Bacon animations.
<br />Ephraim is working on level 3 cinematics and Kevin is waiting on 
<br />the revised texture list from Mike and Travis.
    </div>
</div>

<div style="border:1px solid black;border-left:0px; border-right:0px; border-bottom:0px; overflow:hidden;">
Contributors:
    <div class="marquee" behavior="scroll" direction="up" scrollamount="1" height="200" width="800">
    Nothing yet... We NEED YOU!
    </div>
</div>
<script type="text/javascript">
    $("div.marquee").marquee();
</script>