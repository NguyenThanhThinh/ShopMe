(function (app) {
    app.controller('rootController', rootController);


    rootController.$inject = ['$state', 'authData', 'loginService', '$scope', 'authenticationService','$window','apiService','$interval'];

    function rootController($state, authData, loginService, $scope, authenticationService, $window, apiService, $interval) {
        $scope.AssignedDate = Date; // 'Date' could be assigned too of course:)
        $scope.logOut = logOut;
        $interval(function () {
            // nothing is required here, interval triggers digest automaticaly
        }, 1000)

        function logOut() {
            loginService.logOut();
            $window.localStorage.clear();
            $window.location.reload();
            $state.go('login');
        }
        $scope.authentication = authData.authenticationData;
        authenticationService.validateRequest();
                  
    }
})(angular.module('shopme'));