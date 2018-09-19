(function (app) {
    'use strict';

    app.controller('productUnitEditController', productUnitEditController);

    productUnitEditController.$inject = ['$scope', 'apiService', 'notificationService', '$location', '$stateParams'];

    function productUnitEditController($scope, apiService, notificationService, $location, $stateParams) {
        $scope.productAttribute = {
            Status: true
        }
                                   
        $scope.updateProductAttribute = updateProductAttribute;

        function updateProductAttribute() {
            apiService.put('/api/productunit/update', $scope.productAttribute, addSuccessed, addFailed);
        }
        function loadDetail() {
            apiService.get('/api/productunit/getbyid/' + $stateParams.id, null,
            function (result) {
                $scope.productAttribute = result.data;
            },
            function (result) {
                notificationService.displayError(result.data);
            });
        }

        function addSuccessed() {
            notificationService.displaySuccess($scope.productAttribute.Name + ' đã được cập nhật thành công.');

            $location.url('product_unit');
        }
        function addFailed(response) {
            notificationService.displayError('Cập nhật không thành công.');
            notificationService.displayErrorValidation(response);
        }
        $scope.resetForm = function resetForm() {
            loadDetail();
        }
                          
        function loadProductAttributeGroups() {
            apiService.get('/api/productunit/getallparents', null,
                function (result) {
                    $scope.productAttributeGroups = result.data;
                }, function () {
                    notificationService.displayError('Không tải được danh sách quyền.');
                });
        }
        loadProductAttributeGroups();
        loadDetail();
    }
})(angular.module('shopme.product_unit'));