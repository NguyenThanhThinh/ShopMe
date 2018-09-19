(function (app) {
    'use strict';
    app.service('authenticationService', ['$http', '$q', '$window', 'authData',
    function ($http, $q, $window, authData) {
        var tokenInfo;

        this.setTokenInfo = function (data) {
            tokenInfo = data;
            $window.localStorage["TokenInfo"] = JSON.stringify(tokenInfo);
        }

        this.getTokenInfo = function () {
            return tokenInfo;
        }

        this.removeToken = function () {
            tokenInfo = null;
            $window.localStorage["TokenInfo"] = null;
            $window.localStorage.clear();
        }

        this.init = function () {
            if ($window.localStorage["TokenInfo"]) {
                tokenInfo = JSON.parse($window.localStorage["TokenInfo"]);
                if (tokenInfo != null) {
                    authData.authenticationData.IsAuthenticated = true;
                    authData.authenticationData.userName = tokenInfo.userName;
                }
            }
        }

        this.setHeader = function () {
            delete $http.defaults.headers.common['X-Requested-With'];
            if ((tokenInfo != undefined) && (tokenInfo.accessToken != undefined) && (tokenInfo.accessToken != null) && (tokenInfo.accessToken != "")) {
                $http.defaults.headers.common['Authorization'] = 'Bearer ' + tokenInfo.accessToken;
                $http.defaults.headers.common['Content-Type'] = 'application/x-www-form-urlencoded;charset=utf-8';
            }
        }

        this.validateRequest = function () {
            this.setHeader();
            var url = 'api/home/TestMethod';
            var deferred = $q.defer();
            $http.get(url).then(function () {
                deferred.resolve(null);
            }, function (error) {
                deferred.reject(error);
            });
            return deferred.promise;
        }

        this.init();
    }
    ]);
})(angular.module('shopme.common'));


//(function (app) {
//    'use strict';
//    app.service('authenticationService', ['$http', '$q', '$window', 'localStorageService', 'authData',
//        function ($http, $q, $window, localStorageService, authData) {
//            var tokenInfo;

//            this.setTokenInfo = function (data) {
//                tokenInfo = data;
//                localStorageService.set("TokenInfo", JSON.stringify(tokenInfo));
//            }

//            this.getTokenInfo = function () {
//                return tokenInfo;
//            }

//            this.removeToken = function () {
//                tokenInfo = null;
//                localStorageService.set("TokenInfo", null);
//            }

//            this.init = function () {
//                var tokenInfo = localStorageService.get("TokenInfo");
//                if (tokenInfo) {
//                    tokenInfo = JSON.parse(tokenInfo);
//                    authData.authenticationData.IsAuthenticated = true;
//                    authData.authenticationData.userName = tokenInfo.userName;
//                    authData.authenticationData.accessToken = tokenInfo.accessToken;

//                }
//            }

//            this.setHeader = function () {
//                delete $http.defaults.headers.common['X-Requested-With'];
//                if ((authData.authenticationData != undefined) && (authData.authenticationData.accessToken != undefined) && (authData.authenticationData.accessToken != null) && (authData.authenticationData.accessToken != "")) {
//                    $http.defaults.headers.common['Authorization'] = 'Bearer ' + authData.authenticationData.accessToken;
//                    $http.defaults.headers.common['Content-Type'] = 'application/x-www-form-urlencoded;charset=utf-8';
//                }
//            }

//            this.validateRequest = function () {
//                var url = 'api/home/TestMethod';
//                var deferred = $q.defer();
//                $http.get(url).then(function () {
//                    deferred.resolve(null);
//                }, function (error) {
//                    deferred.reject(error);
//                });
//                return deferred.promise;
//            }

//            this.init();
//        }
//    ]);
//})(angular.module('shopme.common'));