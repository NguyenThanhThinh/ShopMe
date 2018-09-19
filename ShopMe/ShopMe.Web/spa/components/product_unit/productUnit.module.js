/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('shopme.product_unit', ['shopme.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider.state('product_unit', {
            url: "/product_unit",
            templateUrl: "/spa/components/product_unit/productUnitListView.html",
            parent: 'base',
            controller: "productUnitListController"
        })
            .state('add_product_unit', {
                url: "/add_product_unit",
                parent: 'base',
                templateUrl: "/spa/components/product_unit/productUnitAddView.html",
                controller: "productUnitAddController"
            })
            .state('edit_product_unit', {
                url: "/edit_product_unit/:id",
                templateUrl: "/spa/components/product_unit/productUnitEditView.html",
                controller: "productUnitEditController",
                parent: 'base',
            });
    }
})();