module app.subjectlistApp {
    'use strict';

    class inactiveSubjectListController {
        public _subjectList: SubjectList;

        static $inject = ['$scope', '$http', 'app.subjectlistApp.subjectListService'];
        constructor(private $scope, private $http, private _subLSvc: app.subjectlistApp.subjectListService) {
            this._subLSvc.InactiveGetSubject().then(subL=> {
                this._subjectList = subL;
            });
        }
    }

    class activatedSubjectListController {
        public _subjectList: SubjectList;

        static $inject = ['$scope', '$http', 'app.subjectlistApp.subjectListService'];
        constructor(private $scope, private $http, private subLSvc: app.subjectlistApp.subjectListService) {
            this.subLSvc.ActivatedGetSubject().then(subL=> {
                this._subjectList = subL;
            });
        }
    }

    angular
        .module('app.subjectlistApp')
        .controller('app.subjectlistApp.inactiveSubjectListController', inactiveSubjectListController)
        .controller('app.subjectlistApp.activatedSubjectListController', activatedSubjectListController)
        ;
}