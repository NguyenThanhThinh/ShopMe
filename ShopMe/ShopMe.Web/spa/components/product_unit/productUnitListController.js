(function (app) {
    app.controller('productUnitListController', productUnitListController);

    productUnitListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];

    function productUnitListController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.loading = true;
        $scope.productBrands = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.getProductBrands = getProductBrands;
        $scope.keyword = '';
        $scope.numRow = 10;
        $scope.search = search;
        $scope.deleteProductBrand = deleteProductBrand;


        $scope.getSearchName = getSearchName;
        $scope.dataSearch = [];
        $scope.SelectedItem = null;



        $scope.$watch("productBrands", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled');
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true);

        function deleteProductBrand(id) {
            var options = {
                message: 'Bạn có chắc muốn xóa?',
                title: 'Xóa nhãn hiệu',
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
                            apiService.del('/api/productunit/delete', config, function () {
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
            getProductBrands();
        }
        function getProductBrands(page, pageSize) {
            page = page || 0;
            $scope.loading = true;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: $scope.numRow
                }
            }
            apiService.get('api/productunit/getall', config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning('Không có bản ghi nào được tìm thấy.')
                }
                $scope.productBrands = result.data.Items;
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
            apiService.get('api/productunit/getallparents', null, function (result) {
                $scope.dataSearch = result.data;
            }, function () {
                notificationService.displayError('Tải dữ liệu không thành công, thử tải lại trang!');
            });
        }

        $scope.getSearchName();

        $scope.getProductBrands();
    }
})(angular.module('shopme.product_unit'));