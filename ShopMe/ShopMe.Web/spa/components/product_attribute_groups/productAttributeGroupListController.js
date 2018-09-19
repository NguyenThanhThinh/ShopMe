(function (app) {
    'use strict';

    app.controller('productAttributeGroupListController', productAttributeGroupListController);

    productAttributeGroupListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];

    function productAttributeGroupListController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.loading = true;
        $scope.data = [];
        $scope.page = 0;
        $scope.pageCount = 0;
        $scope.search = search;
        $scope.numRow = 10;
        $scope.clearSearch = clearSearch;
        $scope.deleteItem = deleteItem;
        $scope.selectAll = selectAll;
        $scope.keyword = '';
        $scope.getProductAttributeGroups = getProductAttributeGroups;
        $scope.getSearchName = getSearchName;
        $scope.dataSearch = [];
        $scope.SelectedItem = null;

        $scope.deleteMultiple = deleteMultiple;

        function deleteMultiple() {
            var options = {
                message: 'Bạn có chắc muốn xóa các nhóm thuộc tính đã chọn?',
                title: 'Xóa nhóm thuộc tính',
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
                                listId.push(item.ID);
                            });
                            var config = {
                                params: {
                                    checkedProductAttributeGroups: JSON.stringify(listId)
                                }
                            }
                            apiService.del('api/productattributegroup/deletemulti', config, function (result) {
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
                title: 'Xóa nhóm thuộc tính',
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
                            apiService.del('/api/productattributegroup/delete', config, function () {
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
            $scope.keyword = document.getElementById("txtSearch_value").value;
            getProductAttributeGroups();
        }

        function getProductAttributeGroups(page, pageSize) {
            page = page || 0;

            $scope.loading = true;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: $scope.numRow
                }
            }
            apiService.get('api/productattributegroup/getall', config, dataLoadCompleted, dataLoadFailed);
        }

        function dataLoadCompleted(result) {
            $scope.data = result.data.Items;
            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loading = false;

            if ($scope.keyword && $scope.keyword.length) {
                notificationService.displayInfo(result.data.Items.length + ' được tìm thấy');
            }
        }

        function dataLoadFailed(response) {
            notificationService.displayError('Tải dữ liệu không thành công, thử tải lại trang!');
        }

        function clearSearch() {
            $scope.filterExpression = '';
            search();
        }

        $scope.afterSelectedItem = function (selected) {
            if (selected) {
                $scope.SelectedItem = selected.originalObject;
                $scope.keyword = selected.originalObject.Name;
            }
        }

        function getSearchName() {
            apiService.get('api/productattributegroup/getallparents', null, function (result) {
                $scope.dataSearch = result.data;
            }, function () {
                notificationService.displayError('Tải dữ liệu không thành công, thử tải lại trang!');
            });
        }

        $scope.getSearchName();

        $scope.getProductAttributeGroups();
    }
})(angular.module('shopme.product_attribute_groups'));