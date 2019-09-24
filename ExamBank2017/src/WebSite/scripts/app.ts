(function () {
    'use strict';

    angular.module('app', [
        'ui.router',
        'ngAnimate',

        //foundation
        'foundation',
        'foundation.dynamicRouting',
        'foundation.dynamicRouting.animations',

        'foundation.accordion',

        'hc.marked',

        'app.subjectlistApp',
        'app.subjectApp',
        'app.examSuiteApp',
        'app.uploadExamSuiteApp',
        'app.creatEexamSuiteApp',
        'app.createInactiveSubjectApp',
        'app.creatEexamSuiteApp',
        'app.createQuestionApp',
        'app.shared'
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
