(function (app) {
    app.controller('sliderEditController', sliderEditController)

    sliderEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService', '$stateParams'];
    function sliderEditController(apiService, $scope, notificationService, $state, commonService, $stateParams) {
        $scope.update = {}

      

        $scope.UpdateSlider = UpdateSlider;
      

      
        function loadSliderDetail() {
            apiService.get('api/slider/getbyid/' + $stateParams.id, null, function (result) {
                $scope.post = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }

        function UpdateSlider() {
            apiService.put('api/slider/update', $scope.post, function (result) {
                notificationService.displaySuccess(result.data.Name + ' đã được cập nhật.');
                $state.go('posts');
            }, function (error) {
                notificationService.displayError('Cập nhật không thành công!');
                notificationService.displayErrorValidation(respone);
            });
        }

        

        $scope.ChooseImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.addslider.Image = fileUrl;
                })
            }
            finder.popup();
        }
        $scope.resetForm = function resetForm() {
            $scope.addslider = {};
        }
        $scope.resetForm = function resetForm() {
            loadSliderDetail();
        }

        loadSliderDetail();
    }


})(angular.module('shopme.slider'));