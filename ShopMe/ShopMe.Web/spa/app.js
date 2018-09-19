/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('shopme',
            [
             'shopme.application_groups',
             'shopme.application_roles',
             'shopme.application_users',
             'shopme.product_categories',
             'shopme.product_brands',
             'shopme.post_categories',
             'shopme.product_attribute_groups',
             'shopme.product_unit',
             'shopme.posts',
             'shopme.product_templates',
             'shopme.products',
             'shopme.orders',
             'shopme.feedbacks',
                'shopme.statistics',
                'shopme.slider',
             'shopme.common'
            ])
        .config(config)
    .config(configAuthentication);

    config.$inject = ['$stateProvider', '$urlRouterProvider', '$locationProvider'];
    function config($stateProvider, $urlRouterProvider, $locationProvider) {
        $stateProvider
            .state('base', {
                url: '',
                templateUrl: 'spa/shared/views/baseView.html',
                abstract: true
            })
            .state('login', {
                url: '/login',
                templateUrl: "/spa/components/login/loginView.html",
                controller: "loginController"
            })
            .state('home', {
                url: '/admin',
                parent: 'base',
                templateUrl: "/spa/components/home/homeView.html",
                controller: "homeController"
            })
            .state('404notfound', {
                url: '/404notfound.html',
                templateUrl: "/spa/components/home/404NotFoundView.html"
            });

        $urlRouterProvider.otherwise('/login');
    }

    function configAuthentication($httpProvider) {
        $httpProvider.interceptors.push(function ($q, $location) {
            return {
                request: function (config) {

                    return config;
                },
                requestError: function (rejection) {

                    return $q.reject(rejection);
                },
                response: function (response) {
                    if (response.status == "401") {
                        $location.path('/login');
                    }
                    return response;
                },
                responseError: function (rejection) {

                    if (rejection.status == "401") {
                        $location.path('/login');
                    }
                    return $q.reject(rejection);
                }
            };
        });
    }
})();