module app.uploadExamSuiteApp {
    'use strict';

    class uploadExamSuiteController {
        public uploadExamSuite: UploadExamSuite;

        static $inject = ['$scope', '$http'];
        constructor(private $scope, private $http) {

        }
    }

    angular
        .module('app.uploadExamSuiteApp', [])
        .controller('app.uploadExamSuiteApp.uploadExamSuiteController', uploadExamSuiteController)
        ;
}