(function (app) {
    'use strict';

    app.controller('productAttributeGroupAddController', productAttributeGroupAddController);

    productAttributeGroupAddController.$inject = ['$scope', 'apiService', 'notificationService', '$location', 'commonService'];

    function productAttributeGroupAddController($scope, apiService, notificationService, $location, commonService) {
        $scope.productAttributeGroup = {
            ID: 0
        }

        $scope.addProductAttributeGroup = addProductAttributeGroup;
                  
        function addProductAttributeGroup() {
            apiService.post('/api/productattributegroup/create', $scope.productAttributeGroup, addSuccessed, addFailed);
        }

        function addSuccessed() {
            notificationService.displaySuccess($scope.productAttributeGroup.Name + ' đã được thêm mới.');
            $location.url('product_attribute_groups');
        }
        function addFailed(response) {
            notificationService.displayError('Thêm mới không thành công!');
            notificationService.displayErrorValidation(response);
        }
        $scope.resetForm = function resetForm() {
            $scope.productAttributeGroup = {};
        }


    }

})(angular.module('shopme.product_attribute_groups'));