(function () {
    'use strict';
    var app = angular.module('olx-client', []);
    app.config(['$httpProvider', function ($httpProvider) {
        //Enable cross domain calls
        $httpProvider.defaults.useXDomain = true;
        $httpProvider.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded';
        //Remove the header used to identify ajax call  that would prevent CORS from working
        delete $httpProvider.defaults.headers.common['X-Requested-With'];
    }]);
    app.constant('API_BASE_URL', 'http://localhost:1000/api/ad');
    app.constant('PAGE_SIZE', 100);
    app.constant('CREDENTIALS', {
        username : 'admin',
        password : 'bDBsd09PdD8='
    });
})();