(function (app) {
    'use strict';

    app.controller('applicationUserAddController', applicationUserAddController);

    applicationUserAddController.$inject = ['$scope', 'apiService', 'notificationService', '$location', 'commonService'];

    function applicationUserAddController($scope, apiService, notificationService, $location, commonService) {
        $scope.account = {
            Groups: [],
            BirthDay: new Date()
        }


        $scope.addAccount = addAccount;

        function addAccount() {
            apiService.post('/api/applicationUser/add', $scope.account, addSuccessed, addFailed);
        }

        function addSuccessed() {
            notificationService.displaySuccess('Thêm mới người dùng thành công.');

            $location.url('application_users');
        }
        function addFailed(response) {
            notificationService.displayError('Thêm mới không thành công!');

        }

        function loadGroups() {
            apiService.get('/api/applicationGroup/getlistall',
                null,
                function (response) {
                    $scope.groups = response.data;
                }, function (response) {
                    notificationService.displayError('Không tải được danh sách nhóm.');
                });

        }
        $scope.resetForm = function resetForm() {
            $scope.account = {};
        }


        loadGroups();

    }
})(angular.module('shopme.application_users'));