(function () {    
    var ele = $("#username");
    ele.text("Gary Khong");

    var main = $("#main");
    main.on("mouseenter", function () {
        main.css("background-color", "#888");
    });

    main.on("mouseleave", function () {
        main.removeAttr("style")
    });

    var menuItems = $("ul.menu li a");
    menuItems.on("click", function () {
        var me = $(this);
        alert(me.text());
    });
})();
