(function (app) {
    app.controller('productCategoryListController', productCategoryListController);

    productCategoryListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];

    function productCategoryListController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.loading = true;
        $scope.productCategories = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.getProductCagories = getProductCagories;
        $scope.keyword = '';
        $scope.numRow = 10;
        $scope.search = search;
        $scope.deleteProductCategory = deleteProductCategory;
        //$scope.selectAll = selectAll;
        //$scope.deleteMultiple = deleteMultiple;

        $scope.getSearchName = getSearchName;
        $scope.dataSearch = [];
        $scope.SelectedItem = null;

        //function deleteMultiple() {
        //    var options = {
        //        message: 'Bạn có chắc muốn xóa các danh mục đã chọn?',
        //        title: 'Xóa danh mục sản phẩm',
        //        className: 'test-class',
        //        buttons: {
        //            warning: {
        //                label: "Từ chối",
        //                className: "btn-default",
        //                callback: function () {
        //                }
        //            },
        //            success: {
        //                label: "Đồng ý",
        //                className: "btn-primary",
        //                callback: function () {
        //                    var listId = [];
        //                    $.each($scope.selected, function (i, item) {
        //                        listId.push(item.ID);
        //                    });
        //                    var config = {
        //                        params: {
        //                            checkedProductCategories: JSON.stringify(listId)
        //                        }
        //                    }
        //                    apiService.del('api/productcategory/deletemulti', config, function (result) {
        //                        notificationService.displaySuccess('Xóa thành công bản ghi');
        //                        search();
        //                    }, function (error) {
        //                        notificationService.displayError('Xóa không thành công.');
        //                    });
        //                }
        //            }
        //        }
        //    };          
        //    $ngBootbox.customDialog(options);
        //}

        //$scope.isAll = false;
        //function selectAll() {
        //    if ($scope.isAll == false) {
        //        angular.forEach($scope.productCategories, function (item) {
        //            item.checked = true;
        //        });
        //        $scope.isAll = true;
        //    } else {
        //        angular.forEach($scope.productCategories, function (item) {
        //            item.checked = false;
        //        });
        //        $scope.isAll = false;
        //    }
        //}

        //$scope.$watch("productCategories", function (n, o) {
        //    var checked = $filter("filter")(n, { checked: true });
        //    if (checked.length) {
        //        $scope.selected = checked;
        //        $('#btnDelete').removeAttr('disabled');
        //    } else {
        //        $('#btnDelete').attr('disabled', 'disabled');
        //    }
        //}, true);

        function deleteProductCategory(id) {
            var options = {
                message: 'Bạn có chắc muốn xóa?',
                title: 'Xóa danh mục sản phẩm',
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
                            apiService.del('/api/productcategory/delete', config, function () {
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
            getProductCagories();
        }

        function getProductCagories(page, pageSize) {
            page = page || 0;
            $scope.loading = true;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: $scope.numRow
                }
            }
            apiService.get('/api/productcategory/getall', config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning('Không có danh mục nào có tên chứa ký tự: ' + config.params.keyword)
                }
                
                $scope.productCategories = result.data.Items;                
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                $scope.loading = false;
            }, function () {
                notificationService.displayError('Tải dữ liệu không thành công, thử tải lại trang!');
            });
        }

        $scope.afterSelectedItem = function (selected) {
            if (selected) {
                $scope.SelectedItem = selected.originalObject.CustomerEmail;
                $scope.keyword = selected.originalObject.CustomerEmail;
            }
        }

        function getSearchName() {
            apiService.get('/api/productcategory/getallparents', null, function (result) {
                $scope.dataSearch = result.data;
            }, function () {
                notificationService.displayError('Tải dữ liệu không thành công, thử tải lại trang!');
            });
        }

        $scope.getSearchName();
        $scope.getProductCagories();
    }
})(angular.module('shopme.product_categories'));