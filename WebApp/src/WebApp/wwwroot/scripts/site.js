var WEBAPP = (function () {

    function init() {
        setEventHandlers();
    }

    function setEventHandlers() {
        var $sidebarAndWrapper = $("#sidebar-wrapper,#main-wrapper");

        $("#sidebar_btn").on("click", function () {
            $sidebarAndWrapper.toggleClass("hide-sidebar");
        });
    }

    return {
        init : init
    };
})();

WEBAPP.init();