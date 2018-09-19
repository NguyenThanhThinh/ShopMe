(function (app) {
    app.controller('productEditController', productEditController)

    productEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService', '$stateParams'];
    function productEditController(apiService, $scope, notificationService, $state, commonService, $stateParams) {
        $scope.product = {
           
         
           
        }

        $scope.ProductAttributes = [];
        $scope.addingAttribute = null;
        $scope.moreImages = [];
        $scope.UpdateProduct = UpdateProduct;
        $scope.GetSeoTitle = GetSeoTitle;  
        $scope.flatFolders = [];
        $scope.productCategories = {};

        //Gọi CKEditor
        $scope.ckeditorOptions = {
            languague: 'vi',
            height: '250px',
        }

        //Chuyển đổi ký tự thành không dấu
        function GetSeoTitle() {
            $scope.product.Alias = commonService.getSeoTitle($scope.product.Name);
            $scope.product.NormalizedName = commonService.getNormalizedName($scope.product.Name);
        }
       
        function loadProductDetail() {
            apiService.get('api/product/getproductbyid/' + $stateParams.id, null, function (result) {
                $scope.product = result.data;
           
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }

        function UpdateProduct() {
        
            apiService.put('api/product/update', $scope.product, function (result) {
                notificationService.displaySuccess(result.data.Name + ' đã được cập nhật.');
                $state.go('products');
            }, function (error) {
                notificationService.displayError('Cập nhật không thành công!');
                notificationService.displayErrorValidation(response);
            });
        }

        function loadProductCategory() {
            apiService.get('api/productcategory/getallparents', null, function (result) {
                $scope.productCategories = commonService.getTree(result.data, 'CategoryID', 'ParentID');
                $scope.productCategories.forEach(function (item) {
                    commonService.recur(item, 0, $scope.flatFolders);
                });
            }, function () {
                console.log('Tải danh mục không thành công!');
            });
        }

        function loadProductBrand() {
            apiService.get('api/productbrand/getallparents', null, function (result) {
                $scope.productBrands = result.data;
            }, function () {
                console.log('Tải danh sách nhãn hiệu thất bại!');
            });
        }

        function loadProductUnit() {
            apiService.get('api/productunit/getallparents', null, function (result) {
                $scope.productUnit = result.data;
            }, function () {
                console.log('Tải danh sách nhãn hiệu thất bại!');
            });
        }

        

       

       
        
        $scope.ChooseImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.product.Image = fileUrl;
                })
            }
            finder.popup();
        }

      

        $scope.deleteItem = function (index) {
            $scope.moreImages.splice(index, 1);
        }
        $scope.ComparePrice = function () {
            if ($scope.product.OriginalPrice >= $scope.product.Price) {
                $scope.isChange = false;
                return false;
            }

            $scope.isChange = true;

        }
        loadProductCategory();
        loadProductBrand();
        loadProductUnit();
      
        loadProductDetail();
    }


})(angular.module('shopme.products'));