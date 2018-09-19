(function (app) {
    'use strict';

    app.controller('productAttributeGroupEditController', productAttributeGroupEditController);

    productAttributeGroupEditController.$inject = ['$scope', 'apiService', 'notificationService', '$location', '$stateParams'];

    function productAttributeGroupEditController($scope, apiService, notificationService, $location, $stateParams) {
        $scope.productAttributeGroup = {}


        $scope.updateProductAttributeGroup = updateProductAttributeGroup;

        function updateProductAttributeGroup() {
            apiService.put('/api/productattributegroup/update', $scope.productAttributeGroup, addSuccessed, addFailed);
        }
        function loadDetail() {
            apiService.get('/api/productattributegroup/getbyid/' + $stateParams.id, null,
            function (result) {
                $scope.productAttributeGroup = result.data;
            },
            function (result) {
                notificationService.displayError(result.data);
            });
        }

        function addSuccessed() {
            notificationService.displaySuccess($scope.productAttributeGroup.Name + ' đã được cập nhật thành công.');
            $location.url('product_attribute_groups');
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

})(angular.module('shopme.application_groups'));