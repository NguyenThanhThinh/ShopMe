(function (app) {
    'use strict';

    app.controller('applicationUserListController', applicationUserListController);

    applicationUserListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox'];

    function applicationUserListController($scope, apiService, notificationService, $ngBootbox) {
        $scope.loading = true;
        $scope.data = [];
        $scope.page = 0;
        $scope.numRow = 10;
        $scope.pageCount = 0;
        $scope.search = search;
        $scope.clearSearch = clearSearch;
        $scope.deleteItem = deleteItem;

        function deleteItem(id) {
            var options = {
                message: 'Bạn có chắc muốn khóa tài khoản này?',
                title: 'Khóa tài khoản người dùng',
                className: 'test-class',
                buttons: {
                    warning: {
                        label: "Từ chối",
                        className: "btn-default",
                        callback: function () {
                        }
                    },
                    success: {
                        label: "Đồng ý",
                        className: "btn-primary",
                        callback: function () {
                            var config = {
                                params: {
                                    id: id
                                }
                            }
                            apiService.del('/api/applicationUser/delete', config, function () {
                                notificationService.displaySuccess('Khóa tài khoản thành công!');
                                search();
                            },
                            function (response) {
                                if (response.data.Message == null) {
                                    notificationService.displayError('Khóa tài khoản không thành công!');
                                }
                                else {
                                    notificationService.displayError(response.data.Message);
                                }                                  
                            })
                        }
                    }
                }
            };

            $ngBootbox.customDialog(options);
        }

        function search(page) {
            page = page || 0;

            $scope.loading = true;
            var config = {
                params: {
                    page: page,
                    pageSize: $scope.numRow,
                    filter: $scope.filterExpression
                }
            }

            apiService.get('api/applicationUser/getlistpaging', config, dataLoadCompleted, dataLoadFailed);
        }

        function dataLoadCompleted(result){
            $scope.data = result.data.Items;
            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loading = false;

            if ($scope.filterExpression && $scope.filterExpression.length) {
                notificationService.displayInfo(result.data.Items.length + ' tài khoản');
            }
        }
        function dataLoadFailed(response) {
            notificationService.displayError(response.data);
        }

        function clearSearch() {
            $scope.filterExpression = '';
            search();
        }

        $scope.search();
    }
})(angular.module('shopme.application_users'));