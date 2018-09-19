/// <reference path="~/Assets/admin/libs/angular/angular.js" />
(function (app) {
    app.filter('isPublishFilter', function () {
        return function (input) {
            if (input == true)
                return 'fa fa-check true-icon';
            else
                return 'fa fa-close false-icon';
        }
    });
})(angular.module('shopme.common'));