/**
 * Created by Umayr on 10/21/2014.
 */

(function () {
    'use strict';

    angular
        .module('olx-client')
        .controller('dashboard', ['$scope', 'api', 'login', '$timeout', 'PAGE_SIZE',
            function ($scope, api, login, $timeout, PAGE_SIZE) {
                var _setPages = function () {
                    var _first = 1;
                    var _last = Math.ceil($scope.totalCount / PAGE_SIZE);
                    if ($scope.currentPage >= 10) {
                        _first = Math.floor($scope.currentPage / 10) * 10 - 2;
                    }
                    $scope.totalPages = _.range(_first, _last);
                    $scope.pages = _.first($scope.totalPages, 13);
                };
                var _get = function (pageNo) {
                    $scope.loader = true;
                    var iterator = pageNo > 0 ? pageNo * PAGE_SIZE : 0;
                    api.get(iterator).success(function (data) {
                        $scope.response = data.Result;
                        $scope.loader = false;
                        _setPages();
                    });
                };
                $scope.login = {attempt: false};
                $scope.loader = false;
                $scope.response = null;
                $scope.selectedAd = null;
                $scope.totalCount = 0;
                $scope.totalPages = 0;
                $scope.pages = 0;
                $scope.currentPage = 1;
                $scope.selectedAd = null;
                $scope.showImages = false;

                api.count().success(function (data) {
                    $scope.totalCount = data.Result.Count;
                    _setPages();
                });

                $scope.showDottedPagination = function () {
                    return _.last($scope.totalPages) > 10 && ($scope.currentPage < Math.floor(_.last($scope.totalPages) / 10) * 10);
                };

                $scope.loadPage = function (pageNo) {
                    _get(pageNo);
                    $scope.currentPage = pageNo;
                };

                $scope.nextPage = function () {
                    _get(++$scope.currentPage);
                };
                $scope.prevPage = function () {
                    _get(--$scope.currentPage);
                };

                $scope.selectAd = function (item) {
                    $scope.selectedAd = item;
                };

                $scope.loadImages = function (uniqueId) {
                    $scope.loader = true;
                    api.images(uniqueId).success(function (data) {
                        $scope.loader = false;
                        $scope.selectedAd.Images = data.Result.Images;
                        $scope.showImages = true;
                    });
                };
                $scope.loginAttempt = function (e) {
                    e.preventDefault();
                    $scope.login.result = login.attempt($scope.login);
                    if ($scope.login.result.success) {
                        $timeout(function () {
                            $scope.login.attempt = true;
                            _get(0);
                        }, 1000);
                    }
                };
            }]);
})();