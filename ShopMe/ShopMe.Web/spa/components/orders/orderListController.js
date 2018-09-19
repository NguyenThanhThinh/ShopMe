/// <reference path="/Assets/admin/libs/angular/angular.js" />
(function (app) {
    app.controller('orderListController', orderListController);

    orderListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];

    function orderListController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.orders = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.loading = true;
        $scope.numRow = 10;
        $scope.keyword = '';
        $scope.getOrders = getOrders;
        $scope.search = search;
        $scope.orderStatus = [];
        $scope.loadOrderStatus = loadOrderStatus;   
        $scope.getOrderStatus = getOrderStatus;

        $scope.getSearchName = getSearchName;
        $scope.dataSearch = [];
        $scope.SelectedItem = null;

        function search() {
            $scope.keyword = document.getElementById("txtSearch_value").value;
            getOrders();
        }

        function getOrders(page) {
            page = page || 0;
            $scope.loading = true;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: $scope.numRow,
                    filterStatus: $scope.filterStatus
                }
            }
            apiService.get('/api/order/getall', config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning('Không tìm thấy đơn hàng phù hợp!');
                }
                $scope.orders = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                $scope.loading = false;
            }, function () {
                notificationService.displayError('Tải dữ liệu không thành công!');
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

        function getOrderStatus() {
            apiService.get('api/order/orderstatus', null, function (result) {
                $scope.status = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        };

        $scope.afterSelectedItem = function (selected) {
            if (selected) {
                $scope.SelectedItem = selected.originalObject.CustomerEmail;
                $scope.keyword = selected.originalObject.CustomerEmail;
            }
        }

        function getSearchName() {
            var config = {
                params: {
                    keyword: '',
                    page: 1,
                    pageSize: -1
                }
            }
            apiService.get('/api/order/getall', config, function (result) {
                $scope.dataSearch = result.data.Items;
            }, function () {
                console.log('Tải dữ liệu không thành công.');
            });
        }

        $scope.getOrderStatus();
        $scope.getSearchName();
        $scope.getOrders();
        $scope.loadOrderStatus();

    }
})(angular.module('shopme.orders'));