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
            }).finally(function () {
                model.isBusy = false;
            });

        model.addTrip = function () {
            model.isBusy = true;
            model.errorMsg = "";

            $http.post("/api/trips", model.newTrip).then(
                function (response) {
                    model.trips.push(response.data);
                    model.newTrip = {};
                },
                function () {
                    model.errorMsg = "Failed to add trip.";
                }
                ).finally(function () {
                    model.isBusy = false;
                });
        };

    }]);
})(angular.module("app-trips"));