(function () {
    "use strict";

    // Getting the existing modules
    angular.module("app-trips")
    .controller("tripsController", tripsController);

    function tripsController($http) {
        var vm = this;
        vm.trips = [];

        vm.newTrip = {};
        vm.errorMessage = "";
        vm.isBusy = true;
        $http.get("/api/trips")
            .then(function (response) {
                angular.copy(response.data, vm.trips);
            }, function (error) {
                //failure
                vm.errorMessage = "Failed to load data " + error;
                vm.isBusy = false;
            })
        .finally(function () {
            vm.isBusy = false;
        });

        vm.addTrip = function () {
            vm.isBusy = true;
            vm.errorMessage = "";

            $http.post("/api/trips", vm.newTrip)
            .then(function (response) {
                vm.trips.push(response.data);
            }, function () {
                vm.errorMessage = "Failed to save new trip";
            })
            .finally(function() {
                vm.isBusy = false;
            });
        };
    }
})();