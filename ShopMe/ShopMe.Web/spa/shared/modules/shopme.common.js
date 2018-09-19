/// <reference path="~/Assets/admin/libs/angular/angular.js" />

(function () {
    angular.module('shopme.common', ['ui.router', 'ngBootbox', 'ngCkeditor', 'angular-loading-bar',
        'checklist-model', 'chart.js', 'summernote', 'ui.bootstrap', 'ngSanitize', 'datePicker', 'LocalStorageModule', 'angucomplete-alt', 'ui.select', 'ngSanitize'
        ])

        .config(['cfpLoadingBarProvider', function (cfpLoadingBarProvider) {
            cfpLoadingBarProvider.parentSelector = '#loading-bar-container';
    }]).config(['$ngBootboxConfigProvider',function ($ngBootboxConfigProvider) {       
        $ngBootboxConfigProvider.addLocale('vi', { OK: 'Đồng ý', CANCEL: 'Từ chối', CONFIRM: 'Đồng ý' });
        $ngBootboxConfigProvider.setDefaultLocale('vi');   
    }])
})();