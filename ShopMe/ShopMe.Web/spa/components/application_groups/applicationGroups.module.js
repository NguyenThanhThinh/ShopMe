﻿/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('shopme.application_groups', ['shopme.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];
    function config($stateProvider, $urlRouterProvider) {
        $stateProvider.state('application_groups', {
            url: "/application_groups",
            templateUrl: "/spa/components/application_groups/applicationGroupListView.html",
            parent: 'base',
            controller: "applicationGroupListController"
        })
            .state('add_application_group', {
                url: "/add_application_group",
                parent: 'base',
                templateUrl: "/spa/components/application_groups/applicationGroupAddView.html",
                controller: "applicationGroupAddController"
            })
            .state('edit_application_group', {
                url: "/edit_application_group/:id",
                templateUrl: "/spa/components/application_groups/applicationGroupEditView.html",
                controller: "applicationGroupEditController",
                parent: 'base',
            });

    }
})();