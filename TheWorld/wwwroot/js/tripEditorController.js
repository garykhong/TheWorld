(function () {
    "use strict";

    angular.module("app-trips")
        .controller("tripEditorController", tripEditorController);

    function tripEditorController($routeParams, $http) {
        var vm = this;

        vm.tripName = $routeParams.tripName;
        vm.stops = [];
        vm.errorMessage = "";
        vm.isBusy = true;
        vm.newStop = {};

        var url = "/api/trips/" + vm.tripName + "/stops";

        $http.get(url)
            .then(function (response) {
                angular.copy(response.data, vm.stops);
                showMap(vm.stops);
            }, function (error) {
                vm.errorMessage = "Failed to load stops" + error;
            })
            .finally(function () {
                vm.isBusy = false;
            });

        vm.addStop = function () {
            vm.isBusy = true;

            var newStopWithFormattedArrival = {
                name: vm.newStop.name,
                arrival: vm.newStop.arrival.split("/").reverse().join("-")
            }            

            $http.post(url, newStopWithFormattedArrival)
                .then(function (response) {
                    vm.stops.push(response.data);
                    showMap(vm.stops);
                    vm.newStop = {};
                }, function (error) {
                    vm.errorMessage = "Failed to add new stop ";
                })
                .finally(function () {
                    vm.isBusy = false;
                });
        }
    }

    function showMap(stops) {
        if (stops && stops.length > 0) {

            var mapStops = _.map(stops, function (item) {
                return {
                    lat: item.latitude,
                    long: item.longitude,
                    info: item.name
                };
            });
            travelMap.createMap({
                stops: mapStops,
                selector: "#map",
                currentStop: 1,
                initialZoom: 3
            });
        }
    }
})();