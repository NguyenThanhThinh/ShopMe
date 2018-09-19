/// <reference path="/Assets/admin/libs/angular/angular.js" />
(function (app) {
    app.controller('productListController', productListController);

    productListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter', 'commonService'];

    function productListController($scope, apiService, notificationService, $ngBootbox, $filter, commonService) {
        $scope.products = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.loading = true;
        $scope.numRow = 10;
        $scope.keyword = '';
        $scope.getProducts = getProducts;
        $scope.search = search;
        $scope.deleteProduct = deleteProduct;
        $scope.selectAll = selectAll;
        $scope.deleteMultiple = deleteMultiple;

        $scope.getSearchName = getSearchName;
        $scope.dataSearch = [];
        $scope.SelectedItem = null;

        $scope.flatFolders = [];
        $scope.productCategories = {};

        $scope.exportData = function () {
            var blob = new Blob([document.getElementById('exportable').innerHTML], {
                type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
            });
            var dateNow = new Date();
            saveAs(blob, "San_Pham_" + dateNow.getFullYear() + dateNow.getMonth() + dateNow.getDate() + dateNow.getHours() + dateNow.getMinutes() + dateNow.getSeconds() + ".xls");
        };

        function deleteMultiple() {
            var options = {
                message: 'Bạn có chắc muốn xóa các sản phẩm đã chọn?',
                title: 'Xóa sản phẩm',
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
                                    checkedProducts: JSON.stringify(listId)
                                }
                            }
                            apiService.del('api/product/deletemulti', config, function (result) {
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
                angular.forEach($scope.products, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.products, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }

        $scope.$watch("products", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled');
                $('#btnExport').removeAttr('disabled');
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
                $('#btnExport').attr('disabled', 'disabled');
            }
        }, true);
        //Hàm xóa sản phẩm theo ID
        function deleteProduct(id) {
            var options = {
                message: 'Bạn có chắc muốn xóa?',
                title: 'Xóa sản phẩm',
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
                            apiService.del('api/product/delete', config, function () {
                                notificationService.displaySuccess('Xóa thành công');
                                search();
                            }, function () {
                                notificationService.displayError('Xóa không thành công');
                            })
                        }
                    }
                }
            };

            $ngBootbox.customDialog(options);
        }

        function search() {
            $scope.keyword = document.getElementById("txtSearch_value").value;

            $scope.getProducts();
        }

        function getProducts(page) {
            page = page || 0;
            $scope.loading = true;

            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: $scope.numRow,
                    categoryID: $scope.categoryID,
                    brandID: $scope.brandID,
                    showHomePage: $scope.showHomePage,
                    havePublished: $scope.havePublished,
                    sortQuantity: $scope.sortQuantity
                }
            }
            apiService.get('/api/product/getall', config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning('Không có sản phẩm nào có tên chứa ký tự: ' + config.params.keyword);
                }
                $scope.products = result.data.Items;
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
                $scope.SelectedItem = selected.originalObject;
                $scope.keyword = selected.originalObject.Name;
            }
        }

        function getSearchName() {
            apiService.get('/api/product/getallparents', null, function (result) {
                $scope.dataSearch = result.data;
            }, function () {
                notificationService.displayError('Tải dữ liệu không thành công, thử tải lại trang!');
            });
        }

        function loadProductBrand() {
            apiService.get('api/productbrand/getallparents', null, function (result) {
                $scope.productBrands = result.data;
            }, function () {
                console.log('Tải danh sách nhãn hiệu thất bại!');
            });
        }
        function loadProductCategory() {
            apiService.get('api/productcategory/getallparents', null, function (result) {
                $scope.flatFolders = result.data;
             
            }, function () {
                console.log('Tải danh mục không thành công!');
            });
        }

        $scope.getSearchName();
        loadProductCategory();
        loadProductBrand();
        $scope.getProducts();
    }
})(angular.module('shopme.products'));