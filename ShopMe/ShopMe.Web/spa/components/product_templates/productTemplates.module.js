/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('shopme.product_templates', ['shopme.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];
    function config($stateProvider, $urlRouterProvider) {
        $stateProvider.state('product_templates', {
            url: "/product_templates",
            templateUrl: "/app/components/product_templates/productTemplateListView.html",
            parent: 'base',
            controller: "productTemplateListController"
        })
            .state('add_product_template', {
                url: "/add_product_template",
                parent: 'base',
                templateUrl: "/app/components/product_templates/productTemplateAddView.html",
                controller: "productTemplateAddController"
            })
            .state('edit_product_template', {
                url: "/edit_product_template/:id",
                templateUrl: "/app/components/product_templates/productTemplateEditView.html",
                controller: "productTemplateEditController",
                parent: 'base',
            });  
    }
})();