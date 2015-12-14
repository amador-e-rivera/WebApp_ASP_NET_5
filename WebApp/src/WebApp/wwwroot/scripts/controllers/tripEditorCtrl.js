(function (app) {
    "use strict";

    app.controller("tripEditorCtrl", ["$http", "$routeParams", function tripEditorCtrl($http, $routeParams) {
        var model = this;

        model.tripName = $routeParams.tripName;
        model.stop = {};
        model.stops = [];
        model.errorMsg = "";
        model.isBusy = true;

        //Get Stops
        $http.get("/api/trips/" + model.tripName + "/stops")
            .success( function (response) {
                angular.copy(response, model.stops);
                model.errorMsg = "";
                initialize();
            })
            .error(function (error) {
                model.errorMsg = error;
            })
            .finally(function () {
                model.isBusy = false;
            });

        //Post Stop
        model.addStop = function () {
            model.isBusy = true;
            
            $http.post("/api/trips/" + model.tripName + "/stops", model.stop)
            .success(function () {
                model.errorMsg = "";
                model.stops.push(model.stop);
                model.stop = {};
            })
            .error(function (error) {
                model.errorMsg = error;
            })
            .finally(function () {
                model.isBusy = false;
            });

        };

        //Google Maps
        function initialize () {

            var stops = getMapCenter();

            var mapProp = {
                center: stops.center,
                zoom: 4,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };

            var map = new google.maps.Map(document.getElementById("map"), mapProp);
            var path = new google.maps.Polyline({
                path: stops.path,
                strokeColor: "#0000FF",
                strokeOpacity: 0.8,
                strokeWeight: 2
            });

            path.setMap(map);
        }

        function getMapCenter() {
            if (model.stops.length != 0) {
                var lat = 0;
                var lng = 0;
                var path = [];

                model.stops.forEach(function (stop) {
                    lat += stop.latitude;
                    lng += stop.longitude;
                    path.push(new google.maps.LatLng(stop.latitude, stop.longitude));
                });

                lat /= model.stops.length;
                lng /= model.stops.length;

                return {
                    center: new google.maps.LatLng(lat, lng),
                    path: path
                };
            }

            return {
                center: new google.maps.LatLng(39.50, -98.35),
                path: []
            };
        }
    }]);

})(angular.module("app-trips"));