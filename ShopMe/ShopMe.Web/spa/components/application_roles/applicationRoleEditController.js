(function (app) {
    'use strict';

    app.controller('applicationRoleEditController', applicationRoleEditController);

    applicationRoleEditController.$inject = ['$scope', 'apiService', 'notificationService', '$location', '$stateParams'];

    function applicationRoleEditController($scope, apiService, notificationService, $location, $stateParams) {
        $scope.role = {}


        $scope.updateApplicationRole = updateApplicationRole;

        function updateApplicationRole() {
            apiService.put('/api/applicationRole/update', $scope.role, addSuccessed, addFailed);
        }
        function loadDetail() {
            apiService.get('/api/applicationRole/detail/' + $stateParams.id, null,
            function (result) {
                $scope.role = result.data;
            },
            function (result) {
                notificationService.displayError(result.data);
            });
        }

        function addSuccessed() {
            notificationService.displaySuccess($scope.role.Name + ' đã được cập nhật thành công.');

            $location.url('application_roles');
        }
        function addFailed(response) {
            notificationService.displayError('Cập nhật không thành công.');
            notificationService.displayErrorValidation(response);
        }
        $scope.resetForm = function resetForm() {
            loadDetail();
        }
        loadDetail();
    }
})(angular.module('shopme.application_roles'));