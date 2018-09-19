/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('shopme.slider', ['shopme.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider', '$locationProvider'];
    function config($stateProvider, $urlRouterProvider, $locationProvider) {
        $stateProvider.state('slider', {
            url: "/slider",
            parent: 'base',
            templateUrl: "/spa/components/slider/sliderListView.html",
            controller: "sliderListController"
        }).state('add_slider', {
            url: "/add_slider",
            parent: 'base',
            templateUrl: "/spa/components/slider/sliderAddView.html",
            controller: "sliderAddController"
            }).state('edit_slider', {
              url: "/edit_slider/:id",
            parent: 'base',
            templateUrl: "/spa/components/slider/sliderEditView.html",
            controller: "sliderEditController"
        });
    }
})();