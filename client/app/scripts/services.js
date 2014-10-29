/**
 * Created by Umayr on 10/21/2014.
 */

(function () {
    'use strict';

    angular
        .module('olx-client')
        .factory('api', ['$http', 'API_BASE_URL', 'PAGE_SIZE', function ($http, API_BASE_URL, PAGE_SIZE) {
            return {
                count: function () {
                    return $http.get(API_BASE_URL + '/count');
                },
                get: function (iterator, rows) {
                    var top = rows || PAGE_SIZE;

                    var url = '/get?iterator=' + iterator;
                    if (top !== PAGE_SIZE) {
                        url += '&top=' + top;
                    }
                    return $http.get(API_BASE_URL + url);
                },
                images: function (uniqueId) {
                    var url = '/images?uniqueId=' + uniqueId;
                    return $http.get(API_BASE_URL + url);
                }
            };
        }]);

    angular
        .module('olx-client')
        .factory('login', ['CREDENTIALS', function (CREDENTIALS) {
            return {
                attempt: function (login) {
                    if (login.username !== CREDENTIALS.username) {
                        return {
                            success: false,
                            message: 'Booo! Invalid Username.'
                        };
                    }
                    if (login.password !== atob(CREDENTIALS.password)) {
                        return {
                            success: false,
                            message: 'Booo! Invalid Password.'
                        };
                    }

                    if (login.username === CREDENTIALS.username && login.password === atob(CREDENTIALS.password)) {
                        return {
                            success: true,
                            message: 'Redirecting to dashboard.'
                        };
                    }
                }
            };
        }]);
})();