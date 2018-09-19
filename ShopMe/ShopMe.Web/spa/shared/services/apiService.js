﻿/// <reference path="~/Assets/admin/libs/angular/angular.js" />
(function (app) {
    app.factory('apiService', apiService);

    apiService.$inject = ['$http', 'notificationService', 'authenticationService'];
    function apiService($http, notificationService, authenticationService) {
        return {
            get: get,
            post: post,
            put: put,
            del: del
        }

        //Post data
        function post(url, data, success, failure) {
            authenticationService.setHeader();
            $http.post(url, data).then(function (result) {    
                success(result);
            }, function (error) {
                console.log(error.status)
                if (error.status === 401) {
                    notificationService.displayError('Bạn chưa đăng nhập hoặc tài khoản không có quyền truy cập!');
                }
                else if (failure != null) {
                    failure(error);
                }
            });
        }

        //Delete function
        function del(url, data, success, failure) {
            authenticationService.setHeader();
            $http.delete(url, data).then(function (result) {
                success(result);
            }, function (error) {
                console.log(error.status)
                if (error.status === 401) {
                    notificationService.displayError('Bạn chưa đăng nhập hoặc tài khoản không có quyền truy cập!');
                }
                else if (failure != null) {
                    failure(error);
                }
            });
        }

        //Get function.
        function get(url, params, success, failure) {
            authenticationService.setHeader();
            $http.get(url, params).then(function (result) {
                success(result);
            }, function (error) {
                console.log(error.status)
                if (error.status === 401) {
                    notificationService.displayError('Bạn chưa đăng nhập hoặc tài khoản không có quyền truy cập!');
                }
                else if (failure != null) {
                    failure(error);
                }
            });
        }

        function put(url, data, success, failure) {
            authenticationService.setHeader();
            $http.put(url, data).then(function (result) {
                success(result);
            }, function (error) {
                console.log(error.status)
                if (error.status === 401) {
                    notificationService.displayError('Bạn chưa đăng nhập hoặc tài khoản không có quyền truy cập!');
                }
                else if (failure != null) {
                    failure(error);
                }

            });
        }
    }
})(angular.module('shopme.common'));