(function (app) {
    app.controller('postCategoryAddController', postCategoryAddController)

    postCategoryAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService'];
    function postCategoryAddController(apiService, $scope, notificationService, $state, commonService) {
        $scope.postCategory = {
            CreatedDate: new Date(),
            IsPublished: true
        }
 
        $scope.AddPostCategory = AddPostCategory;
        $scope.GetSeoTitle = GetSeoTitle;  
        $scope.flatFolders = [];
        $scope.parentCategories = {};

        function GetSeoTitle() {
            $scope.postCategory.Alias = commonService.getSeoTitle($scope.postCategory.Name);
            $scope.postCategory.NormalizedName = commonService.getNormalizedName($scope.postCategory.Name);
        }

        function AddPostCategory() {
            apiService.post('api/postcategory/create', $scope.postCategory, function (result) {
                notificationService.displaySuccess(result.data.Name + ' đã được thêm mới.');
                $state.go('post_categories');
            }, function (error) {
                notificationService.displayError('Thêm mới không thành công!');
                notificationService.displayErrorValidation(response);
            });
        }

        function loadParentCategory() {
            apiService.get('api/postcategory/getallparents', null, function (result) {
                $scope.parentCategories = commonService.getTree(result.data, 'ID', 'ParentID');
                $scope.parentCategories.forEach(function (item) {
                    commonService.recur(item, 0, $scope.flatFolders);
                });
            }, function () {
                console.log('Tải dữ liệu không thành công.');
            });
        }

        $scope.ChooseImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.postCategory.Image = fileUrl;
                })
            }
            finder.popup();
        }
        $scope.resetForm = function resetForm() {
            $scope.postCategory = {};
        }

        loadParentCategory();
    }             

})(angular.module('shopme.post_categories'));