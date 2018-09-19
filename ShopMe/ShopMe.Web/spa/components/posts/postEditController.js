(function (app) {
    app.controller('postEditController', postEditController)

    postEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService', '$stateParams'];
    function postEditController(apiService, $scope, notificationService, $state, commonService, $stateParams) {
        $scope.post = {}

        //Gọi CKEditor
        $scope.ckeditorOptions = {
            languague: 'vi',
            height: '200px',
        }

        $scope.UpdatePost = UpdatePost;
        $scope.GetSeoTitle = GetSeoTitle;
        $scope.flatFolders = [];
        $scope.postCategories = {};

        function GetSeoTitle() {
            $scope.post.Alias = commonService.getSeoTitle($scope.post.Name);
            $scope.post.NormalizedName = commonService.getNormalizedName($scope.post.Name);
        }

        function loadPostDetail() {
            apiService.get('api/post/getbyid/' + $stateParams.id, null, function (result) {
                $scope.post = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }

        function UpdatePost() {
            apiService.put('api/post/update', $scope.post, function (result) {
                notificationService.displaySuccess(result.data.Name + ' đã được cập nhật.');
                $state.go('posts');
            }, function (error) {
                notificationService.displayError('Cập nhật không thành công!');
                notificationService.displayErrorValidation(respone);
            });
        }

        function loadPostCategory() {
            apiService.get('api/postcategory/getallparents', null, function (result) {
                $scope.postCategories = commonService.getTree(result.data, 'ID', 'ParentID');
                $scope.postCategories.forEach(function (item) {
                    commonService.recur(item, 0, $scope.flatFolders);
                });
            }, function () {
                console.log('Tải danh mục không thành công!');
            });
        }

        $scope.ChooseImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.post.Image = fileUrl;
                })
            }
            finder.popup();
        }

        $scope.resetForm = function resetForm() {
            loadPostDetail();
        }

        loadPostCategory();
        loadPostDetail();
    }


})(angular.module('shopme.posts'));