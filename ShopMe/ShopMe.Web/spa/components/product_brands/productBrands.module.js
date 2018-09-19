/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('shopme.product_brands', ['shopme.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];
    function config($stateProvider, $urlRouterProvider) {
        $stateProvider.state('product_brands', {
            url: "/product_brands",
            parent: 'base',
            templateUrl: "/spa/components/product_brands/productBrandListView.html",
            controller: "productBrandListController"
        }).state('add_product_brand', {
            url: "/add_product_brand",
            parent: 'base',
            templateUrl: "/spa/components/product_brands/productBrandAddView.html",
            controller: "productBrandAddController"
        }).state('edit_product_brand', {
            url: "/edit_product_brand/:id",
            parent: 'base',
            templateUrl: "/spa/components/product_brands/productBrandEditView.html",
            controller: "productBrandEditController"
        });
    }
})();