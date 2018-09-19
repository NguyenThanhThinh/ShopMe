(function (app) {
    'use strict';

    app.controller('productUnitAddController', productUnitAddController);

    productUnitAddController.$inject = ['$scope', 'apiService', 'notificationService', '$location', 'commonService'];

    function productUnitAddController($scope, apiService, notificationService, $location, commonService) {
        $scope.productAttribute = {
          
            Status: true
        }

        $scope.addProductAttribute = addProductAttribute;

        function addProductAttribute() {
            apiService.post('/api/productunit/create', $scope.productAttribute, addSuccessed, addFailed);
        }

        function addSuccessed() {
            notificationService.displaySuccess($scope.productAttribute.Name + ' đã được thêm mới.');

            $location.url('product_unit');
        }
        function addFailed(response) {
            notificationService.displayError('Thêm mới không thành công!');
            notificationService.displayErrorValidation(response);
        }

        function loadProductAttributeGroups() {   
            apiService.get('/api/productunit/getallparents', null,
                function (result) {
                    $scope.productAttributeGroups = result.data;
                }, function () {
                    notificationService.displayError('Không tải được danh sách quyền.');
                });  
        }

        $scope.resetForm = function resetForm() {
            $scope.productAttribute = {};
        }
        loadProductAttributeGroups();
    }
})(angular.module('shopme.product_unit'));