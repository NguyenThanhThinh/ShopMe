(function (app) {
    'use strict';

    app.controller('applicationUserChangePassController', applicationUserChangePassController);

    applicationUserChangePassController.$inject = ['$scope', 'apiService', 'notificationService', '$location', '$stateParams', '$state', 'loginService', '$window'];

    function applicationUserChangePassController($scope, apiService, notificationService, $location, $stateParams, $state, loginService, $window) {
        $scope.account = {}
        $scope.inputType = 'password';

        $scope.changePassword = changePassword;
        $scope.showPass = showPass;

        
        function changePassword() {
            apiService.put('/api/applicationUser/changepass', $scope.account, addSuccessed, addFailed);
        }

        function showPass() {
            if ($scope.inputType == 'password')
                $scope.inputType = 'text';
            else
                $scope.inputType = 'password';
        };

        function loadDetail() {
            apiService.get('/api/applicationUser/detailuser/' + $stateParams.username, null,
            function (result) {
                $scope.account.UserId = result.data;
            },
            function (result) {
                notificationService.displayError('Không thể tải dữ liệu!');
            });
        }

        function addSuccessed() {
            notificationService.displaySuccess('Mật khẩu đã được đổi!');
            setTimeout(function () {
                loginService.logOut();
                $window.localStorage.clear();
                $window.location.reload();
                $state.go('login');
            }, 1000);
            
        }
        function addFailed(response) {
            notificationService.displayError('Đổi mật khẩu không thành công.');
            if (response.data.Message != null) {
                notificationService.displayError(response.data.Message);
            }                                                                       
            notificationService.displayErrorValidation(response);
        }
        loadDetail();
    }
})(angular.module('shopme.application_users'));