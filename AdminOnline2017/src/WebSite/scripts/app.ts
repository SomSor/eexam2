(function () {
    'use strict';

    angular.module('application', [
        'ui.router',
        'ngAnimate',
        'ngMaterial',
        //foundation
        'foundation',
        'foundation.dynamicRouting',
        'foundation.dynamicRouting.animations',

        'foundation.accordion',

        'application.mainApp',
        'application.shared',
        'application.syncResultApp',
        'application.syncTestRegisApp',
        'application.testBankApp',
        'application.testResult',
        'application.printResultApp'
    ])
        .config(config)
        .run(run)
        ;

    config.$inject = ['$urlRouterProvider', '$locationProvider'];

    function config($urlProvider, $locationProvider) {
        $urlProvider.otherwise('/');

        $locationProvider.html5Mode({
            enabled: false,
            requireBase: false
        });

        $locationProvider.hashPrefix('!');
    }

    function run() {
        FastClick.attach(document.body);
    }

})();
