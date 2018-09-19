(function (app) {
    'use strict';

    app.controller('productTemplateEditController', productTemplateEditController);

    productTemplateEditController.$inject = ['$scope', 'apiService', 'notificationService', '$location', '$stateParams'];

    function productTemplateEditController($scope, apiService, notificationService, $location, $stateParams) {
        $scope.productTemplate = {
            ProductAttributes: []
        };
        $scope.ProductAttributes = [];
        $scope.addingAttribute = null;
        $scope.updateProductTemplate = updateProductTemplate;
        $scope.addAttribute = addAttribute;
        $scope.removeAttribute = removeAttribute;

        function addAttribute(attr) {
            var index = $scope.ProductAttributes.indexOf(attr);
            $scope.ProductAttributes.splice(index, 1);
            $scope.productTemplate.ProductAttributes.push(attr);
            $scope.addingAttribute = null;
        };

        function removeAttribute(attr) {
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
           
      

        function updateProductTemplate() {
            apiService.put('/api/producttemplate/update', $scope.productTemplate, addSuccessed, addFailed);
        }

        function loadDetail() {
            apiService.get('/api/producttemplate/getbyid/' + $stateParams.id, null,
            function (result) {
                var i, index, attributeIds;
                $scope.productTemplate = result.data;

                attributeIds = $scope.ProductAttributes.map(function (item) { return item.ID; });
                for (i = 0; i < $scope.productTemplate.ProductAttributes.length; i = i + 1) {
                    index = attributeIds.indexOf($scope.productTemplate.ProductAttributes[i].ID);
                    attributeIds.splice(index, 1);
                    $scope.ProductAttributes.splice(index, 1);
                }
            },
            function (result) {
                notificationService.displayError(result.data);
            });
        }

        function addSuccessed() {
            notificationService.displaySuccess($scope.productTemplate.Name + ' đã được cập nhật thành công.');
            $location.url('product_templates');
        }
        function addFailed(response) {
            notificationService.displayError('Cập nhật không thành công.');
            notificationService.displayErrorValidation(response);
        }


        getProductAttributes();
        loadDetail();

    }

})(angular.module('shopme.product_templates'));