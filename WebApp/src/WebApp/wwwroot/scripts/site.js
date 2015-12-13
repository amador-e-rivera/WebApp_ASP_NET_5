(function () {
    "use strict";

    var WEBAPP = function () {
        function init() {
            setEventHandlers();
        }

        function setEventHandlers() {
            var $sidebarAndWrapper = $("#sidebar-wrapper,#main-wrapper, #user, .menu");

            $("#sidebar_btn").on("click", function () {
                $sidebarAndWrapper.toggleClass("hide-sidebar");
            });
        }

        return {
            init: init
        };
    };

    var program = WEBAPP();

    program.init();
})();