/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('shopme.statistics', ['shopme.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];
    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('statistic_revenue', {
                url: "/statistic_revenue",
                parent: 'base',
                templateUrl: "/spa/components/statistic/revenueStatisticView.html",
                controller: "revenueStatisticController"
            }).state('statistic_product', {
                url: "/statistic_product",
                parent: 'base',
                templateUrl: "/spa/components/statistic/productStatisticView.html",
                controller: "productStatisticController"
            });
    }
})();