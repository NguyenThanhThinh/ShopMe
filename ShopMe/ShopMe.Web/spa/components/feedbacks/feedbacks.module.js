/// <reference path="/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('shopme.feedbacks', ['shopme.common']).config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider'];
    function config($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('feedbacks', {
                url: "/feedbacks",
                parent:'base',
                templateUrl: "/spa/components/feedbacks/feedbackListView.html",
                controller: "feedbackListController"
            }).state('feedback_edit', {
                url: "/feedback_edit/:id",
                parent: 'base',
                templateUrl: "/spa/components/feedbacks/feedbackEditView.html",
                controller: "feedbackEditController"
            });
    }
})();