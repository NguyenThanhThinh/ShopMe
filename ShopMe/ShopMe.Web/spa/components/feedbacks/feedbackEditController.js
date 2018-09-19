(function (app) {
    app.controller('feedbackEditController', feedbackEditController)

    feedbackEditController.$inject = ['apiService', '$scope', 'notificationService', '$state', 'commonService','$stateParams'];
    function feedbackEditController(apiService, $scope, notificationService, $state, commonService, $stateParams) {
        $scope.feedback = {}
        

        //Hàm gọi CKeditor
        $scope.ckeditorOptions = {
            languague: 'vi',
            height: '200px',
        }

        $scope.EditFeedback = EditFeedback;

        
        function EditFeedback() {
            $scope.feedback.Status = true;
            apiService.put('api/feedback/update', $scope.feedback, function (result) {
                notificationService.displaySuccess('cập nhật thành công!');
                $state.go('feedbacks');
            }, function (error) {
                notificationService.displayError('cập nhật thất bại!');
                notificationService.displayErrorValidation(respone);
            });
        }
         

        
        function loadFeedbackDetail() {
            apiService.get('api/feedback/getbyid/' + $stateParams.id, null, function (result) {
                $scope.feedback = result.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }

      
        loadFeedbackDetail();
    }


})(angular.module('shopme.feedbacks'));