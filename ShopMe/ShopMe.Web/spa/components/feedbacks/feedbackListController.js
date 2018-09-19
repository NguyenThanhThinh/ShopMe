/// <reference path="/Assets/admin/libs/angular/angular.js" />
(function (app) {
    app.controller('feedbackListController', feedbackListController);

    feedbackListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];

    function feedbackListController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.feedbacks = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.getFeedbacks = getFeedbacks;
        $scope.loading = true;
        $scope.numRow = 10;
        $scope.keyword = '';
        $scope.search = search;
        $scope.deleteFeedback = deleteFeedback;
        $scope.selectAll = selectAll;
        $scope.deleteMultiple = deleteMultiple;

        $scope.getSearchName = getSearchName;
        $scope.dataSearch = [];
        $scope.SelectedItem = null;
         
        function deleteMultiple() {
            var options = {
                message: 'Bạn có chắc muốn xóa các phản hồi đã chọn?',
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
                                    checkedFeedbacks: JSON.stringify(listId)
                                }
                            }
                            apiService.del('api/feedback/deletemulti', config, function (result) {
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
                angular.forEach($scope.feedbacks, function (item) {
                    item.checked = true;
                });
                $scope.isAll = true;
            } else {
                angular.forEach($scope.feedbacks, function (item) {
                    item.checked = false;
                });
                $scope.isAll = false;
            }
        }

        $scope.$watch("feedbacks", function (n, o) {
            var checked = $filter("filter")(n, { checked: true });
            if (checked.length) {
                $scope.selected = checked;
                $('#btnDelete').removeAttr('disabled');
            } else {
                $('#btnDelete').attr('disabled', 'disabled');
            }
        }, true);
        

        function deleteFeedback(id) {
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
                            apiService.del('api/feedback/delete', config, function () {
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
            getFeedbacks();
        }
        function getFeedbacks(page) {
            page = page || 0;
            $scope.loading = true;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: $scope.numRow
                }
            }
            apiService.get('/api/feedback/getall', config, function (result) {
                if (result.data.TotalCount == 0) {
                    notificationService.displayWarning('Không có phản hồi nào có email chứa ký tự: <b>' + config.params.keyword + '</b>');
                }
                $scope.feedbacks = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
                $scope.loading = false;
            }, function () {
                console.log('Tải dữ liệu không thành công.');
            });
        }

        $scope.afterSelectedItem = function (selected) {
            if (selected) {
                $scope.SelectedItem = selected.originalObject;
                $scope.keyword = selected.originalObject.Email;
            }
        }

        function getSearchName() {
            apiService.get('/api/feedback/getallparents', null, function (result) {
                $scope.dataSearch = result.data;
            }, function () {
                console.log('Tải dữ liệu không thành công.');
            });
        }

        $scope.getSearchName();
        $scope.getFeedbacks();
    }
})(angular.module('shopme.feedbacks'));