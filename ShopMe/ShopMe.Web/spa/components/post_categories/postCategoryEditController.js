(function (app) {
    app.controller('postCategoryEditController', postCategoryEditController)

    postCategoryEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', '$stateParams', 'commonService'];
    function postCategoryEditController(apiService, $scope, notificationService, $state, $stateParams, commonService) {
        $scope.postCategory = {
            CreatedDate: new Date(),
            IsPublished: true
        }

        $scope.UpdatePostCategory = UpdatePostCategory;
        $scope.GetSeoTitle = GetSeoTitle;
        $scope.flatFolders = [];
        $scope.parentCategories = {};

        //Hàm xử lý SEO Title và xử lý tên không dấu
        function GetSeoTitle() {
            $scope.postCategory.Alias = commonService.getSeoTitle($scope.postCategory.Name);
            $scope.postCategory.NormalizedName = commonService.getNormalizedName($scope.postCategory.Name);
        }

        //Hàm tải thông tin chi tiết, nhận vào tham số là id của postCategory
        function loadPostCategoryDetail() {
            apiService.get('api/postcategory/getbyid/' + $stateParams.id, null, function (result) {
                $scope.postCategory = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }

        //Hàm cập nhật thông tin
        function UpdatePostCategory() {
            apiService.put('api/postcategory/update', $scope.postCategory,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được cập nhật.');
                    $state.go('post_categories');
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công.');
                    notificationService.displayErrorValidation(response);
                });
        }

        //Hàm load danh sách danh mục, không tham số đầu vào
        function loadParentCategory() {
            apiService.get('api/postcategory/getallparents', null, function (result) {
                $scope.parentCategories = commonService.getTree(result.data, 'ID', 'ParentID');
                $scope.parentCategories.forEach(function (item) {
                    commonService.recur(item, 0, $scope.flatFolders);
                });
            }, function () {
                console.log('Tải dữ liệu thất bại.');
            });
        }

        //Chọn ảnh
        $scope.ChooseImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.postCategory.Image = fileUrl;
                })
            }
            finder.popup();
        }
        
        //Reset form
        $scope.resetForm = function resetForm() {
            loadPostCategoryDetail();
        }

        loadParentCategory();
        loadPostCategoryDetail();
    }


})(angular.module('shopme.post_categories'));