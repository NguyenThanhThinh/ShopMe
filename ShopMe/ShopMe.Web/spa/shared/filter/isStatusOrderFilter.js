/// <reference path="~/Assets/admin/libs/angular/angular.js" />
(function (app) {
    app.filter('isStatusOrderFilter', function () {
        return function (input) {
            switch (input) {
                case 'Pending':
                    return 'label label-danger';
                    break;
                case 'Processing':
                    return 'label label-warning';
                    break;
                case 'Unconfirmed':
                    return 'label label-default';
                    break;
                case 'Confirmed':
                    return 'label label-success';
                    break;
                case 'Complete':
                    return 'label label-info';
                    break;
                case 'Cancelled':
                    return 'label label-default';
                    break;
                case 'HasShipped':
                    return 'label label-success';
                    break;
                case 'Packed':
                    return 'label label-primary';
                    break;
                case 'Unpaid':
                    return 'label label-danger';
                    break;
                case 'Paid':
                    return 'label label-success';
                    break;
                default:
                    return 'label label-default';
            }
        }
    });
})(angular.module('shopme.common'));









