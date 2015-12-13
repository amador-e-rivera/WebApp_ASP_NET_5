(function (app) {
    "use strict";

    app.controller("tripsCtrl", ["$scope", "$http", function tripsCtrl($scope, $http) {
        var model = this;
        model.trips = [];
        model.newTrip = {};
        model.errorMsg = "";
        model.isBusy = true;

        $http.get("/api/trips").then(
            function (response) {
                angular.copy(response.data, model.trips);
                model.errorMsg = "";
            },
            function (error) {
                model.errorMsg = error;
            }).finally(function () { model.isBusy = false; });

        model.addTrip = function () {
            model.trips.push(
                {
                    name: model.newTrip.name,
                    created: new Date()
                });

            model.newTrip = {};
        };

    }]);
})(angular.module("app-trips"));