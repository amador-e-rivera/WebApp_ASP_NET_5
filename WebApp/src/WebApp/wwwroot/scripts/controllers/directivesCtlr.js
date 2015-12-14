(function (app) {
    "use strict";

    app.directive("waitCursor", function () {
        return {
            templateUrl: "/views/waitCursor.html"
        };
    });
})(angular.module("app-trips"));