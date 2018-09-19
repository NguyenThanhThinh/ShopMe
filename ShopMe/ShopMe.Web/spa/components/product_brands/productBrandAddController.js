(function (app) {
    app.controller('productBrandAddController', productBrandAddController)

    productBrandAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService'];
    function productBrandAddController(apiService, $scope, notificationService, $state, commonService) {
        $scope.productBrand = {
            CreatedDate: new Date(),
            Status: true
        }
                     
        $scope.AddProductBrand = AddProductBrand;
        $scope.GetSeoTitle = GetSeoTitle;

        function GetSeoTitle() {
            $scope.productBrand.Alias = commonService.getSeoTitle($scope.productBrand.Name);
        }
                
        function AddProductBrand() {
            apiService.post('api/productbrand/create', $scope.productBrand, function (result) {
                notificationService.displaySuccess(result.data.Name + ' đã được thêm mới.');
                $state.go('product_brands');
            }, function (error) {
                notificationService.displayError('Thêm mới không thành công!')

            });
        }

        $scope.ChooseImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.productBrand.Image = fileUrl;
                })
            }
            finder.popup();
        }

        $scope.resetForm = function resetForm() {
            $scope.productBrand = {};
        }                       
    }
    

})(angular.module('shopme.product_brands'));