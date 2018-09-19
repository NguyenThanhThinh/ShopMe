(function (app) {
    app.controller('sliderAddController', sliderAddController)

    sliderAddController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService'];
    function sliderAddController(apiService, $scope, notificationService, $state, commonService) {
        $scope.addslider = {
            CreatedDate: new Date(),
            Status: true
        }

        $scope.AddSlider = AddSlider;
     

        

        function AddSlider() {
            apiService.post('api/slider/create', $scope.addslider, function (result) {
                notificationService.displaySuccess(result.data.Description + ' đã được thêm mới.');
                $state.go('slider');
            }, function (error) {
                notificationService.displayError('Thêm mới không thành công!');
                notificationService.displayErrorValidation(response);
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

    
    }

})(angular.module('shopme.slider'));