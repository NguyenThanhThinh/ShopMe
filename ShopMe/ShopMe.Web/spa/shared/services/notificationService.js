(function (app) {
    app.factory('notificationService', notificationService);

    function notificationService() {
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "positionClass": "toast-top-right",
            "onclick": null,
            "fadeIn": 300,
            "fadeOut": 1000,
            "timeOut": 3000,
            "extendedTimeOut": 1000
        };

        function displaySuccess(message) {
            toastr.success(message);
        }

        function displayError(error) {
            if (Array.isArray(error)) {
                error.each(function (err) {
                    toastr.error(err);
                });
            }
            else {
                toastr.error(error);
            }
        }

        function displayWarning(message) {
            toastr.warning(message);
        }
        function displayInfo(message) {
            toastr.info(message);
        }
        function displayErrorValidation(response) {
            if (response.data.ModelState != null) {
                for (var key in response.data.ModelState) {
                    for (var i = 0; i < response.data.ModelState[key].length; i++) {
                        toastr.error(response.data.ModelState[key][i]);
                    }
                }
            }
        }
        return {
            displayErrorValidation: displayErrorValidation,
            displaySuccess: displaySuccess,
            displayError: displayError,
            displayWarning: displayWarning,
            displayInfo: displayInfo
        }
    }
})(angular.module('shopme.common'));