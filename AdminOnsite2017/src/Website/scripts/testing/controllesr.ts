module application.testingApp {
    'use strict';

    class testingController {
        public _testingData: TestingVM;
        public _showInModal: shared.TestRegistrationRespone;
        public userPID: string;
        public userSheetID: string;
        static $inject = ['$scope', 'application.testingApp.testingService', 'application.shared.sharedService'];
        constructor(private $scope, private testingSvc: application.testingApp.testingService, private shareSvc: application.shared.sharedService) {
            this.testingSvc.ListTesting().then(te => {
                this._testingData = te
                if (this._testingData != null && this._testingData.TestRegistrations != null) {
                    this._testingData.TestRegistrations.forEach((re, index) => {
                        this.start("time" + index.toString(), new Date(re.EndExamThruTime.toString()));
                    });
                }
            });
        }
        public EndTest(pid: string, sheetid: string) {
            this.shareSvc.EndTest(pid, sheetid);
        }
        public ResumeTest(pid: string, sheetid: string) {
            this.shareSvc.Resume(pid, sheetid);
        }
        public start(elementId: string, endTime: Date) {
            setInterval(() =>
                this.Inclement(endTime, elementId), 1000);
        }
        public Inclement(timeSendToME: Date, elementId:string) {
            var element = document.getElementById(elementId);
            var Result = timeSendToME.getTime() - new Date().getTime();
            var Hours = Math.floor(Result / 1000 / 60 / 60);
            var Minutes = Math.floor(Result / 1000 / 60 % 60);
            var Seconds = Math.floor(Result / 1000%60);
            var TimeString = Hours + ':' + Minutes + ':' + Seconds;
            if (Result <= 0) { TimeString = '0:0:0'}
            element.innerHTML = TimeString;
        }

    }

    angular
        .module('application.testingApp')
        .controller('application.testingApp.testingController', testingController);
}