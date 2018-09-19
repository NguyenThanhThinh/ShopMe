/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('shopme.application_users', ['shopme.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];

    function config($stateProvider, $urlRouterProvider) {

        $stateProvider.state('application_users', {
            url: "/application_users",
            templateUrl: "/spa/components/application_users/applicationUserListView.html",
            parent: 'base',
            controller: "applicationUserListController"
        })
            .state('add_application_user', {
                url: "/add_application_user",
                parent: 'base',
                templateUrl: "/spa/components/application_users/applicationUserAddView.html",
                controller: "applicationUserAddController"
            })
            .state('edit_application_user', {
                url: "/edit_application_user/:id",
                templateUrl: "/spa/components/application_users/applicationUserEditView.html",
                controller: "applicationUserEditController",
                parent: 'base',
            }).state('change_Password', {
                url: "/change_Password/:username",
                templateUrl: "/spa/components/application_users/applicationUserChangePassView.html",
                controller: "applicationUserChangePassController",
                parent: 'base',
            });
    }
})();