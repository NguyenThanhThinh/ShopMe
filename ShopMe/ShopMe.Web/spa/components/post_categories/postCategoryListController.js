(function (app) {
    app.controller('postCategoryListController', postCategoryListController);

    postCategoryListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];

    function postCategoryListController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.loading = true;
        $scope.postCategories = [];
        $scope.page = 0;
        $scope.numRow = 10;
        $scope.pagesCount = 0;
        $scope.getPostCagories = getPostCagories;
        $scope.keyword = '';
        $scope.search = search;  
        $scope.deletePostCategory = deletePostCategory;   
        $scope.selectAll = selectAll;              
        $scope.deleteMultiple = deleteMultiple;
                                 
        $scope.getSearchName = getSearchName;
        $scope.dataSearch = [];
        $scope.SelectedItem = null;

        function deleteMultiple() {
            var options = {
                message: 'Bạn có chắc muốn xóa các danh mục bài viết đã chọn?',
                title: 'Xóa danh mục',
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
                                    checkedPostCategories: JSON.stringify(listId)
                                }
                            }
                            apiService.del('api/postcategory/deletemulti', config, function (result) {
                                notificationService.displaySuccess('Xóa thành công bản ghi');
                                search();
                            }, function (error) {
                                notificationService.displayError('Xóa không thành công.');
                            });
                        }
                    }
                }
            };

            $ngBootbox.customDialog(options);
        }

        $scope.isAll = false;
        function selectAll() {
            if ($scope.isAll == false) {
                angular.forEach($scope.postCategories, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.postCategories, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }

        $scope.$watch("postCategories", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled');
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true);

        function deletePostCategory(id) {
            var options = {
                message: 'Bạn có chắc muốn xóa?',
                title: 'Xóa danh mục',
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
                            apiService.del('api/postcategory/delete', config, function () {
                                notificationService.displaySuccess('Xóa thành công');
                                search();
                            }, function () {
                                notificationService.displayError('Xóa không thành công')
                            })
                        }
                    }
                }
            };

            $ngBootbox.customDialog(options);
        }

        function search() {
            $scope.keyword = document.getElementById("txtSearch_value").value;
            getPostCagories();

        }

        function getPostCagories(page) {
            page = page || 0;
            $scope.loading = true;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: $scope.numRow
                }
            }
            apiService.get('/api/postcategory/getall', config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning('Không có danh mục nào có tên chứa ký tự: ' + config.params.keyword)
                }

                $scope.postCategories = result.data.Items;
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
            apiService.get('/api/postcategory/getallparents', null, function (result) {
                $scope.dataSearch = result.data;
            }, function () {
                notificationService.displayError('Tải dữ liệu không thành công, thử tải lại trang!');
            });
        }

        $scope.getSearchName();


        $scope.getPostCagories();
    }
})(angular.module('shopme.post_categories'));