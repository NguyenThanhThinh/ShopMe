/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('shopme.orders', ['shopme.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];
    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('orders', {
                url: "/orders",
                parent:'base',
                templateUrl: "/spa/components/orders/orderListView.html",
                controller: "orderListController"
            }).state('order_edit', {
                url: "/order_edit/:id",
                parent: 'base',
                templateUrl: "/spa/components/orders/orderEditView.html",
                controller: "orderEditController"

            }).state('order_add', {
                url: "/order_add",
                parent: 'base',
                templateUrl: "/spa/components/orders/orderAddView.html",
                controller: "orderAddController"
            })
            .state('order_print', {
                url: "/order_print/:id",
                templateUrl: "/spa/components/orders/orderPrintView.html",
                controller: "orderPrintController",
                reload:true
            });
    }
})();