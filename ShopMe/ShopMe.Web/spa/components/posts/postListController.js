/// <reference path="/Assets/admin/libs/angular/angular.js" />
(function (app) {
    app.controller('postListController', postListController);

    postListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];

    function postListController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.posts = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.getPosts = getPosts;
        $scope.loading = true;
        $scope.numRow = 10;
        $scope.keyword = '';
        $scope.search = search;
        $scope.deletePost = deletePost;
        $scope.selectAll = selectAll;
        $scope.deleteMultiple = deleteMultiple;

        $scope.getSearchName = getSearchName;
        $scope.dataSearch = [];
        $scope.SelectedItem = null;

         //Xóa nhiều bài viết
        function deleteMultiple() {
            var options = {
                message: 'Bạn có chắc muốn xóa các bài viết đã chọn?',
                title: 'Xóa bài viết',
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
                                    checkedPosts: JSON.stringify(listId)
                                }
                            }
                            apiService.del('api/post/deletemulti', config, function (result) {
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
                angular.forEach($scope.posts, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.posts, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }

        $scope.$watch("posts", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled');
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true);
        //Hàm xóa bài viết theo ID
        function deletePost(id) {
            var options = {
                message: 'Bạn có chắc muốn xóa?',
                title: 'Xóa bài viết',
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
                            apiService.del('api/post/delete', config, function () {
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
        //Thao tác tìm kiếm
        function search() {
            $scope.keyword = document.getElementById("txtSearch_value").value;
            getPosts();
        }
        //Hàm lấy danh sách bài viết
        function getPosts(page) {
            page = page || 0;
            $scope.loading = true;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: $scope.numRow
                }
            }
            apiService.get('/api/post/getall', config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning('Không có bài viết nào có tên chứa ký tự: ' + config.params.keyword);
                }
                $scope.posts = result.data.Items;
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
            apiService.get('/api/post/getallparents', null, function (result) {
                $scope.dataSearch = result.data;
            }, function () {
                notificationService.displayError('Tải dữ liệu không thành công, thử tải lại trang!');
            });
        }

        $scope.getSearchName();
        $scope.getPosts();
    }
})(angular.module('shopme.posts'));