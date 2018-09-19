(function (app) {
    app.controller('postAddController', postAddController)

    postAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService'];
    function postAddController(apiService, $scope, notificationService, $state, commonService) {
        $scope.post = {
            CreatedDate: new Date(),
            IsPublished: true

        }
        //Hàm gọi CKeditor
        $scope.ckeditorOptions = {
            languague: 'vi',
            height: '200px',
        }

        $scope.AddPost = AddPost;
        $scope.GetSeoTitle = GetSeoTitle;
        $scope.flatFolders = [];
        $scope.postCategories = {};


         //Chuyển đổi ký tự thành không dấu
        function GetSeoTitle() {
            $scope.post.Alias = commonService.getSeoTitle($scope.post.Name);
            $scope.post.NormalizedName = commonService.getNormalizedName($scope.post.Name);
        }

        //Thêm bài viết
        function AddPost() {
            apiService.post('api/post/create', $scope.post, function (result) {
                notificationService.displaySuccess(result.data.Name + ' đã được thêm mới.');
                $state.go('posts');
            }, function (error) {
                notificationService.displayError('Thêm mới không thành công!');
                notificationService.displayErrorValidation(respone);
            });
        }
         //Load danh sách danh mục bài viết
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

        //Reset lại form
        $scope.resetForm = function resetForm() {
            $scope.post = {};
        }


        loadPostCategory();
    }


})(angular.module('shopme.posts'));