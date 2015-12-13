(function (app) {
    "use strict";

    app.controller("tripsCtrl", ["$scope", function tripsCtrl($scope) {
        var model = this;

        model.trips = [
            {
                name: "West Coast Trip",
                created: new Date()
            },
            {
                name: "Europe Trip",
                created: new Date()
            }
        ];

    }]);
})(angular.module("app-trips"));