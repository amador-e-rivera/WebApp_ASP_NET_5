(function () {
    "use strict";

    angular.module("app-trips", ["ngRoute"])
    .config(function ($routeProvider) {

        $routeProvider.when("/", {
            controller: "tripsCtrl",
            controllerAs: "model",
            templateUrl: "/views/tripsView.html"
        });

        $routeProvider.when("/editor/:tripName", {
            controller: "tripEditorCtrl",
            controllerAs: "model",
            templateUrl: "/views/tripEditorView.html"
        });

        $routeProvider.otherwise({ redirectTo: "/"});
    });

})();