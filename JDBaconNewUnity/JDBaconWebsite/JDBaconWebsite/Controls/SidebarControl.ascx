<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SidebarControl.ascx.cs" Inherits="JDBaconWebsite.Pages.SidebarControl" %>

<script type="text/javascript">
    ddaccordion.init({                  //top level headers initialization
        headerclass: "expandable",      //Shared CSS class name of headers group that are expandable
        contentclass: "categoryitems",  //Shared CSS class name of contents group
        revealtype: "click",            //Reveal content when user clicks or onmouseover the header? Valid value: "click", "clickgo", or "mouseover"
        mouseoverdelay: 200,            //if revealtype="mouseover", set delay in milliseconds before header expands onMouseover
        collapseprev: true,             //Collapse previous content (so only one open at any time)? true/false 
        defaultexpanded: [0],           //index of content(s) open by default [index1, index2, etc]. [] denotes no content
        onemustopen: false,             //Specify whether at least one header should be open always (so never all headers closed)
        animatedefault: false,          //Should contents open by default be animated into view?
        persiststate: true,             //persist state of opened contents within browser session?
        toggleclass: ["", "openheader"], //Two CSS classes to be applied to the header when it's collapsed and expanded, respectively ["class1", "class2"]
        togglehtml: ["prefix", "", ""], //Additional HTML added to the header when it's collapsed and expanded, respectively  ["position", "html1", "html2"] (see docs)
        animatespeed: "fast",           //speed of animation: integer in milliseconds (ie: 200), or keywords "fast", "normal", or "slow"
        oninit: function (headers, expandedindices) {
            //custom code to run when headers have initalized
            //do nothing
        },
        onopenclose: function (header, index, state, isuseractivated) {
            //custom code to run whenever a header is opened or closed
            //do nothing
        }
    })

</script>

<div class="arrowlistmenu">
    <h3 class="menuheader"><a href="../HomePage.aspx">Home</a></h3>
    <h3 class="menuheader"><a target="_blank" href="http://www.kickstarter.com/projects/206354589/jd-bacon-the-game?ref=live">Kick Starter!</a></h3>
    <h3 class="menuheader expandable">About Rouge 6</h3>
    <ul class="categoryitems">
        <li><a href="../Pages/AboutUs.aspx">Who We Are</a></li>
        <li><a href="../Pages/OurGoals.aspx">Our Goals</a></li>
    </ul>
    <h3 class="menuheader expandable">Story of JD</h3>
    <ul class="categoryitems">
        <li><a href="../Pages/BackStory.aspx">Back Story</a></li>
        <li><a href="../Pages/Characters.aspx">Characters</a></li>
    </ul>
    <h3 class="menuheader expandable">Concepts</h3>
    <ul class="categoryitems">
        <li><a href="../Pages/LevelConcepts.aspx" >Level Concepts</a></li>
        <li><a href="../Pages/CharacterConcepts.aspx" >Character Concepts</a></li>
        <%--<li><a href="../Pages/CinematicConcepts.aspx" >Cinematic Concepts</a></li>--%>
    </ul>
    <h3 class="menuheader expandable">Game Status</h3>
    <ul class="categoryitems">
        <li><a href="../Pages/ProgressUpdates.aspx" >Progress Update</a></li>
        <li><a href="../Pages/StretchGoals.aspx" >Stretch Goals</a></li>
    </ul>
    <h3 class="menuheader expandable">Contact Us</h3>
    <ul class="categoryitems">
        <li><a href="../Pages/ContactUs.aspx" >Contact Us</a></li>
    </ul>
</div>
<script type="text/javascript">
    $(function () {
        $(".arrowlistmenu .categoryitems").each(function (index, element) {

            $("li a", this).each(function (aIndex, aElement) {
                var currentLink = $(this).attr("href");
                if (currentLink.indexOf("?expandable") < 0) {
                    $(this).attr("href", currentLink + "?expandable=" + index);
                }
            });
        });
    });
</script>