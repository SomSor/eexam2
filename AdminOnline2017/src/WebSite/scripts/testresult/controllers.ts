module application.testResult {
    'use strict';

    class testResultController {
        private testDate: Date;
        private testResult: TestResultVM;
        private prepareData: boolean;
        private failToLoad: boolean;
        private centerName: string;
        private canterId: string;
        static $inject = ['$scope', 'application.testResult.testResultService','application.shared.sharedService'];
        constructor(private $scope, private testResultSvc: application.testResult.testResultService, private sharedSVC: application.shared.sharedService) {
            this.canterId = this.sharedSVC._centerData._id;
            this.testDate = new Date();
            this.testResultSvc.GetTestResultByDate(this.testDate).then(re => {
                this.testResult = re;
                this.prepareData = true;
                console.log(re);
            }, error => {
                this.prepareData = true;
                this.failToLoad = true;
                console.log("Fail to loading TestResul");
            });
        }
        public getTestResultByDate() {
            this.testResultSvc.GetTestResultByDate(this.testDate).then(re => {
                this.testResult = re;
                this.prepareData = true;
            }, error => {
                this.prepareData = true;
                this.failToLoad = true;
                console.log("Fail to loading TestResul");
            });
        }
        public goPrintResult() {
            //window.open('print.html#!/qrcode/' + pid, '_blank');
            var newDateString = this.testDate.getFullYear() + '-' + this.testDate.getMonth() + '-' + this.testDate.getDate();
            console.log(newDateString);
            window.open('/?layout=none#!/printResult/' + newDateString ,'_blank');
            console.log(this.testDate);
            console.log(this.testDate.toString());
            console.log(this.testDate.toDateString());
        }
    }

    class testResultPrintController {
        private _datestring: string;
        private testDate: Date;
        private prepareData: boolean;
        private failToLoad: boolean;
        private canterId: string;
        private testResult: TestResultVM;
        static $inject = ['$scope', 'application.testResult.testResultService', 'application.shared.sharedService', '$stateParams'];
        constructor(private $scope, private testResultSvc: application.testResult.testResultService, private sharedSVC: application.shared.sharedService, private $stateParams) {
            this._datestring = this.$stateParams.datestring;
            this.testDate = new Date(this._datestring);
            this.canterId = this.sharedSVC._centerData._id;
            this.testResultSvc.GetTestResultByDate(this.testDate).then(re => {
                this.testResult = re;
                this.prepareData = true;
                console.log(re);
                setTimeout(function () { window.print(); }, 10);
            }, error => {
                this.prepareData = true;
                this.failToLoad = true;
                console.log("Fail to loading TestResul");
            });
        }
    }
    angular
        .module('application.testResult')
        .controller('application.testResult.testResultController', testResultController)
        .controller('application.testResult.testResultPrintController', testResultPrintController);
}