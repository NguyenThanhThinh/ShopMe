(function (app) {
    app.controller('sliderListController', sliderListController);

    sliderListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox', '$filter'];

    function sliderListController($scope, apiService, notificationService, $ngBootbox, $filter) {
        $scope.loading = true;
        $scope.postCategories = [];
        $scope.page = 0;
        $scope.numRow = 10;
        $scope.pagesCount = 0;
        $scope.getPostCagories = getPostCagories;
        $scope.keyword = '';
        $scope.search = search;
        $scope.deletePostCategory = deletePostCategory;
      
 

        $scope.getSearchName = getSearchName;
        $scope.dataSearch = [];
     

     

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
                            apiService.del('api/slider/delete', config, function () {
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
            apiService.get('/api/slider/getall', config, function (result) {
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

        

        function getSearchName() {
            apiService.get('/api/slider/getallparents', null, function (result) {
                $scope.dataSearch = result.data;
            }, function () {
                notificationService.displayError('Tải dữ liệu không thành công, thử tải lại trang!');
            });
        }

        $scope.getSearchName();


        $scope.getPostCagories();
    }
})(angular.module('shopme.slider'));