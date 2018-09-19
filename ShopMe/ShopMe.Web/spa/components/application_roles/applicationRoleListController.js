(function (app) {
    'use strict';

    app.controller('applicationRoleListController', applicationRoleListController);

    applicationRoleListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];

    function applicationRoleListController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.loading = true;
        $scope.data = [];
        $scope.page = 0;
        $scope.pageCount = 0;
        $scope.search = search;
        $scope.numRow = 10;
        $scope.clearSearch = clearSearch;
        $scope.deleteItem = deleteItem;
        $scope.selectAll = selectAll;
        $scope.deleteMultiple = deleteMultiple;

        function deleteMultiple() {
            var options = {
                message: 'Bạn có chắc muốn xóa các loại quyền đã chọn?',
                title: 'Xóa quyền',
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
                            var listId = [];
                            $.each($scope.selected, function (i, item) {
                                listId.push(item.Id);
                            });
                            var config = {
                                params: {
                                    checkedList: JSON.stringify(listId)
                                }
                            }
                            apiService.del('api/applicationRole/deletemulti', config, function (result) {
                                notificationService.displaySuccess('Xóa thành công bản ghi.');
                                search();
                            }, function (error) {
                                notificationService.displayError('Xóa không thành công');
                            });
                        }
                    }
                }
            };       
            $ngBootbox.customDialog(options);                 
        }

        $scope.isAll = false;
        function selectAll() {
            if ($scope.isAll === false) {
                angular.forEach($scope.data, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.data, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }

        $scope.$watch("data", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled');
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true);

        function deleteItem(id) {
            var options = {
                message: 'Bạn có chắc muốn xóa?',
                title: 'Xóa quyền',
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
                            apiService.del('/api/applicationRole/delete', config, function () {
                                notificationService.displaySuccess('Đã xóa thành công.');
                                search();
                            },
                            function () {
                                notificationService.displayError('Xóa không thành công.');
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
                    pageSize:  $scope.numRow,
                    filter: $scope.filterExpression
                }
            }

            apiService.get('api/applicationRole/getlistpaging', config, dataLoadCompleted, dataLoadFailed);
        }

        function dataLoadCompleted(result) {
            $scope.data = result.data.Items;
            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loading = false;
            if ($scope.filterExpression && $scope.filterExpression.length) {
                notificationService.displayInfo(result.data.Items.length + ' được tìm thấy');
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
})(angular.module('shopme.application_roles'));