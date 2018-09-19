/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('shopme.product_attribute_groups', ['shopme.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];
    function config($stateProvider, $urlRouterProvider) {
        $stateProvider.state('product_attribute_groups', {
            url: "/product_attribute_groups",
            templateUrl: "/app/components/product_attribute_groups/productAttributeGroupListView.html",
            parent: 'base',
            controller: "productAttributeGroupListController"
        })
            .state('add_product_attribute_group', {
                url: "/add_product_attribute_group",
                parent: 'base',
                templateUrl: "/app/components/product_attribute_groups/productAttributeGroupAddView.html",
                controller: "productAttributeGroupAddController"
            })
            .state('edit_product_attribute_group', {
                url: "/edit_product_attribute_group/:id",
                templateUrl: "/app/components/product_attribute_groups/productAttributeGroupEditView.html",
                controller: "productAttributeGroupEditController",
                parent: 'base',
            });

    }
})();