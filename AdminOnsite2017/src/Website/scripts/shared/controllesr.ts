module application.shared {
    'use strict';

    export class sharedController {
        public static centerID_shared: string;
        static $inject = ['$scope', 'application.shared.sharedService'];
        constructor(private $scope, private sharedSvc: application.shared.sharedService) {
        }
    }

    angular
        .module('application.shared')
        .controller('application.shared.sharedController', sharedController);
}