(function (app) {
    app.controller('productAddController', productAddController)

    productAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService'];
    function productAddController(apiService, $scope, notificationService, $state, commonService) {
        $scope.product = {
            CreatedDate: new Date(),
            Status: true,
            ViewCount: 0,
            ProductRating:0,
           
        }

        $scope.ProductAttributes = [];
        $scope.addingAttribute = null;
        $scope.moreImages = [];
        $scope.AddProduct = AddProduct;
        $scope.GetSeoTitle = GetSeoTitle;
        $scope.flatFolders = [];
        $scope.productCategories = {};
        $scope.productUnit = {};



        //Hàm gọi CKeditor
        $scope.ckeditorOptions = {
            languague: 'vi',
            height: '250px',
        }

        //Chuyển đổi ký tự thành không dấu
        function GetSeoTitle() {
            $scope.product.Alias = commonService.getSeoTitle($scope.product.Name);
            $scope.product.NormalizedName = commonService.getNormalizedName($scope.product.Name);
        }

        function AddProduct() {
          
            apiService.post('api/product/create', $scope.product, function (result) {
                notificationService.displaySuccess(result.data.Name + ' đã được thêm mới.');
                $state.go('products');
            }, function (error) {
                notificationService.displayError('Thêm mới không thành công!');
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

        function getProductAttributes() {
            apiService.get('api/productattribute/getallparents', null, function (result) {
                $scope.ProductAttributes = result.data;
            }, function () {
                console.log('Tải dữ liệu không thành công!');
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

        $scope.ChooseMoreImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.moreImages.push(fileUrl);
                })

            }
            finder.popup();
        }

        $scope.deleteItem = function (index) {
            $scope.moreImages.splice(index, 1);
        }

        $scope.ComparePrice = function ()
        {
            if ($scope.product.OriginalPrice>=  $scope.product.Price)
            {
                $scope.isChange = false;
                return false;
            }

            $scope.isChange = true;
           
        }

        loadProductCategory();
        loadProductBrand();
        loadProductUnit();
       

    }


})(angular.module('shopme.products'));