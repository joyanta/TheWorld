//site.js

(function () {
    //var ele = $("#username");
    //ele.text( "Joyanta");

    //var main = $("#main");
    //main.on("mouseenter", function () {
    //    main.style = "background: #888;";
    //});

    //main.on("mouseleave", function () {
    //    main.style = "";
    //});


    //var menuItems = $("ul.menu li a");
    //menuItems.on("click", function () {
    //    var me = $(this);

    //    alert(me.text());
    //});

    var $sitebarAndWrapper = $("#sidebar, #wrapper");
    var $icon = $("#sidebarToggle i.fa");

    $("#sidebarToggle").on("click", function () {
        $sitebarAndWrapper.toggleClass("hide-sidebar");
        if ($sitebarAndWrapper.hasClass("hide-sidebar")){
            $icon.removeClass("fa-angle-left");
            $icon.addClass("fa-angle-right");
        } else {
            $icon.addClass("fa-angle-left");
            $icon.removeClass("fa-angle-right");
        }
    });
})();
