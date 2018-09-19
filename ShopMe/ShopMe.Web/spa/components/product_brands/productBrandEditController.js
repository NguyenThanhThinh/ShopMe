(function (app) {
    app.controller('productBrandEditController', productBrandEditController)

    productBrandEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$stateParams', 'commonService'];
    function productBrandEditController(apiService, $scope, notificationService, $state, $stateParams, commonService) {
        $scope.productBrand = {
            CreatedDate: new Date(),
            Status: true
        }

        $scope.UpdateProductBrand = UpdateProductBrand;
        $scope.GetSeoTitle = GetSeoTitle;

        function GetSeoTitle() {
            $scope.productBrand.Alias = commonService.getSeoTitle($scope.productBrand.Name);
        }

        function loadProductBrandDetail() {
            apiService.get('api/productbrand/getbyid/' + $stateParams.id, null, function (result) {
                $scope.productBrand = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }

        function UpdateProductBrand() {
            apiService.put('api/productbrand/update', $scope.productBrand,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được cập nhật.');
                    $state.go('product_brands');
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công.');
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
            loadProductBrandDetail();
        }
                                        
        loadProductBrandDetail();
    }


})(angular.module('shopme.product_brands'));