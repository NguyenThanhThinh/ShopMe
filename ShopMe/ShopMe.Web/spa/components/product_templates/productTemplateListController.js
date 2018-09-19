(function (app) {
    'use strict';

    app.controller('productTemplateListController', productTemplateListController);

    productTemplateListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];

    function productTemplateListController($scope, apiService, notificationService, $ngBootbox, $filter) {
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
        $scope.getProductTemplates = getProductTemplates;

        $scope.getSearchName = getSearchName;
        $scope.dataSearch = [];
        $scope.SelectedItem = null;

        $scope.deleteMultiple = deleteMultiple;

        function deleteMultiple() {
            var options = {
                message: 'Bạn có chắc muốn xóa các mẫu đã chọn?',
                title: 'Xóa nhóm mẫu thuộc tính',
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
                                    checkedProductTemplates: JSON.stringify(listId)
                                }
                            }
                            apiService.del('api/producttemplate/deletemulti', config, function (result) {
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
                title: 'Xóa mẫu danh sách thuộc tính',
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
                            apiService.del('/api/producttemplate/delete', config, function () {
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

        function search() {
            $scope.keyword = document.getElementById("txtSearch_value").value;
            getProductTemplates();
        }

        function getProductTemplates(page, pageSize) {
            page = page || 0;

            $scope.loading = true;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: $scope.numRow
                }
            }
            apiService.get('api/producttemplate/getall', config, dataLoadCompleted, dataLoadFailed);
        }

        function dataLoadCompleted(result) {
            $scope.data = result.data.Items;
            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loading = false;

            if ($scope.keyword && $scope.keyword.length) {
                notificationService.displayInfo('Tìm thấy ' +result.data.Items.length + ' mẫu');
            }
        }

        function dataLoadFailed(response) {
            notificationService.displayError('Tải dữ liệu không thành công, thử tải lại trang!');
        }

        function clearSearch() {
            $scope.keyword = '';
            search();
        }

        $scope.afterSelectedItem = function (selected) {
            if (selected) {
                $scope.SelectedItem = selected.originalObject;
                $scope.keyword = selected.originalObject.Name;
            }
        }

        function getSearchName() {
            apiService.get('/api/producttemplate/getallparents', null, function (result) {
                $scope.dataSearch = result.data;
            }, function () {
                notificationService.displayError('Tải dữ liệu không thành công, thử tải lại trang!');
            });
        }

        $scope.getSearchName();

        $scope.getProductTemplates();
    }
})(angular.module('shopme.product_templates'));