(function (app) {
    'use strict';

    app.controller('productTemplateAddController', productTemplateAddController);

    productTemplateAddController.$inject = ['$scope', 'apiService', 'notificationService', '$location', 'commonService'];

    function productTemplateAddController($scope, apiService, notificationService, $location, commonService) {
        $scope.productTemplate = {
            ProductAttributes: []
        };
        $scope.ProductAttributes = [];
        $scope.addingAttribute = null;

        $scope.addAttribute = function addAttribute(attr) {
            var index = $scope.ProductAttributes.indexOf(attr);
            $scope.ProductAttributes.splice(index, 1);
            $scope.productTemplate.ProductAttributes.push(attr);
            $scope.addingAttribute = null;
        };

        $scope.removeAttribute = function removeAttribute(attr) {
            var index = $scope.productTemplate.ProductAttributes.indexOf(attr);
            $scope.productTemplate.ProductAttributes.splice(index, 1);
            $scope.ProductAttributes.push(attr);
        };

        function getProductAttributes() {
            apiService.get('api/productattribute/getallparents', null, function (result) {
                $scope.ProductAttributes = result.data;
            }, function () {
                console.log('Tải dữ liệu không thành công!');
            });
        }
                 
        $scope.addProductTemplate = addProductTemplate;
                  
        function addProductTemplate() {
            apiService.post('/api/producttemplate/create', $scope.productTemplate, addSuccessed, addFailed);
        }

        function addSuccessed() {
            notificationService.displaySuccess($scope.productTemplate.Name + ' đã được thêm mới.');
            $location.url('product_templates');
        }
        function addFailed(response) {
            notificationService.displayError('Thêm mới không thành công!');
            notificationService.displayErrorValidation(response);
        }
      
        getProductAttributes();
    }

})(angular.module('shopme.product_templates'));