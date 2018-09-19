(function (app) {
    app.controller('orderPrintController', orderPrintController)

    orderPrintController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$stateParams', '$window'];
    function orderPrintController(apiService, $scope, notificationService, $state, $stateParams, $window) {
        $scope.order = []
        $scope.loadOrderDetail = loadOrderDetail;
        $scope.loadShopInfo = loadShopInfo;
        $scope.currentDate = new Date();

        function loadOrderDetail() {
            apiService.get('api/order/getbyid/' + $stateParams.id, null, function (result) {
                $scope.order = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        };

        function loadShopInfo() {
            apiService.get('api/systemconfig/getshopinfo', null, function (result) {
                $scope.shopInfo = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }


        $scope.$on('$viewContentLoaded', function () {
            setTimeout(function () { window.print();}, 900);
            
        });

        loadOrderDetail();
        loadShopInfo();
       
    }


})(angular.module('shopme.orders'));