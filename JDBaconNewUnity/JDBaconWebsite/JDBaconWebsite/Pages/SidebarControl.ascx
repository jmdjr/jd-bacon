﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SidebarControl.ascx.cs" Inherits="JDBaconWebsite.Pages.SidebarControl" %>
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
    <h3 class="menuheader expandable">Home</h3>
    <ul class="categoryitems">
        <li><a href="HomePage.aspx?expandable=0">Home Page</a></li>
    </ul>
    <h3 class="menuheader expandable">About Rouge 6</h3>
    <ul class="categoryitems">
        <li><a href="AboutUs.aspx?expandable=1">Who We Are</a></li>
        <li><a href="OurGoals.aspx?expandable=1">Our Goals</a></li>
    </ul>
    <h3 class="menuheader expandable">Story of JD</h3>
    <ul class="categoryitems">
        <li><a href="BackStory.aspx?expandable=2">Back Story</a></li>
        <li><a href="Characters.aspx?expandable=2">Characters</a></li>
    </ul>
    <h3 class="menuheader expandable">Concepts</h3>
    <ul class="categoryitems">
        <li><a href="LevelConcepts.aspx?expandable=3" >Level Concepts</a></li>
        <li><a href="CharacterConcepts.aspx?expandable=3" >Character Concepts</a></li>
        <li><a href="CinematicConcepts.aspx?expandable=3" >Cinematic Concepts</a></li>
    </ul>
    <h3 class="menuheader expandable">Game Status</h3>
    <ul class="categoryitems">
        <li><a href="ProgressUpdates.aspx?expandable=4" >Progress Update</a></li>
        <li><a href="StretchGoals.aspx?expandable=4" >Stretch Goals</a></li>
    </ul>
    <h3 class="menuheader expandable">Contact Us</h3>
    <ul class="categoryitems">
        <li><a href="ContactUs.aspx?expandable=5" >Contact Us</a></li>
    </ul>
</div>