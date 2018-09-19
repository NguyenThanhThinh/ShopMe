(function (app) {
    app.controller('homeController', homeController);

    homeController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox'];

    function homeController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.latestOrder = [];
        $scope.latestProduct = [];
        $scope.latestFeedbacks = [];
        $scope.orderStatus = [];
        $scope.loadHome = loadHome;
        $scope.count = count;
        $scope.countbrand = countbrand;

        $scope.countOrderNew = countOrderNew;
        $scope.loadLatest = loadLatest;
        $scope.loadOrderStatus = loadOrderStatus;

        function loadHome() {
            apiService.get('/api/home/TestMethod', null, function () {
                $scope.loadLatest();
                $scope.loadOrderStatus();
            
          
                
            }, function () {
                notificationService.displayError("Bạn chưa đăng nhập hoặc tài khoản không có quyền truy cập!");
            });
        }

        function loadLatest() {
            var config = {
                params: {
                    top: 5
                }
            }
            apiService.get('api/order/getlatestorder', config, function (result) {
                $scope.latestOrder = result.data;
            }, function () {
                notificationService.displayError('Tải danh sách đơn hàng mới không thành công!');
                });
          
                     
            apiService.get('api/product/getlatestproduct', config, function (result) {
                $scope.latestProduct = result.data;
            }, function () {
                notificationService.displayError('Tải danh sách sản phẩm mới không thành công!')
            });

            apiService.get('api/product/gethethang', config, function (result) {
                $scope.getHetHang = result.data;
            }, function () {
                notificationService.displayError('Tải danh sách đơn hàng mới không thành công!');
            });

        }

        function count() {
            apiService.get('/api/product/count', null, function (result) {
                $scope.count = result.data;
            }, function () {
                notificationService.displayError('Tải dữ liệu không thành công, thử tải lại trang!');
            });
        }
        function countOrderNew() {
            apiService.get('/api/order/countnew', null, function (result) {
                $scope.countOrderNew = result.data;
            }, function () {
                notificationService.displayError('Tải dữ liệu không thành công, thử tải lại trang!');
            });
        }


        function countbrand() {
            apiService.get('/api/productbrand/count', null, function (result) {
                $scope.countbrand = result.data;
            }, function () {
                notificationService.displayError('Tải dữ liệu không thành công, thử tải lại trang!');
            });
        }
        function loadOrderStatus() {
            var listStatus = ["Pending", "Processing", "Unconfirmed", "Confirmed", "Complete", "Cancelled", "Unpaid", "Paid", "HasShipped", "Packed"];
            var config = {
                params: { 
                    listCode: JSON.stringify(listStatus)
                }
            }
            apiService.get('api/systemconfig/getorderstatus', config, function (result) {
                $scope.orderStatus = result.data;
            }, function () {
                notificationService.displayError('Tải đơn đơn hàng thất bại!');
            });
        }

        $scope.loadHome();
        count();
        countbrand();
        countOrderNew();
      
    
    }
})(angular.module('shopme'));