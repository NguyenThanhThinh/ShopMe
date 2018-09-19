/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('shopme.posts', ['shopme.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];
    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('posts', {
                url: "/posts",
                parent:'base',
                templateUrl: "/app/components/posts/postListView.html",
                controller: "postListController"
            }).state('post_add', {
                url: "/post_add",
                parent: 'base',
                templateUrl: "/app/components/posts/postAddView.html",
                controller: "postAddController"
            }).state('post_edit', {
                url: "/post_edit/:id",
                parent: 'base',
                templateUrl: "/app/components/posts/postEditView.html",
                controller: "postEditController"
            });
    }
})();