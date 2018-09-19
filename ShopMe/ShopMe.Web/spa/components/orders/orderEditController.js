(function (app) {
    app.controller('orderEditController', orderEditController)

    orderEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService', '$stateParams', '$window'];
    function orderEditController(apiService, $scope, notificationService, $state, commonService, $stateParams, $window) {
        $scope.order = []

        $scope.loading = true;
        $scope.isActiveShipping = false;
        $scope.isActiveBilling = false;
        $scope.paymentStatus = [];
        $scope.UpdateOrder = UpdateOrder;
        //$scope.changePaymentStatus = changePaymentStatus;
        $scope.editShippingAddress = editShippingAddress;
        $scope.editBillingAddress = editBillingAddress;
        $scope.getPaymentStatus = getPaymentStatus;
        $scope.getOrderStatus = getOrderStatus;
        $scope.loadShopInfo = loadShopInfo;

        //Gọi CKEditor
        $scope.ckeditorOptions = {
            languague: 'vi',
            height: '250px',
        }
        function editShippingAddress() {
            if (!$scope.isActiveShipping) {
                $scope.isActiveShipping = true;
            }
            else {
                $scope.isActiveShipping = false;
            }
        }

        function editBillingAddress() {
            if (!$scope.isActiveBilling) {
                $scope.isActiveBilling = true;
            }
            else {
                $scope.isActiveBilling = false;
            }
        }

        function loadOrderDetail() {
            apiService.get('api/order/getbyid/' + $stateParams.id, null, function (result) {
                $scope.order = result.data;
                $scope.loading = false;
            }, function (error) {
                notificationService.displayError(error.data);
                $scope.loading = false;
            });
        };


        function changePaymentStatus(orderId, statusId) {
            apiService.put('api/order/changepaymentstatus/' + orderId, statusId, function (result) {
                notificationService.displaySuccess(result.data.Name + 'đã được cập nhật.')
            });
        };

        function UpdateOrder() {
            apiService.put('api/order/update', $scope.order, function (result) {
                notificationService.displaySuccess('Cập nhật thành công.');
                $scope.isActiveBilling = false;
                $scope.isActiveShipping = false;
                //$state.go('orders');
            }, function (error) {
                notificationService.displayError('Cập nhật không thành công!');
                notificationService.displayErrorValidation(response);
            });
        };

        function getPaymentStatus() {
            apiService.get('api/order/paymentstatus', null, function (result) {
                $scope.paymentStatus = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        };

        function getOrderStatus() {
            apiService.get('api/order/orderstatus', null, function (result) {
                $scope.status = result.data;
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

        getPaymentStatus();
        getOrderStatus();
        loadOrderDetail();
        loadShopInfo();
    }


})(angular.module('shopme.orders'));