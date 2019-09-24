module application.exambankApp {
    'use strict';

    class exambankController {
        public _examdata: ExamDataVM;
        public _downloadRequest :DownloadRequest;
        static $inject = ['$scope', 'application.exambankApp.exambankService'];
        constructor(private $scope, private exambankSvc: application.exambankApp.exambankService) {
            this.exambankSvc.ListExamData().then(ex=> { this._examdata = ex });
        }
        public downloadRequest(subjectcode: string, examlanguage: string, voicelanguage: string, quatity: number) {
            this.exambankSvc.Download(subjectcode, examlanguage, voicelanguage, quatity);
        }
    }

    angular
        .module('application.exambankApp')
        .controller('application.exambankApp.exambankController', exambankController);
}