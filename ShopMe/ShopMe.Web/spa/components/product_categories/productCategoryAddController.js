(function (app) {
    app.controller('productCategoryAddController', productCategoryAddController)

    productCategoryAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService'];
    function productCategoryAddController(apiService, $scope, notificationService, $state, commonService) {
        $scope.productCategory = {
            CreatedDate: new Date(),
            Status: true

        }
                     
        $scope.AddProductCategory = AddProductCategory;
        $scope.GetSeoTitle = GetSeoTitle;
        $scope.flatFolders = [];
        $scope.parentCategories = {};

        function GetSeoTitle() {
            $scope.productCategory.Alias = commonService.getSeoTitle($scope.productCategory.Name);
        }
                
        function AddProductCategory() {
            apiService.post('api/productcategory/create', $scope.productCategory, function (result) {
                notificationService.displaySuccess(result.data.Name + ' đã được thêm mới.');
                $state.go('product_categories');
            }, function (error) {
                notificationService.displayError('Thêm mới không thành công!')

            });
        }

        function loadParentCategory() {
            apiService.get('api/productcategory/getallparents', null, function (result) {
                $scope.parentCategories = commonService.getTree(result.data, 'CategoryID', 'ParentID');
                $scope.parentCategories.forEach(function (item) {
                    commonService.recur(item, 0, $scope.flatFolders);
                });
               
            }, function () {
                console.log('Tải danh mục sản phẩm thất bại!');
            });
        }

       
        $scope.resetForm = function resetForm() {
            $scope.productCategory = {};
        }

        loadParentCategory();
    }
    

})(angular.module('shopme.product_categories'));